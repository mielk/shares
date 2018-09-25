function ChartController(params) {
    var self = this;
    self.ChartController = true;

    //State
    var company = params.initialCompany || STOCK.COMPANIES.getCompany(1);
    var timeframe = params.initialTimeframe || STOCK.TIMEFRAMES.defaultValue();
    var showPeaks = params.showPeaks || true;
    var showTrendlines = params.showTrendlines || true;
    var indicators = {
        PRICE: params.showPriceChart || true, //true,
        MACD: params.showMACDChart || false, //true,
        ADX: params.showADXChart || false //true
    };


    //Panels.
    var optionPanel = (function (params) {

        var controls = {}

        function initialize() {
            getControls();
            updateView();
            //loadCompanyOptions();
            //loadTimeframeOptions();
            assignEvents();
        }

        function getControls() {
            controls.container = document.getElementById(params.optionPanelId);
            controls.companyDropdown = document.getElementById(params.companyDropdownId);
            controls.timeframeDropdown = document.getElementById(params.timeframeDropdownId);
            controls.showPeaksCheckbox = document.getElementById(params.showPeaksCheckboxId);
            controls.showTrendlinesCheckbox = document.getElementById(params.showTrendlinesCheckboxId);
            controls.showMACDCheckbox = document.getElementById(params.showMACDCheckboxId);
            controls.showADXCheckbox = document.getElementById(params.showADXCheckboxId);
        }

        function loadCompanyOptions() {

            var companies = params.companies || STOCK.COMPANIES.getList();

            companies.sort(function (a, b) {
                return a.name.localeCompare(b.name);
            });

            for (var iterator in companies) {
                var item = companies[iterator];
                var option = 1;
                $(controls.companyDropdown).append($('<option>', {
                    value: item.id,
                    text: item.name,
                    selected: (company && item.id === company.id ? true : false)
                }));
            }

            //Convert it into Select2.
            //$(controls.timeframeDropdown).select2();

        }

        function loadTimeframeOptions() {
            var timeframes = params.timeframes || STOCK.TIMEFRAMES.getValues();

            for (var iterator in timeframes) {
                var item = timeframes[iterator];
                var option = 1;
                $(controls.timeframeDropdown).append($('<option>', {
                    value: item.id,
                    text: item.name,
                    selected: (timeframe && item.id === timeframe.symbol ? true : false)
                }));
            }

            //Convert it into Select2.
            //$(controls.timeframeDropdown).select2();

        }

        function updateView() {
            $(controls.showPeaksCheckbox).prop('checked', showPeaks);
            $(controls.showTrendlinesCheckbox).prop('checked', showTrendlines);
            $(controls.showMACDCheckbox).prop('checked', indicators.MACD);
            $(controls.showADXCheckbox).prop('checked', indicators.ADX);
        }

        function assignEvents() {
            //[Show peaks] checkbox.
            $(controls.showPeaksCheckbox).bind({
                click: function (e) {
                    var $this = $(this);
                    changeShowPeaksSetting($this.is(':checked'));
                }
            });

            //[Show trendlines] checkbox.
            $(controls.showTrendlinesCheckbox).bind({
                click: function (e) {
                    var $this = $(this);
                    changeShowTrendlinesSetting($this.is(':checked'));
                }
            });

            //[Show ADX] checkbox.
            $(controls.showADXCheckbox).bind({
                click: function (e) {
                    var $this = $(this);
                    changeShowADXSetting($this.is(':checked'));
                }
            });

            //[Show MACD] checkbox.
            $(controls.showMACDCheckbox).bind({
                click: function (e) {
                    var $this = $(this);
                    changeShowMACDSetting($this.is(':checked'));
                }
            });

            //[Change company].
            $(controls.companyDropdown).bind({
                change: function (e) {
                    changeCompany(this.value);
                }
            });


            //[Change timeframe].
            $(controls.timeframeDropdown).bind({
                change: function (e) {
                    changeTimeframe(this.value);
                }
            });

        }

        initialize();

        return {

        };

    })(params);
    var chartContainer = null;

    //Data.
    var dataInfo = { };


    //Changing properties.
    function changeCompany(id) {
        company = STOCK.COMPANIES.getCompany(id);
        self.trigger({
            type: 'changeCompany',
            timeframe: timeframe,
            company: company
        });
    }

    function changeTimeframe(id) {
        timeframe = STOCK.TIMEFRAMES.getItem(id);
        self.trigger({
            type: 'changeTimeframe',
            timeframe: timeframe,
            company: company
        });
    }

    function changeShowPeaksSetting(_value) {
        if (showPeaks != _value) {
            showPeaks = _value;
            self.trigger({
                type: 'showPeaks',
                value: showPeaks
            });
        }
    }

    function changeShowTrendlinesSetting(_value) {
        if (showTrendlines != _value) {
            showTrendlines = _value;
            self.trigger({
                type: 'showTrendlines',
                value: showTrendlines
            });
        }
    }

    function changeShowADXSetting(_value) {
        if (indicators.ADX != _value) {
            indicators.ADX = _value;
            self.trigger({
                type: 'showADX',
                value: indicators.ADX
            });
        }
    }

    function changeShowMACDSetting(_value) {
        if (indicators.MACD != _value) {
            indicators.MACD = _value;
            self.trigger({
                type: 'showMACD',
                value: indicators.MACD
            });
        }
    }





    function initialize() {
        chartContainer = new ChartsContainer({
            controller: self,
            chartContainerId: params.chartsContainerId,
            company: company,
            timeframe: timeframe,
            showPeaks: showPeaks,
            showTrendlines: showTrendlines,
            showADX: indicators.ADX,
            showMACD: indicators.MACD
        });
    }

    function run() {
        var properties = { assetId: company.id, timeframeId: 6 }; //timeframe.id };
        triggerLoadDataInfo(properties);
        triggerLoadData(properties);
    }

    function triggerLoadDataInfo(properties) {
        mielk.db.fetch(
            'Data',
            'GetDataSetsInfo',
            properties,
            {
                async: false,
                callback: function(res){ loadDataInfo(res); },
                err: function (msg) { alert(msg.status + ' | ' + msg.statusText); }
            }
        );
    }

    function triggerLoadData(properties) {
        mielk.db.fetch(
            'Data',
            'GetDataSets',
            properties,
            {
                async: true,
                callback: function (res) { loadData(res); },
                err: function (msg) { alert(msg.status + ' | ' + msg.statusText); }
            }
        );
    }

    function loadDataInfo(resultFromDb) {
        dataInfo = resultFromDb.info;
        self.trigger({
            type: 'dataInfoLoaded',
            params: dataInfo
        });
    }

    function loadData(resultFromDb) {
        data = resultFromDb;
        self.trigger({
            type: 'dataLoaded',
            params: { data: data }
        });
    }



    //Public API.
    self.run = run;
    self.getDataInfo = function () {
        return dataInfo;
    };

    initialize();

}
ChartController.prototype.bind = function (e) {
    $(self).bind(e);
}
ChartController.prototype.trigger = function (e) {
    $(self).trigger(e);
}


//mielk.db.fetch(
//    'Data',
//    'GetDataSetsInfo',
//    properties,
//    {
//        async: false,
//        callback: function (res) {

//            if (res == null) {
//                res = {
//                    firstDate: mielk.dates.fromString('2005-01-04')
//                    , lastDate: mielk.dates.fromString('2017-12-08')
//                    , minLevel: 14.75
//                    , maxLevel: 456.5
//                    , counter: 3342
//                };
//            }

//            var arr = { firstDate: res.firstDate, lastDate: res.lastDate, minLevel: res.minLevel * 1, maxLevel: res.maxLevel * 1, counter: res.counter * 1 };

//            firstDate = arr.firstDate; //mielk.dates.fromCSharpDateTime(arr.firstDate);
//            lastDate = arr.lastDate; //mielk.dates.fromCSharpDateTime(arr.lastDate);
//            minLevel = arr.minLevel;
//            maxLevel = arr.maxLevel;


//            actualQuotationsCounter = arr.counter;

//            //Create object for quotations (with slot for each date
//            //between [firstDate] and [lastDate].
//            createQuotationsSets();
//            realQuotationsCounter = Object.keys(quotations).length;

//            //Flag this data set as having properties already loaded.
//            propertiesLoaded = true;

//            //Create [properties] object to be returns.
//            properties = {
//                firstDate: firstDate,       //The date of the first quotation
//                lastDate: lastDate,         //The date of the last quotation
//                minLevel: minLevel,         //The minimum level of the price
//                maxLevel: maxLevel,         //The maximum level of the price
//                actualQuotationsCounter: actualQuotationsCounter,   //The number of quotations in the database
//                realQuotationsCounter: realQuotationsCounter        //The expected number of quotations
//            };

//            //If function has been passed as a parameter, call it.
//            if (mielk.objects.isFunction(fn)) {
//                fn(properties);
//            }

//        },
//        err: function (msg) {
//            alert(msg.status + ' | ' + msg.statusText);
//        }
//    }
//);



//mielk.db.fetch(
//    'Data',
//    'GetDataSets',
//    params,
//    {
//        async: true,
//        callback: function (res) {

//            var _quotations = res.quotations;
//            var _trendlines = res.trendlines;


//            //Populate quotations collection.
//            assignQuotations(_quotations);

//            //If function has been passed as a parameter, call it.
//            if (mielk.objects.isFunction(fn)) {
//                fn({
//                    initial: !initialized,
//                    obj: quotations,
//                    arr: quotationsArray,
//                    trendlines: _trendlines,
//                    complete: (startIndex === 0)
//                });
//            }

//            if (startIndex > 0) {
//                fetchQuotations(fn, { initialized: true, endIndex: startIndex - 1 });
//            }

//        }
//    }
//);