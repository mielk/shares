function Chart(parent, params) {

    'use strict';

    //[Meta].
    var self = this;
    self.Chart = true;
    self.parent = parent;


    self.params = (function () {
        var type = params.type;
        var timeframe = params.timeframe;


        //Setters
        /* Other properties should not be changed after being set during object initialization */


        //Getters
        function getType() {
            return type;
        }

        function getTimeframe() {
            return timeframe;
        }


        return {
            getType: getType,
            getTimeframe: getTimeframe
        }

    })();


    self.ui = (function () {
        var parentContainer = self.parent.ui.getChartsContainer();

        function render() {
            var x = 1;
        }

        return {
            render: render
        }

    })();



}

Chart.prototype.bind = function (e) {
    $(this).bind(e);
}
Chart.prototype.trigger = function (e) {
    $(this).trigger(e);
}