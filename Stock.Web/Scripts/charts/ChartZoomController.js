//Each object of [Chart] class represents a chart (all div's required for a single chart)
//for a single timeframe.
function ChartZoomController(parent, params) {

    'use strict';

    //[Meta].
    var self = this;
    self.ChartZoomController = true;
    self.parent = parent;


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
                }
            });
        })();

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
        }

        function createItemsArray(value) {
            items = [];
            value.forEach(function (item) {
                items[item.DateIndex] = {
                    index: item.DateIndex,
                    date: mielk.dates.fromCSharpDateTime(item.Date),
                    item: item
                }
            });
        }

        function setValueRanges(value) {
            valueRanges = value;
        }

        function setTrendlines(chartType, value) {
            if (trendlines === undefined) {
                trendlines = {};
            }
            trendlines[chartType] = value;
        }


        //Getters.
        function getDataSetInfo() {
            return dataSetInfo;
        }

        function getStartIndex(){
            return dataSetInfo.startIndex;
        }

        function getItems() {
            return items;
        }

        function getValueRange(chartType) {
            return valueRanges[chartType];
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
            getValueRange: getValueRange
        }

    })();


    self.ui = (function () {
        var chartsContainer = document.getElementById('charts-container');
        var datesContainer = document.getElementById('date-line-content');


        function getVisibleWidth() {
            return $(chartsContainer).width();
        }

        return {
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

        //Helper methods.
        function getX(value) {
            var candlesFromFirst = value - self.data.getStartIndex();
            return value * itemWidth + candlePadding;
        }

        return {
            countVisibleCandles: countVisibleCandles,
            appendXCoordinates: appendXCoordinates
        }

    })();

}

ChartZoomController.prototype.bind = function (e) {
    $(this).bind(e);
}
ChartZoomController.prototype.trigger = function (e) {
    $(this).trigger(e);
}