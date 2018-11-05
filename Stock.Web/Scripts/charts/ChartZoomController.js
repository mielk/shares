﻿//Each object of [Chart] class represents a chart (all div's required for a single chart)
//for a single timeframe.
function ChartZoomController(parent, params) {

    'use strict';

    //[Meta].
    var self = this;
    self.ChartZoomController = true;
    self.parent = parent;
    self.isInitialized = false;


    self.params = (function () {
        var itemWidth = params.itemWidth;
        var index = params.index;
        var timeframe = params.timeframe;
        var isActive = false;


        //Setters
        function setIsActive(value) {
            if (isActive != value) {
                isActive = value;
                self.trigger({
                    type: (value ? 'activate' : 'deactivate')
                });
            }
        }

        /* Other properties should not be changed after being set during object initialization */


        //Getters
        function getItemWidth(){
            return itemWidth;
        }

        function getIndex(){
            return index;
        }

        function getTimeframe() {
            return timeframe;
        }

        function getIsActive(){
            return isActive;
        }


        return {
            setIsActive: setIsActive,
            getItemWidth: getItemWidth,
            getIndex: getIndex,
            getTimeframe: getTimeframe,
            getIsActive: getIsActive
        }

    })();


    self.events = (function () {

        (function bindToParent() {
            self.parent.bind({
                zoom: function (e) {
                    self.params.setIsActive(e.activeChartZoomController === self)
                },
                changeShowPeaksSetting: function (e) {
                    self.trigger({
                        type: 'changeShowPeaksSetting',
                        value: e.value
                    });
                },
                changeShowTrendlinesSetting: function (e) {
                    self.trigger({
                        type: 'changeShowTrendlinesSetting',
                        value: e.value
                    });
                },
                changeShowADXSetting: function (e) {
                    self.trigger({
                        type: 'changeShowADXSetting',
                        value: e.value
                    });
                },
                changeShowMACDSetting: function (e) {
                    self.trigger({
                        type: 'changeShowMACDSetting',
                        value: e.value
                    });
                }
            });
        })();

        function triggerHorizontalSlideEvent(x) {
            if (x) {
                var left = self.dates.getLeftOffset() + x;
                if (left <= 0) {
                    self.trigger({
                        type: 'horizontalSlide',
                        left: left
                    });
                }
            }
        }

        function triggerScrollToX(left) {
            if (left !== undefined && left !== null) {
                self.dates.scrollToX(left);
                self.trigger({
                    type: 'scrollToX',
                    left: left
                });

            }
        }

        return {
            triggerHorizontalSlideEvent: triggerHorizontalSlideEvent,
            triggerScrollToX: triggerScrollToX
        }

    })();


    self.data = (function () {
        var dataSetInfo;
        var items;
        var valueRanges;
        var trendlines;


        //Setters.
        function setDataSetInfo(value) {
            dataSetInfo = value;
        }

        function setItems(value) {
            createItemsArray(value);
            addSpareItems();
            self.layout.appendXCoordinates(items);
            self.dates.renderDatesLine();
        }

        function createItemsArray(value) {
            var timeframe = self.params.getTimeframe();
            var item;
            var date;

            items = [];
            for (var i = dataSetInfo.startIndex; i <= dataSetInfo.endIndex; i++) {
                if (value[i]) {
                    item = value[i];
                    date = item.date;
                } else {
                    item = null;
                    date = timeframe.next(date);
                }

                items[i] = {
                    index: i,
                    date: date,
                    item: item
                }
            }
        }

        function setValueRanges(value) {
            valueRanges = value;
            self.charts.insertCharts();
        }

        function setTrendlines(chartType, value) {
            if (trendlines === undefined) {
                trendlines = {};
            }
            trendlines[chartType] = createTrendlinesArray(value, chartType);
            self.layout.appendXCoordinatesForTrendlines(trendlines[STOCK.INDICATORS.PRICE]);
        }

        function createTrendlinesArray(trendlines, chartType) {
            var arr = [];
            var counter = 0;
            trendlines.forEach(function (trendline) {
                arr[counter++] = {
                    trendline: trendline,
                    chartType: chartType
                }
            });
            return arr;
        }




        //Getters.
        function getDataSetInfo() {
            return dataSetInfo;
        }

        function getStartIndex(){
            return dataSetInfo.startIndex;
        }

        function getLastQuotationIndex(){
            return dataSetInfo.endIndex;
        }

        function getItems() {
            return items;
        }

        function getValueRange(chartType) {
            return valueRanges[chartType.name];
        }

        function getTrendlines(chartType) {
            return trendlines[chartType];
        }



        //Other operations.
        function addSpareItems() {
            var visibleCandles = self.layout.countVisibleCandles();
            var date = items[items.length - 1].date;
            var timeframe = self.params.getTimeframe();
            for (var i = 1; i <= visibleCandles; i++) {
                var index = dataSetInfo.endIndex + i;
                date = timeframe.next(date);
                items[index] = {
                    index: index,
                    date: date
                };
            }
        }


        return {
            setDataSetInfo: setDataSetInfo,
            setItems: setItems,
            setValueRanges: setValueRanges,
            setTrendlines: setTrendlines,
            getDataSetInfo: getDataSetInfo,
            getStartIndex: getStartIndex,
            getItems: getItems,
            getValueRange: getValueRange,
            getTrendlines: getTrendlines,
            getLastQuotationIndex: getLastQuotationIndex
        }

    })();


    self.ui = (function () {
        var parentContainer = document.getElementById('charts-container');
        var chartsContainer;


        //[Inserting components]
        function insertChartsContainer() {
            chartsContainer = $('<div/>', {
                'class': 'chart-zoom-meta-container'
            }).appendTo(parentContainer)[0];
        }


        //[Getters]
        function getVisibleWidth() {
            if (chartsContainer === undefined) insertChartsContainer();
            return $(chartsContainer).width();
        }

        function getChartsContainer() {
            if (chartsContainer === undefined) insertChartsContainer();
            return chartsContainer;
        }


        return {
            getChartsContainer: getChartsContainer,
            getVisibleWidth: getVisibleWidth
        }

    })();


    self.layout = (function () {
        var itemWidth = self.params.getItemWidth();
        var spaceShare = STOCK.CONFIG.candle.space;
        var candleWidth = itemWidth * (1 - spaceShare);
        var candlePadding = itemWidth * spaceShare / 2;
        //----------------------------------------------------------------------

        function countVisibleCandles() {
            var candleWidth = self.params.getItemWidth();
            var visibleWidth = self.ui.getVisibleWidth();
            return Math.ceil(visibleWidth / candleWidth);
        }

        function appendXCoordinates(items) {
            items.forEach(function (item) {
                var left = getX(item.index);
                var right = left + candleWidth;
                var middle = (left + right) / 2;

                item.coordinates = {
                    left: Math.round(left),
                    width: Math.round(right - left)
                };
                item.coordinates.right = item.coordinates.left + item.coordinates.width;
                item.coordinates.middle = item.coordinates.left + (item.coordinates.width - 1) / 2;

            });
        }

        function appendXCoordinatesForTrendlines(trendlines) {
            var leftOffset = STOCK.CONFIG.trendlines.leftOffset;
            var closedRightOffset = STOCK.CONFIG.trendlines.closedRightOffset;
            var openRightOffset = STOCK.CONFIG.trendlines.openRightOffset;
            var lastQuotationIndex = self.data.getLastQuotationIndex();

            trendlines.forEach(function (item) {
                var trendline = item.trendline;

                item.footholds = {
                    start: trendline.range.start - leftOffset,
                    end: trendline.range.end ? trendline.range.end + closedRightOffset : lastQuotationIndex + openRightOffset
                }

                //item.coordinates = {
                //    baseX: getX(item.footholds.start),
                //    counterX: getX(item.footholds.end)
                //}
                
            });
        }

        //Helper methods.
        function getX(value) {
            var candlesFromFirst = value - self.data.getStartIndex();
            return candlesFromFirst * itemWidth + candlePadding;
        }

        return {
            countVisibleCandles: countVisibleCandles,
            appendXCoordinates: appendXCoordinates,
            appendXCoordinatesForTrendlines: appendXCoordinatesForTrendlines
        }

    })();


    self.dates = (function () {
        var datesContainer = document.getElementById('date-line-content');
        var verticalGridLinesContainer;
        var labelsContainer;
        var crossHair;
        var currentDateLabel;


        //[Getters]
        function getLeftOffset() {
            return $(verticalGridLinesContainer).position().left;
        }


        function renderDatesLine() {
            var items = self.data.getItems();
            var timeframe = self.params.getTimeframe();
            var prevDate = null;
            var left = 0;

            if (labelsContainer === undefined) insertLabelsContainer();
            if (verticalGridLinesContainer === undefined) insertVerticalGridLinesContainer();
            if (crossHair === undefined) insertCrossHair();
            if (currentDateLabel === undefined) insertCurrentDateLabel();

            items.forEach(function (item) {
                var date = item.date;
                var period = timeframe.getPeriodLabelChange(prevDate, date);
                if (period.periodChanged) {
                    addPeriodSeparator(item.coordinates.left, period.periodLabel);
                }
                left = item.coordinates.left;
                prevDate = date;
            });

            //Resize labels container.
            $(labelsContainer).css('width', (left + 1) + 'px');

        }

        //[Actual render functions]
        function insertLabelsContainer() {
            labelsContainer = $('<div/>', {
                'class': 'date-labels-container',
                'id': 'date-labels-container'
            }).appendTo(datesContainer)[0];
        }

        function insertVerticalGridLinesContainer() {
            verticalGridLinesContainer = $('<div/>', {
                'class': 'vertical-grid-lines',
                'id': 'vertical-grid-lines'
            }).appendTo(self.ui.getChartsContainer())[0];
        }

        function insertCrossHair() {
            crossHair = $('<div/>', {
                'class': 'crosshair-vertical-line'
            }).appendTo(verticalGridLinesContainer)[0];
        }

        function insertCurrentDateLabel() {
            currentDateLabel = $('<div/>', {
                'class': 'current-data-label'
            }).css({
                visibility: 'hidden'
            }).appendTo(labelsContainer)[0];
        }


        function addPeriodSeparator(x, label) {
            var verticalLine = $('<div/>', {
                'class': 'date-vertical-line'
            }).css({
                'left': x + 'px'
            }).appendTo(verticalGridLinesContainer)[0];

            var ticker = $('<div/>', {
                'class': 'date-label-ticker'
            }).css({
                'left': x + 'px'
            }).appendTo(labelsContainer)[0];

            var label = $('<div/>', {
                'class': 'date-label',
                'html': label
            }).appendTo(labelsContainer)[0];
            var width = $(label).width();
            $(label).css('left', (x - width / 2) + 'px');

        }


        //[Actions]
        function updateCurrentDateIndicators(x, date, index) {
            var left = x - $(verticalGridLinesContainer).offset().left;
            var labelLeft = Math.max(0, left - $(currentDateLabel).width() / 2);
            var caption = mielk.dates.toString(date, false) + '<br/>[' + index + ']';

            $(crossHair).css({
                left: left + 'px',
                visibility: 'visible'
            });
            $(currentDateLabel).html(caption).css({
                visibility: 'visible',
                left: labelLeft + 'px'
            });
        }

        function hideCurrentDateIndicators() {
            $(crossHair).css({
                visibility: 'hidden'
            });
            $(currentDateLabel).css({
                visibility: 'hidden'
            });
        }

        function scrollToX(left) {
            return $(verticalGridLinesContainer).css('left', left + 'px');
        }

        //[Events]
        (function bindEvents() {
            self.bind({
                horizontalSlide: function (e) {
                    var left = e.left;
                    $(verticalGridLinesContainer).css('left', left + 'px');
                    $(labelsContainer).css('left', left + 'px');
                }
            });
        })();


        return {
            renderDatesLine: renderDatesLine,
            updateCurrentDateIndicators: updateCurrentDateIndicators,
            hideCurrentDateIndicators: hideCurrentDateIndicators,
            getLeftOffset: getLeftOffset,
            scrollToX: scrollToX
        }

    })();


    self.charts = (function () {
        var valuesChart;
        var adxChart;
        var macdChart;

        function insertCharts() {
            insertValuesChart();
            insertAdxChart();
            insertMacdChart();
        }

        function insertValuesChart() {
            var type = STOCK.INDICATORS.PRICE; 
            valuesChart = new Chart(self, {
                type: type,
                timeframe: self.params.getTimeframe(),
                itemWidth: self.params.getItemWidth()
            });
            valuesChart.data.setData(self.data.getDataSetInfo(), self.data.getItems(), self.data.getTrendlines(type), self.data.getValueRange(type));
        }

        function insertAdxChart() {
            if (self.parent.params.getShowAdx()) {
                var x = 1 / 0;
            }
        }

        function insertMacdChart() {
            if (self.parent.params.getShowMacd()) {
                var x = 1 / 0;
            }
        }

        return {
            insertCharts: insertCharts
        }

    })();


}

ChartZoomController.prototype.bind = function (e) {
    $(this).bind(e);
}
ChartZoomController.prototype.trigger = function (e) {
    $(this).trigger(e);
}