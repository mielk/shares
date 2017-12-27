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
        },
        postTimelineResize: function (e) {
            runPostTimelineResizeAction(e.params);
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
            viewHeight: position.viewHeight,
            viewWidth: position.viewWidth,
            top: position.top,
            left: position.left,
            svgWidth: position.svgWidth,
            svgHeight: position.svgHeight,
            svgBaseWidth: self.baseSize.width,
            svgBaseHeight: self.baseSize.height
        };
        return result;
    }

    function runPostTimelineResizeAction(params) {
        if (params.resized) {
            self.renderer.sizer.resizeHorizontally(params);
        } else if (params.relocated) {
            self.renderer.sizer.moveHorizontally(params);
        }
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
            var resized = false;
            if (e.height) {
                $(svgContainer).height(e.height);
                resized = true;
            }
            if (e.width) {
                $(svgContainer).width(e.width);
                resized = true;
            }
            if (e.top) {
                $(svgContainer).css({ top: e.top + 'px' });
            }
            if (e.left) {
                $(svgContainer).css({ left: e.left + 'px' });
            }

            if (resized) {
                self.renderer.pathCalculator.updateSizeParams();
            }

            self.trigger({
                type: 'resize',
                height: e.height,
                width: e.width,
                top: e.top,
                left: e.left
            });
        }

        return {
            getContainer: function () {
                return parentContainer;
            },
            getSvgContainer: function () {
                return svgContainer;
            },
            getSize: function () {
                return size;
            },
            getWidth: function () {
                return size.width;
            },
            getHeight: function () {
                return size.height;
            },
            getSvg: function () {
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
                svgWidth: $(svgDiv).width(),
                svgHeight: $(svgDiv).height(),
                viewLeftX: -position.left,
                viewRightX: -position.left + width,
                viewWidth: width,
                viewHeight: height
            };
        };

        function getSvgSize() {
            return {
                width: $(svgDiv).width(),
                height: $(svgDiv).height()
            }
        }

        return {
            getPosition: getPosition,
            getSvgSize: getSvgSize
        };

    }();

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


    //Services.
    self.sizer = function () {

        function adjustVertically() {
            var layout = self.parent.layout.getPosition();
            var itemsRange = self.pathCalculator.getItemsRange(layout.viewLeftX, layout.viewRightX);
            var dataInfo = self.data.getPartDataInfo(itemsRange.firstIndex, itemsRange.lastIndex);
            var verticalAdjustments = self.pathCalculator.calculateVerticalAdjustments(dataInfo, layout.viewHeight);
            self.parent.ui.resize({ height: verticalAdjustments.height, top: verticalAdjustments.top });
        }

        function resizeHorizontally(params) {
            var layout = self.parent.layout.getPosition();
            var width = layout.viewWidth / params.relativeWidth;
            var left = params.relativeLeft * layout.svgWidth * (-1);
            self.parent.ui.resize({ left: left, width: width });
            adjustVertically();
        }

        function moveHorizontally(params) {
            var layout = self.parent.layout.getPosition();
            var left = params.relativeLeft * layout.svgWidth * (-1);
            self.parent.ui.resize({ left: left });
            adjustVertically();
        }

        return {
            adjustVertically: adjustVertically,
            resizeHorizontally: resizeHorizontally,
            moveHorizontally: moveHorizontally
        };

    }();


    //API.
    self.render = function () {
        var paths = self.pathCalculator.calculate();
        self.svg.clear();
        paths.forEach(function (item) {
            if (item.isCirclesSet) {
                item.sets.forEach(function (obj) {
                    var circle = self.svg.circle(obj.x, obj.y, obj.radius);
                    circle.attr({
                        'stroke': obj.stroke,
                        'stroke-width': 1,
                        'fill': obj.fill
                    });
                });
            } else if (item.path) {
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
            var firstIndex = Math.max(first, 0);
            var lastIndex = Math.min(last, quotations.length - 1);
            var firstItem = quotations[firstIndex];
            var lastItem = quotations[lastIndex];
            var max = firstItem.quotation.High;
            var min = firstItem.quotation.Low;
            for (var i = firstIndex + 1; i <= lastIndex; i++) {
                var item = quotations[i];
                if (item.quotation.High > max) max = item.quotation.High;
                if (item.quotation.Low < min) min = item.quotation.Low;
            }

            return {
                startDate: firstItem.Date,
                startIndex: firstItem.DateIndex,
                endDate: lastItem.Date,
                endIndex: lastItem.DateIndex,
                counter: (lastIndex - firstIndex + 1),
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
            getTrendlines: getTrendlines,
            getDataInfo: getDataInfo,
            getPartDataInfo: getPartDataInfo
        };

    })();

    self.pathCalculator = (function () {
        var dataInfo = {};
        var quotations = [];
        var trendlines = [];
        var params = {};
        //SVG paths.
        var ascendingPaths = [];
        var descendingPaths = [];
        var trendlinePaths = [];
        var extremaCircles = [];


        function calculate() {
            dataInfo = self.data.getDataInfo();
            quotations = self.data.getQuotations();
            trendlines = self.data.getTrendlines();
            updateSizeParams();

            prepareQuotationsSvgPaths();
            prepareTrendlinesSvgPaths();
            //prepareExtremaPaths();

            return getCombinedPaths();

        }

        function prepareQuotationsSvgPaths() {
            quotations.forEach(function (item) {
                var pathInfo = calculateQuotationPath(item.quotation);
                var arr = pathInfo.isAscending ? ascendingPaths : descendingPaths;
                arr.push(pathInfo.path);
            });
        }

        function prepareTrendlinesSvgPaths() {
            trendlines.forEach(function (item) {
                var pathInfo = calculateTrendlinePath(item);
                trendlinePaths.push(pathInfo.path);
            });
        }

        function prepareExtremaPaths() {
            quotations.forEach(function (item) {
                var price = item.price;
                if (price) {
                    var pathInfo = calculateExtremaPaths(item);
                    if (pathInfo) {
                        extremaCircles.push(pathInfo);
                    }
                }
            });
        }

        function updateSizeParams() {
            var svgSize = self.parent.layout.getSvgSize();
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
                       'M' + middle + ',' + shadeTop + 'L' + middle + ',' + bodyTop + 'Z' +
                       'M' + middle + ',' + shadeBottom + 'L' + middle + ',' + bodyBottom + 'Z';

            return {
                isAscending: isAscending,
                path: path
            };

        }

        function calculateTrendlinePath(item) {
            var startIndex = item.StartIndex - 3;
            var endIndex = (item.EndIndex ? item.EndIndex : quotations.length - 1) + 3;
            var startLevel = (startIndex - item.BaseStartIndex) * item.Slope + item.BaseLevel;
            var endLevel = (endIndex - item.BaseStartIndex) * item.Slope + item.BaseLevel;

            var startX = getX(startIndex);
            var startY = getY(startLevel);
            var endX = getX(endIndex);
            var endY = getY(endLevel);

            var path = 'M' + startX + ',' + startY + 'L' + endX + ',' + endY;
            return {
                type: 'trendline',
                path: path
            };

        }

        function calculateExtremaPaths(item) {
            var price = item.price;
            var distance = STOCK.CONFIG.peaks.distance;
            var value = 0;
            var isMin = null;

            if (price.PeakByClose || price.PeakByHigh) {
                value = Math.max(price.PeakByClose ? price.PeakByClose.Value : 0, price.PeakByHigh ? price.PeakByHigh.Value : 0);
                isMin = false;
            } else if (price.TroughByClose || price.TroughByLow) {
                value = Math.max(price.TroughByClose ? price.TroughByClose.Value : 0, price.TroughByLow ? price.TroughByLow.Value : 0);
                isMin = true;
            } else {
                return null;
            }

            var scale = Math.min(1, value / 50);
            var greyscale = 255 * (1 - scale);
            var x = getX(item.DateIndex) + (params.bodyWidth / 2);
            var y = isMin ? getY(item.quotation.Low) + distance : getY(item.quotation.High) - distance;

            return {
                item: item,
                x: x,
                y: y,
                radius: Math.min(value / 5, 10),
                stroke: 'rgb(' + greyscale + ',' + greyscale + ',' + greyscale + ')',
                fill: 'rgba(' + (isMin ? '255, 0' : '0, 255') + ', 0, ' + scale + ')'
            };


        }



        //Add peak/through indicators.
        function addExtremumLabel(extrema, isMin) {
            var dist = STOCK.CONFIG.peaks.distance;
            var extremum = isMin ?
                Math.max(item.troughByClose ? item.troughByClose.Value : 0, item.troughByLow ? item.troughByLow.Value : 0) :
                Math.max(item.peakByClose ? item.peakByClose.Value : 0, item.peakByHigh ? item.peakByHigh.Value : 0);
            if (!extremum) return;



            return true;

        }



        //Helper methods.
        function getX(value) {
            var candlesFromFirst = value - dataInfo.startIndex;
            return candlesFromFirst * params.singleItemWidth + params.candlePadding;
        }

        function getY(value) {
            var pointsDistance = dataInfo.max - value;
            return pointsDistance * params.oneHeight;
        }

        function getItemForX(x) {
            return Math.min(Math.floor(x / params.singleItemWidth), dataInfo.counter - 1);
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
                path: trendlinePaths.join(''),
                attr: {
                    'stroke': 'black',
                    'stroke-width': 0.3,
                    'fill': STOCK.CONFIG.trendlines.color
                }
            });
            result.push({
                isCirclesSet: true,
                sets: extremaCircles
            });

            return result;

        }


        //Reverse engineering.
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
            updateSizeParams: updateSizeParams,
            getItemsRange: getItemsRange,
            calculateVerticalAdjustments: calculateVerticalAdjustments
        };

    })();

}
mielk.objects.extend(AbstractSvgRenderer, PriceSvgRenderer);





















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

//self.updateTrendlines = function (trendlines) {
//    self.trendlines = trendlines;

//    if (self.params.complete && self.params.runWhenComplete) {
//        self.startAnalysis();
//    }

//}