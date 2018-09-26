function ChartsContainer(params) {

    'use strict';

    //[Meta]
    var self = this;
    self.ChartsContainer = true;
    var controller = params.controller;

    //[Parameters].
    self.company = params.company;
    self.timeframe = params.timeframe;

    //[Settings].
    self.settings = {
        prices: { visible: true, properties: { trendlines: params.showTrendlines, peaks: params.showPeaks }}
    };
    //self.settings[STOCK.INDICATORS.MACD.name] = { visible: params.showMACD };




    function changeCompany(_company) {
        if (self.company !== _company) {
            self.company = _company;
            reset();
            load();
        }
    }

    function changeTimeframe(_timeframe) {
        if (self.timeframe !== _timeframe) {
            self.timeframe = _timeframe;
            load();
        }
    }

    function changeSimulationId(id) {
        if (self.simulationId !== id) {
            self.simulationId = id;
        }
    }



    function load() {
        self.parentContainer = document.getElementById(params.chartContainerId);
        self.chart = new Chart(self, { type: STOCK.INDICATORS.PRICE, timeframe: self.timeframe });
        //self.valuePanel = new ChartValuePanel(self);
        //self.datetimeLine = new DateTimeLine(self);
    }

    function assignEvents() {
        controller.bind({
            changeCompany: function (e) {
                changeCompany(e.company);
            },
            changeTimeframe: function (e) {
                changeTimeframe(e.timeframe);
            },
            changeSimulation: function (e) {

            },
            showMACD: function (e) {
                self.settings[STOCK.INDICATORS.MACD.name].visible = e.value;
            },
            showADX: function (e) {
                self.settings[STOCK.INDICATORS.ADX.name].visible = e.value;
            },
            dataInfoLoaded: function (e) {
                dataInfoLoaded(e.params);
            },
            dataLoaded: function (e) {
                dataLoaded(e.params);
            }
        });
    }




    function dataInfoLoaded(info) {
        var params = {
            counter: info.Counter,
            startIndex: info.StartIndex,
            startDate: info.StartDate,
            endIndex: info.EndIndex,
            endDate: info.EndDate,
            max: info.MaxLevel,
            min: info.MinLevel,
            levelDifference: (info.MaxLevel - info.MinLevel)
        };
        self.trigger({
            type: 'dataInfoLoaded',
            params: params
        });
    }

    function dataLoaded(params) {
        var data = params.data;
        self.trigger({
            type: 'dataLoaded',
            params: { data: data }
        });
    }




    //Public API.
    self.getController = function () {
        return controller;
    };

    //Initialization.
    (function initialize() {
        load();
        assignEvents();
    })();

}
ChartsContainer.prototype.bind = function (e) {
    $(this).bind(e);
}
ChartsContainer.prototype.trigger = function (e) {
    $(this).trigger(e);
}