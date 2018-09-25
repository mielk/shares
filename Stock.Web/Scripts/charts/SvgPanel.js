function SvgPanel(params) {

    'use strict';

    //[Meta]
    var self = this;
    self.SvgPanel = true;
    self.parent = params.parent;
    self.type = params.type;
    self.key = params.key;
    self.index = params.index;
    self.candleWidth = params.candleWidth;
    self.isRendered = false;

    self.render = function() {
        var r = self.renderer;
        r.setDataInfo(self.parent.dataInfo);
        r.setData(self.parent.data)
        r.renderQuotations();
        r.renderExtrema();
        self.isRendered = true;

        //    r.render();
        //    self.trigger({
        //        type: 'postRender',
        //        params: getPostRenderProperties()
        //    });

    }

    //function getPostRenderProperties() {
    //    var dataInfo = self.renderer.data.getDataInfo();
    //    var position = self.layout.getPosition();
    //    var result = {
    //        maxValue: dataInfo.max,
    //        minValue: dataInfo.min,
    //        viewHeight: position.viewHeight,
    //        viewWidth: position.viewWidth,
    //        top: position.top,
    //        left: position.left,
    //        svgWidth: position.svgWidth,
    //        svgHeight: position.svgHeight,
    //        svgBaseWidth: self.baseSize.width,
    //        svgBaseHeight: self.baseSize.height
    //    };
    //    return result;
    //}

    //function runPostTimelineResizeAction(params) {
    //    if (params.resized) {
    //        self.renderer.sizer.resizeHorizontally(params);
    //    } else if (params.relocated) {
    //        self.renderer.sizer.moveHorizontally(params);
    //    }
    //}


    //[UI]
    self.baseSize = {
        width: Math.floor(self.candleWidth * self.parent.getItemsRange()),
        height: Math.floor(STOCK.CONFIG.svgPanel.height)
    };

    self.ui = (function () {
        var parentContainer = params.container;
        var candlesKey = self.key + '_candles';
        var trendlinesKey = self.key + '_trendlines';
        var extremaKey = self.key + '_extrema';
        var svgCandles = null;
        var svgExtrema = null;

        //Candles container.
        var svgsContainer = $('<div/>', {
            'class': 'chart-svg-panel',
            id: candlesKey
        }).css({
            'height': (self.baseSize.height + 100) + 'px',
            'width': self.baseSize.width + 'px',
            'left': 0,
            'top': 0,
            'visibility': 'hidden'
        }).appendTo(parentContainer)[0];



        //var svgCandles = Raphael(candlesKey);
        //svgCandles.setViewBox(0, 0, self.baseSize.width, self.baseSize.height, true);
        //svgCandles.canvas.setAttribute('preserveAspectRatio', 'none');

        //var svgTrendlines = Raphael(trendlinesKey);
        //svgTrendlines.setViewBox(0, 0, self.baseSize.width, self.baseSize.height, true);
        //svgTrendlines.canvas.setAttribute('preserveAspectRatio', 'none');

        //Extrema container.



        function insertSvgQuotations() {
            var svg = mielk.svg.createSvg();
            var height = $(svgsContainer).height() - 100;
            
            svg.setAttribute('viewBox', '0 0 ' + self.baseSize.width + ' ' + height);
            svg.setAttribute('preserveAspectRatio', 'none meet');
            svg.style.top = '50px';
            svg.style.height = height + 'px';

            svgsContainer.appendChild(svg);
            return svg;

        }

        function insertSvgExtrema() {
            var svg = mielk.svg.createSvg();
            var width = $(svgsContainer).width();
            var height = $(svgsContainer).height();

            svg.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svg.setAttribute('preserveAspectRatio', 'none meet');
            svg.style.top = 0;
            svg.style.height = height + 'px';

            svgsContainer.appendChild(svg);
            return svg;

        }

        function resize(e) {
            var resized = false;
            if (e.height) {
                $(svgsContainer).height(e.height);
                resized = true;
            }
            if (e.width) {
                $(svgsContainer).width(e.width);
                resized = true;
            }
            if (e.top) {
                $(svgsContainer).css({ top: e.top + 'px' });
            }
            if (e.left) {
                $(svgsContainer).css({ left: e.left + 'px' });
            }

            if (resized) {
                self.renderer.updateSizeParams();
            }

            self.trigger({
                type: 'resize',
                height: e.height,
                width: e.width,
                top: e.top,
                left: e.left
            });
        }

        function show() {
            $(svgsContainer).css('visibility', 'visible').css('display', 'block');
        }

        function hide() {
            $(svgsContainer).css('visibility', 'hidden').css('display', 'none');
        }

        return {
            getContainer: function () {
                return parentContainer;
            },
            getSvgsContainer: function () {
                return svgsContainer;
            },
            getCandlesSvg: function () {
                if (svgCandles === null) {
                    svgCandles = insertSvgQuotations();
                }
                return svgCandles;
            },
            getTrendlinesSvg: function () {
                return svgTrendlines;
            },
            getExtremaSvg: function () {
                if (svgExtrema === null) {
                    svgExtrema = insertSvgExtrema();
                }
                return svgExtrema;
            },
            resize: resize,
            show: show,
            hide: hide
        };

    })();

    self.renderer = self.type.svgRenderer({
        parent: self
    });

    //Layout service.
    self.layout = function () {

        var parentDiv = self.ui.getContainer();
        var candlesSvg = self.ui.getCandlesSvg();

        function getPosition() {
            var position = $(candlesSvg).position();
            var width = $(parentDiv).width();
            var height = $(parentDiv).height();
            var right = position.left + width;
            var bottom = position.top + height;
            return {
                left: position.left,
                top: position.top,
                right: right,
                bottom: bottom,
                svgWidth: $(candlesSvg).width(),
                svgHeight: $(candlesSvg).height(),
                viewLeftX: -position.left,
                viewRightX: -position.left + width,
                viewWidth: width,
                viewHeight: height
            };
        };

        function getCandlesSvgSize() {
            return {
                width: $(candlesSvg).width(),
                height: $(candlesSvg).height()
            }
        }

        function getExtremaSvgOffset() {
            var extremaSvg = self.ui.getExtremaSvg();
            var candlesSvg = self.ui.getCandlesSvg();

            if (extremaSvg && candlesSvg) {
                var extremaHeight = $(extremaSvg).height();
                var candlesHeight = $(candlesSvg).height();
                return extremaHeight - candlesHeight;
            }

        }

        return {
            getPosition: getPosition,
            getCandlesSvgSize: getCandlesSvgSize,
            getExtremaSvgOffset: getExtremaSvgOffset
        };

    }();

    self.show = function () {
        self.ui.show();
    };

    self.hide = function () {
        self.ui.hide();
    };

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
    self.drawObjects = [];

    //Data manager.
    self.dataInfo = {};
    self.quotations = [];
    self.trendlines = [];
    self.extrema = [];

    self.params = {
        created: true,
        candleWidth: self.parent.candleWidth,
        spaceShare: STOCK.CONFIG.candle.space
    };

    self.updateSizeParams = function () {
        var candlesSvgSize = self.parent.layout.getCandlesSvgSize();
        self.params.extremaSvgOffset = self.parent.layout.getExtremaSvgOffset();
        self.params.oneUnitHeight = candlesSvgSize.height / self.dataInfo.levelDifference;
        self.params.candlePadding = self.params.candleWidth * self.params.spaceShare / 2;
        self.params.bodyWidth = self.params.candleWidth * (1 - self.params.spaceShare);
    };


    //Setters
    self.setDataInfo = function (dataInfo) {
        self.dataInfo = dataInfo;
        self.updateSizeParams();
    }
    self.setData = function (data) {
        self.quotations = data.quotations;
        self.trendlines = data.trendlines;
        self.extrema = (function () {
            var result = [];
            data.quotations.forEach(function (item) {
                if (item.price.PeakByClose) {
                    result.push(item);
                } else if (item.price.PeakByHigh) {
                    result.push(item);
                } else if (item.price.TroughByClose) {
                    result.push(item);
                } else if (item.price.TroughByLow) {
                    result.push(item);
                }
            });

            return result;

        })();
    }


    self.data = (function () {
        function getPartDataInfo(first, last) {
            var firstIndex = Math.max(first, 0);
            var lastIndex = Math.min(last, self.quotations.length - 1);
            var firstItem = self.quotations[firstIndex];
            var lastItem = self.quotations[lastIndex];
            var max = firstItem.quotation.High;
            var min = firstItem.quotation.Low;
            for (var i = firstIndex + 1; i <= lastIndex; i++) {
                var item = self.quotations[i];
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

        };

        return {
            getPartDataInfo: getPartDataInfo
        };

    })();


    //Services.
    self.sizer = function () {

        function adjustVertically() {
            var layout = self.parent.layout.getPosition();
            var itemsRange = self.quotationsPathMaker.getItemsRange(layout.viewLeftX, layout.viewRightX);
            var dataInfo = self.data.getPartDataInfo(itemsRange.firstIndex, itemsRange.lastIndex);
            var verticalAdjustments = self.quotationsPathMaker.calculateVerticalAdjustments(dataInfo, layout.viewHeight);
            self.parent.ui.resize({ height: verticalAdjustments.height, top: verticalAdjustments.top });
        }

        //function resizeHorizontally(params) {
        //    var layout = self.parent.layout.getPosition();
        //    var width = layout.viewWidth / params.relativeWidth;
        //    var left = params.relativeLeft * layout.svgWidth * (-1);
        //    self.parent.ui.resize({ left: left, width: width });
        //    adjustVertically();
        //}

        //function moveHorizontally(params) {
        //    var layout = self.parent.layout.getPosition();
        //    var left = params.relativeLeft * layout.svgWidth * (-1);
        //    self.parent.ui.resize({ left: left });
        //    adjustVertically();
        //}

        return {
              adjustVertically: adjustVertically
            //, resizeHorizontally: resizeHorizontally
            //, moveHorizontally: moveHorizontally
        };

    }();


    //API.
    self.render = function () {
        self.renderQuotations();
        self.renderTrendlines();
        self.renderExtrema();
    };

    self.renderQuotations = function () {
        var mode = 0;   //0 - paths | 1 - rectangles
        var svg = self.parent.ui.getCandlesSvg();
        var strokeWidth = STOCK.CONFIG.candle.strokeWidth;

        if (mode === 0) {
            var paths = self.quotationsPathMaker.getPaths(self.quotations);
            paths.forEach(function (item) {
                var path = mielk.svg.createPath(item.path);
                path.style.stroke = item.attr.stroke;
                path.style.strokeWidth = item.attr.strokeWidth + 'px';
                path.style.fill = item.attr.fill;
                path.style.vectorEffect = 'non-scaling-stroke';
                path.style.shapeRendering = 'crispEdges'
                svg.appendChild(path);
            });
        } else if (mode === 1){
            //var rectangles = self.quotationsPathMaker.getPaths(quotations);
            //rectangles.forEach(function (item) {
            //    var rectangle = document.createElementNS("http://www.w3.org/2000/svg", 'rect');
            //    rectangle.setAttribute('x', item.x);
            //    rectangle.setAttribute('y', item.y);
            //    rectangle.setAttribute('height', item.height);
            //    rectangle.setAttribute('width', item.width);
            //    rectangle.style.stroke = item.stroke;
            //    rectangle.style.strokeWidth = strokeWidth + 'px';
            //    rectangle.style.fill = item.fill;
            //    rectangle.style.vectorEffect = 'non-scaling-stroke';
            //    rectangle.style.shapeRendering = 'crispEdges'
            //    svg.appendChild(rectangle);
            //    //svg.path(item.path).attr(item.attr);
            //});
        }

        self.sizer.adjustVertically();

    };


    self.renderTrendlines = function () {
    };


    self.renderExtrema = function () {
        var svg = self.parent.ui.getExtremaSvg();
        var strokeWidth = STOCK.CONFIG.candle.strokeWidth;
        var circles = self.extremaPathMaker.getCircles(self.extrema);
        circles.forEach(function (item) {
            var circle = mielk.svg.createCircle(item.x, item.y, item.radius, item.fill, item.stroke);
            circle.style.strokeWidth = item.strokeWidth + 'px';
            circle.style.vectorEffect = 'non-scaling-stroke';
            circle.style.shapeRendering = 'crispEdges'
            svg.appendChild(circle);
        });


        //var rect = mielk.svg.createRectangle(100, 50, "blue");
        //var circle = mielk.svg.createCircle(10, 10, 20, 'blue', 'black');
        //svg.appendChild(rect);
        //svg.appendChild(circle);
        var x = 1;

    };

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

    self.positioner = (function () {

        //Helper methods.
        function getX(value) {
            var candlesFromFirst = value - self.dataInfo.startIndex;
            return value * self.params.candleWidth + self.params.candlePadding;
        }

        function getY(value) {
            var pointsDistance = self.dataInfo.max - value;
            return pointsDistance * self.params.oneUnitHeight;
        }

        function getItemForX(x) {
            return Math.min(Math.floor(x / self.params.candleWidth), self.dataInfo.counter - 1);
        }

        return {
            getX: getX,
            getY: getY,
            getItemForX: getItemForX
        };

    })();

    self.quotationsPathMaker = (function () {


        //Paths
        function getPaths(quotations) {
            var ascendingPaths = [];
            var descendingPaths = [];
            var shadowPaths = [];

            quotations.forEach(function (item) {
                var pathInfo = generateQuotationPaths(item.quotation);
                var arr = pathInfo.isAscending ? ascendingPaths : descendingPaths;
                arr.push(pathInfo.path);
                shadowPaths.push(pathInfo.shadowPath);
            });

            return getCombinedPaths(ascendingPaths, descendingPaths, shadowPaths);
        }

        function generateQuotationPaths(quotation) {
            var isAscending = (quotation.Close > quotation.Open);
            var bodyTop = self.positioner.getY(isAscending ? quotation.Close : quotation.Open);
            var bodyBottom = self.positioner.getY(isAscending ? quotation.Open : quotation.Close);
            var shadeTop = self.positioner.getY(quotation.High);
            var shadeBottom = self.positioner.getY(quotation.Low);
            var left = self.positioner.getX(quotation.DateIndex);
            var right = left + self.params.bodyWidth;
            var middle = left + (self.params.bodyWidth / 2);

            var path = 'M' + left + ',' + bodyBottom + 'L' + left + ',' + bodyTop + 'L' +
                       right + ',' + bodyTop + 'L' + right + ',' + bodyBottom + 'Z';
            var shadowPath = 'M' + middle + ',' + shadeTop + 'L' + middle + ',' + bodyTop + 'Z' +
                       'M' + middle + ',' + shadeBottom + 'L' + middle + ',' + bodyBottom + 'Z';

            return {
                isAscending: isAscending,
                path: path,
                shadowPath: shadowPath
            };

        }

        function getCombinedPaths(ascending, descending, shadows) {
            var result = [];
            result.push({
                path: ascending.join(''),
                attr: {
                    'id': 'ascending-candles-path',
                    'stroke': STOCK.CONFIG.candle.color.ascendingLine,
                    'strokeWidth': STOCK.CONFIG.candle.strokeWidth,
                    'fill': STOCK.CONFIG.candle.color.ascendingBody
                }
            });
            result.push({
                path: descending.join(''),
                attr: {
                    'id': 'descending-candles-path',
                    'stroke': STOCK.CONFIG.candle.color.descendingLine,
                    'strokeWidth': STOCK.CONFIG.candle.strokeWidth,
                    'fill': STOCK.CONFIG.candle.color.descendingBody
                }
            });
            result.push({
                path: shadows.join(''),
                attr: {
                    'id': 'shadows-path',
                    'stroke': STOCK.CONFIG.candle.color.shadow,
                    'strokeWidth': STOCK.CONFIG.candle.strokeWidth,
                    'fill': STOCK.CONFIG.candle.color.shadow
                }
            });

            return result;

        }


        //Rectangles
        function getRectangles(quotations) {
            var rectangles = [];
            quotations.forEach(function (item) {
                var result = generateQuotationRectangles(item.quotation);
                rectangles.push(result.body);
                rectangles.push(result.topShadow);
                rectangles.push(result.bottomShadow);
            });
        }

        function generateQuotationRectangles(quotation) {
            var isAscending = (quotation.Close > quotation.Open);
            var bodyTop = self.positioner.getY(isAscending ? quotation.Close : quotation.Open);
            var bodyBottom = self.positioner.getY(isAscending ? quotation.Open : quotation.Close);
            var shadeTop = self.positioner.getY(quotation.High);
            var shadeBottom = self.positioner.getY(quotation.Low);
            var left = self.positioner.getX(quotation.DateIndex);
            var right = left + self.params.bodyWidth;
            var middle = left + (self.params.bodyWidth / 2);

            var path = 'M' + left + ',' + bodyBottom + 'L' + left + ',' + bodyTop + 'L' +
                       right + ',' + bodyTop + 'L' + right + ',' + bodyBottom + 'Z';
            var shadowPath = 'M' + middle + ',' + shadeTop + 'L' + middle + ',' + bodyTop + 'Z' +
                       'M' + middle + ',' + shadeBottom + 'L' + middle + ',' + bodyBottom + 'Z';

            return {
                isAscending: isAscending,
                path: path,
                shadowPath: shadowPath
            };


            //return {
            //    body: {
            //        fill: (isAscending ? STOCK.CONFIG.candle.color.ascendingBody : STOCK.CONFIG.candle.color.descendingBody),
            //        stroke: (isAscending ? STOCK.CONFIG.candle.color.ascendingLine : STOCK.CONFIG.candle.color.descendingLine),
            //        height: (bodyBottom - bodyTop),
            //        width: (right - left),
            //        y: bodyTop,
            //        x: left
            //    },
            //    topShadow: {
            //        fill: (STOCK.CONFIG.candle.color.shadow),
            //        stroke: (STOCK.CONFIG.candle.color.shadow),
            //        height: (bodyTop - shadeTop),
            //        width: '1',
            //        y: shadeTop,
            //        x: middle
            //    },
            //    bottomShadow: {
            //        fill: (STOCK.CONFIG.candle.color.shadow),
            //        stroke: (STOCK.CONFIG.candle.color.shadow),
            //        height: (shadeBottom - bodyBottom),
            //        width: '1',
            //        y: bodyBottom,
            //        x: middle
            //    }
            //};

        }



        //Reverse engineering.
        function getItemsRange(left, right) {
            var firstItem = self.positioner.getItemForX(left);
            var lastItem = self.positioner.getItemForX(right);
            return {
                firstIndex: firstItem,
                lastIndex: lastItem
            }
        }

        function calculateVerticalAdjustments(partDataInfo, visibleHeight) {
            var modifiedMin = partDataInfo.min * 0.97;
            var modifiedMax = partDataInfo.max * 1.03;
            var levelDifference = Math.abs(modifiedMin - modifiedMax);
            var coefficient = self.dataInfo.levelDifference / levelDifference;

            var newHeight = Math.ceil(visibleHeight * coefficient);
            var newTop = Math.ceil((self.dataInfo.max - modifiedMax) * (newHeight / self.dataInfo.levelDifference));

            return {
                height: newHeight,
                top: Math.floor(-newTop)
            };
        }


        return {
              getPaths: getPaths
            , getRectangles: getRectangles
            , getItemsRange: getItemsRange
            , calculateVerticalAdjustments: calculateVerticalAdjustments
        };


    })();

    self.trendlinesPathMaker = (function () {

        //var trendlinePaths = [];
        //var trendlines = [];


        //function prepareTrendlinesSvgPaths() {
        //    trendlines.forEach(function (item) {
        //        var pathInfo = calculateTrendlinePath(item);
        //        trendlinePaths.push(pathInfo.path);
        //    });
        //}

        //function calculateTrendlinePath(item) {
        //    var startIndex = item.StartIndex - 3;
        //    var endIndex = (item.EndIndex > 0 ? item.EndIndex : quotations.length - 1) + 3;
        //    var startLevel = (startIndex - item.BaseStartIndex) * item.Slope + item.BaseLevel;
        //    var endLevel = (endIndex - item.BaseStartIndex) * item.Slope + item.BaseLevel;

        //    var startX = getX(startIndex);
        //    var startY = getY(startLevel);
        //    var endX = getX(endIndex);
        //    var endY = getY(endLevel);

        //    var path = 'M' + startX + ',' + startY + 'L' + endX + ',' + endY;
        //    return {
        //        type: 'trendline',
        //        path: path
        //    };

        //}

        //result.push({
        //    path: trendlinePaths.join(''),
        //    attr: {
        //        'stroke': 'black',
        //        'stroke-width': 2,
        //        'fill': STOCK.CONFIG.trendlines.color
        //    }
        //});



    })();

    self.extremaPathMaker = (function () {
        var extremaCircles = [];
        var distance = STOCK.CONFIG.peaks.distance;

        function getCircles(extrema) {
            extremaCircles = [];
            prepareExtremaSvgPaths(extrema);
            return extremaCircles;
        }

        function prepareExtremaSvgPaths(extrema) {
            extrema.forEach(function (item) {
                var circle = calculateExtremumCircle(item);
                extremaCircles.push(circle);
            });
        }

        function calculateExtremumCircle(item) {
            var price = item.price;
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

            var scale = Math.min(1, (100 - value) / 50);
            var greyscale = Math.ceil(255 * (1 - scale));
            var x = self.positioner.getX(item.DateIndex) + (self.params.bodyWidth / 2);
            var y = isMin ?
                        self.positioner.getY(item.quotation.Low) + distance + self.params.extremaSvgOffset / 2 :
                        self.positioner.getY(item.quotation.High) - distance + self.params.extremaSvgOffset / 2;

            return {
                item: item,
                x: x,
                y: y,
                radius: Math.max(Math.ceil(value - 50, 0)),
                stroke: 'rgb(' + greyscale + ',' + greyscale + ',' + greyscale + ')',
                fill: 'rgba(' + (isMin ? '255, 0' : '0, 255') + ', 0, ' + scale + ')'
            };

        }



        ////Add peak/through indicators.
        //function addExtremumLabel(extrema, isMin) {
        //    var dist = STOCK.CONFIG.peaks.distance;
        //    var extremum = isMin ?
        //        Math.max(item.troughByClose ? item.troughByClose.Value : 0, item.troughByLow ? item.troughByLow.Value : 0) :
        //        Math.max(item.peakByClose ? item.peakByClose.Value : 0, item.peakByHigh ? item.peakByHigh.Value : 0);
        //    if (!extremum) return;

        //    return true;

        //}


        //result.push({
        //    isCirclesSet: true,
        //    sets: extremaCircles
        //});

        return {
            getCircles: getCircles
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