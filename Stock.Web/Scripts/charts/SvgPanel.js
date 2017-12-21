function SvgPanel(params) {

    'use strict';

    //[Meta]
    var self = this;
    self.SvgPanel = true;
    self.parent = params.parent;
    self.type = params.type;
    self.key = params.key + '_svg';

    //Bind events.
    self.parent.bind({
        dataLoaded: function (e) {
            dataLoaded(e.params);
        },
        dataInfoLoaded: function (e) {
            dataInfoLoaded(e.params);
        }
    });


    //Loading data functions.
    function dataInfoLoaded(params) {
        var r = self.renderer;
        r.setDataInfo(params.data);
    }

    function dataLoaded(params) {
        var r = self.renderer;
        r.setData(params.data);
        r.render();
        self.trigger({
            type: 'postRender',
            params: getPostRenderProperties()
        });
    }

    function getPostRenderProperties() {
        var dataInfo = self.renderer.data.getDataInfo();
        var position = self.layout.getPosition();
        var result = {
            maxValue: dataInfo.max,
            minValue: dataInfo.min,
            viewHeight: position.height,
            viewWidth: position.width,
            top: position.top,
            left: position.left,
            svgWidth: position.svgWidth,
            svgHeight: position.svgHeight,
            svgBaseWidth: self.baseSize.width,
            svgBaseHeight: self.baseSize.height
        };
        return result;
    }



    //[UI]
    self.baseSize = {
        width: STOCK.CONFIG.svgPanel.width,
        height: STOCK.CONFIG.svgPanel.height
    }; 
    self.ui = (function () {

        var parentContainer = params.container;

        //Append SVG container.
        var svgContainer = $('<div/>', {
            'class': 'chart-svg-panel',
            id: self.key
        }).css({
            'height': self.baseSize.height + 'px',
            'width': self.baseSize.width + 'px',
            'left': 0,
            'top': 0
        }).appendTo(parentContainer);

        var svg = Raphael(self.key);
        svg.setViewBox(0, 0, self.baseSize.width, self.baseSize.height, true);
        svg.canvas.setAttribute('preserveAspectRatio', 'none');



        function resize(e) {
            if (e.height) $(svgContainer).height(e.height);
            if (e.top) $(svgContainer).css({ top: e.top + 'px' });

            self.trigger({
                type: 'resize',
                height: e.height,
                width: e.width,
                top: e.top,
                left: e.left
            });
        }


        return {
            getContainer: function(){
                return parentContainer;
            },
            getSvgContainer: function() {
                return svgContainer;
            },
            getSize: function(){
                return size;
            },
            getWidth: function(){
                return size.width;
            },
            getHeight: function(){
                return size.height;
            },
            getSvg: function() {
                return svg;
            },
            resize: resize
        };

    })();
    self.renderer = self.type.svgRenderer({
        parent: self,
        svg: self.ui.getSvg()
    });


    //Layout service.
    self.layout = function () {

        var parentDiv = self.ui.getContainer();
        var svgDiv = self.ui.getSvgContainer();

        function getPosition() {
            var position = $(svgDiv).position();
            var width = $(parentDiv).width();
            var height = $(parentDiv).height();
            var right = position.left + width;
            var bottom = position.top + height;
            return {
                left: position.left,
                top: position.top,
                right: right,
                bottom: bottom,
                width: width,
                height: height,
                svgWidth: $(svgDiv).width(),
                svgHeight: $(svgDiv).height()
            };
        };

        return {
            getPosition: getPosition
        };

    }();


    //function findQuotation(x) {
    //    if (self.renderer) {
    //        return self.renderer.findQuotation(x);
    //    } else {
    //        return null;
    //    }
    //}

    //function getInfo(quotation) {
    //    if (self.renderer) {
    //        return self.renderer.getInfo(quotation);
    //    }
    //}

    //API.
    //self.render = render;
    //self.loadQuotations = loadQuotations;
    //self.findQuotation = findQuotation;
    //self.getInfo = getInfo;

}
SvgPanel.prototype.bind = function (e) {
    $(self).bind(e);
}
SvgPanel.prototype.trigger = function (e) {
    $(self).trigger(e);
}

function AbstractSvgRenderer(params) {

    'use strict';

    var self = this;
    self.AbstractSvgRenderer = true;
    self.parent = params.parent;
    self.svg = params.svg;
    self.drawObjects = [];

    self.params = {
        created: true
    };

    //Events.
    self.parent.bind({
        resize: function (e) {
            //if (e.height) $(self.svg).css({ 'height': e.height + 'px' });
        }
    });


    //Services.
    self.sizer = function () {

        function adjustVertically() {
            var layout = self.parent.layout.getPosition();
            var itemsRange = self.pathCalculator.getItemsRange(layout.left, layout.right);
            var dataInfo = self.data.getPartDataInfo(itemsRange.firstIndex, itemsRange.lastIndex);
            var verticalAdjustments = self.pathCalculator.calculateVerticalAdjustments(dataInfo, layout.height);
            self.parent.ui.resize({ height: verticalAdjustments.height, top: verticalAdjustments.top });
        }

        function scale() {

        }

        return {
            adjustVertically: adjustVertically,
            scale: scale
        };

    }();



    //API.
    self.render = function () {
        var paths = self.pathCalculator.calculate();
        self.svg.clear();
        paths.forEach(function (item) {
            if (item.path) {
                self.svg.path(item.path).attr(item.attr);
            }
        });
        self.sizer.adjustVertically();
    };

    self.setDataInfo = function (e) {
        self.data.setDataInfo(e);
    }

    self.setData = function (e) {
        self.data.setData(e);
    }

    self.getSvgSize = function () {
        var width = self.svg.width;
        var height = self.svg.height;
        return {
            width: width,
            height: height
        };
    }

}

function PriceSvgRenderer(params) {

    'use strict';

    AbstractSvgRenderer.call(this, params);
    var self = this;
    self.PriceSvgRenderer = true;
    self.type = STOCK.INDICATORS.PRICE;

    //Add parameters specific for this type of chart (i.e. for ADX minimum allowed is 0).
    self.params.minAllowed = 0;
    self.params.maxAllowed = null;

    //Data manager.
    self.data = (function () {
        var dataInfo = {};
        var quotations = [];
        var trendlines = [];


        function setData(params) {
            quotations = params.quotations;
            trendlines = params.trendlines;
        }

        function setDataInfo(data) {
            dataInfo = data;
        }

        function setQuotations(data) {
            quotations = $quotations;
            //self.renderer.rerenderTrendlines
        }

        function setTrendlines(data) {
            trendlines = $trendlines;
            //self.renderer.rerenderTrendlines
        }



        function getDataInfo() {
            return dataInfo;
        }

        function getQuotations() {
            return quotations;
        }

        function getTrendlines() {
            return trendlines;
        }

        function getPartDataInfo(first, last) {
            var firstItem = quotations[first];
            var lastItem = quotations[last];
            var max = firstItem.quotation.High;
            var min = firstItem.quotation.Low;
            for (var i = first + 1; i <= last; i++) {
                var item = quotations[i];
                if (item.quotation.High > max) max = item.quotation.High;
                if (item.quotation.Low < min) min = item.quotation.Low;
            }

            return {
                startDate: firstItem.Date,
                startIndex: firstItem.DateIndex,
                endDate: lastItem.Date,
                endIndex: lastItem.DateIndex,
                counter: (last - first + 1),
                max: max,
                min: min,
                levelDifference: max - min
            };
            
        }


        return {
            setQuotations: setQuotations,
            setTrendlines: setTrendlines,
            setData: setData,
            setDataInfo: setDataInfo,
            getQuotations: getQuotations,
            getTrendlins: getTrendlines,
            getDataInfo: getDataInfo,
            getPartDataInfo: getPartDataInfo
        };

    })();

    self.pathCalculator = (function () {
        var dataInfo = {};
        var quotations = [];
        var params = {};
        var ascendingPaths = [];
        var descendingPaths = [];
        var trendlines = [];

        function calculate() {
            dataInfo = self.data.getDataInfo();
            quotations = self.data.getQuotations();
            calculateSizes();
            for (var i = 0; i < quotations.length; i++) {
                var item = quotations[i];
                var pathInfo = calculateQuotationPath(item.quotation);
                if (pathInfo.isAscending) {
                    ascendingPaths.push(pathInfo.path);
                } else {
                    descendingPaths.push(pathInfo.path);
                };
            }

            return getCombinedPaths();

        }

        function calculateSizes() {
            var svgSize = self.getSvgSize();
            params.singleItemWidth = svgSize.width / dataInfo.counter;
            params.oneHeight = svgSize.height / dataInfo.levelDifference;
            params.candlePadding = params.singleItemWidth * STOCK.CONFIG.candle.space / 2;
            params.bodyWidth = params.singleItemWidth - params.candlePadding;
        }

        function calculateQuotationPath(item) {
            var isAscending = (item.Close > item.Open);
            var bodyTop = getY(isAscending ? item.Close : item.Open);
            var bodyBottom = getY(isAscending ? item.Open : item.Close);
            var shadeTop = getY(item.High);
            var shadeBottom = getY(item.Low);
            var left = getX(item.DateIndex);
            var right = left + params.bodyWidth;
            var middle = left + (params.bodyWidth / 2);
            
            var path = 'M' + left + ',' + bodyBottom + 'L' + left + ',' + bodyTop + 'L' +
                       right + ',' + bodyTop + 'L' + right + ',' + bodyBottom + 'Z' +
                       'M' + middle + ',' + shadeBottom + 'L' + middle + ',' + bodyBottom + 'Z' +
                       'M' + middle + ',' + shadeTop + 'L' + middle + ',' + bodyTop + 'Z';

            return {
                isAscending: isAscending,
                path: path
            };

        }

        function getX(value) {
            var candlesFromFirst = value - dataInfo.startIndex;
            return candlesFromFirst * params.singleItemWidth + params.candlePadding;
        }

        function getY(value) {
            var pointsDistance = dataInfo.max - value;
            return pointsDistance * params.oneHeight;
        }

        function getCombinedPaths() {
            var result = [];
            result.push({
                path: ascendingPaths.join(''),
                attr: {
                        'stroke': 'black',
                        'stroke-width': 0.3,
                        'fill': STOCK.CONFIG.candle.color.ascending
                    }
            });
            result.push({
                path: descendingPaths.join(''),
                attr: {
                    'stroke': 'black',
                    'stroke-width': 0.3,
                    'fill': STOCK.CONFIG.candle.color.descending
                }
            });
            result.push({
                path: trendlines.join(''),
                attr: {
                    'stroke': 'black',
                    'stroke-width': 0.3,
                    'fill': STOCK.CONFIG.trendlines.color
                }
            });

            return result;

        }


        //Reverse engineering.
        function getItemForX(x) {
            return Math.min(Math.floor(x / params.singleItemWidth), dataInfo.counter - 1);
        }

        function getItemsRange(left, right) {
            var firstItem = getItemForX(left);
            var lastItem = getItemForX(right);
            return {
                firstIndex: firstItem,
                lastIndex: lastItem
            }
        }

        function calculateVerticalAdjustments(partDataInfo, visibleHeight) {
            var modifiedMin = partDataInfo.min * 0.97;
            var modifiedMax = partDataInfo.max * 1.03;
            var levelDifference = Math.abs(modifiedMin - modifiedMax);
            var coefficient = dataInfo.levelDifference / levelDifference;

            var newHeight = Math.ceil(visibleHeight * coefficient);
            var newTop = (dataInfo.max - modifiedMax) * (newHeight / dataInfo.levelDifference);

            return {
                height: newHeight,
                top: Math.floor(-newTop)
            };
        }


        return {
            calculate: calculate,
            getItemsRange: getItemsRange,
            calculateVerticalAdjustments: calculateVerticalAdjustments
        };

    })();

}
mielk.objects.extend(AbstractSvgRenderer, PriceSvgRenderer);


PriceSvgRenderer.prototype = {

    fetchCircleObjects: function (items) {
        var array = [];
        items.forEach(function (item) {
            if (item.extrema.length) {
                item.extrema.forEach(function (extremum) {
                    array.push(extremum);
                });
            }
        });

        return array;

    },

    getInfo: function (quotation) {
        var info = (quotation ?
                        'Open: ' + quotation.open + ' | ' +
                        'Low: ' + quotation.low + ' | ' +
                        'High: ' + quotation.high + ' | ' +
                        'Close: ' + quotation.close
                        : '');
        
        return info;

    }

};



























////Parameters specific for this type of chart.
//self.size = params.size;
//self.quotations = params.quotations;
//self.trendlines = params.trendlines;
//self.paths = {};
//self.offset = 0;


//function calculateHorizontalBound(items) {
//    var singleWidth = STOCK.CONFIG.candle.width;
//    var totalWidth = items.length * singleWidth;

//    self.offset = self.parent.parent.offset();
//    //self.params.firstItem = items.length - 1 - Math.floor((self.offset + self.size.width) / singleWidth);
//    self.params.firstItem = 0;
//    //self.params.lastItem = items.length - 1 - Math.floor(self.offset / singleWidth);
//    self.params.lastItem = items.length - 1;
//    self.params.singleWidth = singleWidth;
//    self.params.totalWidth = totalWidth;

//}

//function findFirstNonEmptyIndex(index, direction) {
//    var step = (direction ? (direction / Math.abs(direction)) : 1);
//    var quotation = null;

//    for (var i = index; i >= 0 && i <= self.quotations.length; i += step) {
//        quotation = self.quotations[index];
//        if (quotation) {
//            return i;
//        }
//    }

//    return index;

//}

////Function to calculate vertical limits for the current chart and measures for a single unit.
//function calculateVerticalBounds(items, height) {

//    //Find [min] and [max] value.

//    //[Handling gaps in prices]
//    //Function [findFirstNonEmptyIndex] implement for handling gaps in prices.
//    //If user moves to the screen where there are only gaps, range.min and range.max are null
//    //and this function cannot proceed.
//    var range = self.findVerticalRange(items, 
//                        findFirstNonEmptyIndex(self.params.firstItem, 1), 
//                        findFirstNonEmptyIndex(self.params.lastItem, -1));

//    //[Handling gaps in prices]
//    //If user moves to the screen where there are only gaps, range.min and range.max are null
//    //and this function cannot proceed.
//    if (range.min === null && range.max === null) {
//        range = self.findVerticalRange(items);
//    }

//    var min = range.min;
//    var max = range.max;
//    var difference = max - min;

//    var bottom = min - STOCK.CONFIG.chart.margin * difference;
//    bottom = self.params.minAllowed !== null ? Math.max(bottom, self.params.minAllowed) : bottom;
//    var top = max + STOCK.CONFIG.chart.margin * difference;
//    top = self.params.maxAllowed !== null ? Math.min(top, self.params.maxAllowed) : top;
//    var distance = top - bottom;
//    var unitHeight = height / distance;

//    //Add vertical bounds of the chart.
//    mielk.objects.addProperties(
//        self.params,
//        {   min: min,
//            max: max,
//            top: top,
//            bottom: bottom,
//            unitHeight: unitHeight  }
//    );

//}

////Funkcja zwraca najniższą i najwyższą wartość dla zakresu ograniczonego 
////przez indeksy [first] i [last] w zestawie danych [items].
//self.findVerticalRange = function (items, first, last) {
//    var $first = first || 0;
//    var $last = last || items.length - 1;

//    var min = mielk.arrays.getMin(items, self.fnMinEvaluation, $first, $last);
//    var max = mielk.arrays.getMax(items, self.fnMaxEvaluation, $first, $last);

//    return {
//        min: min,
//        max: max
//    };

//}

//self.countTrendlineValue = function(trendline, index){
//    return (index - trendline.BaseStartIndex) * trendline.Slope + trendline.BaseLevel;
//}

//self.createTrendlinePath = function (trendline, params) {
//    var INITIAL_OFFSET = 2;
//    var AFTER_OFFSET = 20;
//    var bodyWidth = params.width - params.space;
//    var initialIndex = Math.max(0, trendline.StartIndex - INITIAL_OFFSET);
//    //var endIndex = Math.max(0, trendline.EndIndex - INITIAL_OFFSET);
//    var endIndex = Math.max(0, trendline.CounterStartIndex - INITIAL_OFFSET);
//    if (self.quotations[initialIndex].coordinates) {
//        var x1 = self.quotations[initialIndex].coordinates.middle;
//        var value1 = self.countTrendlineValue(trendline, initialIndex);
//        var y1 = self.getY(value1);
//        var boundIndex = Math.min(trendline.CounterStartIndex + AFTER_OFFSET, self.quotations.length - 1);
//        var x2 = self.quotations[boundIndex].coordinates.middle;// + (AFTER_OFFSET * params.width);
//        var value2 = self.countTrendlineValue(trendline, boundIndex);
//        var y2 = self.getY(value2);

//        var attr = {
//            'stroke': '#888',
//            'stroke-width': 1
//        };

//        //Calculate coordinates.
//        var path = 'M' + x1 + ',' + y1 + 'L' + x2 + ',' + y2;

//    }

//    //Save the coordinates of this item's candle 
//    //(used later to display values on the chart and to scale charts).
//    trendline.coordinates = {
//        x1: x1,
//        y1: y1,
//        x2: x2,
//        y2: y2
//    };

//    return {
//        path: path,
//        attr: attr
//    }

//}

////Funkcja zwraca wszystkie obiekty, które mają zostać narysowane na tym wykresie.
//self.getDrawObjects = function (quotations) {

//    //Calculate offsets and ranges.
//    calculateHorizontalBound(quotations);
//    calculateVerticalBounds(quotations, self.size.height);

//    //Params calculated here for performance reasons.
//    var params = {
//        width: STOCK.CONFIG.candle.width,
//        space: STOCK.CONFIG.candle.width * STOCK.CONFIG.candle.space,
//        other: self.parent.properties
//    };

//    //Create SVG path for each single quotation.
//    var items = new Array(quotations.length);
//    for (var i = self.params.firstItem; i <= self.params.lastItem; i++) {
//        var invertedIndex = quotations.length - i;
//        if (quotations[i])
//            items[i] = self.createBasePath(invertedIndex, quotations[i], params);
//    }


//    //Join all SVG paths together and return them.
//    return self.totalPaths(items);

//}

////Funkcja zwracająca linie trendu do narysowania w formacie ścieżek SVG.
//self.getDrawTrendlines = function (trendlines) {

//    var paths = {
//        finished: '',
//        active: ''
//    };

//    var params = {
//        width: STOCK.CONFIG.candle.width,
//        space: STOCK.CONFIG.candle.width * STOCK.CONFIG.candle.space,
//        other: self.parent.properties
//    };

//    if (trendlines) {
//        trendlines.forEach(function (trendline) {
//            if (trendline.ShowOnChart) {
//                var res = self.createTrendlinePath(trendline, params);
//                if (item.IsFinished) {
//                    paths.finished += res.path;
//                } else {
//                    paths.active += res.path;
//                }
//            }
//        });
//    }

//    //Add properties for each path.
//    var array = [];
//    array.push({
//        path: paths.finished,
//        attr: {
//            'stroke': '#888',
//            'stroke-width': 1
//        }
//    });

//    array.push({
//        path: paths.active,
//        attr: {
//            'stroke': 'black',
//            'stroke-width': 1
//        }
//    });

//    return array;

//}

////Funkcja obliczająca pozycję pionową dla danej wartości (w zależności od wysokości kontenera).
//self.getY = function (value) {
//    return self.size.height * (this.params.top - value) / (this.params.top - this.params.bottom);
//};

////Funkcja zwraca proporcje pomiędzy podanym zakresem a całym zakresem dla aktualnego zestawu danych.
//self.getRatio = function (range) {
//    var ratio = (range.max - range.min) / (self.params.top - self.params.bottom);
//    return ratio;
//}

////Funkcja zwraca notowanie, którego świeca jest aktualnie wyświetlona w odległości [x] od lewej
////krawędzi ramki z notowaniami.
//self.findQuotation = function (x) {
//    var firstItem = null;
//    var index = self.params.firstItem - 1;

//    while (!firstItem) {
//        index++;
//        firstItem = self.quotations[index];
//        if (index >= self.params.lastItem) break;
//    }

//    if (!firstItem) return null;

//    var firstItemOffset = firstItem.coordinates.left;
//    var itemsOffset = Math.floor(x / self.params.singleWidth);
//    var modItemsOffset = x % self.params.singleWidth;
//    var foundItemIndex = index + itemsOffset + (-firstItemOffset + modItemsOffset > self.params.singleWidth ? 1 : 0) - (index - self.params.firstItem);

//    return self.quotations[foundItemIndex];
//}

////Funkcja odświeża notowania przypisane do tego wykresu. Wywoływana kilka razy podczas pobierania
////danych z bazy, ponieważ notowania są pobierane paczkami.
//self.updateQuotations = function (quotations, complete) {
//    self.quotations = quotations;
//    self.params.complete = complete;

//    if (self.params.complete && self.params.runWhenComplete) {
//        self.startAnalysis();
//    }

//}


//self.updateTrendlines = function (trendlines) {
//    self.trendlines = trendlines;

//    if (self.params.complete && self.params.runWhenComplete) {
//        self.startAnalysis();
//    }

//}

//self.startAnalysis = function () {
//    var analyzer = self.parent.type.analyzer();

//    if (analyzer) {
//        analyzer.run(self.quotations);
//    }

//    self.parent.render();

//}
