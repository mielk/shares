function Chart(parent, params) {

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


        //[Setters]
        function setData($dataSetInfo, $items, $valuesRange) {
            dataSetInfo = $dataSetInfo;
            items = $items;
            valuesRange = $valuesRange;
            setVisibilityRange();
            self.svg.render();
        }

        function setVisibilityRange() {
            var offset = 0.05 * (valuesRange.min + valuesRange.max) / 2;
            var min = Math.floor(valuesRange.min - offset);
            var max = Math.floor(valuesRange.max + offset);
            self.ui.setVisibleRange(min, max);
        }


        //[Getters]
        function getDataSetInfo() {
            return dataSetInfo;
        }

        function getItems() {
            return items;
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
            getItems: getItems,
            getValuesRange: getValuesRange,
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
            svgItems.style.top = '0px';
            svgItems.style.height = height + 'px';
            svgItems.style.width = width + 'px';
            chartContainer.appendChild(svgItems);

        }

        function insertSvgExtrema() {
            svgExtrema = mielk.svg.createSvg();
            var height = svgBoxHeight + svgItemsBoxOffset;
            var width = calculateItemsWidth();
            var top = (svgItemsBoxTop - svgItemsBoxOffset / 2);

            svgExtrema.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgExtrema.setAttribute('preserveAspectRatio', 'none meet');
            svgExtrema.style.top = top + 'px';
            svgExtrema.style.height = height + 'px';
            svgExtrema.style.width = width + 'px';

            chartContainer.appendChild(svgExtrema);

        }

        function insertSvgTrendlines() {
            svgTrendlines = mielk.svg.createSvg();
            var height = svgBoxHeight + svgItemsBoxOffset;
            var width = calculateItemsWidth();
            var top = (svgItemsBoxTop - svgItemsBoxOffset / 2);

            svgTrendlines.setAttribute('viewBox', '0 0 ' + width + ' ' + height);
            svgTrendlines.setAttribute('preserveAspectRatio', 'none meet');
            svgTrendlines.style.top = top + 'px';
            svgTrendlines.style.height = height + 'px';
            svgTrendlines.style.width = width + 'px';

            chartContainer.appendChild(svgItems);

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

        function getSvgItemsBoxOffset() {
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
            if (svgItems === undefined) insertSvgTrendlines();
            return svgTrendlines;
        }


        return {
            setSvgHeight: setSvgHeight,
            setVisibleRange: setVisibleRange,
            getSvgItemsBoxOffset: getSvgItemsBoxOffset,
            getSvgBoxHeight: getSvgBoxHeight,
            getItemsSvg: getItemsSvg,
            getExtremaSvg: getExtremaSvg,
            getTrendlinesSvg: getTrendlinesSvg,
            getVisibleRange: getVisibleRange
        }

    })();


    self.svg = (function () {
        var type = self.params.getType();
        var svgBoxHeight = self.ui.getSvgBoxHeight();
        var itemsSvgOffset = self.ui.getSvgItemsBoxOffset();
        var oneUnitHeight;
        var visibleRange = {};
        

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
            renderValuesAndHorizontalGridLines();
        }

        function renderItems() {
            var svg = self.ui.getItemsSvg();
            var valuesRange = self.data.getValuesRange();
            var visibleRange = self.ui.getVisibleRange();
            var items = self.data.getItems();

            function getY(value) {
                var pointsDistance = valuesRange.max - value;
                return pointsDistance * oneUnitHeight;
            }

            (function adjustSvgHeight() {
                var svgHeight = Math.ceil(visibleRange.height * (valuesRange.max - valuesRange.min) / (visibleRange.max - visibleRange.min));
                var svgTop = visibleRange.height * (visibleRange.max - valuesRange.max) / (visibleRange.max - visibleRange.min);
                self.ui.setSvgHeight(svgHeight, svgTop);
                svgBoxHeight = self.ui.getSvgBoxHeight();
            })();

            (function calculateUnitHeight() {
                var levelsDistance = valuesRange.max - valuesRange.min;
                oneUnitHeight = svgBoxHeight / levelsDistance;
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
                        var body = {
                            fill: (quotation.priceUp ? priceUpBodyInsideColor : priceDownBodyInsideColor),
                            stroke: (quotation.priceUp ? priceUpBodyBorderColor : priceDownBodyBorderColor),
                            strokeWidth: item.coordinates.width < (strokeWidth * 2 + 1) ? 0 : strokeWidth,
                            height: item.coordinates.bodyHeight,
                            width: item.coordinates.width + 1,
                            y: item.coordinates.bodyTop + 0.5,
                            x: item.coordinates.left + 0.5
                        };
                        var shadow = {
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

                $(svg).empty();
                rectangles.forEach(function (item) {
                    var rectangle = mielk.svg.createRectangle(item.width, item.height, item.x, item.y, item.fill)
                    rectangle.style.stroke = item.stroke;
                    rectangle.style.strokeWidth = item.strokeWidth + 'px';
                    rectangle.style.vectorEffect = 'non-scaling-stroke';
                    rectangle.style.shapeRendering = 'crispEdges'
                    svg.appendChild(rectangle);
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
                        x: x,
                        y: y,
                        radius: radius,
                        stroke: stroke,
                        fill: fill
                    }
                }

                items.forEach(function (item) {
                    if (item.item) {
                        var price = item.item.price;
                        if (price.hasPeak()) {
                            var value = Math.max(price.peakByClose ? price.peakByClose.Value : 0, price.peakByHigh ? price.peakByHigh.Value : 0);
                            var circle = generateCircleObject(value, false, item);
                            if (circle) circles.push(circle);
                        }

                        if (price.hasTrough()) {
                            var value = Math.max(price.troughByClose ? price.troughByClose.Value : 0, price.troughByLow ? price.troughByLow.Value : 0);
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
                });

            })();


        }

        function renderPriceTrendlines() {
        }

        function renderValuesAndHorizontalGridLines() {
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


}

Chart.prototype.bind = function (e) {
    $(this).bind(e);
}
Chart.prototype.trigger = function (e) {
    $(this).trigger(e);
}