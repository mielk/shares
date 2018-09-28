function ChartController(params) {
    var self = this;
    self.ChartController = true;

    //Properties
    self.params = (function(){
        var company = params.initialCompany || STOCK.COMPANIES.getCompany(1);
        var timeframe =  params.initialTimeframe || STOCK.TIMEFRAMES.defaultValue();
        var showPeaks = params.showPeaks || true;
        var showTrendlines = params.showTrendlines || true;
        var indicators = {
            PRICE: params.showPriceChart || true, //true,
            MACD: params.showMACDChart || false, //true,
            ADX: params.showADXChart || false //true
        };


        //Setters
        function setCompany(id) {
            company = STOCK.COMPANIES.getCompany(id);
            self.trigger({
                type: 'changeCompany',
                timeframe: timeframe,
                company: company
            });
        }

        function setTimeframe(id) {
            timeframe = STOCK.TIMEFRAMES.getItem(id);
            self.trigger({
                type: 'changeTimeframe',
                timeframe: timeframe,
                company: company
            });
        }

        function setShowPeaks(value) {
            if (showPeaks != value) {
                showPeaks = value;
                self.trigger({
                    type: 'showPeaks',
                    value: showPeaks
                });
            }
        }

        function setShowTrendlines(value) {
            if (showTrendlines != value) {
                showTrendlines = value;
                self.trigger({
                    type: 'showTrendlines',
                    value: showTrendlines
                });
            }
        }

        function setShowAdx(value) {
            if (indicators.ADX != value) {
                indicators.ADX = value;
                self.trigger({
                    type: 'showADX',
                    value: indicators.ADX
                });
            }
        }

        function setShowMacd(value) {
            if (indicators.MACD != value) {
                indicators.MACD = value;
                self.trigger({
                    type: 'showMACD',
                    value: indicators.MACD
                });
            }
        }


        //Getters
        function getCompany() {
            return company;
        }

        function getTimeframe() {
            return timeframe;
        }

        function getShowPeaks() {
            return showPeaks;
        }

        function getShowTrendlines() {
            return showTrendlines;
        }

        function getShowAdx() {
            return indicators.ADX;
        }

        function getShowMacd() {
            return indicators.MACD;
        }



        return {
            setCompany: setCompany,
            setTimeframe: setTimeframe,
            setShowPeaks: setShowPeaks,
            setShowTrendlines: setShowTrendlines,
            setShowAdx: setShowAdx,
            setShowMacd: setShowMacd,
            getCompany: getCompany,
            getTimeframe: getTimeframe,
            getShowPeaks: getShowPeaks,
            getShowTrendlines: getShowTrendlines,
            getShowAdx: getShowAdx,
            getShowMacd: getShowMacd
        }


    })();


    //Panels.
    self.optionPanel = (function (params) {

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
            $(controls.showPeaksCheckbox).prop('checked', self.params.getShowPeaks);
            $(controls.showTrendlinesCheckbox).prop('checked', self.params.getShowTrendlines);
            $(controls.showMACDCheckbox).prop('checked', self.params.getShowMACD);
            $(controls.showADXCheckbox).prop('checked', self.params.getShowADX);
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


    //Children.
    self.children = (function () {
        var chartZoomControllers = [];
        var activeChartZoomController;


        //Setters
        function setActiveChartZoomController(item) {
            activeChartZoomController = item;
            self.trigger({
                type: 'zoom',
                activeChartZoomController: item
            });
        };

        function addZoomController(index, item) {
            chartZoomControllers[index] = item;
        }


        //Getters
        function getAllControllers() {
            return chartZoomControllers.slice(0);
        }

        function getActiveChartZoomController() {
            return activeChartZoomController;
        }

        function noActiveChart() {
            return activeChartZoomController === undefined;
        }

        function getChartZoomController(index) {
            return chartZoomControllers[index];
        }


        return {
            setActiveChartZoomController: setActiveChartZoomController,
            addZoomController: addZoomController,
            getActiveChartZoomController: getActiveChartZoomController,
            getChartZoomController: getChartZoomController,
            noActiveChart: noActiveChart,
            getAllControllers: getAllControllers
        };

    })();


    //Data.
    self.data = (function () {
        var dataSetInfo;
        var data;
        var valueRanges;


        //Loaders
        function loadDataSetInfo(resultFromDb) {
            dataSetInfo = adjustDataInfo(resultFromDb.info);
            feedChildrenObjects(feedChildWithDataSetInfo);
        }

        function loadData(resultFromDb) {
            setData(resultFromDb.quotations);
            feedChildrenObjects(feedChildWithData);
        }


        //Setters
        function setDataSetInfo(value) {
            dataSetInfo = value;
        }

        function setData(value) {
            data = value;
            calculateValueRanges();
        }

        function calculateValueRanges() {
            var extremumValues = {
                price: { min: null, max: null },
                adx: { min: null, max: null },
                macd: { min: null, max: null }
            };

            data.forEach(function (item) {

                //Quotations.
                if (extremumValues.price.min === null || extremumValues.price.min > item.quotation.Low) {
                    extremumValues.price.min = item.quotation.Low;
                }
                if (extremumValues.price.max === null || extremumValues.price.max < item.quotation.High) {
                    extremumValues.price.max = item.quotation.High;
                }

                //ADX.

                //MACD.

            });

            valueRanges = extremumValues;

        }


        //Getters
        function getDataSetInfo() {
            return dataInfo;
        };

        function getData() {
            return data;
        }


        //Triggers
        function triggerLoadDataSetInfo(properties) {
            mielk.db.fetch(
                'Data',
                'GetDataSetsInfo',
                properties,
                {
                    async: false,
                    callback: function (res) {
                        loadDataSetInfo(res);
                    },
                    err: function (msg) {
                        alert(msg.status + ' | ' + msg.statusText);
                    }
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


        //Modifiers.
        function adjustDataInfo(arr) {
            return {
                counter: arr.Counter,
                endDate: mielk.dates.fromCSharpDateTime(arr.EndDate),
                endIndex: arr.EndIndex,
                startDate: mielk.dates.fromCSharpDateTime(arr.StartDate),
                startIndex: arr.StartIndex
            }
        }


        //Other methods.
        function feedChildrenObjects(fn) {
            var activeChild = self.children.getActiveChartZoomController();
            var allChildren = self.children.getAllControllers();

            fn(activeChild);
            //allChildren.forEach(function (item) {
            //    if (item !== activeChild) {
            //        fn(item);
            //    }
            //});

        }

        function feedChildWithDataSetInfo(item) {
            item.data.setDataSetInfo(dataSetInfo);
        }

        function feedChildWithData(item) {
            item.data.setItems(data);
            item.data.setValueRanges(valueRanges);
        }



        return {
            loadDataSetInfo: loadDataSetInfo,
            loadData: loadData,
            setDataSetInfo: setDataSetInfo,
            setData: setData,
            getDataSetInfo: getDataSetInfo,
            getData: getData,
            triggerLoadDataSetInfo: triggerLoadDataSetInfo,
            triggerLoadData: triggerLoadData
        };

    })();


    //Run.
    self.run = function () {
        var properties = { assetId: self.params.getCompany().id, timeframeId: self.params.getTimeframe().id };
        self.data.triggerLoadDataSetInfo(properties);
        self.data.triggerLoadData(properties);
    };


    //Initializing.
    (function initialize() {
        var step = STOCK.CONFIG.candle.svgLevelsZoom;
        var maxWidth = STOCK.CONFIG.candle.maxWidth;
        var minWidth = STOCK.CONFIG.candle.minWidth;
        var width = maxWidth;
        var index = 1;

        while (width > minWidth) {
            var chartZoomController = new ChartZoomController(self, {
                index: index,
                timeframe: self.params.getTimeframe(),
                itemWidth: width
            });
            self.children.addZoomController(index++, chartZoomController);
            width = width / step;
        }

        (function setInitialActiveChartZoomController() {
            var defaultWidth = STOCK.CONFIG.candle.defaultWidth;
            self.children.getAllControllers().forEach(function (item) {
                if (self.children.noActiveChart()) {
                    if (item.params.getItemWidth() < defaultWidth) {
                        self.children.setActiveChartZoomController(item);
                    }
                }
            });
        })();

    })();


}
ChartController.prototype.bind = function (e) {
    $(self).bind(e);
}
ChartController.prototype.trigger = function (e) {
    $(self).trigger(e);
}