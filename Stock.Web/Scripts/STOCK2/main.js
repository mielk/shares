function Main() {

    'use strict';

    //Meta variables.
    var self = this;
    self.Main = true;

    //Modules.
    var modules = {};

    //UI
    var controls = {}


    function loadControls() {
        controls.chartButton = document.getElementById('charts-menu');
        //controls.analysisButton = document.getElementById('analysis-menu');
        //controls.simulationButton = document.getElementById('simulation-menu');
        controls.contentContainer = document.getElementById('content-container');
    }

    function assignEvents() {
        $(controls.chartButton).bind({
            click: function (e) {
                runChartModule();
            }
        });

        $(controls.analysisButton).bind({
            click: function (e) {
                runAnalysisModule();
            }
        });

        $(controls.simulationButton).bind({
            click: function (e) {
                runSimulationModule();
            }
        });

    }

    function runChartModule() {
        modules.chart = modules.chart || new Module({
            name: 'chart',
            controller: new ChartController({
                optionsPanelId: 'options-panel',
                companyDropdownId: 'company-dropdown',
                timeframeDropdownId: 'timeframe-dropdown',
                showPeaksCheckboxId: 'show-peaks-checkbox',
                showTrendlinesCheckboxId: 'show-trendlines-checkbox',
                showMACDCheckboxId: 'show-macd-checkbox',
                showADXCheckboxId: 'show-adx-checkbox',
                chartsContainerId: 'charts-container',
                simulationId: 0
            }),
            run: function (e) {
                e.run();
            },
            divId: 'charts-panel'
        });
        modules.chart.run();
    }

    function runAnalysisModule() {
        modules.analysis = modules.analysis || new Module({
            name: 'analysis',
            controller: new AnalysisController({
                runAnalysisButtonId: 'run-analysis-button',
                infoPanelId: 'info-panel'
            }),
            run: function (e) {
                e.run();
            },
            divId: 'analysis-panel'
        });
        modules.analysis.run();
    }

    function initialize() {
        loadControls();
        assignEvents();
        runChartModule();
    }


    //Private classes.
    function Module(params){
        var self = this;
        self.Module = true;
        self.name = params.name;
        self.controller = params.controller;
        self.runFn = params.run;
        self.divId = params.divId;
        self.div = document.getElementById(self.divId);

        self.hideOtherPanels = function () {
            $(controls.contentContainer).children().each(function () {
                if (this.id === self.divId) {
                    $(this).css({
                        'display': 'block',
                        'visibility': 'visible'
                    });
                } else {
                    $(this).css({
                        'display': 'none',
                        'visibility': 'hidden'
                    });
                }
            });
        }

        self.run = function () {
            //Hide other panels and show this one.
            self.hideOtherPanels();
            self.runFn(self.controller);
        }

    }


    //Public API.
    self.initialize = initialize;

}

$(function () {
    window.Main = new Main();
    Main.initialize();
});
