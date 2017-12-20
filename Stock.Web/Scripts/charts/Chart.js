//Each object of [Chart] class represents a chart (all div's required for a single chart)
//for a single timeframe.
function Chart(parentContainer, params) {

    'use strict';

    //[Meta].
    var self = this;
    self.Chart = true;
    self.type = params.type;
    self.key = 'chart_' + params.type.name;

    //Properties.
    var parent = parentContainer;
    var controller = parent.getController();

    //UI.
    var chartDivId = 'actual-chart-container';
    var infoDivId = 'quote-info-panel';
    var svgDiv = 'chart-svg-panel-container';
    var chartDiv = document.getElementById(chartDivId);
    var infoDiv = document.getElementById(infoDivId);
    var svgDiv = document.getElementById(svgDiv);
    var svg = new SvgPanel({
        parent: self,
        key: self.key,
        container: svgDiv,
        type: self.type
    });;

    //Events.
    (function bindEvents() {
        parent.bind({
            dataInfoLoaded: function (e) {
                dataInfoLoaded(e.params);
            },
            dataLoaded: function(e){
                dataLoaded(e.params);
            }
        });
    })();

    function dataInfoLoaded(params) {
        self.trigger({
            type: 'dataInfoLoaded',
            params: { data: params }
        });
    }

    function dataLoaded(params) {
        self.trigger({
            type: 'dataLoaded',
            params: { data: params.data }
        });
    }




    //function loadControls() {
    //    controls.container = $('<div/>', {
    //        'class': 'chart-container'
    //    }).css({
    //        'background-color': params.type.color,
    //        'height': params.height + 'px'
    //    }).appendTo(params.container);


    //    controls.values = $('<div/>', {
    //        'class': 'chart-container-values'
    //    }).css({
    //        'width': STOCK.CONFIG.valueScale.width + 'px'
    //    }).appendTo(controls.container);


    //    controls.chart = $('<div/>', {
    //        'class': 'chart-container-visible'
    //    }).css({
    //        'right': STOCK.CONFIG.valueScale.width + 'px'
    //    }).appendTo(controls.container);


    //    controls.infoBox = $('<div/>', {
    //        'class': 'chart-infobox'
    //    }).appendTo(controls.chart);


    //    controls.eventsLayer = $('<div/>', {
    //        'class': 'chart-events-layer'
    //    }).css({
    //        'right': STOCK.CONFIG.valueScale.width + 'px'
    //    }).appendTo(controls.container);


    //    if (visible) {
    //        show();
    //    } else {
    //        hide();
    //    }

    //}


    //function loadQuotations(quotations) {
    //    if (type.name !== STOCK.INDICATORS.ADX.name) {
    //        svg.loadQuotations(quotations);
    //    }
    //}

    //function slide(offset) {
    //    var x = 1;
    //    //svg.render();
    //}

    //function scale() {
    //    var x = 1;
    //}

    //function render() {
    //    svg.render();
    //}

    //function hover(x) {
    //    var quotation = svg.findQuotation(x);
    //    var timeframe = parent.timeframe();
    //    var atLeastDaily = (timeframe.period >= STOCK.TIMEFRAMES.D1.period);

    //    parent.trigger({
    //        type: 'showInfo',
    //        quotation: quotation,
    //        date: quotation ? mielk.dates.toString(quotation.date, !atLeastDaily) : '-'
    //    });
    //}

    //function showInfo(quotation, date) {
    //    $(controls.infoBox).html(date + '  |  ' + svg.getInfo(quotation));
    //}

    //function show() {
    //    $(controls.container).css({
    //        'display': 'block',
    //        'visibility': 'visible'
    //    });
    //}

    //function hide() {
    //    $(controls.container).css({
    //        'display': 'none',
    //        'visibility': 'hidden'
    //    });
    //}

    //function destroy() {
    //    $(controls.container).remove();
    //}



    //self.show = show;
    //self.hide = hide;
    //self.loadQuotations = loadQuotations;
    //self.slide = slide;
    //self.parent = parent;
    //self.render = render;
    //self.offset = function () {
    //    return parent.offset.value;
    //}
    //self.hover = hover;
    //self.destroy = destroy;
    ////self.showInfo = showInfo;

    //self.showTrendlines = function () {
    //    return displayTrendlines;
    //}
    //self.showCandlestickFormations = function () {
    //    return displayCandlestickFormations;
    //}

}
Chart.prototype.bind = function (e) {
    $(this).bind(e);
}
Chart.prototype.trigger = function (e) {
    $(this).trigger(e);
}


function ChartEventsLayer(params) {

    'use strict';

    var self = this;
    self.ChartEventsLayer = true;
    self.parent = params.parent;
    var controls = {};
    self.moving = {
        state: false,
        start: null
    };



    function initialize() {
        loadControls();
        assignEvents();
    }

    function loadControls() {
        controls.container = params.container;
    }

    function assignEvents() {
        $(controls.container).bind({
            mousedown: function (e) {
                self.moving.state = true;
                self.moving.start = e.pageX;
            },
            mouseup: function (e) {
                if (self.moving.state) {
                    self.moving.state = false;
                    slide(e.pageX);
                }
            },
            mousemove: function (e) {
                if (self.moving.state) {
                    slide(e.pageX);   
                }
                showInfo(e.pageX - $(controls.container).offset().left);
            }
        });


        $(document).bind({
            mouseup: function (e) {
                self.moving.state = false;
            }
        });

    }


    function slide(x) {
        var start = self.moving.start;
        self.moving.start = x;
        self.parent.parent.slide(x - start);
    }

    function showInfo(x) {
        self.parent.hover(x);
    }

    //Public API.
    self.bind = function (e) {
        $(self).bind(e);
    }
    self.trigger = function (e) {
        $(self).trigger(e);
    }


    initialize();


}

//function Chart(params) {
//    self.key = params.key;
//    self.controller = params.controller;
//    self.div = mielk.resizableDiv({
//        parent: params.container,
//        id: params.key || 'key',
//        minHeight: params.minHeight || 200,
//        maxHeight: params.maxHeight || 800,
//        height: params.height || 0,
//        'class': 'chart'
//    });
//    self.items = mielk.hashTable();
//    self.currentItemsSet = null;
//    self.displayDateScale = self.type.displayDateScale;

//    //Create SVG manager.
//    self.svg = new SvgPanel(self);    

//    //Cursor.
//    self.hoverService = new ChartHoverService(self);

//    //Drawing layer.
//    self.drawLayer = new ChartDrawLayer(self);
    

//    //Events listener.
//    (function eventsListener() {


//        //Changing company or timeframe.
//        var companyChangeHandler = (function() {
//            self.controller.bind({
//                'changeCompany changeTimeframe': function (e) {
//                    self.svg.reset();
//                    self.company = e.company || self.company;
//                    self.timeframe = e.timeframe || self.timeframe;
//                }
//            });
//        })();


//        //Resizing chart panel.
//        var resizerHandler = (function() {
//            self.div.bind({
//                resize: function (e) {
//                    self.svg.resize();
//                }
//            });
//            $(window).resize(function () {
//                self.svg.resize();
//            });
//        })();


//        //Hovering.
//        var scrollHandler = (function () {

//            self.parent.bind({
//                moveChart: function(e) {
//                    self.svg.move(e.x, e.y);
//                }
//            });

//        })();


//        var scaleHandler = (function() {

//        })();

//        var hoverHandler = (function () {
//            //$(self.svg.ui.chartContainer).bind({
//            self.drawLayer.bind({
//                mousemove: function (e) {
//                    self.parent.trigger({
//                        type: 'hoverItem',
//                        pageX: e.pageX,
//                        pageY: e.pageY,
//                        x: e.offsetX,
//                        y: e.offsetY,
//                        source: self
//                    });
//                }
//            });


//            self.parent.bind({
//                hoverItem: function (e) {
//                    self.hoverService.hover(e);
//                }
//            });


//        })();


        
//    })();

//}
//Chart.prototype = {
//    injectQuotations: function (timeframe, quotations, reload) {
//        var analyzer = this.type.analyzer();
//        var items = analyzer.run(quotations);
//        this.items.setItem(timeframe.id, items);
//        this.currentItemsSet = items;
//        if (reload) this.reload(timeframe);
//    },
//    reload: function (timeframe) {
//        var items = this.items.getItem(timeframe.id);
//        this.currentItemsSet = items;
//        this.svg.reload(timeframe, items);
//    },
//    content: function () {
//        return this.div.content();
//    },
//    getItems: function (timeframe) {
//        if (timeframe) {
//            return this.items.getItem(timeframe.id) || [];
//        } else {
//            return this.currentItemsSet || [];
//        }
//    }
//};
















//function ChartDrawLayer(chart) {

//    'use strict';

//    var self = this;
//    self.ChartDrawLayer = true;
//    self.chart = chart;
//    self.leftClicked = false;
//    self.rightClicked = false;
//    self.resetPosition();

//    self.container = $('<div/>', {        
//        'class': 'draw-layer'
//    }).appendTo(self.chart.content());

//    //Event listener.
//    $(self.container).bind({
//        mousedown: function (e) {
//            self.leftClicked = (e.which === 1);
//            self.rightClicked = (e.which === 3);
//            self.position = {
//                x: e.offsetX,
//                y: e.offsetY
//            };
//            mielk.notify.display('clicked | x: ' + self.position.x + '; y: ' + self.position.y);
//        },
//        mouseup: function(e) {
//            self.leftClicked = false;
//            self.rightClicked = false;
//            self.resetPosition();
//            mielk.notify.display('released');
//        },
//        mousemove: function (e) {
//            if (self.leftClicked) {
                
//                //Przesuwanie wykresem.
//                var position = { x: e.offsetX, y: e.offsetY };
//                var offset = {
//                    x: self.position.x - position.x,
//                    y: self.position.y - position.y
//                };
//                self.chart.parent.trigger({
//                    type: 'moveChart',
//                    x: offset.x,
//                    y: offset.y
//                });
                
//            } else if (self.rightClicked) {
//                //Skalowanie wykresu.
//            } else {
//                //Rysowanie linii trendu.
//            }
//        },
//        leave: function(e) {
//            mielk.notify.display('leaved');
//        }
//    });

//}

//ChartDrawLayer.prototype = {    
//    trigger: function(e) {
//        this.container.trigger(e);
//    },
//    bind: function(e) {
//        this.container.bind(e);
//    },
//    resetPosition: function() {
//        this.position = { x: 0, y: 0 };
//    }
//};


//function ChartHoverService(chart) {
    
//    'use strict';

//    var self = this;
//    self.ChartHoverService = true;
//    self.chart = chart;
//    self.currentItem = null;
//    self.detailsPanel = null;
//    self.horizontalLine = null;
//    self.verticalLine = null;
//    self.valueIndicator = null;
//    self.dateIndicator = null;
//    self.crosshairPosition = {
//        x: 0,
//        y: 0
//    };
//    //self.crosshair = null;


//    self.prepareUserInterface();

//}
//ChartHoverService.prototype = {

//    prepareUserInterface: function () {
//        var self = this;
        
//        self.horizontalLine = $('<div/>', {
//            'class': 'crosshair crosshair-horizontal'
//        }).appendTo(self.chart.svg.ui.chartContainer);

//        self.valueIndicator = $('<div/>', {
//            'class': 'scale-indicator value-indicator'
//        }).appendTo(self.chart.svg.ui.valuesContainer);

//        self.verticalLine = $('<div/>', {
//            'class': 'crosshair crosshair-vertical'
//        }).appendTo(self.chart.svg.ui.chartContainer);
        
//        self.dateIndicator = $('<div/>', {
//            'class': 'scale-indicator date-indicator'
//        }).appendTo(self.chart.svg.ui.datesContainer);

//        self.detailsPanel = $('<div/>', {
//            'class': 'chart-details-panel'
//        }).appendTo(self.chart.svg.ui.chartContainer);

//    },
    
//    hover: function (e) {
//        var self = this;

//        self.locateCrosshair(e.x, e.y, e.source === self.chart);
//        if (e.x > 0) {
//            self.displayItemSummary(e.x);
            
//        }
        

//        //if (item) {
//        //    self.currentItem = item;
//        //}

//    },
    
//    locateCrosshair: function (x, y, horizontal) {
//        var self = this;

//        if (horizontal) {
            
//            $(self.horizontalLine).css({
//                'display': 'block'
//            });

//            if (y > 0) {
//                $(self.horizontalLine).css({
//                    'top': y + 'px'
//                });
//                self.crosshairPosition.y = y;
//            }

//            self.displayValueIndicator(y);


//        } else {
//            $(self.horizontalLine).css({
//                'display': 'none'
//            });

//            $(self.valueIndicator).css({
//                'display': 'none'
//            });

//        }


//        if (x > 0) {
//            $(self.verticalLine).css({
//                'left': x + 'px'
//            });
//            self.crosshairPosition.x = x;
//        }

//    },

//    displayValueIndicator: function (y) {
//        var self = this;
//        var p = self.chart.svg.params;

//        //If params is null, chart is not loaded yet.
//        if (p && y > 0) {
//            var fromBottom = p.height - y;
//            var value = (p.top - p.bottom) * (fromBottom / p.height) + p.bottom;

//            $(self.valueIndicator).css({
//                'display': 'block',
//                'top': y + 'px'
//            });
//            $(self.valueIndicator).html(value.toFixed(2));

//        }
//    },
    
//    displayDateIndicator: function(x, date) {
//        var self = this;

//        //If params is null, chart is not loaded yet.
//        if (x > 0) {
//            var value = mielk.dates.toString(date);

//            $(self.dateIndicator).html(value);
//            $(self.dateIndicator).css({
//                'display': (value ? 'block' : 'none'),
//                'left': x + 'px'
//            });

//        }
//    },

//    displayItemSummary: function(x) {
//        var self = this;
//        var chart = this.chart;

//        //var item = mielk.arrays.firstGreater(chart.currentItemsSet, x, function ($i, $x) {
//        var item = mielk.arrays.firstGreater(chart.svg.visibleItems, x, function ($i, $x) {
//            if ($i.left > $x) return 1;
//            if ($i.right < $x) return -1;
//            return true;
//        });


//        if (item) {
//            var labelFactory = this.chart.type.labelFactory();
//            var labels = labelFactory.produceLabels(item);

//            $(self.detailsPanel).empty();
//            for (var i = 0; i < labels.length; i++) {
//                var set = labels[i];
//                set.appendTo(self.detailsPanel);
//            }

//            self.displayDateIndicator(x, item.date);

//        }


//    }

//};