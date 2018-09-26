function SvgPanel(params) {

    'use strict';

    //[Meta]
    var self = this;
    self.SvgPanel = true;
    self.parent = params.parent;
    self.type = params.type;
    self.timeframe = params.timeframe;
    self.key = params.key;
    self.index = params.index;
    self.candleWidth = params.candleWidth;
    self.isRendered = false;
    self.data = self.parent.data;

    self.render = function() {
        var chr = self.chartRenderer;
        chr.setDataInfo(self.parent.dataInfo);
        chr.setData(self.data)
        chr.renderQuotations();
        chr.renderExtrema();

        var dr = self.datesRenderer;
        dr.render(self.data);

        self.isRendered = true;

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
        var chartContainer = self.parent.ui.getChartContainer();
        var parentContainer = self.parent.ui.getSvgContainer();
        var svgCandles = null;
        var svgExtrema = null;
        var svgTrendlines = null;
        //[Dates]
        var datesContainer = self.parent.ui.getDatesContainer();
        var datesLabelsContainer = null;
        var datesVerticalSeparatorsContainer = null;


        //Candles container.
        var svgsContainer = $('<div/>', {
            'class': 'chart-svg-panel',
            id: self.key + '-svgs-container'
        }).css({
            'height': (self.baseSize.height + 100) + 'px',
            'width': self.baseSize.width + 'px',
            'left': 0,
            'top': 0,
            'visibility': 'hidden'
        }).appendTo(parentContainer)[0];



        function insertSvgQuotations() {
            var svg = mielk.svg.createSvg();
            var height = $(svgsContainer).height() - 100;
            
            svg.setAttribute('id', self.key + '-candles');
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

            svg.setAttribute('id', self.key + '-extrema');
            svg.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svg.setAttribute('preserveAspectRatio', 'none meet');
            svg.style.top = 0;
            svg.style.height = height + 'px';

            svgsContainer.appendChild(svg);
            return svg;

        }

        function insertSvgTrendlines() {
            var svg = mielk.svg.createSvg();
            var width = $(svgsContainer).width();
            var height = $(svgsContainer).height();

            svg.setAttribute('id', self.key + '-trendlines');
            svg.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svg.setAttribute('preserveAspectRatio', 'none meet');
            svg.style.top = 0;
            svg.style.height = height + 'px';

            svgsContainer.appendChild(svg);
            return svg;

        }


        //[Dates]
        function insertDatesLabelsContainer() {
            var div = $('<div/>', {
                'class': 'date-labels-container',
                id: self.key + '-date-labels-container'
            }).css({
                'visibility': 'visible'
            }).appendTo(datesContainer)[0];
            return div;
        }

        function insertDatesVerticalSeparatorsContainer() {
            var div = $('<div/>', {
                'class': 'date-vertical-lines-container',
                id: self.key + '-date-vertical-lines-container'
            }).css({
                'visibility': 'visible'
            }).appendTo(chartContainer)[0];
            return div;
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
                self.chartRenderer.updateSizeParams();
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
                if (svgTrendlines === null) {
                    svgTrendlines = insertSvgTrendlines();
                }
                return svgTrendlines;
            },
            getExtremaSvg: function () {
                if (svgExtrema === null) {
                    svgExtrema = insertSvgExtrema();
                }
                return svgExtrema;
            },
            getDatesLabelsContainer: function () {
                if (datesLabelsContainer === null) {
                    datesLabelsContainer = insertDatesLabelsContainer();
                }
                return datesLabelsContainer;
            },
            getDatesVerticalSeparatorContainer: function () {
                if (datesVerticalSeparatorsContainer === null) {
                    datesVerticalSeparatorsContainer = insertDatesVerticalSeparatorsContainer();
                }
                return datesVerticalSeparatorsContainer;
            },
            resize: resize,
            show: show,
            hide: hide
        };

    })();

    self.chartRenderer = self.type.svgRenderer({
        parent: self
    });

    self.datesRenderer = new DatesRenderer({
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

        function getTotalCandlesVisibleCounter() {
            var candleWidth = self.candleWidth;
            var visibleWidth = $(parentDiv).width();
            return Math.ceil(visibleWidth / candleWidth);
        }

        return {
            getPosition: getPosition,
            getCandlesSvgSize: getCandlesSvgSize,
            getExtremaSvgOffset: getExtremaSvgOffset,
            getTotalCandlesVisibleCounter: getTotalCandlesVisibleCounter
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
        var mode = 1;   //0 - paths | 1 - rectangles
        var svg = self.parent.ui.getCandlesSvg();

        if (mode === 0) {
            var strokeWidth = STOCK.CONFIG.candle.strokeWidth;
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
            var rectangles = self.quotationsPathMaker.getRectangles(self.quotations);
            rectangles.forEach(function (item) {
                var rectangle = mielk.svg.createRectangle(item.width, item.height, item.x, item.y, item. fill)
                rectangle.style.stroke = item.stroke;
                rectangle.style.strokeWidth = item.strokeWidth + 'px';
                rectangle.style.vectorEffect = 'non-scaling-stroke';
                rectangle.style.shapeRendering = 'crispEdges'
                svg.appendChild(rectangle);
            });
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

        function getQuotationCoordinate(quotation) {
            var isAscending = (quotation.Close > quotation.Open);
            var bodyTop = self.positioner.getY(isAscending ? quotation.Close : quotation.Open);
            var bodyBottom = self.positioner.getY(isAscending ? quotation.Open : quotation.Close);
            var shadeTop = self.positioner.getY(quotation.High);
            var shadeBottom = self.positioner.getY(quotation.Low);
            var left = self.positioner.getX(quotation.DateIndex);
            var right = left + self.params.bodyWidth;
            var middle = left + (self.params.bodyWidth / 2);

            return {
                isAscending: isAscending,
                bodyTop: bodyTop,
                bodyBottom: bodyBottom,
                shadeTop: shadeTop,
                shadeBottom: shadeBottom,
                left: left,
                right: right,
                middle: middle
            }

        }

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
            var c = getQuotationCoordinate(quotation);
            var left = Math.round(c.left);
            var right = Math.round(c.right);
            var middle = Math.round(c.middle);
            var bodyTop = Math.round(c.bodyTop);
            var bodyBottom = Math.round(c.bodyBottom);
            var shadeTop = Math.round(c.shadeTop);
            var shadeBottom = Math.round(c.shadeBottom);

            var path = 'M' + left + ',' + bodyBottom + 'L' + left + ',' + bodyTop + 'L' +
                       right + ',' + bodyTop + 'L' + right + ',' + bodyBottom + 'Z';
            var shadowPath = 'M' + middle + ',' + shadeTop + 'L' + middle + ',' + bodyTop + 'Z' +
                       'M' + middle + ',' + shadeBottom + 'L' + middle + ',' + bodyBottom + 'Z';

            //var path = 'M' + c.left + ',' + c.bodyBottom + 'L' + c.left + ',' + c.bodyTop + 'L' +
            //           c.right + ',' + c.bodyTop + 'L' + c.right + ',' + c.bodyBottom + 'Z';
            //var shadowPath = 'M' + c.middle + ',' + c.shadeTop + 'L' + c.middle + ',' + c.bodyTop + 'Z' +
            //           'M' + c.middle + ',' + c.shadeBottom + 'L' + c.middle + ',' + c.bodyBottom + 'Z';

            return {
                isAscending: c.isAscending,
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
                var result = generateQuotationRectangles(item);
                rectangles.push(result.shadow);
                rectangles.push(result.body);
            });
            return rectangles;
        }

        function generateQuotationRectangles(item) {
            var quotation = item.quotation;
            var c = getQuotationCoordinate(quotation);
            item.width = Math.round(c.right - c.left);
            item.x = Math.round(c.left);

            return {
                body: {
                    fill: (c.isAscending ? STOCK.CONFIG.candle.color.ascendingBody : STOCK.CONFIG.candle.color.descendingBody),
                    stroke: (c.isAscending ? STOCK.CONFIG.candle.color.ascendingLine : STOCK.CONFIG.candle.color.descendingLine),
                    strokeWidth: item.width < (STOCK.CONFIG.candle.strokeWidth * 2 + 1) ? 0 : STOCK.CONFIG.candle.strokeWidth,
                    height: Math.round(c.bodyBottom - c.bodyTop),
                    width: item.width + 1,
                    y: Math.round(c.bodyTop) + 0.5,
                    x: item.x + 0.5
                },
                shadow: {
                    fill: (STOCK.CONFIG.candle.color.shadow),
                    stroke: (STOCK.CONFIG.candle.color.shadow),
                    strokeWidth: 0,
                    height: Math.ceil(c.shadeBottom - c.shadeTop),
                    width: STOCK.CONFIG.candle.strokeWidth,
                    y: Math.round(c.shadeTop),
                    x: item.x + (item.width + 1) / 2
                }
            };

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

        return {
            getCircles: getCircles
        };

    })();

}
mielk.objects.extend(AbstractSvgRenderer, PriceSvgRenderer);



function DatesRenderer(params) {

    'use strict';

    var self = this;
    self.DatesRenderer = true;
    self.parent = params.parent;
    self.labelsContainer = self.parent.ui.getDatesLabelsContainer();
    self.verticalLinesContainer = self.parent.ui.getDatesVerticalSeparatorContainer();

    self.render = function (data) {

        //Assign values to inner variables.
        self.quotations = data.quotations;

        var visiblesOnScreen = self.parent.layout.getTotalCandlesVisibleCounter();
        var prevDate = null;
        var left = 0;


        //Loaded quotations.
        self.quotations.forEach(function (item) {
            var date = mielk.dates.fromCSharpDateTime(item.Date);
            var period = self.parent.timeframe.getPeriodLabelChange(prevDate, date);
            if (period.periodChanged) {
                self.addPeriodSeparator(item.x, period.periodLabel);
            }
            left = item.x;
            prevDate = date;
        });


        //Future dates.
        for (var i = 0; i < visiblesOnScreen; i++) {
            left = Math.ceil(left + self.parent.candleWidth);
            var date = self.parent.timeframe.next(prevDate);
            var period = self.parent.timeframe.getPeriodLabelChange(prevDate, date);
            if (period.periodChanged) {
                self.addPeriodSeparator(left, period.periodLabel);
            }
            prevDate = date;
        }


        //Resize labels container.
        $(self.labelsContainer).css('width', (left + 1) + 'px');


    };

    self.addPeriodSeparator = function (x, label) {
        var verticalLine = $('<div/>', {
            'class': 'date-vertical-line'
        }).css({
            'left': x + 'px'
        }).appendTo(self.verticalLinesContainer)[0];

        var ticker = $('<div/>', {
            'class': 'date-label-ticker'
        }).css({
            'left': x + 'px'
        }).appendTo(self.labelsContainer)[0];

        var label = $('<div/>', {
            'class': 'date-label',
            'html': label
        }).appendTo(self.labelsContainer)[0];
        var width = $(label).width();
        $(label).css('left', (x - width / 2) + 'px');

    }

}

















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