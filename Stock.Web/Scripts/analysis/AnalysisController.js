/*
 * Controller for managing charts.
 */
function AnalysisController(params) {
    var self = this;
    self.AnalysisController = true;
    self.fromScratch = params.fromScratch || false; //Zmienić, żeby odczytywało z check boxa.

    var controls = {}

    function loadControls() {
        controls.runAnalysisButton = document.getElementById(params.runAnalysisButtonId);
        controls.infoPanel = document.getElementById(params.infoPanelId);
    }

    function assignEvents() {
        $(controls.runAnalysisButton).bind({
            click: function (e) {
                runAnalysis({

                });
            }
        });
    }


    function arrayToString(arr, items) {
        var str = '';
        var startIndex = Math.max(0, arr.length - items);
        var endIndex = arr.length - 1;
        for (var i = startIndex; i <= endIndex; i++) {
            str += arr[i] + '\n';
        }
        return str;
    }
    

    function runAnalysis(params) {
        var fromScratch = params.fromScratch || false;
        var assetsList = STOCK.COMPANIES.getList();
        var timeframes = STOCK.TIMEFRAMES.getValues();
        var infos = [];

        assetsList.forEach(function (asset) {

            timeframes.forEach(function (timeframe) {

                var info = asset.name + ' | ' + timeframe.name;
                infos.push(info);
                var text = arrayToString(infos, 5)
                $(controls.infoPanel).html(text);


                mielk.db.fetch(
                    'Process',
                    'RunProcess',
                    {
                        asset: asset.name,
                        timeframe: timeframe.symbol,
                        fromScratch: false,
                        analysisTypes: 'prices,macd'
                    },
                    {
                        async: false,
                        callback: function (result) {

                            if (result) {
                                mielk.notify.display(asset.name + ' ' + timeframe.symbol + ' successfully loaded', 1);
                            } else {
                                mielk.notify.display('Error when processing ' + asset.name + ' ' + timeframe.symbol, 0);
                            }

                        }
                    }
                );

            });

        });



    }


    function initialize() {
        loadControls();
        assignEvents();
    }


    //Public API.
    self.initialize = initialize;

}

AnalysisController.prototype = {
    bind: function (e) {
        $(this).bind(e);
    },
    trigger: function (e) {
        $(this).trigger(e);
    },
    run: function () {
        this.initialize();
    }
};