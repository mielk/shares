function Chart(parent, params) {

    'use strict';

    //[Meta].
    var self = this;
    self.Chart = true;
    self.parent = parent;
    self.verticalResizeIndex = 0;


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

        function setVisibilityRange() {
            //var offset = 0.04 * (valuesRange.min + valuesRange.max) / 2;
            //var min = Math.floor(valuesRange.min - offset);
            //var max = Math.floor(valuesRange.max + offset);
            //self.ui.setVisibleRange(min, max);
            self.ui.setVisibleRange(valuesRange.min, valuesRange.max);
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

        function getTrendline(id) {
            for (var i = 0; i < trendlines.length; i++) {
                var trendline = trendlines[i];
                if (trendline.trendline.id === id) {
                    return trendline;
                }
            }
        }

        function getValuesRange() {
            return valuesRange;
        }

        function countNonEmptyItems() {
            return dataSetInfo.endIndex - dataSetInfo.startIndex + 1;
        }

        function findItemByLeft(left) {
            for(var i = 0; i < items.length; i++){
                var item = items[i];
                if (item) {
                    if (item.coordinates.left >= left) {
                        return item;
                    }
                }
            }
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
            getTrendline: getTrendline,
            countNonEmptyItems: countNonEmptyItems,
            findItemByLeft: findItemByLeft
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
        var svgPreview;
        var svgBoxHeight = 1000;
        var svgDetailsBoxOffset = 100;
        var svgItemsBoxTopAnchor = 0;
        var svgItemsBoxTop = 0;
        var svgLeft = 0;
        var svgExtraWidth = 200;
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

        (function saveHeightValuesInVisibleRangeObject() {
            visibleRange.height = $(chartContainer).height();
            svgBoxHeight = visibleRange.height;
            visibleRange.svgHeight = svgBoxHeight;
        })();

        //Rendering SVG containers.
        function insertSvgItems() {
            svgItems = mielk.svg.createSvg();
            var height = svgBoxHeight;
            var width = calculateSvgWidth();

            svgItems.setAttribute('id', 'items');
            svgItems.setAttribute('preserveAspectRatio', 'none meet');
            svgItems.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgItems.style.height = height + 'px';
            svgItems.style.top = svgItemsBoxTopAnchor + 'px';
            svgItems.style.width = width + 'px';
            svgItems.style.left = svgLeft + 'px';
            svgItems.style.zIndex = 2;
            chartContainer.appendChild(svgItems);

        }

        function insertSvgExtrema() {
            svgExtrema = mielk.svg.createSvg();
            var height = svgBoxHeight + svgDetailsBoxOffset;
            var width = calculateSvgWidth();
            var top = (svgItemsBoxTop - svgDetailsBoxOffset / 2);

            svgExtrema.setAttribute('id', 'extrema');
            svgExtrema.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgExtrema.setAttribute('preserveAspectRatio', 'none meet');
            svgExtrema.style.height = height + 'px';
            svgExtrema.style.top = top + 'px';
            svgExtrema.style.width = width + 'px';
            svgExtrema.style.left = svgLeft + 'px';
            svgItems.style.zIndex = 2;

            chartContainer.appendChild(svgExtrema);

        }

        function insertSvgTrendlines() {
            svgTrendlines = mielk.svg.createSvg();
            var height = svgBoxHeight + svgDetailsBoxOffset;
            var width = calculateSvgWidth();
            var top = (svgItemsBoxTop - svgDetailsBoxOffset / 2);

            svgTrendlines.setAttribute('id', 'trendlines');
            svgTrendlines.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgTrendlines.setAttribute('preserveAspectRatio', 'none meet');
            svgTrendlines.style.height = height + 'px';
            svgTrendlines.style.top = top + 'px';
            svgTrendlines.style.width = width + 'px';
            svgTrendlines.style.left = svgLeft + 'px';
            svgItems.style.zIndex = 2;

            chartContainer.appendChild(svgTrendlines);

        }

        function insertSvgPreview() {
            svgPreview = mielk.svg.createSvg();
            var height = svgBoxHeight;
            var width = calculateSvgWidth();

            svgPreview.setAttribute('id', 'preview');
            svgPreview.setAttribute('preserveAspectRatio', 'none meet');
            svgPreview.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgPreview.style.height = height + 'px';
            svgPreview.style.top = svgItemsBoxTopAnchor + 'px';
            svgPreview.style.width = width + 'px';
            svgPreview.style.left = svgLeft + 'px';
            chartContainer.appendChild(svgPreview);
            svgItems.style.zIndex = 1;

        }


        //Adjusting SGV containers size & position.
        function setSvgHeight(height) {
            var heightChange = height - svgBoxHeight;
            var topChange = (heightChange / -2);
            svgBoxHeight = height;
            var width = calculateSvgWidth();

            //[Items]
            svgItems.setAttribute('viewBox', '0 0 ' + width + ' ' + svgBoxHeight);
            svgItems.style.height = svgBoxHeight + 'px';
            svgItemsBoxTopAnchor += topChange;
            svgItemsBoxTop += topChange;
            svgItems.style.top = svgItemsBoxTop + 'px';

            //var valuesContainerTopOffset = self.valuesPanel.getTopOffset();
            //mielk.notify.display('{setSvgHeight} || height: ' + height + ' | svgItemsBoxTopAnchor: ' + svgItemsBoxTopAnchor + ' | svgItems.style.top: ' + svgItems.style.top + ' | values 300 top: ' + valuesContainerTopOffset);

            //[Extrema]
            if (svgExtrema) {
                svgExtrema.setAttribute('viewBox', '0 0 ' + width + ' ' + (svgBoxHeight + svgDetailsBoxOffset));
                svgExtrema.style.height = (svgBoxHeight + svgDetailsBoxOffset) + 'px';
                svgExtrema.style.top = (svgItemsBoxTop - svgDetailsBoxOffset / 2) + 'px';
            }

            //[Trendlines]
            if (svgTrendlines) {
                svgTrendlines.setAttribute('viewBox', '0 0 ' + width + ' ' + (svgBoxHeight + svgDetailsBoxOffset));
                svgTrendlines.style.height = (svgBoxHeight + svgDetailsBoxOffset) + 'px';
                svgTrendlines.style.top = (svgItemsBoxTop - svgDetailsBoxOffset / 2) + 'px';
            }

            //[Preview]
            if (svgPreview) {
                svgPreview.setAttribute('viewBox', '0 0 ' + width + ' ' + svgBoxHeight);
                svgPreview.style.height = svgBoxHeight + 'px';
                svgPreview.style.top = svgItemsBoxTop + 'px';
            }

            visibleRange.svgHeight = svgBoxHeight;
            
        }

        function setSvgLeft(left) {
            if (left || left === 0) {
                svgLeft = left;
                if (svgItems) svgItems.style.left = svgLeft + 'px';
                if (svgExtrema) svgExtrema.style.left = svgLeft + 'px';
                if (svgTrendlines) svgTrendlines.style.left = svgLeft + 'px';
                if (svgPreview) svgPreview.style.left = svgLeft + 'px';
            }
        }

        function offsetSvgVertically(top) {
            if (top || top === 0) {
                svgItemsBoxTop = top;
                if (svgItems) svgItems.style.top = svgItemsBoxTop + 'px';
                if (svgExtrema) svgExtrema.style.top = (svgItemsBoxTop - svgDetailsBoxOffset / 2) + 'px';
                if (svgTrendlines) svgTrendlines.style.top = (svgItemsBoxTop - svgDetailsBoxOffset / 2) + 'px';
                if (svgPreview) svgPreview.style.top = svgItemsBoxTop + 'px';
            }
        }



        //Helper functions.
        function calculateSvgWidth() {
            var nonEmptyItemsCounter = self.data.countNonEmptyItems();
            var itemWidth = Math.ceil(self.params.getItemWidth());
            return (nonEmptyItemsCounter * itemWidth) + svgExtraWidth;
        }


        //Access to SVG panels and properties.
        function setVisibleRange(min, max) {
            visibleRange.min = min;
            visibleRange.max = max;
            visibleRange.unit = visibleRange.height / (visibleRange.max - visibleRange.min);
            visibleRange.pixelValue = (visibleRange.max - visibleRange.min) / visibleRange.height;
            visibleRange.svgUnit = visibleRange.svgHeight / (visibleRange.max - visibleRange.min);
            visibleRange.svgPixelValue = (visibleRange.max - visibleRange.min) / visibleRange.svgHeight;
        }

        function stretchVisibleRange(heightSizeChange) {
            var valuesRange = self.data.getValuesRange();            
            var newHeight = visibleRange.svgHeight + heightSizeChange;
            var newUnit = newHe1ight / (valuesRange.max - valuesRange.min);
            var itemsOnScreen = visibleRange.height / newUnit;
            var middle = (visibleRange.max + visibleRange.min) / 2
            var newMax = middle + itemsOnScreen / 2;
            var newMin = middle - itemsOnScreen / 2;
            visibleRange.svgHeight = newHeight;
            visibleRange.unit = newUnit;
            visibleRange.svgUnit = newUnit;
            visibleRange.max = newMax;
            visibleRange.min = newMin;            
            visibleRange.pixelValue = 1 / visibleRange.unit;
            visibleRange.svgPixelValue = visibleRange.pixelValue;
        }

        function offsetVisibleRange(offset) {
            visibleRange.max += visibleRange.pixelValue * offset;
            visibleRange.min += visibleRange.pixelValue * offset;
        }

        function getVisibleRange() {
            return visibleRange;
        }

        function getSvgBoxHeight() {
            return svgBoxHeight;
        }

        function getSvgDetailsBoxTopOffset() {
            return svgDetailsBoxOffset;
        }

        function getSvgAnchorTopOffset() {
            return svgItemsBoxTopAnchor;
        }

        function getSvgTopOffset() {
            return svgItemsBoxTop;
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

        function getPreviewSvg() {
            if (svgPreview === undefined) insertSvgPreview();
            return svgPreview;
        }

        function getSvgLeftOffset() {
            return $(svgItems).offset().left;
        }

        function scrollSvgsHorizontally(left) {
            setSvgLeft(left);
        }


        //Access to other HTML components.
        function getValueLabelsContainer() {
            return valuesLabelsContainer;
        }

        function getValueLabelsContainerHeight() {
            return $(valuesLabelsContainer).height();
        }

        function getHorizontalGridLinesContainer() {
            return horizontalGridLinesContainer;
        }

        function getChartContainer() {
            return chartContainer;
        }


        //Showing/hiding components
        function showHideTrendlinesSvg(value) {
            $(svgTrendlines).css({
                'visibility': (value ? 'visible' : 'hidden')
            });
        }

        function showHideExtremaSvg(value) {
            $(svgExtrema).css({
                'visibility': (value ? 'visible' : 'hidden')
            });
        }

        function scrollToXByItem(index, offset) {
            var item = self.data.getItem(index);
            if (item){
                var left = item.coordinates.left + offset;
                setTimeout(self.parent.events.triggerScrollToX(-left), 0);
            }
        }


        //[EVENTS]
        (function bindEvents() {
            self.parent.bind({
                horizontalSlide: function (e) {
                    setSvgLeft(e.left);
                },
                scrollToX: function (e) {
                    setSvgLeft(e.left);
                }
            });
        })();



        return {
            setSvgHeight: setSvgHeight,
            setSvgLeft: setSvgLeft,
            offsetSvgVertically: offsetSvgVertically,
            setVisibleRange: setVisibleRange,
            stretchVisibleRange: stretchVisibleRange,
            offsetVisibleRange: offsetVisibleRange,
            getSvgLeftOffset: getSvgLeftOffset,
            getSvgDetailsBoxTopOffset: getSvgDetailsBoxTopOffset,
            getSvgAnchorTopOffset: getSvgAnchorTopOffset,
            getSvgTopOffset: getSvgTopOffset,
            getSvgBoxHeight: getSvgBoxHeight,
            getItemsSvg: getItemsSvg,
            getExtremaSvg: getExtremaSvg,
            getTrendlinesSvg: getTrendlinesSvg,
            getPreviewSvg: getPreviewSvg,
            getVisibleRange: getVisibleRange,
            getHorizontalGridLinesContainer: getHorizontalGridLinesContainer,
            getValueLabelsContainer: getValueLabelsContainer,
            getValueLabelsContainerHeight: getValueLabelsContainerHeight,
            getChartContainer: getChartContainer,
            //Showing/hiding components
            showHideTrendlinesSvg: showHideTrendlinesSvg,
            showHideExtremaSvg: showHideExtremaSvg,
            scrollToXByItem: scrollToXByItem
        }

    })();


    self.svg = (function () {
        var type = self.params.getType();
        var svgBoxHeight = self.ui.getSvgBoxHeight();
        var itemsSvgOffset = self.ui.getSvgDetailsBoxTopOffset();
        var visibleRange = {};
        var resizeIndex;
        

        //[RENDERING]
        function render() {
            resizeIndex = self.verticalResizeIndex;
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
            self.valuesPanel.renderValuesAndHorizontalGridLines();
        }

        function renderItems() {
            var svg = self.ui.getItemsSvg();
            var valuesRange = self.data.getValuesRange();
            var visibleRange = self.ui.getVisibleRange();
            var items = self.data.getItems();
            
            function getY(value) {
                var pointsDistance = valuesRange.max - value;
                return pointsDistance * visibleRange.svgUnit;
            }

            (function adjustSvgHeight() {
                var svgHeight = visibleRange.svgHeight;
                self.ui.setSvgHeight(svgHeight);
                visibleRange = self.ui.getVisibleRange();
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

            function generateAndInsertSvgRectangles() {
                var priceUpBodyInsideColor = STOCK.CONFIG.candle.color.ascendingBody;
                var priceUpBodyBorderColor = STOCK.CONFIG.candle.color.ascendingLine
                var priceDownBodyInsideColor = STOCK.CONFIG.candle.color.descendingBody;
                var priceDownBodyBorderColor = STOCK.CONFIG.candle.color.descendingLine;
                var shadowColor = STOCK.CONFIG.candle.color.shadow;
                var strokeWidth = STOCK.CONFIG.candle.strokeWidth;
                var rectangles = [];

                for (var i = 0; i < items.length; i++) {
                    if (resizeIndex !== self.verticalResizeIndex) break;
                    var item = items[i];
                    if (item) {
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
                    }
                }

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

                if (resizeIndex === self.verticalResizeIndex) {
                    $(svg).empty();
                    for (var i = 0; i < rectangles.length; i++) {
                        var item = rectangles[i];
                        if (item) {
                            if (resizeIndex !== self.verticalResizeIndex) break;
                            var rectangle = mielk.svg.createRectangle(item.width, item.height, item.x, item.y, item.fill)
                            rectangle.style.stroke = item.stroke;
                            rectangle.style.strokeWidth = item.strokeWidth + 'px';
                            rectangle.style.vectorEffect = 'non-scaling-stroke';
                            rectangle.style.shapeRendering = 'crispEdges'
                            svg.appendChild(rectangle);
                            appendSvgComponentToItem(rectangle, item);
                        }
                    }
                }

            }
            
            setTimeout(generateAndInsertSvgRectangles(), 0);

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

                for (var i = 0; i < items.length; i++) {
                    if (resizeIndex !== self.verticalResizeIndex) break;
                    var item = items[i];
                    if (item) {
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
                    }
                }

                if (resizeIndex === self.verticalResizeIndex) {
                    $(svg).empty();
                    for (var i = 0; i < circles.length; i++) {
                        if (resizeIndex !== self.verticalResizeIndex) break;
                        var item = circles[i];
                        if (item) {
                            var circle = mielk.svg.createCircle(item.x, item.y, item.radius, item.fill, item.stroke);
                            circle.style.strokeWidth = item.strokeWidth + 'px';
                            circle.style.vectorEffect = 'non-scaling-stroke';
                            circle.style.shapeRendering = 'crispEdges'
                            svg.appendChild(circle);
                            appendSvgComponentToItem(circle, item);
                        }
                    }
                }
                
            })();

        }

        function renderPriceTrendlines() {
            var svg = self.ui.getTrendlinesSvg();
            var trendlines = self.data.getTrendlines();
            var quotes = self.data.getItems();
            var valuesRange = self.data.getValuesRange();
            var visibleRange = self.ui.getVisibleRange();

            function getY(value) {
                var pointsDistance = valuesRange.max - value;
                return pointsDistance * visibleRange.svgUnit;
            }

            (function appendCoordinates() {

                function calculateCoordinatesForPoint(x) {
                    return {
                        x: x,
                        y: edgePointsCoordinates.base.y + (x - edgePointsCoordinates.base.x) * viewSlope
                    }
                    return
                }

                for (var i = 0; i < trendlines.length; i++) {
                    var item = trendlines[i];
                    if (item) {

                        if (resizeIndex !== self.verticalResizeIndex) break;

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
                                y: getY(item.trendline.edgePoints.base.level) + itemsSvgOffset / 2
                            },
                            counter: {
                                x: linkedItems.counter.coordinates.middle,
                                y: getY(item.trendline.edgePoints.counter.level) + itemsSvgOffset / 2
                            }
                        };
                        viewSlope = (edgePointsCoordinates.counter.y - edgePointsCoordinates.base.y) / (edgePointsCoordinates.counter.x - edgePointsCoordinates.base.x);

                        item.coordinates = {
                            start: calculateCoordinatesForPoint(linkedItems.start.coordinates.middle),
                            end: calculateCoordinatesForPoint(linkedItems.end.coordinates.middle)
                        }

                    }

                }

            })();

            (function generateAndInsertSvgPaths() {
                var paths = [];
                var strokeWidth = STOCK.CONFIG.trendlines.width;
                var stroke = STOCK.CONFIG.trendlines.color;

                for (var i = 0; i < trendlines.length; i++) {
                    var item = trendlines[i];
                    if (i === 12) {
                        var x = 1;
                    }
                    if (item) {
                        if (resizeIndex !== self.verticalResizeIndex) break;
                        var path = {
                            d: 'M ' + item.coordinates.start.x + ' ' + item.coordinates.start.y + 'L' + item.coordinates.end.x + ' ' + item.coordinates.end.y,
                            //stroke: 'rgba(0, 0, 0, ' + item.trendline.value / 100 + ')',
                            stroke: 'rgba(0, 0, 0, 0.5)',
                            strokeWidth: strokeWidth,
                            trendline: item
                        };
                        paths.push(path);
                    }
                }

                function appendSvgComponentToItem(component, item) {
                    item.svgPath = component;
                }

                if (resizeIndex === self.verticalResizeIndex) {
                    $(svg).empty();
                    for (var i = 0; i < paths.length; i++) {
                        var item = paths[i];
                        if (item) {
                            var path = mielk.svg.createPath(item.d);
                            path.style.strokeWidth = item.strokeWidth + 'px';
                            path.style.stroke = item.stroke;
                            path.style.vectorEffect = 'non-scaling-stroke';
                            path.style.shapeRendering = 'crispEdges'
                            svg.appendChild(path);
                            appendSvgComponentToItem(path, item.trendline);
                        }
                    }
                }

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
        var labelInfo = {};
        var labels = {};
        var zoomStep = 100;
        var zoomInProgress = false;

        function getTopOffset() {
            return valueLabelsContainer.offsetTop;
        }

        function renderValuesAndHorizontalGridLines() {
            var valuesRange = self.data.getValuesRange();
            var visibleRange = self.ui.getVisibleRange();
            labelInfo.unitHeight = visibleRange.unit;

            $(horizontalLinesContainer).empty();
            $(valueLabelsContainer).empty();
            labels = {};

            (function locateContainers() {
                var height = self.ui.getSvgBoxHeight();
                var top = self.ui.getSvgTopOffset();
                $(valueLabelsContainer).height(height);
                $(valueLabelsContainer).css('top', top + 'px');
                $(horizontalLinesContainer).height(height);
                $(horizontalLinesContainer).css('top', top + 'px');
            })();

            (function insertHtmlComponents() {
                var arr = generateDisplayedValuesArray(valuesRange, visibleRange);
                labelInfo.min = arr[0];
                labelInfo.max = arr[arr.length - 1];
                labelInfo.arr = arr;
                insertLabels(arr, visibleRange, valuesRange);
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

        function insertLabels(arr, visibleRange, valuesRange) {
            for (var i = arr.length - 1; i >= 0; i--) {
                var value = arr[i];
                var item = labels[value];
                if (item === undefined || item === null) {
                    var y = ((valuesRange ? valuesRange.max : visibleRange.max) - value) * labelInfo.unitHeight;

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
                        'html': value.toFixed(4)
                    }).appendTo(valueLabelsContainer)[0];

                    var height = $(label).height();
                    $(label).css('top', (y - height / 2) + 'px');

                    labels[value] = {
                        label: label,
                        horizontalLine: horizontalLine,
                        ticker: ticker
                    };
                }

            }
        }

        function calculateGridLinesStep(range) {
            var factors = [1, 2, 5];
            var baseDistance = (range.max - range.min) / 10;
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

        function generateDisplayedValuesArray(valuesRange, visibleRange) {
            var addSpaceRatio = 0; //0.1;
            var step = calculateGridLinesStep(visibleRange);
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

        function updateCurrentValueIndicators(y, value){
            var top = y - $(horizontalLinesContainer).offset().top;
            var labelTop = top - $(currentValueLabel).height() / 2;
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


        (function bindEvents() {

            $(self).bind({
                verticalSlide: function (e) {
                    if (e.y) {
                        var top = $(valueLabelsContainer).position().top + e.y;
                        self.ui.offsetVisibleRange(e.y);
                        $(valueLabelsContainer).css('top', top + 'px');
                        $(horizontalLinesContainer).css('top', top + 'px');
                        adjustLabels(top);
                        self.ui.offsetSvgVertically(top);
                    }
                }
            });

            $(valueLabelsContainer).bind({
                contextmenu: function (e) {
                    e.preventDefault();
                }
                , mousedown: function (e) {
                    if (!zoomInProgress) {
                        setTimeout(handleMouseDown(e), 0);
                    }
                }
                //, mousemove: function (e) {
                //    setTimeout(handleMouseMove(e), 0);
                //}
            });
        })();


        function handleMouseDown(e) {
            if (e.buttons === 1) {
                setTimeout(zoom(zoomStep));
            } else if (e.buttons === 2) {
                setTimeout(zoom(-zoomStep));
            }
        }

        function zoom(change) {
            zoomInProgress = true;
            if (change != 0) {
                self.verticalResizeIndex++;
                self.ui.stretchVisibleRange(change);
                var visibleRange = self.ui.getVisibleRange();
                self.svg.render();
            }
            zoomInProgress = false;
        }

        function adjustLabels(top) {
            var valuesRange = self.data.getValuesRange();
            var visibleRange = self.ui.getVisibleRange();
            var arr = generateDisplayedValuesArray(valuesRange, visibleRange);
            if (arr[arr.length - 1] !== labelInfo.max || arr[0] !== labelInfo.min) {
                insertLabels(arr, visibleRange, valuesRange);
                labelInfo.min = arr[0];
                labelInfo.max = arr[arr.length - 1];
                labelInfo.arr = arr;
            };
        }
        

        return {
            getTopOffset: getTopOffset,
            renderValuesAndHorizontalGridLines: renderValuesAndHorizontalGridLines,
            updateCurrentValueIndicators: updateCurrentValueIndicators,
            hideCurrentValueIndicators: hideCurrentValueIndicators
        }

    })();


    self.events = (function () {
        var topOffset = self.ui.getSvgDetailsBoxTopOffset();
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
            var x = e.clientX - self.ui.getSvgLeftOffset();
            var y = e.clientY - $(eventsLayer).offset().top;
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
                if (item && item.coordinates.left > x) {
                    while (items[i].coordinates.right > x) {
                        if (--i < startIndex) {
                            return item;
                        }
                        item = items[i];
                    }
                } else if (item && item.coordinates.right < x) {
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
            return visibleRange.max - (visibleRange.pixelValue * y);
        }

        function showInfo(e) {
            var res = findCoordinates(e);
            if (res.item) {
                setTimeout(self.parent.dates.updateCurrentDateIndicators(e.pageX, res.item.date, res.item.index), 0);
                setTimeout(self.valuesPanel.updateCurrentValueIndicators(e.pageY, res.value), 0);
                setTimeout(self.legend.updateLegend(res.item), 0);
            }
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
                var offsetY = (e.pageY - moveParams.y);
                moveParams.y = e.pageY;
                setTimeout(self.parent.events.triggerHorizontalSlideEvent(offsetX), 0);
                if (offsetY) {
                    setTimeout(self.trigger({ type: 'verticalSlide', y: offsetY }));
                }
            }
        }

        function startMoveMode(e) {
            resetMoveParamsObject();
            moveParams.state = true;
            moveParams.x = e.clientX;
            moveParams.y = e.pageY;
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
            if (!self.layers.extremaVisible) return;

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
        var panelButtonsContainer;
        var trendlinePanelButton;
        var trendHitsPanelButton;
        var trendBreaksPanelButton;
        var trendRangesPanelButton;

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

            panelButtonsContainer = $('<div/>', {
                'class': 'panel-buttons-container'
            }).appendTo(parentContainer)[0];

            trendlinePanelButton = $('<span/>', {
                'class': 'show-panel-button open-panel-button',
                'html': 'Trendlines'
            }).appendTo(panelButtonsContainer)[0];

            trendHitsPanelButton = $('<span/>', {
                'class': 'show-panel-button open-panel-button',
                'html': 'Hits'
            }).appendTo(panelButtonsContainer)[0];

            trendBreaksPanelButton = $('<span/>', {
                'class': 'show-panel-button open-panel-button',
                'html': 'Breaks'
            }).appendTo(panelButtonsContainer)[0];

            trendRangesPanelButton = $('<span/>', {
                'class': 'show-panel-button open-panel-button',
                'html': 'Ranges'
            }).appendTo(panelButtonsContainer)[0];


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
                    if (!self.trendlinesPanel.isOpen()) {
                        self.trendlinesPanel.show(STOCK.TRENDEVENTS.trendLine);
                    }
                }
            });

            $(trendHitsPanelButton).bind({
                click: function (e) {
                    if (!self.trendlinesPanel.isOpen()) {
                        self.trendlinesPanel.show(STOCK.TRENDEVENTS.trendHit);
                    }
                }
            });

            $(trendBreaksPanelButton).bind({
                click: function (e) {
                    if (!self.trendlinesPanel.isOpen()) {
                        self.trendlinesPanel.show(STOCK.TRENDEVENTS.trendBreak);
                    }
                }
            });

            $(trendRangesPanelButton).bind({
                click: function (e) {
                    if (!self.trendlinesPanel.isOpen()) {
                        self.trendlinesPanel.show(STOCK.TRENDEVENTS.trendRange);
                    }
                }
            });

            self.bind({
                trendlinesPanelOpen: function (e) {
                    trendlinePanelButton.classList.remove('open-panel-button');
                    trendHitsPanelButton.classList.remove('open-panel-button');
                    trendBreaksPanelButton.classList.remove('open-panel-button');
                    trendRangesPanelButton.classList.remove('open-panel-button');
                },
                trendlinesPanelClosed: function (e) {
                    trendlinePanelButton.classList.add('open-panel-button');
                    trendHitsPanelButton.classList.add('open-panel-button');
                    trendBreaksPanelButton.classList.add('open-panel-button');
                    trendRangesPanelButton.classList.add('open-panel-button');
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
        var eventType = null;
        var moveParams;
        //[UI components]
        var panel;
        var titleBar;
        var titleCaption;
        var closeButton;
        var container;
        var grid = {};
        var factory;
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

            titleCaption = $('<span/>', {
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

        function show($eventType) {
            eventType = $eventType;

            (function formatPanel() {
                $(panel).css('visibility', 'visible');
                $(titleCaption).html(eventType.name);
            })();

            self.trigger({ type: 'trendlinesPanelOpen' });
            setTimeout(showHideAllTrendlines(false), 0);
            insertData();
            status = true;
        }

        function hide() {
            $(panel).css('visibility', 'hidden');
            self.trigger({ type: 'trendlinesPanelClosed' });
            setTimeout(showHideAllTrendlines(true), 0);
            status = false;
        }

        function showHideAllTrendlines(value) {
            var trendlines = self.data.getTrendlines();
            trendlines.forEach(function (item) {
                item.svgPath.style.visibility = (value ? 'visible' : 'hidden');
            });
        }


        //DATA
        function insertData() {
            factory = getFactory();
            var columns = factory.getColumns();
            
            function calculateTotalWidth(columns) {
                var value = 0;
                columns.forEach(function (item) {
                    value += item.width;
                });
                return value;
            }

            $(function insertActualGrid () {
                $(container).empty();
                grid.object = $(container).FancyGrid({
                    width: calculateTotalWidth(columns) + 20,
                    height: 600,
                    data: factory.getData(),
                    paging: {
                        pageSize: 20,
                        pageSizeData: [5, 10, 20, 50],
                        barType: 'both'
                    },
                    events: [{                        
                        filter: function (grid, filters) {
                            mapGridHtmlElementsToRows();
                            addEventsToCells();
                        }
                    }],
                    columns: columns
                });

                (function getReferencesToUsefulDomComponents() {
                    grid.body = grid.object.body.el.dom;
                })();

                (function adjustFancyGridView() {

                    //Hide logo
                    var tags = container.getElementsByTagName('a');
                    for (var i = 0; i < tags.length; i++) {
                        tags[i].style.cssText = '';
                        tags[i].style.visibility = 'hidden';
                    }

                    //Adjust width.
                    var gridContainer = container.childNodes[0];
                    var gridCenter = gridContainer.getElementsByClassName('fancy-grid-center')[0];
                    var width = mielk.text.onlyDigits(gridCenter.style.width, true)[0];
                    gridContainer.style.width = width + 'px';

                })();

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

                mapGridHtmlElementsToRows();
                addEventsToCells();

            });

        }

        function mapGridHtmlElementsToRows() {
            var rows = [];
            var data = grid.object.data;
            var rowHeight = grid.object.cellHeight;
            for (var i = 0; i < data.length; i++) {
                rows[i] = {
                    index: i,
                    cells: []
                }
            }
            var cells = container.getElementsByClassName('fancy-grid-cell');
            for (var i = 0; i < cells.length; i++) {
                var cell = cells[i];
                var top = cell.offsetTop;
                var rowIndex = Math.round(top / rowHeight)
                rows[rowIndex].cells.push(cell);
            }
            grid.rows = rows;
        };

        function addEventsToCells() {
            grid.rows.forEach(function (row) {
                var cells = row.cells;
                cells.forEach(function (cell) {
                    $(cell).off();
                    $(cell).bind({
                        mouseover: function (e) {
                            activateRow(row);
                        },
                        click: function (e) {
                            e.stopPropagation();
                            changeRowSelection(row);
                        }
                    });
                });
            });

            $(grid.body).bind({
                mouseleave: function () {
                    setTimeout(deactivateCurrentRow(), 0);
                }
            });

        }

        function getFactory() {
            if (eventType === STOCK.TRENDEVENTS.trendLine) {
                return trendLinesFactory;
            } else if (eventType === STOCK.TRENDEVENTS.trendHit) {
                return trendHitsFactory;
            } else if (eventType === STOCK.TRENDEVENTS.trendBreak) {
                return trendBreaksFactory;
            } else if (eventType === STOCK.TRENDEVENTS.trendRange) {
                return trendRangesFactory;
            }
        }

        //Data & columns factories.
        var trendLinesFactory = (function () {

            function getData() {
                var result = [];
                var arr = [];
                var trendlines = self.data.getTrendlines();
                trendlines.forEach(function (item) {
                    var trendline = item.trendline;
                    arr.push(trendline);
                });

                arr.forEach(function (item) {
                    result.push({
                        id: item.id,
                        baseDateIndex: item.edgePoints.base.index,
                        baseLevel: item.edgePoints.base.level,
                        counterDateIndex: item.edgePoints.counter.index,
                        counterLevel: item.edgePoints.counter.level,
                        angle: item.slope.toFixed(4),
                        startDateIndex: item.range.start,
                        endDateIndex: (item.range.end ? item.range.end : ''),
                        isOpen: !item.isClosed,
                        value: item.value.toFixed(2),
                    });
                });

                return result;

            }

            function getColumns() {
                return [
                    {
                        index: 'id',
                        title: 'Id',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'baseDateIndex',
                        title: 'Base index',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'baseLevel',
                        title: 'Base level',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'counterDateIndex',
                        title: 'Counter index',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'counterLevel',
                        title: 'Counter level',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'angle',
                        title: 'Angle',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'startDateIndex',
                        title: 'Start',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'endDateIndex',
                        title: 'End',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'isOpen',
                        title: 'Is open',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'value',
                        title: 'Value',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }
                ];
            }

            function showDataItemOnChart(dataItem) {
                if (dataItem) {
                    var trendline = self.data.getTrendline(dataItem.id);
                    trendline.svgPath.style.visibility = 'visible';
                }
            }

            function hideDataItemOnChart(dataItem) {
                if (dataItem) {
                    var trendline = self.data.getTrendline(dataItem.id);
                    if (trendline) trendline.svgPath.style.visibility = 'hidden';
                }
            }

            function scrollToItem(dataItem) {
                var baseDateIndex = dataItem.baseDateIndex;
                var leftOffset = self.ui.getSvgLeftOffset();
                var chartContainer = self.ui.getChartContainer();
                var width = $(chartContainer).width();

                var firstVisibleItem = self.data.findItemByLeft(-leftOffset);
                var lastVisibleItem = self.data.findItemByLeft(-leftOffset + width);

                if (dataItem.startDateIndex < firstVisibleItem.index) {
                    self.ui.scrollToXByItem(dataItem.startDateIndex, -100);
                } else if (dataItem.startDateIndex > lastVisibleItem.index) {
                    self.ui.scrollToXByItem(dataItem.startDateIndex, -100);
                }

            }

            return {
                getData: getData,
                getColumns: getColumns,
                showDataItemOnChart: showDataItemOnChart,
                hideDataItemOnChart: hideDataItemOnChart,
                scrollToItem: scrollToItem
            }

        })();

        var trendHitsFactory = (function () {
            var svgObjects = [];
            var previewFill = STOCK.CONFIG.trendlines.previewFill;

            function getData() {
                var result = [];
                var arr = [];
                var trendlines = self.data.getTrendlines();
                trendlines.forEach(function (item) {
                    var trendline = item.trendline;
                    var trendHits = trendline.getAllTrendHits();
                    trendHits.forEach(function (hit) {
                        arr[hit.id] = hit;
                    });
                });

                arr.forEach(function (item) {
                    result.push({
                        id: item.id,
                        trendlineId: item.trendRange.trendline.id,
                        trendRangeId: item.trendRange.id,
                        value: item.value.toFixed(2),
                        startDateIndex: item.extremumGroup.dates.start,
                        endDateIndex: item.extremumGroup.dates.end,
                        extremumValue: item.extremumGroup.getValue().toFixed(2),
                        gap: item.evaluation.gap.toFixed(4),
                        relativeGap: (item.evaluation.relativeGap * 100).toFixed(2),
                        pointsForDistance: item.evaluation.pointsForDistance.toFixed(2),
                        pointsForValue: item.evaluation.pointsForValue.toFixed(2)
                    });
                });

                return result;

            }

            function getColumns() {

                return [
                    {
                        index: 'id',
                        title: 'Id',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'trendlineId',
                        title: 'Trendline',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'trendRangeId',
                        title: 'Trend range',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'value',
                        title: 'Value',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'startDateIndex',
                        title: 'Start',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'endDateIndex',
                        title: 'End',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'extremumValue',
                        title: 'Extremum value',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'gap',
                        title: 'Gap',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'relativeGap',
                        title: 'Gap [%]',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'pointsForDistance',
                        title: 'Distance points',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'pointsForValue',
                        title: 'Value points',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }
                ];
            }

            function showDataItemOnChart(dataItem) {
                if (dataItem) {
                    var trendlineWrapper = self.data.getTrendline(dataItem.trendlineId);
                    if (trendlineWrapper) {
                        if (trendlineWrapper.svgPath) trendlineWrapper.svgPath.style.visibility = 'visible';
                        var trendline = trendlineWrapper.trendline;
                        if (trendline) {
                            var trendHit = trendline.getTrendHitById(dataItem.trendRangeId, dataItem.id);
                            if (trendHit) {
                                var range = trendHit.extremumGroup.dates;
                                var items = [];

                                for(var i = range.start; i <= range.end; i++){
                                    var item = self.data.getItem(i);
                                    if (item) items.push(item);
                                }
                                var coordinates = getCoordinatesFromItemsSet(items);
                                var rectangle = mielk.svg.createRectangle(coordinates.width, coordinates.height, coordinates.left, coordinates.top - 5, previewFill);
                                self.ui.getPreviewSvg().appendChild(rectangle);
                                svgObjects.push(rectangle);

                            }
                        }
                    }
                }
            }

            function hideDataItemOnChart(dataItem) {
                if (dataItem) {
                    var trendlineWrapper = self.data.getTrendline(dataItem.trendlineId);
                    if (trendlineWrapper && trendlineWrapper.svgPath) {
                        trendlineWrapper.svgPath.style.visibility = 'hidden';
                    }
                }

                svgObjects.forEach(function (object) {
                    var parent = object.parentNode;
                    if (parent) {
                        parent.removeChild(object);
                    }
                });
                svgObjects.length = 0;

            }

            function scrollToItem(dataItem) {
                var x = 1;
            }

            return {
                getData: getData,
                getColumns: getColumns,
                showDataItemOnChart: showDataItemOnChart,
                hideDataItemOnChart: hideDataItemOnChart,
                scrollToItem: scrollToItem
            }

        })();

        var trendBreaksFactory = (function () {
            var svgObjects = [];
            var previewFill = STOCK.CONFIG.trendlines.previewFill;

            function getData() {
                var result = [];
                var arr = [];
                var trendlines = self.data.getTrendlines();
                trendlines.forEach(function (item) {
                    var trendline = item.trendline;
                    var trendBreaks = trendline.getAllTrendBreaks();
                    trendBreaks.forEach(function (tb) {
                        arr[tb.id] = tb;
                    });
                });

                arr.forEach(function (item) {
                    result.push({
                        id: item.id,
                        trendlineId: item.trendRange.trendline.id,
                        trendRangeId: item.trendRange.id,
                        value: item.value.toFixed(2),
                        dateIndex: item.index,
                        fromAbove: item.fromAbove,
                        breakDayAmplitude: (item.evaluation.breakDayAmplitude * 100).toFixed(2),
                        previousDayPoints: (item.evaluation.previousDayPoints * 100).toFixed(2),
                        nextDaysMinDistancePoints: (item.evaluation.nextDaysMinDistancePoints * 100).toFixed(2),
                        nextDaysMaxVariancePoints: (item.evaluation.nextDaysMaxVariancePoints * 100).toFixed(2)
                    });
                });

                return result;

            }

            function getColumns() {
                return [
                    {
                        index: 'id',
                        title: 'Id',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'trendlineId',
                        title: 'Trendline',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'trendRangeId',
                        title: 'Trend range',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'value',
                        title: 'Value',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'dateIndex',
                        title: 'Index',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'fromAbove',
                        title: 'From above (?)',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'breakDayAmplitude',
                        title: 'Break day',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'previousDayPoints',
                        title: 'Previous day',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'nextDaysMinDistancePoints',
                        title: 'Min distance',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'nextDaysMaxVariancePoints',
                        title: 'Max variance',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }
                ];
            }

            function showDataItemOnChart(dataItem) {
                if (dataItem) {
                    var trendlineWrapper = self.data.getTrendline(dataItem.trendlineId);
                    if (trendlineWrapper) {
                        if (trendlineWrapper.svgPath) trendlineWrapper.svgPath.style.visibility = 'visible';
                        var trendline = trendlineWrapper.trendline;
                        if (trendline) {
                            var trendBreak = trendline.getTrendBreakById(dataItem.trendRangeId, dataItem.id);
                            if (trendBreak) {
                                var dateIndex = trendBreak.index;
                                var items = [];

                                for (var i = dateIndex - 2; i <= (dateIndex + 2); i++) {
                                    var item = self.data.getItem(i);
                                    if (item) items.push(item);
                                }

                                var coordinates = getCoordinatesFromItemsSet(items);
                                var rectangle = mielk.svg.createRectangle(coordinates.width, coordinates.height, coordinates.left, coordinates.top - 5, previewFill);
                                self.ui.getPreviewSvg().appendChild(rectangle);
                                svgObjects.push(rectangle);

                            }
                        }
                    }
                }
            }

            function hideDataItemOnChart(dataItem) {
                if (dataItem) {
                    var trendlineWrapper = self.data.getTrendline(dataItem.trendlineId);
                    if (trendlineWrapper && trendlineWrapper.svgPath) {
                        trendlineWrapper.svgPath.style.visibility = 'hidden';
                    }
                }

                svgObjects.forEach(function (object) {
                    var parent = object.parentNode;
                    if (parent) {
                        parent.removeChild(object);
                    }
                });
                svgObjects.length = 0;

            }

            function scrollToItem(dataItem) {
                var x = 1;
            }

            return {
                getData: getData,
                getColumns: getColumns,
                showDataItemOnChart: showDataItemOnChart,
                hideDataItemOnChart: hideDataItemOnChart,
                scrollToItem: scrollToItem
            }

        })();

        var trendRangesFactory = (function () {
            var svgObjects = [];
            var previewFill = STOCK.CONFIG.trendlines.previewFill;

            function getData() {
                var result = [];
                var arr = [];
                var trendlines = self.data.getTrendlines();
                trendlines.forEach(function (item) {
                    var trendline = item.trendline;
                    var trendRanges = trendline.trendRanges;
                    trendRanges.forEach(function (range) {
                        arr.push(range);
                    });
                });

                arr.forEach(function (item) {
                    result.push({
                        id: item.id,
                        trendlineId: item.trendline.id,
                        isPeak: item.isPeak ? false : true,
                        value: item.value.toFixed(2),
                        baseIndex: (item.base.TrendBreak ? item.base.index : item.base.extremumGroup.master.price.dataItem.index),
                        baseType: item.base.TrendHit ? 'hit' : 'break',
                        baseValue: item.base.value.toFixed(2),
                        counterIndex: (item.counter.TrendBreak ? item.counter.index : item.counter.extremumGroup.master.price.dataItem.index),
                        counterType: item.counter.TrendHit ? 'hit' : 'break',
                        counterValue: item.counter.value.toFixed(2),
                        totalCandles: item.stats.totalCandles,
                        extremumPriceCrossPenaltyPoints: item.stats.extremumPriceCross.penaltyPoints ? item.stats.extremumPriceCross.penaltyPoints.toFixed(4) : '',
                        extremumPriceCrossCounter: item.stats.extremumPriceCross.counter,
                        OCPriceCrossPenaltyPoints: item.stats.openClosePriceCross.penaltyPoints ? item.stats.openClosePriceCross.penaltyPoints.toFixed(4) : '',
                        OCPriceCrossCounter: item.stats.openClosePriceCross.counter,
                        averageVariation: item.stats.variation.average.toFixed(2),
                        extremumVariation: item.stats.variation.extremum.toFixed(2),
                        openCloseVariation: item.stats.variation.openClose.toFixed(2)
                        
                    });
                });

                return result;

            }

            function getColumns() {
                return [
                    {
                        index: 'id',
                        title: 'Id',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'trendlineId',
                        title: 'Trendline',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: { header: true }
                    }, {
                        index: 'isPeak',
                        title: 'Is peak (?)',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'center',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'value',
                        title: 'Value',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'baseIndex',
                        title: 'Base index',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'baseType',
                        title: 'Base type',
                        type: 'string',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'baseValue',
                        title: 'Base value',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'counterIndex',
                        title: 'Counter index',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'counterType',
                        title: 'Counter type',
                        type: 'string',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'counterValue',
                        title: 'Counter value',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'totalCandles',
                        title: 'Total candles',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'extremumPriceCrossPenaltyPoints',
                        title: 'Ext price cross',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'extremumPriceCrossCounter',
                        title: 'Ext price count',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'OCPriceCrossPenaltyPoints',
                        title: 'OC price cross',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'OCPriceCrossCounter',
                        title: 'OC price count',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'averageVariation',
                        title: 'Average variation',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'extremumVariation',
                        title: 'Extremum variation',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }, {
                        index: 'openCloseVariation',
                        title: 'OC variation',
                        type: 'number',
                        width: 100,
                        sortable: true,
                        align: 'center',
                        cellAlign: 'right',
                        filter: {
                            header: true
                        }
                    }
                ];
            }

            function showDataItemOnChart(dataItem) {
                if (dataItem) {
                    var trendlineWrapper = self.data.getTrendline(dataItem.trendlineId);
                    if (trendlineWrapper) {
                        if (trendlineWrapper.svgPath) trendlineWrapper.svgPath.style.visibility = 'visible';
                        (function insertHighlightRectangle() {
                            var items = [];
                            var isPeak = dataItem.isPeak;
                            var trendline = trendlineWrapper.trendline;
                            var startIndex = dataItem.baseIndex;
                            var endIndex = dataItem.counterIndex;

                            for (var i = startIndex; i <= endIndex; i++) {
                                var item = self.data.getItem(i);
                                if (item) items.push(item);
                            }

                            
                            var trendlineTouchPoints = {
                                start: {
                                    index: items[0].index,
                                    price: trendline.calculatePriceForDateIndex(items[0].index)
                                },
                                end: {
                                    index: items[items.length - 1].index,
                                    price: trendline.calculatePriceForDateIndex(items[items.length - 1].index)
                                }
                            };

                            function calculateTouchPointY(item, price) {
                                var high = item.item.price.quotation.high;
                                var low = item.item.price.quotation.low;
                                var shadowTop = item.coordinates.shadowTop;
                                var shadowBottom = item.coordinates.shadowBottom;
                                var ratio = ((high - low) > 0 ? (high - price) / (high - low) : 0)
                                var y = ratio * (shadowBottom - shadowTop) + shadowTop;
                                return y;
                            }

                            trendlineTouchPoints.start.x = items[0].coordinates.middle;
                            trendlineTouchPoints.start.y = calculateTouchPointY(items[0], trendlineTouchPoints.start.price);
                            trendlineTouchPoints.end.x = items[items.length - 1].coordinates.middle;
                            trendlineTouchPoints.end.y = calculateTouchPointY(items[items.length - 1], trendlineTouchPoints.end.price);

                            var d = 'M ' + trendlineTouchPoints.end.x + ' ' + trendlineTouchPoints.end.y + 
                                    'L' + trendlineTouchPoints.start.x + ' ' + trendlineTouchPoints.start.y;
                            items.forEach(function (item) {
                                var c = item.coordinates;
                                var y = (isPeak ? c.bodyTop : c.bodyBottom);
                                if (y) {
                                    var startX = (item.index === trendlineTouchPoints.start.index ? c.middle : c.left);
                                    var endX = (item.index === trendlineTouchPoints.end.index ? c.middle : c.right);
                                    d += 'L' + startX + ' ' + y + 'L' + endX + ' ' + y;
                                }
                            });
                            d += 'Z';

                            var path = mielk.svg.createPath(d);
                            path.style.fill = previewFill;
                            path.style.strokeWidth = 0;
                            self.ui.getPreviewSvg().appendChild(path);
                            svgObjects.push(path);

                        })();
                    }
                }
            }

            function hideDataItemOnChart(dataItem) {
                if (dataItem) {
                    var trendlineWrapper = self.data.getTrendline(dataItem.trendlineId);
                    if (trendlineWrapper && trendlineWrapper.svgPath) {
                        trendlineWrapper.svgPath.style.visibility = 'hidden';
                    }
                }

                svgObjects.forEach(function (object) {
                    var parent = object.parentNode;
                    if (parent) {
                        parent.removeChild(object);
                    }
                });
                svgObjects.length = 0;

            }

            function scrollToItem(dataItem) {
                var x = 1;
            }

            return {
                getData: getData,
                getColumns: getColumns,
                showDataItemOnChart: showDataItemOnChart,
                hideDataItemOnChart: hideDataItemOnChart,
                scrollToItem: scrollToItem
            }

        })();;

        function getCoordinatesFromItemsSet(items) {
            var coordinates = {};

            coordinates.left = items[0].coordinates.left - 1;
            coordinates.right = items[items.length - 1].coordinates.right + 3;
            coordinates.top = items[0].coordinates.shadowTop;
            coordinates.bottom = items[0].coordinates.shadowBottom;

            items.forEach(function (item) {
                if (item.coordinates.shadowTop < coordinates.top) coordinates.top = item.coordinates.shadowTop;
                if (item.coordinates.shadowBottom > coordinates.bottom) coordinates.bottom = item.coordinates.shadowBottom;
            });
            coordinates.width = coordinates.right - coordinates.left;
            coordinates.height = coordinates.bottom - coordinates.top + 10;

            return coordinates;

        }

        function changeRowSelection(row) {
            var rowIndex = (row ? row.index : -1);
            var currentIndex = (grid.selectedRow ? grid.selectedRow.index : -1);
            if (rowIndex === currentIndex) {
                grid.selectedRow = null;
                grid.activeDataItem = null;
                setTimeout(unselectRow(row), 0);
            } else {
                unselectRow(grid.selectedRow);
                grid.selectedRow = row;
                grid.selectedDataItem = grid.object.store.dataView[row.index].data;
                setTimeout(markRowAsSelected(row), 0);
                setTimeout(factory.showDataItemOnChart(grid.selectedDataItem), 0);
            }
        }

        function activateRow(row) {
            setTimeout(deactivateCurrentRow(), 0);
            grid.activeRow = row;
            grid.activeDataItem = grid.object.store.dataView[row.index].data;
            setTimeout(highlightRow(grid.activeRow), 0);
            setTimeout(factory.showDataItemOnChart(grid.activeDataItem), 0);
            setTimeout(factory.scrollToItem(grid.activeDataItem), 0);
        }

        function deactivateCurrentRow() {
            if (grid.activeRow === grid.selectedRow) {
            } else {
                setTimeout(unhighlightRow(grid.activeRow), 0);
                setTimeout(factory.hideDataItemOnChart(grid.activeDataItem), 0);
            }
        }

        function highlightRow(row) {
            row.cells.forEach(function (cell) {
                cell.classList.add('highlighted-grid-cell');
            });
        }

        function unhighlightRow(row) {
            if (row) {
                row.cells.forEach(function (cell) {
                    cell.classList.remove('highlighted-grid-cell');
                });
            }
        }

        function markRowAsSelected(row) {
            if (row) {
                row.cells.forEach(function (cell) {
                    cell.classList.add('selected-grid-cell');
                });
            }
        }

        function unmarkRowAsSelected(row) {
            if (row) {
                row.cells.forEach(function (cell) {
                    cell.classList.remove('selected-grid-cell');
                });
            }
        }

        function unselectRow(row) {
            if (row) {
                unmarkRowAsSelected(row);
                grid.selectedRow === null;
                setTimeout(factory.hideDataItemOnChart(grid.selectedDataItem), 0);
            }
        }


        return {
            isOpen: isOpen,
            show: show,
            hide: hide
        }

    })();


    self.layers = (function () {
        var extremaVisible = self.parent.parent.optionPanel.extremaVisible();
        var trendlinesVisible = self.parent.parent.optionPanel.trendlinesVisible();
        var adxVisible = self.parent.parent.optionPanel.adxVisible();
        var macdVisible = self.parent.parent.optionPanel.macdVisible();

        //Events.
        (function bindToParent() {
            self.parent.bind({
                changeShowPeaksSetting: function (e) {
                    extremaVisible = e.value;
                    self.ui.showHideExtremaSvg(extremaVisible);
                },
                changeShowTrendlinesSetting: function (e) {
                    trendlinesVisible = e.value;
                    self.ui.showHideTrendlinesSvg(trendlinesVisible);
                },
                changeShowADXSetting: function (e) {
                    adxVisible = e.value;
                    self.ui.showHideTrendlinesSvg(adxVisible);
                },
                changeShowMACDSetting: function (e) {
                    macdVisible = e.value;
                    self.ui.showHideTrendlinesSvg(macdVisible);
                }
            });
        })();

        function extremaVisible() {
            return extremaVisible;
        }

        function trendlinesVisible() {
            return trendlinesVisible;
        }
        
        function adxVisible() {
            return adxVisible;
        }

        function macdVisible() {
            return macdVisible;
        }


        return {
            extremaVisible: extremaVisible,
            trendlinesVisible: trendlinesVisible,
            adxVisible: adxVisible,
            macdVisible: macdVisible
        }

    })();

}

Chart.prototype.bind = function (e) {
    $(this).bind(e);
}
Chart.prototype.trigger = function (e) {
    $(this).trigger(e);
}