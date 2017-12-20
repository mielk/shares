//Each object of [Chart] class represents a chart (all div's required for a single chart)
//for a single timeframe.
function TimeframeChartsContainer(params) {

    'use strict';

    //[Meta].
    var self = this;
    self.TimeframeChartsContainer = true;

    //Properties.
    var parent = params.parent;
    var key = params.key || mielk.numbers.generateUUID();
    //self.singleItemWidth = STOCK.CONFIG.candle.width;

    //Data sets.
    var dataSet;
    var properties;
    var quotations;

    //UI.
    var controls = { };
    var timeScale;
    var charts = {};
    self.offset = {
        value: -50,
        min: -100,
        max: undefined
    }


    function initialize() {
        //Generate GUI.
        loadControls();

        //Load data set and its properties.
        dataSet = parent.company.getDataSet(parent.timeframe);
        dataSet.loadProperties(loadProperties);

        //Draw actual chart.
        //properties = {
        //    realQuotationsCounter: 3342
        //    , firstDate: mielk.dates.fromString('2005-01-04')
        //    , lastDate: mielk.dates.fromString('2017-12-08')
        //    , width: 3342 * STOCK.CONFIG.candle.width
        //};
        loadCharts();
        loadTimeScale();

        //Assign events. It must be placed here, after charts and time scale is already loaded.
        assignEvents();

        dataSet.loadQuotations(loadQuotations);

    }

    function loadControls() {
        controls.container = $('<div/>', {
            'class': 'timeframe-charts-container',
            id: 'chart-container-' + key
        }).css({
            'background-color': 'red'
        }).appendTo(params.container);

    }

    function assignEvents() {

    }

    function loadProperties($properties) {
        properties = $properties;
        properties.width = properties.realQuotationsCounter * STOCK.CONFIG.candle.width;
    }

    function loadQuotations(result) {
        quotations = result;

        //Set max offset.
        self.offset.max = result.arr.length * STOCK.CONFIG.candle.width
                            - $(controls.container).width()
                            + STOCK.CONFIG.valueScale.width;

        //Propagate to charts.
        var arrCharts = mielk.arrays.fromObject(charts);
        arrCharts.forEach(function (chart) {
            chart.loadQuotations(result);
        });

    }

    function loadTimeScale() {
        //Load date scale. This is the first moment, when this is possible, because 
        //earlier it was not known how many quotations this chart will contain.
        timeScale = new ChartTimeScale({
            parent: self
            , container: controls.container
            , counter: properties.realQuotationsCounter
            , firstDate: properties.firstDate
            , lastDate: properties.lastDate
            , width: properties.width
        });
    }

    function loadCharts() {
        addChart(STOCK.INDICATORS.PRICE, 0);
        //addChart(STOCK.INDICATORS.MACD, 1);
        //addChart(STOCK.INDICATORS.ADX, 2);
    }

    function addChart(type, index) {
        charts[type.name] = new Chart({
            parent: self,
            key: key,
            index: index,
            type: type,
            container: controls.container,
            visible: parent.settings[type.name].visible,
            height: type.initialHeight,
            width: properties.width,
            properties: parent.settings[type.name].properties
        });
    }



    function activate() {
        $(controls.container).css({
            'display': 'block',
            'visibility': 'visible'
        });
    }

    function deactivate() {
        $(controls.container).css({
            'display': 'none',
            'visibility': 'hidden'
        });
    }

    self.slide = function (offset) {
        var $offset = self.offset.value + offset;
        self.offset.value = Math.max(Math.min($offset, self.offset.max), self.offset.min);

        mielk.arrays.fromObject(charts).forEach(function (chart) {
            chart.slide(self.offset);
        });
    }

    self.scale = function () {
        mielk.arrays.fromObject(charts).forEach(function (chart) {
            chart.scale();
        });
    }

    //Public API.
    self.bind = function (e) {
        $(self).bind(e);
    }
    self.trigger = function (e) {
        $(self).trigger(e);
    }
    self.activate = activate;
    self.deactivate = deactivate;
    self.parent = parent;
    self.timeframe = function () {
        return parent.timeframe;
    }


    initialize();


}



function ChartTimeScale(params) {

    'use strict';

    var self = this;
    self.ChartTimeScale = true;
    self.parent = params.parent;

    //Properties.
    self.counter = params.counter;
    self.firstDate = params.firstDate;
    self.lastDate = params.lastDate;

    //State.
    self.scaling = {
        state: false,
        start: 0
    }

    //UI.
    var controls = {};


    function initialize() {
        loadControls();
        assignEvents();
    }

    //określić minimalną szerokość - nie może być niższa niż szerokość całego panelu.

    function loadControls() {

        var valueScaleWidth = STOCK.CONFIG.valueScale.width;

        controls.container = $('<div/>', {
            'class': 'date-scale-container'
        }).css({
            'height': STOCK.CONFIG.timeScale.height + 'px',
            'padding-right': valueScaleWidth + 'px'
        }).appendTo(params.container);

        controls.labels = $('<div/>', {
            'class': 'date-scale-labels'
        }).css({
            'right': valueScaleWidth + 'px'
        }).appendTo(controls.container);

        controls.slider = $('<div/>', {
            'class': 'date-scale-slider'
        }).css({
            'right': valueScaleWidth + 'px'
        }).appendTo(controls.container);

    }
    
    function assignEvents() {
        $(controls.slider).bind({
            mousedown: function (e) {
                self.scaling.state = true;
                self.scaling.start = e.pageX;
            },
            mouseup: function (e) {
                if (self.scaling.state) {
                    self.scaling.state = false;
                    slide(e.pageX);
                }
            },
            mousemove: function (e) {
                if (self.scaling.state) {
                    slide(e.pageX);
                }
            }
        });

        $(document).bind({
            mouseup: function (e) {
                self.scaling.state = false;
            }
        });
    }

    function slide(x) {
        var start = self.scaling.start;
        self.scaling.start = x;
        var offset = x - start;
        var newWidth = STOCK.CONFIG.candle.width - offset / 100;
        STOCK.CONFIG.candle.modifyWidth(newWidth);
        self.parent.scale();
    }



    //Public API.
    self.bind = function (e) {
        $(self).bind(e);
    };
    self.trigger = function (e) {
        $(self).trigger(e);
    };
    self.destroy = function () {
        $(controls.container).remove();
    };


    initialize();

}