function ChartValuePanel(parentChartContainer) {

    'use strict';

    //[Meta].
    var self = this;
    self.ChartValuePanel = true;

    //Properties.
    var parent = parentChartContainer;

}




//function ChartValuesPanel(params) {

//    'use strict';

//    var self = this;
//    self.ChartValuesPanel = true;
//    self.parent = params.parent;
//    var controls = {};


//    function initialize() {
//        loadControls();
//        assignEvents();
//    }

//    function loadControls() {
//        controls.container = params.container;
//    }

//    function assignEvents() {
//        $(controls.container).bind({
//            dblclick: function (e) {
//                self.trigger({
//                    type: 'autoscale'
//                });
//            }
//        });
//    }


//    //Public API.
//    self.bind = function (e) {
//        $(self).bind(e);
//    }
//    self.trigger = function (e) {
//        $(self).trigger(e);
//    }


//    initialize();

//}

