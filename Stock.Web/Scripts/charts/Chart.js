function Chart(parent, params) {

    'use strict';

    //[Meta].
    var self = this;
    self.Chart = true;
    self.type = params.type;
    self.timeframe = params.timeframe;
    self.key = 'chart_' + params.type.name;

    //Properties.
    var parent = parentContainer;
    var controller = parent.getController();

}

Chart.prototype.bind = function (e) {
    $(this).bind(e);
}
Chart.prototype.trigger = function (e) {
    $(this).trigger(e);
}