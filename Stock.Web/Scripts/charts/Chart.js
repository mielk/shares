﻿function Chart(parent, params) {

    'use strict';

    //[Meta].
    var self = this;
    self.Chart = true;
    self.parent = parent;


    self.params = (function () {
        var type = params.type;
        var timeframe = params.timeframe;
        var itemWidth = params.itemWidth;
        //var extremaLayer = params.

        //Setters
        /* Other properties should not be changed after being set during object initialization */


        //Getters
        function getType() {
            return type;
        }

        function getTimeframe() {
            return timeframe;
        }

        function getItemWidth(){
            return itemWidth;
        }


        return {
            getType: getType,
            getTimeframe: getTimeframe,
            getItemWidth: getItemWidth
        }

    })();


    self.data = (function () {
        var dataSetInfo;
        var items;
        var valuesRange;
        var trendlines;


        //[Setters]
        function setData($dataSetInfo, $items, $trendlines, $valuesRange) {
            dataSetInfo = $dataSetInfo;
            items = $items.slice(0);
            trendlines = $trendlines.slice(0);
            valuesRange = $valuesRange;
            setVisibilityRange();
            self.svg.render();
        }

        //function setTrendlines($trendlines) {
        //    var counter = 0;
        //    $trendlines.forEach(function (trendline) {
        //        trendlines[counter++] = {
        //            trendline: trendline
        //        };
        //    });
        //}

        function setVisibilityRange() {
            var offset = 0.04 * (valuesRange.min + valuesRange.max) / 2;
            var min = Math.floor(valuesRange.min - offset);
            var max = Math.floor(valuesRange.max + offset);
            self.ui.setVisibleRange(min, max);
        }


        //[Getters]
        function getDataSetInfo() {
            return dataSetInfo;
        }

        function getStartIndex() {
            return dataSetInfo.startIndex;
        }

        function getEndIndex() {
            return dataSetInfo.endIndex;
        }

        function getItems() {
            return items;
        }

        function getItem(index) {
            if (index > 0 && index < items.length) {
                return items[index];
            }
        }

        function getTrendlines() {
            return trendlines;
        }

        function getValuesRange() {
            return valuesRange;
        }

        function countNonEmptyItems() {
            return dataSetInfo.endIndex - dataSetInfo.startIndex + 1;
        }


        return {
            setData: setData,
            getDataSetInfo: getDataSetInfo,
            getStartIndex: getStartIndex,
            getEndIndex: getEndIndex,
            getItems: getItems,
            getItem: getItem,
            getValuesRange: getValuesRange,
            getTrendlines: getTrendlines,
            countNonEmptyItems: countNonEmptyItems
        }

    })();


    self.ui = (function () {
        var type = self.params.getType();
        var height = type.initialHeight;
        var parentContainer = self.parent.ui.getChartsContainer();
        var mainContainer;
        var chartContainer;
        var horizontalGridLinesContainer;
        var valuesContainer;
        var valuesLabelsContainer;
        //[SVG components]
        var svgItems;
        var svgExtrema;
        var svgTrendlines;
        var svgBoxHeight = 1000;
        var svgItemsBoxOffset = 100;
        var svgItemsBoxTop = 0;
        var svgLeft = 0;
        var svgTrendlinesRightExtention = 200;
        //[Visibility]
        var visibleRange = {};
        //------------------------------------------------------------------

        //[Actual render functions]
        (function insertRequiredHtmlComponents() {

            mainContainer = $('<div/>', {
                'class': 'chart-and-values-container'
            }).css({
                'height': height + 'px'
            }).appendTo(parentContainer)[0];

            chartContainer = $('<div/>', {
                'class': 'chart-container'
            }).appendTo(mainContainer)[0];
            visibleRange.height = $(chartContainer).height();

            horizontalGridLinesContainer = $('<div/>', {
                'class': 'horizontal-grid-lines'
            }).appendTo(chartContainer)[0];

            valuesContainer = $('<div/>', {
                'class': 'values-container'
            }).appendTo(mainContainer)[0];

            valuesLabelsContainer = $('<div/>', {
                'class': 'value-labels-container'
            }).appendTo(valuesContainer)[0];

        })();


        //Rendering SVG containers.
        function insertSvgItems() {
            svgItems = mielk.svg.createSvg();
            var height = svgBoxHeight;
            var width = calculateItemsWidth();

            svgItems.setAttribute('preserveAspectRatio', 'none meet');
            svgItems.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgItems.style.height = height + 'px';
            svgItems.style.top = '0px';
            svgItems.style.width = width + 'px';
            svgItems.style.left = svgLeft + 'px';
            chartContainer.appendChild(svgItems);

        }

        function insertSvgExtrema() {
            svgExtrema = mielk.svg.createSvg();
            var height = svgBoxHeight + svgItemsBoxOffset;
            var width = calculateItemsWidth();
            var top = (svgItemsBoxTop - svgItemsBoxOffset / 2);

            svgExtrema.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgExtrema.setAttribute('preserveAspectRatio', 'none meet');
            svgExtrema.style.height = height + 'px';
            svgExtrema.style.top = top + 'px';
            svgExtrema.style.width = width + 'px';
            svgExtrema.style.left = svgLeft + 'px';

            chartContainer.appendChild(svgExtrema);

        }

        function insertSvgTrendlines() {
            svgTrendlines = mielk.svg.createSvg();
            var height = svgBoxHeight + svgItemsBoxOffset;
            var width = calculateItemsWidth() + svgTrendlinesRightExtention;
            var top = (svgItemsBoxTop - svgItemsBoxOffset / 2);

            svgTrendlines.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgTrendlines.setAttribute('preserveAspectRatio', 'none meet');
            svgTrendlines.style.height = height + 'px';
            svgTrendlines.style.top = top + 'px';
            svgTrendlines.style.width = width + 'px';
            svgTrendlines.style.left = svgLeft + 'px';

            chartContainer.appendChild(svgTrendlines);

        }


        //Adjusting SGV containers heights.
        function setSvgHeight(height, top) {
            svgBoxHeight = height;
            var width = calculateItemsWidth();

            //[Items]
            svgItems.setAttribute('viewBox', '0 0 ' + width + ' ' + svgBoxHeight);
            svgItems.style.height = svgBoxHeight + 'px';
            if (top !== undefined) {
                svgItemsBoxTop = top
                svgItems.style.top = top + 'px';
            }

            //[Extrema]
            if (svgExtrema) {
                svgExtrema.setAttribute('viewBox', '0 0 ' + width + ' ' + (svgBoxHeight + svgItemsBoxOffset));
                svgExtrema.style.height = (svgBoxHeight + svgItemsBoxOffset) + 'px';
                if (top !== undefined) svgExtrema.style.top = (top - svgItemsBoxOffset / 2) + 'px';
            }

            //[Trendlines]
            if (svgTrendlines) {
                svgTrendlines.setAttribute('viewBox', '0 0 ' + width + ' ' + (svgBoxHeight + svgItemsBoxOffset));
                svgTrendlines.style.height = (svgBoxHeight + svgItemsBoxOffset) + 'px';
                if (top !== undefined) svgTrendlines.style.top = (top - svgItemsBoxOffset / 2) + 'px';
            }

            
        }


        //Helper functions.
        function calculateItemsWidth() {
            var nonEmptyItemsCounter = self.data.countNonEmptyItems();
            var itemWidth = Math.ceil(self.params.getItemWidth());
            return nonEmptyItemsCounter * itemWidth;
        }


        //Access to SVG panels and properties.
        function setVisibleRange(min, max) {
            visibleRange.min = min;
            visibleRange.max = max;
        }

        function getVisibleRange() {
            return visibleRange;
        }

        function getSvgBoxHeight() {
            return svgBoxHeight;
        }

        function getSvgItemsBoxTopOffset() {
            return svgItemsBoxOffset;
        }

        function getItemsSvg() {
            if (svgItems === undefined) insertSvgItems();
            return svgItems;
        }

        function getExtremaSvg() {
            if (svgExtrema === undefined) insertSvgExtrema();
            return svgExtrema;
        }

        function getTrendlinesSvg() {
            if (svgTrendlines === undefined) insertSvgTrendlines();
            return svgTrendlines;
        }

        function getSvgLeftOffset() {
            return $(svgItems).offset().left;
        }


        //Access to other HTML components.
        function getValueLabelsContainer() {
            return valuesLabelsContainer;
        }

        function getHorizontalGridLinesContainer() {
            return horizontalGridLinesContainer;
        }

        function getChartContainer() {
            return chartContainer;
        }




        //[EVENTS]
        (function bindEvents() {
            self.parent.bind({
                horizontalSlide: function (e) {
                    svgLeft = e.left;
                    if (svgItems) svgItems.style.left = svgLeft + 'px';
                    if (svgExtrema) svgExtrema.style.left = svgLeft + 'px';
                    if (svgTrendlines) svgTrendlines.style.left = svgLeft + 'px';
                }
            });
        })();




        return {
            setSvgHeight: setSvgHeight,
            setVisibleRange: setVisibleRange,
            getSvgLeftOffset: getSvgLeftOffset,
            getSvgItemsBoxTopOffset: getSvgItemsBoxTopOffset,
            getSvgBoxHeight: getSvgBoxHeight,
            getItemsSvg: getItemsSvg,
            getExtremaSvg: getExtremaSvg,
            getTrendlinesSvg: getTrendlinesSvg,
            getVisibleRange: getVisibleRange,
            getHorizontalGridLinesContainer: getHorizontalGridLinesContainer,
            getValueLabelsContainer: getValueLabelsContainer,
            getChartContainer: getChartContainer
        }

    })();


    self.svg = (function () {
        var type = self.params.getType();
        var svgBoxHeight = self.ui.getSvgBoxHeight();
        var itemsSvgOffset = self.ui.getSvgItemsBoxTopOffset();
        var unitHeight;
        var visibleRange = {};
        

        //[RENDERING]
        function render() {
            if (type === STOCK.INDICATORS.PRICE) {
                renderPrices();
            } else if (type === STOCK.INDICATORS.ADX) {
                renderAdx();
            } else if (type === STOCK.INDICATORS.MACD) {
                renderMacd();
            }
        }


        //[PRICES]
        function renderPrices() {
            renderItems();
            renderPricesExtrema();
            renderPriceTrendlines();
            self.valuesPanel.renderValuesAndHorizontalGridLines(unitHeight);
        }

        function renderItems() {
            var svg = self.ui.getItemsSvg();
            var valuesRange = self.data.getValuesRange();
            var visibleRange = self.ui.getVisibleRange();
            var items = self.data.getItems();

            function getY(value) {
                var pointsDistance = valuesRange.max - value;
                return pointsDistance * unitHeight;
            }

            (function adjustSvgHeight() {
                var svgHeight = Math.ceil(visibleRange.height * (valuesRange.max - valuesRange.min) / (visibleRange.max - visibleRange.min));
                var svgTop = visibleRange.height * (visibleRange.max - valuesRange.max) / (visibleRange.max - visibleRange.min);
                self.ui.setSvgHeight(svgHeight, svgTop);
                svgBoxHeight = self.ui.getSvgBoxHeight();
            })();

            (function calculateUnitHeight() {
                var levelsDistance = valuesRange.max - valuesRange.min;
                unitHeight = svgBoxHeight / levelsDistance;
            })();

            (function appendVerticalCoordinates() {
                items.forEach(function (item) {
                    if (item.item) {
                        var quotation = item.item.price.quotation;
                        item.coordinates.shadowTop = Math.round(getY(quotation.high));
                        item.coordinates.bodyTop = Math.round(getY(quotation.priceUp ? quotation.close : quotation.open));
                        item.coordinates.bodyBottom = Math.round(getY(quotation.priceUp ? quotation.open : quotation.close));
                        item.coordinates.shadowBottom = Math.round(getY(quotation.low));
                        item.coordinates.bodyHeight = item.coordinates.bodyBottom - item.coordinates.bodyTop;
                        item.coordinates.shadowHeight = item.coordinates.shadowBottom - item.coordinates.shadowTop;;
                    }
                });
            })();

            (function generateAndInsertSvgRectangles() {
                var priceUpBodyInsideColor = STOCK.CONFIG.candle.color.ascendingBody;
                var priceUpBodyBorderColor = STOCK.CONFIG.candle.color.ascendingLine
                var priceDownBodyInsideColor = STOCK.CONFIG.candle.color.descendingBody;
                var priceDownBodyBorderColor = STOCK.CONFIG.candle.color.descendingLine;
                var shadowColor = STOCK.CONFIG.candle.color.shadow;
                var strokeWidth = STOCK.CONFIG.candle.strokeWidth;
                var rectangles = [];

                items.forEach(function (item) {
                    if (item.item) {
                        var quotation = item.item.price.quotation;
                        var hasBody = item.coordinates.bodyHeight >= 1;
                        var body = {
                            type: 'body',
                            item: item,
                            fill: (hasBody ? (quotation.priceUp ? priceUpBodyInsideColor : priceDownBodyInsideColor) : shadowColor),
                            stroke: (quotation.priceUp ? priceUpBodyBorderColor : priceDownBodyBorderColor),
                            strokeWidth: (hasBody ? (item.coordinates.width < (strokeWidth * 2 + 1) ? 0 : strokeWidth) : 0),
                            height: (item.coordinates.bodyHeight < 1 ? 1 : item.coordinates.bodyHeight),
                            width: (hasBody ? item.coordinates.width + 1 : item.coordinates.width + 2),
                            y: item.coordinates.bodyTop + 0.5,
                            x: (hasBody ? item.coordinates.left + 0.5 : item.coordinates.left - 0.5)
                        };
                        var shadow = {
                            type: 'shadow',
                            item: item,
                            fill: shadowColor,
                            stroke: shadowColor,
                            strokeWidth: 0,
                            height: item.coordinates.shadowHeight,
                            width: strokeWidth,
                            y: item.coordinates.shadowTop,
                            x: item.coordinates.left + (item.coordinates.width + 1) / 2
                        };
                        rectangles.push(shadow);
                        rectangles.push(body);
                    }
                });

                function appendSvgComponentToItem(component, params) {
                    var item = params.item;
                    var type = params.type;
                    
                    if (item.svg === undefined) item.svg = {};
                    if (type === 'body') {
                        item.svg.body = component;
                    } else if (type === 'shadow') {
                        item.svg.shadow = component;
                    }

                }

                $(svg).empty();
                rectangles.forEach(function (item) {
                    var rectangle = mielk.svg.createRectangle(item.width, item.height, item.x, item.y, item.fill)
                    rectangle.style.stroke = item.stroke;
                    rectangle.style.strokeWidth = item.strokeWidth + 'px';
                    rectangle.style.vectorEffect = 'non-scaling-stroke';
                    rectangle.style.shapeRendering = 'crispEdges'
                    svg.appendChild(rectangle);
                    appendSvgComponentToItem(rectangle, item);
                });

            })();

        }

        function renderPricesExtrema() {
            var svg = self.ui.getExtremaSvg();
            var items = self.data.getItems();
            var valuesRange = self.data.getValuesRange();
            var distanceFromExtremum = STOCK.CONFIG.peaks.distance;

            (function generateAndInsertSvgCircles() {
                var circles = [];

                function generateCircleObject(value, isMin, item) {
                    var scale = Math.max(Math.min((value - 50) / 50, 1), 0);
                    var greyscale = Math.ceil(255 * (1 - scale));
                    var x = item.coordinates.left + (item.coordinates.width + 1) / 2;
                    var y = isMin ? item.coordinates.shadowBottom + distanceFromExtremum + itemsSvgOffset / 2 : item.coordinates.shadowTop - distanceFromExtremum + itemsSvgOffset / 2;
                    var radius = Math.max(Math.ceil((value - 50) / 2, 0));
                    var fill = 'rgba(' + (isMin ? '255, 0' : '0, 255') + ', 0, ' + scale + ')';
                    var stroke = 'rgb(' + greyscale + ',' + greyscale + ',' + greyscale + ')';
                    return {
                        item: item,
                        isPeak: !isMin,
                        x: x,
                        y: y,
                        radius: radius,
                        stroke: stroke,
                        fill: fill
                    }
                }

                function appendSvgComponentToItem(component, params) {
                    var item = params.item;
                    if (item.svg === undefined) item.svg = {};
                    if (item.svg.extrema === undefined) item.svg.extrema = {};
                    if (params.isPeak) {
                        item.svg.extrema.peak = component;
                    } else {
                        item.svg.extrema.trough = component;
                    }

                }

                items.forEach(function (item) {
                    if (item.item) {
                        var price = item.item.price;
                        if (price.hasPeak()) {
                            var value = Math.max(price.peakByClose ? price.peakByClose.value : 0, price.peakByHigh ? price.peakByHigh.value : 0);
                            var circle = generateCircleObject(value, false, item);
                            if (circle) circles.push(circle);
                        }

                        if (price.hasTrough()) {
                            var value = Math.max(price.troughByClose ? price.troughByClose.value : 0, price.troughByLow ? price.troughByLow.value : 0);
                            var circle = generateCircleObject(value, true, item);
                            if (circle) circles.push(circle);
                        }
                    }
                });

                $(svg).empty();
                circles.forEach(function (item) {
                    var circle = mielk.svg.createCircle(item.x, item.y, item.radius, item.fill, item.stroke);
                    circle.style.strokeWidth = item.strokeWidth + 'px';
                    circle.style.vectorEffect = 'non-scaling-stroke';
                    circle.style.shapeRendering = 'crispEdges'
                    svg.appendChild(circle);
                    appendSvgComponentToItem(circle, item);
                });

            })();

        }

        function renderPriceTrendlines() {
            var svg = self.ui.getTrendlinesSvg();
            var trendlines = self.data.getTrendlines();
            var valuesRange = self.data.getValuesRange();

            function getY(value) {
                var pointsDistance = valuesRange.max - value;
                return pointsDistance * unitHeight;
            }

            (function appendCoordinates() {
                trendlines.forEach(function (item) {
                    var viewSlope = 0;
                    var linkedItems = {
                        start: self.data.getItem(item.footholds.start),
                        base: self.data.getItem(item.trendline.edgePoints.base.index),
                        counter: self.data.getItem(item.trendline.edgePoints.counter.index),
                        end: self.data.getItem(item.footholds.end)
                    };
                    var edgePointsCoordinates = {
                        base: {
                            x: linkedItems.base.coordinates.middle,
                            y: Math.round(getY(item.trendline.edgePoints.base.level)) + itemsSvgOffset / 2
                        },
                        counter: {
                            x: linkedItems.counter.coordinates.middle,
                            y: Math.round(getY(item.trendline.edgePoints.counter.level)) + itemsSvgOffset / 2
                        }
                    };
                    viewSlope = (edgePointsCoordinates.counter.y - edgePointsCoordinates.base.y) / (edgePointsCoordinates.counter.x - edgePointsCoordinates.base.x);


                    function calculateCoordinatesForPoint(x) {
                        return {
                            x: x,
                            y: Math.round(edgePointsCoordinates.base.y + (x - edgePointsCoordinates.base.x) * viewSlope)
                        }
                        return 
                    }

                    item.coordinates = {
                        start: calculateCoordinatesForPoint(linkedItems.start.coordinates.middle),
                        end: calculateCoordinatesForPoint(linkedItems.end.coordinates.middle)
                    }

                });
            })();

            (function generateAndInsertSvgPaths() {
                var paths = [];
                var strokeWidth = STOCK.CONFIG.trendlines.width;
                var stroke = STOCK.CONFIG.trendlines.color;

                trendlines.forEach(function (item) {
                    var path = {
                        d: 'M ' + item.coordinates.start.x + ' ' + item.coordinates.start.y + 'L' + item.coordinates.end.x + ' ' + item.coordinates.end.y,
                        //stroke: 'rgba(0, 0, 0, ' + item.trendline.value / 100 + ')',
                        stroke: 'rgba(0, 0, 0, 0.5)',
                        strokeWidth: strokeWidth,
                        trendline: item.trendline
                    };
                    paths.push(path);
                });

                function appendSvgComponentToItem(component, item) {
                    item.svgPath = component;
                }

                $(svg).empty();
                paths.forEach(function (item) {
                    var path = mielk.svg.createPath(item.d);
                    path.style.strokeWidth = item.strokeWidth + 'px';
                    path.style.stroke = item.stroke;
                    path.style.vectorEffect = 'non-scaling-stroke';
                    path.style.shapeRendering = 'crispEdges'
                    svg.appendChild(path);
                    appendSvgComponentToItem(path, item.trendline);
                });

            })();

        }


        //[ADX]
        function renderAdx() {
        }


        //[MACD]
        function renderMacd() {

        }


        return {
            render: render
        }

    })();


    self.valuesPanel = (function(){
        var horizontalLinesContainer = self.ui.getHorizontalGridLinesContainer();
        var valueLabelsContainer = self.ui.getValueLabelsContainer();
        var crossHair;
        var currentValueLabel;

        function renderValuesAndHorizontalGridLines(unitHeight) {
            var valuesRange = self.data.getValuesRange();
            var visibleRange = self.ui.getVisibleRange();

            function calculateGridLinesStep() {
                var factors = [1, 2, 5];
                var baseDistance = (visibleRange.max - visibleRange.min) / 10;
                var log = Math.log(baseDistance) / Math.LN10;
                
                var possibleSteps = [];
                factors.forEach(function (value) {
                    possibleSteps.push(Math.pow(10, Math.floor(log)) * value);
                });
                possibleSteps.push(Math.pow(10, Math.ceil(log)));

                var minDistance;
                var result;
                possibleSteps.forEach(function (value) {
                    var distance = Math.abs(value - baseDistance);
                    if (minDistance === undefined || minDistance > distance) {
                        minDistance = distance;
                        result = value;
                    }
                });

                return result;

            }

            function generateDisplayedValuesArray() {
                var addSpaceRatio = 0.1;
                var step = calculateGridLinesStep();
                var min = Math.min(valuesRange.min, visibleRange.min);
                var max = Math.max(valuesRange.max, visibleRange.max);
                var spaceOffset = addSpaceRatio * ((max + min) / 2);
                min -= spaceOffset;
                max += spaceOffset;

                var arr = [];
                var remainder = min % step;
                var value = min - remainder + (remainder < 0 ? 0 : step);
                while (value <= max) {
                    arr.push(value);
                    value += step;
                }

                return arr;

            }
            
            (function insertHtmlComponents() {
                var arr = generateDisplayedValuesArray();
                var topValue = arr[arr.length - 1];
                var y;

                for (var i = arr.length - 1; i >= 0; i--) {
                    y = (topValue - arr[i]) * unitHeight;

                    var horizontalLine = $('<div/>', {
                        'class': 'value-horizontal-line'
                    }).css({
                        'top': y + 'px'
                    }).appendTo(horizontalLinesContainer)[0];

                    var ticker = $('<div/>', {
                        'class': 'value-label-ticker'
                    }).css({
                        'top': y + 'px'
                    }).appendTo(valueLabelsContainer)[0];

                    var label = $('<div/>', {
                        'class': 'value-label',
                        'html': arr[i].toFixed(4)
                    }).appendTo(valueLabelsContainer)[0];

                    var height = $(label).height();
                    $(label).css('top', (y - height / 2) + 'px');

                }

                var top = (visibleRange.max - topValue) * unitHeight;
                $(horizontalLinesContainer).css({
                    height: y + 'px',
                    top: top + 'px'
                });

                $(valueLabelsContainer).css({
                    height: y + 'px',
                    top: top + 'px'
                });

            })();

            (function insertCrossHair() {
                crossHair = $('<div/>', {
                    'class': 'crosshair-horizontal-line'
                }).appendTo(horizontalLinesContainer)[0];
            })();

            (function insertCurrentDateLabel() {
                currentValueLabel = $('<div/>', {
                    'class': 'current-value-label'
                }).css({
                    visibility: 'hidden'
                }).appendTo(valueLabelsContainer)[0];
            })();

        }

        function updateCurrentValueIndicators(y, value){
            var top = y - $(horizontalLinesContainer).offset().top;
            var labelTop = Math.max(0, top - $(currentValueLabel).height() / 2);
            var caption = value.toFixed(4);

            $(crossHair).css({
                top: top + 'px',
                visibility: 'visible'
            });
            $(currentValueLabel).html(caption).css({
                visibility: 'visible',
                top: labelTop + 'px'
            });
        }

        function hideCurrentValueIndicators() {
            $(crossHair).css({
                visibility: 'hidden'
            });
            $(currentValueLabel).css({
                visibility: 'hidden'
            });
        }


        return {
            renderValuesAndHorizontalGridLines: renderValuesAndHorizontalGridLines,
            updateCurrentValueIndicators: updateCurrentValueIndicators,
            hideCurrentValueIndicators: hideCurrentValueIndicators
        }

    })();


    self.events = (function () {
        var topOffset = self.ui.getSvgItemsBoxTopOffset();
        var eventsLayer;
        var highlightedExtremumCircle;
        var moveParams;
        var activeOutterPanels = [];
        
        
        //[HELPER METHODS]
        function anyPanelsActive() {
            return activeOutterPanels.length > 0;
        }


        //[ACTIONS]
        function findCoordinates(e) {
            var x = e.pageX - self.ui.getSvgLeftOffset();
            var y = e.pageY - $(eventsLayer).offset().top;
            var item = findItemByX(x);
            var value = getValueByY(y);

            return {
                item: item,
                value: value
            };

        }

        function findItemByX(x) {
            var width = self.params.getItemWidth();
            var items = self.data.getItems();
            var startIndex = self.data.getStartIndex();
            var endIndex = items.length - 1;
            var i = Math.floor(x / width) + startIndex;

            if (i > endIndex) {
                return items[endIndex];
            } else {
                var item = items[i];
                if (item.coordinates.left > x) {
                    while (items[i].coordinates.right > x) {
                        if (--i < startIndex) {
                            return item;
                        }
                        item = items[i];
                    }
                } else if (item.coordinates.right < x) {
                    while (items[++i].coordinates.left < x) {
                        item = items[i];
                        if (i >= items.length) {
                            return item;
                        }
                    }
                }
                return item;
            }

        }

        function getValueByY(y) {
            var visibleRange = self.ui.getVisibleRange();
            var pixelUnits = (visibleRange.max - visibleRange.min) / visibleRange.height;
            return visibleRange.max - (pixelUnits * y);
        }

        function showInfo(e) {
            var res = findCoordinates(e);
            setTimeout(self.parent.dates.updateCurrentDateIndicators(e.pageX, res.item.date, res.item.index), 0);
            setTimeout(self.valuesPanel.updateCurrentValueIndicators(e.pageY, res.value), 0);
            setTimeout(self.legend.updateLegend(res.item), 0);
        }



        //[Sliding chart]
        function resetMoveParamsObject() {
            moveParams = {
                state: false,
                x: null,
                y: null
            };
        }

        function slide(e) {
            if (moveParams && moveParams.state) {
                var offsetX = (e.clientX - moveParams.x);
                moveParams.x = e.clientX;
                var offsetY = (e.clientY - moveParams.y);
                moveParams.y = e.clientY;
                setTimeout(self.parent.events.triggerHorizontalSlideEvent(offsetX), 0);

                //self.trigger({
                //    type: 'verticalSlide',
                //    y: offsetY
                //});
            }
        }

        function startMoveMode(e) {
            resetMoveParamsObject();
            moveParams.state = true;
            moveParams.x = e.clientX;
            moveParams.y = e.clientY;
        }

        function endMoveMode(e) {
            resetMoveParamsObject();
            showInfo(e);
        }


        //[Extrema]
        function isPointWithinCircle(circle, x, y) {
            if (circle) {
                var circleX = circle.cx.baseVal.value;
                var circleY = circle.cy.baseVal.value;// + topOffset / 2;
                var radius = circle.r.baseVal.value;
                var deltaX = Math.abs(x - circleX);
                var deltaY = Math.abs(y - circleY);
                var distance = Math.sqrt(Math.pow(deltaX, 2) + Math.pow(deltaY, 2));
                return distance <= radius;
            }
        }

        function handleExtremumDetailsClick(e) {

            if (highlightedExtremumCircle) return;

            var x = e.pageX - self.ui.getSvgLeftOffset();
            var y = e.pageY - $(self.ui.getExtremaSvg()).offset().top;
            var items = self.data.getItems();
            var clickedItem = findItemByX(x);
            var index = clickedItem.index;

            function displayExtremumInfo(params) {

                function getBetterExtremum(items) {
                    var better;
                    var value;
                    items.forEach(function (item) {
                        if (item) {
                            if (value === undefined || item.value > value) {
                                value = item.value;
                                better = item;
                            }
                        }
                    });
                    return better;
                }

                if (params) {
                    var svgLayer = self.ui.getExtremaSvg();
                    var svgTop = mielk.ui.getComponentCssOffset(svgLayer, 'top');
                    var svgLeft = mielk.ui.getComponentCssOffset(svgLayer, 'left');
                    var circle = params.circle;

                    highlightedExtremumCircle = {
                        circle: circle,
                        strokeWidth: circle.style.strokeWidth,
                        stroke: circle.style.stroke,
                        fill: circle.style.fill
                    };

                    circle.style.strokeWidth = '2px';
                    circle.style.stroke = 'black';
                    circle.style.fill = (params.isPeak ? 'rgba(0, 255, 0, 1)' : 'rgba(255, 0, 0, 1)');

                    var circleLeft = circle.cx.baseVal.value - circle.r.baseVal.value + svgLeft;
                    var circleRight = circle.cx.baseVal.value + circle.r.baseVal.value + svgLeft;
                    var circleTop = circle.cy.baseVal.value - circle.r.baseVal.value + svgTop;
                    var circleBottom = circleTop + 2 * circle.r.baseVal.value;
                    var betterExtremum = getBetterExtremum(params.extrema);
                    self.legend.updateExtremumDetailsPanel(betterExtremum, circleLeft, circleRight, circleTop, circleBottom);
                }

            }

            function findClickedCircle() {
                for (var i = index - 10; i <= index + 10; i++) {
                    var item = items[i];
                    if (item) {
                        var price = (item.item ? item.item.price : null);
                        var svgExtrema = (item.svg ? item.svg.extrema : null);
                        if (svgExtrema && price) {
                            if (isPointWithinCircle(svgExtrema.peak, x, y)) {
                                return {
                                    circle: svgExtrema.peak,
                                    isPeak: true,
                                    extrema: [price.peakByClose, price.peakByHigh]
                                }
                            } else if (isPointWithinCircle(svgExtrema.trough, x, y)) {
                                return {
                                    circle: svgExtrema.trough,
                                    isPeak: false,
                                    extrema: [price.troughByClose, price.troughByLow]
                                }
                            }
                        }
                    }
                }
            }

            displayExtremumInfo(findClickedCircle());

        }

        function checkIfExtremumDetailsLeft(e) {
            if (highlightedExtremumCircle) {
                var x = e.pageX - self.ui.getSvgLeftOffset();
                var y = e.pageY - $(eventsLayer).offset().top;
                var circle = highlightedExtremumCircle.circle;
                if (!isPointWithinCircle(circle, x, y)) {
                    circle.style.strokeWidth = highlightedExtremumCircle.strokeWidth;
                    circle.style.stroke = highlightedExtremumCircle.stroke;
                    circle.style.fill = highlightedExtremumCircle.fill;
                    highlightedExtremumCircle = undefined;
                    self.legend.hideExtremumDetailsPanel();
                }
            }
        }



        //[EVENT HANDLERS]
        function handleLeavingChartPanel() {
            self.parent.dates.hideCurrentDateIndicators();
            self.valuesPanel.hideCurrentValueIndicators();
            resetMoveParamsObject();
        }

        function handleMouseDown(e) {
            if (anyPanelsActive()) return;
            if (e.buttons === 1) {
                setTimeout(handleLeftButtonMouseDown(e), 0);
            } else if (e.buttons === 2) {
                setTimeout(handleRightButtonMouseDown(e), 0);
            }
        }

        function handleLeftButtonMouseDown(e) {
            setTimeout(handleExtremumDetailsClick(e), 0);
            setTimeout(startMoveMode(e), 0);
        }

        function handleRightButtonMouseDown(e) {
            setTimeout(startMoveMode(e), 0);
        }



        function handleMouseUp(e) {
            if (e.which === 1) {
                setTimeout(handleLeftButtonMouseUp(e), 0);
            } else if (e.which === 2) {
                setTimeout(handleRightButtonMouseUp(e), 0);
            }
        }

        function handleLeftButtonMouseUp(e) {
            setTimeout(endMoveMode(e), 0);
        }

        function handleRightButtonMouseUp(e) {
            setTimeout(endMoveMode(e), 0);
        }



        function handleMouseMove(e) {
            if (anyPanelsActive()) return;
            if (e.buttons === 0) {
                handleNoButtonMouseMove(e);
            } else if (e.buttons === 1) {
                handleLeftButtonMouseMove(e);
            } else if (e.buttons === 2) {
                handleRightButtonMouseMove(e);
            }
        }

        function handleNoButtonMouseMove(e) {
            setTimeout(showInfo(e), 0);
            setTimeout(checkIfExtremumDetailsLeft(e), 0);
        }

        function handleLeftButtonMouseMove(e) {
            setTimeout(slide(e), 0);
            setTimeout(showInfo(e), 0);
        }

        function handleRightButtonMouseMove(e) {
            setTimeout(slide(e), 0);
        }



        (function insertEventsLayer() {
            var chartContainer = self.ui.getChartContainer();
            eventsLayer = $('<div/>', {
                'class': 'events-layer'
            }).appendTo(chartContainer)[0];
        })();

        (function bindEvents() {

            self.bind({
                panelMoveModeStart: function (e) {
                    activeOutterPanels.push(e.name);
                },
                panelMoveModeEnd: function (e) {
                    var tag = e.name;
                    for (var i = activeOutterPanels.length - 1; i >= 0; i--) {
                        if (activeOutterPanels[i] === tag) {
                            activeOutterPanels.splice(i, 1);
                        }
                    }
                }
            });

            $(eventsLayer).bind({
                contextmenu: function (e) {
                    e.preventDefault();
                },
                mousedown: function (e) {
                    setTimeout(handleMouseDown(e), 0);
                },
                mouseup: function (e) {
                    e.preventDefault();
                    setTimeout(handleMouseUp(e), 0);
                },
                mousemove: function (e) {
                    setTimeout(handleMouseMove(e), 0);
                },
                mouseleave: function (e) {
                    setTimeout(handleLeavingChartPanel(), 0);
                }
            });
        })();

    })();


    self.legend = (function () {
        var legendContainer;
        var legendItems = {};
        var extremumDetailsInfoPanel;
        var extremumDetailItems = [];
        var trendlinePanelButton;

        (function insertLegendComponents() {
            var parentContainer = self.ui.getChartContainer();
            legendContainer = $('<div/>', {
                'class': 'legend-container'
            }).appendTo(parentContainer)[0];

            (function insertExtremumDetailsInfoPanel() {
                extremumDetailsInfoPanel = $('<div/>', {
                    'class': 'extremum-details-panel'
                }).css({
                    visibility: 'hidden'
                }).appendTo(parentContainer)[0];

                var extremumDetailItem = function ($caption, $fn, $decimalPlaces, $fontBold) {
                    var caption = $caption;
                    var fn = $fn;
                    var decimalPlaces = ($decimalPlaces === undefined ? 2 : $decimalPlaces);
                    var fontBold = $fontBold || false;
                    var mainSpan;
                    var propertySpan;
                    var valueSpan;

                    (function render() {
                        mainSpan = $('<span/>', {
                            'class': 'extremum-detail-item'
                        }).appendTo(extremumDetailsInfoPanel)[0];

                        propertySpan = $('<span/>', {
                            'class': 'extremum-detail-label',
                            html: $caption
                        }).css({
                            'font-weight': fontBold ? 'bold' : 'normal'
                        }).appendTo(mainSpan)[0];

                        valueSpan = $('<span/>', {
                            'class': 'extremum-detail-value'
                        }).css({
                            'font-weight': fontBold ? 'bold' : 'normal'
                        }).appendTo(mainSpan)[0];

                    })();

                    function updateValue(item) {
                        var value = fn(item);
                        var caption;
                        if (value === true) {
                            caption = 'TRUE';
                        } else if (value === false) {
                            caption = 'FALSE';
                        } else if (value === undefined || value === null) {
                            caption = '';
                        } else {
                            caption = value.toFixed(decimalPlaces);
                        }
                        $(valueSpan).html(caption);
                    }

                    return {
                        updateValue: updateValue
                    }

                }

                var separator = function () {
                    var item = $('<div/>', {
                        'class': 'extremum-detail-items-separator'
                    }).appendTo(extremumDetailsInfoPanel)[0];
                }

                extremumDetailItems.push(new extremumDetailItem('Index', function(item) { return item.price.dataItem.index; }, 0, true));
                extremumDetailItems.push(new extremumDetailItem('Value', function (item) { return item.value; }, 2, true));
                extremumDetailItems.push(new extremumDetailItem('Is open', function (item) { return item.stats.isOpen; } , 0, true));
                separator();
                extremumDetailItems.push(new extremumDetailItem('Earlier counter', function (item) { return item.stats.earlier.counter; }, 0));
                extremumDetailItems.push(new extremumDetailItem('Earlier amplitude', function (item) { return item.stats.earlier.amplitude; }));
                extremumDetailItems.push(new extremumDetailItem('Earlier total area', function (item) { return item.stats.earlier.totalArea; }));
                extremumDetailItems.push(new extremumDetailItem('Earlier average area', function (item) { return item.stats.earlier.averageArea; }));
                extremumDetailItems.push(new extremumDetailItem('Earlier change [1]', function (item) { return item.stats.earlier.change1; }));
                extremumDetailItems.push(new extremumDetailItem('Earlier change [2]', function (item) { return item.stats.earlier.change2; }));
                extremumDetailItems.push(new extremumDetailItem('Earlier change [3]', function (item) { return item.stats.earlier.change3; }));
                extremumDetailItems.push(new extremumDetailItem('Earlier change [5]', function (item) { return item.stats.earlier.change5; }));
                extremumDetailItems.push(new extremumDetailItem('Earlier change [10]', function (item) { return item.stats.earlier.change10; }));
                separator();
                extremumDetailItems.push(new extremumDetailItem('Later counter', function (item) { return item.stats.later.counter; }, 0));
                extremumDetailItems.push(new extremumDetailItem('Later amplitude', function (item) { return item.stats.later.amplitude; }));
                extremumDetailItems.push(new extremumDetailItem('Later total area', function (item) { return item.stats.later.totalArea; }));
                extremumDetailItems.push(new extremumDetailItem('Later average area', function (item) { return item.stats.later.averageArea; }));
                extremumDetailItems.push(new extremumDetailItem('Later change [1]', function (item) { return item.stats.later.change1; }));
                extremumDetailItems.push(new extremumDetailItem('Later change [2]', function (item) { return item.stats.later.change2; }));
                extremumDetailItems.push(new extremumDetailItem('Later change [3]', function (item) { return item.stats.later.change3; }));
                extremumDetailItems.push(new extremumDetailItem('Later change [5]', function (item) { return item.stats.later.change5; }));
                extremumDetailItems.push(new extremumDetailItem('Later change [10]', function (item) { return item.stats.later.change10; }));
            })();

            trendlinePanelButton = $('<span/>', {
                'class': 'show-trendline-panel-button open-panel-button',
                'html': 'Trendlines'
            }).appendTo(parentContainer)[0];

        })();


        //[RENDERING]
        function renderPriceLegend() {

            var legendItem = function($name, $symbol){
                var name = $name;
                var symbol = $symbol;
                var mainSpan;
                var symbolSpan;
                var priceSpan;

                (function render() {
                    mainSpan = $('<span/>', {
                        'class': 'legend-item'
                    }).css({
                        visibility: 'hidden'
                    }).appendTo(legendContainer)[0];

                    symbolSpan = $('<span/>', {
                        'class': 'legend-price-label',
                        html: symbol
                    }).appendTo(mainSpan)[0];

                    priceSpan = $('<span/>', {
                        'class': 'legend-price-value'
                    }).appendTo(mainSpan)[0];

                })();

                function updateValue(value) {
                    $(mainSpan).css('visibility', 'visible');
                    $(priceSpan).html(value.toFixed(4));
                }

                return {
                    updateValue: updateValue
                }

            }

            legendItems.open = legendItem('open', 'O');
            legendItems.high = legendItem('high', 'H');
            legendItems.low = legendItem('low', 'L');
            legendItems.close = legendItem('close', 'C');

        }

        function renderAdxLegend() {

        }

        function renderMacdLegend() {

        }

        (function render() {
            var type = self.params.getType();
            if (type === STOCK.INDICATORS.PRICE) {
                renderPriceLegend();
            } else if (type === STOCK.INDICATORS.ADX) {
                renderAdxLegend();
            } else if (type === STOCK.INDICATORS.MACD) {
                renderMacdLegend();
            }
        })()


        //[UPDATING]
        function updateLegend(item) {
            var type = self.params.getType();
            if (type === STOCK.INDICATORS.PRICE) {
                updatePriceLegend(item);
            } else if (type === STOCK.INDICATORS.ADX) {
                updateAdxLegend(item);
            } else if (type === STOCK.INDICATORS.MACD) {
                updateMacdLegend(item);
            }
        }

        function updatePriceLegend(item) {
            if (item.item) {
                var quotation = item.item.price.quotation;
                legendItems.open.updateValue(quotation.open);
                legendItems.high.updateValue(quotation.high);
                legendItems.low.updateValue(quotation.low);
                legendItems.close.updateValue(quotation.close);
            }
        }

        function updateAdxLegend(item) {

        }

        function updateMacdLegend(item) {

        }

        function updateExtremumDetailsPanel(extremum, circleLeft, circleRight, circleTop, circleBottom) {
            var margin = 6;
            var detailsInfoPanelOffset = 3;
            //Top
            var parentHeight = $(self.ui.getChartContainer()).height();
            var panelHeight = $(extremumDetailsInfoPanel).outerHeight(true);
            var top = circleTop;
            var bottom = top + panelHeight;
            if (bottom > parentHeight - margin) {
                top = Math.max(0, circleBottom - panelHeight);
            }
            //Left
            var parentWidth = $(self.ui.getChartContainer()).width();
            var panelWidth = $(extremumDetailsInfoPanel).outerWidth(true);
            var left = circleRight + detailsInfoPanelOffset;
            var right = left + panelWidth;
            if (right > parentWidth - margin) {
                left = circleLeft - detailsInfoPanelOffset - panelWidth;
            }


            $(extremumDetailsInfoPanel).css({
                top: top + 'px',
                left: left + 'px',
                visibility: 'visible'
            });

            extremumDetailItems.forEach(function (item) {
                item.updateValue(extremum);
            });

        }

        function hideExtremumDetailsPanel() {
            $(extremumDetailsInfoPanel).css({
                visibility: 'hidden'
            });
        }


        //EVENTS
        (function bindEvents() {
            $(trendlinePanelButton).bind({
                click: function (e) {
                    self.trendlinesPanel.show();
                }
            });

            self.bind({
                trendlinesPanelOpen: function (e) {
                    trendlinePanelButton.classList.remove('open-panel-button');
                },
                trendlinesPanelClosed: function (e) {
                    trendlinePanelButton.classList.add('open-panel-button');
                }
            });

        })();


        return {
            updateLegend: updateLegend,
            updateExtremumDetailsPanel: updateExtremumDetailsPanel,
            hideExtremumDetailsPanel: hideExtremumDetailsPanel
        }


    })();


    self.trendlinesPanel = (function () {
        var PANEL_NAME = 'trendlines-panel';
        var status = false;
        var moveParams;
        //[UI components]
        var panel;
        var titleBar;
        var closeButton;
        var container;
        //---------------------------------------------------


        //RENDERING
        (function renderElements() {
            panel = $('<div/>', {
                'id': 'trendlines-details-panel',
                'class': 'trendlines-details-panel'
            }).css({
                'visibility': 'hidden'
            }).appendTo(document.body)[0];

            titleBar = $('<div/>', {
                'class': 'trendlines-details-panel-title-bar'
            }).appendTo(panel)[0];

            var titleCaption = $('<span/>', {
                'class': 'trendlines-details-panel-title-caption',
                html: 'Trendlines'
            }).appendTo(titleBar)[0];

            closeButton = $('<div/>', {
                'class': 'trendlines-details-panel-close-button'
            }).appendTo(titleBar)[0];

            container = $('<div/>', {
                'id': 'trendlines-details-panel-content',
                'class': 'trendlines-details-panel-content'
            }).appendTo(panel)[0];
            
        })();


        //CONTROL EVENTS
        (function bindControlEvents() {

            $(closeButton).bind({
                click: function (e) {
                    hide();
                }
            });

            $(titleBar).bind({
                mousedown: function (e) {
                    setTimeout(handleMouseDown(e), 0);
                },
                mouseup: function (e) {
                    e.preventDefault();
                    setTimeout(handleMouseUp(e), 0);
                },
                mousemove: function (e) {
                    setTimeout(handleMouseMove(e), 0);
                }
            });

            $(document.body).bind({
                mouseup: function (e) {
                    setTimeout(endMoveMode(), 0);
                },
                mousemove: function (e) {
                    setTimeout(handleMouseMove(e), 0);
                }
            });

        })();


        //MOVING
        function handleLeavingTrendlinesPanel() {
            //setTimeout(resetMoveParamsObject(), 0);
        }

        function handleMouseDown(e) {
            setTimeout(startMoveMode(e), 0);
        }

        function handleMouseMove(e) {
            setTimeout(slide(e), 0);
        }

        function handleMouseUp(e) {
            setTimeout(endMoveMode(e), 0);
        }

        //[Sliding chart]
        function resetMoveParamsObject() {
            moveParams = {
                state: false,
                x: null,
                y: null
            };
        }

        function slide(e) {
            if (moveParams && moveParams.state) {
                var offsetX = (e.screenX - moveParams.x);
                moveParams.x = e.screenX;
                var offsetY = (e.screenY - moveParams.y);
                moveParams.y = e.screenY;

                var currentPosition = {
                    left: mielk.ui.getComponentCssOffset(panel, 'left'),
                    top: mielk.ui.getComponentCssOffset(panel, 'top')
                }

                $(panel).css({
                    'left': (currentPosition.left + offsetX) + 'px',
                    'top': (currentPosition.top + offsetY) + 'px'
                });

            }
        }

        function startMoveMode(e) {
            self.trigger({ type: 'panelMoveModeStart', name: PANEL_NAME });
            resetMoveParamsObject();
            moveParams.state = true;
            moveParams.x = e.screenX;
            moveParams.y = e.screenY;
        }

        function endMoveMode(e) {
            self.trigger({ type: 'panelMoveModeEnd', name: PANEL_NAME });
            resetMoveParamsObject();
        }



        //API
        function isOpen() {
            return status;
        }

        function show() {
            $(panel).css('visibility', 'visible');
            (function centerOnScreen() {
                var browserSize = {
                    width: $(window).width(),
                    height: $(window).height()
                };
                var panelSize = {
                    width: $(panel).width(),
                    height: $(panel).height()
                };
                var coordinates = {
                    left: (browserSize.width - panelSize.width) / 2,
                    top: (browserSize.height - panelSize.height) / 2
                };
                $(panel).css({
                    'left': coordinates.left + 'px',
                    'top': coordinates.top + 'px'
                });
            })();
            self.trigger({ type: 'trendlinesPanelOpen' });
            insertData();
        }

        function hide() {
            $(panel).css('visibility', 'hidden');
            self.trigger({ type: 'trendlinesPanelClosed' });
        }



        //DATA
        function insertData() {
            var data = [
                  { name: 'Ted', surname: 'Smith', company: 'Electrical Systems', age: 30 },
                  { name: 'Ed', surname: 'Johnson', company: 'Energy and Oil', age: 35 },
                  { name: 'Sam', surname: 'Williams', company: 'Airbus', age: 38 },
                  { name: 'Alexander', surname: 'Brown', company: 'Renault', age: 24 },
                  { name: 'Nicholas', surname: 'Miller', company: 'Adobe', age: 33 },
                  { name: 'Andrew', surname: 'Thompson', company: 'Google', age: 28 },
                  { name: 'Ryan', surname: 'Walker', company: 'Siemens', age: 39 },
                  { name: 'John', surname: 'Scott', company: 'Cargo', age: 45 },
                  { name: 'James', surname: 'Phillips', company: 'Pro bugs', age: 30 },
                  { name: 'Brian', surname: 'Edwards', company: 'IT Consultant', age: 23 },
                  { name: 'Jack', surname: 'Richardson', company: 'Europe IT', age: 24 },
                  { name: 'Alex', surname: 'Howard', company: 'Cisco', age: 27 },
                  { name: 'Carlos', surname: 'Wood', company: 'HP', age: 36 },
                  { name: 'Adrian', surname: 'Russell', company: 'Micro Systems', age: 31 },
                  { name: 'Jeremy', surname: 'Hamilton', company: 'Big Machines', age: 30 },
                  { name: 'Ivan', surname: 'Woods', company: '', age: 24 },
                  { name: 'Peter', surname: 'West', company: 'Adobe', age: 26 },
                  { name: 'Scott', surname: 'Simpson', company: 'IBM', age: 29 },
                  { name: 'Lorenzo', surname: 'Tucker', company: 'Intel', age: 29 },
                  { name: 'Randy', surname: 'Grant', company: 'Bridges', age: 30 },
                  { name: 'Arthur', surname: 'Gardner', company: 'Google', age: 31 },
                  { name: 'Orlando', surname: 'Ruiz', company: 'Apple', age: 32 }
            ];
            
            $(function () {
                $(container).empty();
                $(container).FancyGrid({
                    width: 401,
                    height: 400,
                    data: data,
                    events: [{
                        rowclick: function(grid, rowIndex, dataItem){
                            var x = dataItem;
                        }
                    }],
                    columns: [{
                        index: 'company',
                        title: 'Company',
                        type: 'string',
                        width: 100,
                        sortable: true
                    }, {
                        index: 'name',
                        title: 'Name',
                        type: 'string',
                        width: 100,
                        sortable: true
                    }, {
                        index: 'surname',
                        title: 'Sur Name',
                        type: 'string',
                        width: 100,
                        sortable: true
                    }, {
                        index: 'age',
                        title: 'Age',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        filter: {
                            header: true
                        }
                    }]
                });

                var tags = container.getElementsByTagName('a');
                for (var i = 0; i < tags.length; i++) {
                    tags[i].style.cssText = '';
                    tags[i].style.visibility = 'hidden';
                }

            });

        }



        return {
            isOpen: isOpen,
            show: show,
            hide: hide
        }

    })();

}

Chart.prototype.bind = function (e) {
    $(this).bind(e);
}
Chart.prototype.trigger = function (e) {
    $(this).trigger(e);
}