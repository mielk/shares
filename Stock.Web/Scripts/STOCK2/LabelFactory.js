function AbstractLabelFactory() {
    
    'use strict';

    var self = this;
    self.AbstractLabelFactory = true;
    self.DATE_TAB = 'date';

    self.produceLabels = function (item) {
        var labels = [];
        var values = self.getValues(item);

        values.each(function(k, v) {
            var container = $('<div/>', {
                'class': 'chart-detail-line-container'
            });

            var label = $('<div/>', {
                'class': 'chart-detail-line-label',
                'html': k
            }).appendTo(container);

            var value = $('<div/>', {
                'class': 'chart-detail-line-value',
                'html': v
            }).appendTo(container);

            if (k === self.DATE_TAB) {
                $(container).addClass('date-line');
                $(label).remove();
                $(value).addClass('date-value');
            }


            


            labels.push(container);

        });

        return labels;

    };

}



function PriceLabelFactory() {
    AbstractLabelFactory.call(this);
    var self = this;
    self.PriceLabelFactory = true;
}
mielk.objects.extend(AbstractLabelFactory, PriceLabelFactory);
PriceLabelFactory.prototype = {
    
    getValues: function (item) {
        var set = new mielk.hashTable(null);

        if (item) {
            set.setItem(this.DATE_TAB, mielk.dates.toString(item.date));
            set.setItem('Open: ', (item.open).toFixed(2));
            set.setItem('High: ', (item.high).toFixed(2));
            set.setItem('Low: ', (item.low).toFixed(2));
            set.setItem('Close: ', (item.close).toFixed(2));
            set.setItem('Volume: ', mielk.numbers.addThousandSeparator(item.volume, ' '));
        }

        return set;

    }
    
};



function MacdLabelFactory() {
    AbstractLabelFactory.call(this);
    var self = this;
    self.MacdLabelFactory = true;
}
mielk.objects.extend(AbstractLabelFactory, MacdLabelFactory);
MacdLabelFactory.prototype = {

    getValues: function (item) {
        var set = new mielk.hashTable(null);

        if (item) {
            set.setItem(this.DATE_TAB, mielk.dates.toString(item.date));
            set.setItem('MACD: ', (item.macd).toFixed(2));
            set.setItem('Signal: ', (item.signal).toFixed(2));
            set.setItem('Histogram: ', (item.histogram).toFixed(2));
        }

        return set;

    }

};



function AdxLabelFactory() {
    AbstractLabelFactory.call(this);
    var self = this;
    self.AdxLabelFactory = true;
}
mielk.objects.extend(AbstractLabelFactory, AdxLabelFactory);
AdxLabelFactory.prototype = {

    getValues: function (item) {
        var set = new mielk.hashTable(null);

        if (item) {
            set.setItem(this.DATE_TAB, mielk.dates.toString(item.date));
            set.setItem('DI+: ', (item.diPlus).toFixed(2));
            set.setItem('DI-: ', (item.diMinus).toFixed(2));
            set.setItem('ADX: ', (item.adx).toFixed(2));
        }

        return set;

    }

};
