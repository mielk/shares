function DataItem(params) {

    'use strict';

    var self = this;
    self.DataItem = true;
    params.dataItem = self;

    self.date = mielk.dates.fromCSharpDateTime(params.Date);
    self.assetId = params.ShareId || params.AssetId;
    self.timeframeId = params.TimeframeId;
    self.index = params.DateIndex;
    self.price = new Price(params);
    self.macd = (params.Macd ? new Macd(params) : null);
    self.adx = (params.Adx ? new Adx(params) : null);
}

function Price(params) {

    'use strict';

    var self = this;
    self.Price = true;
    self.dataItem = params.dataItem;

    self.quotation = new Quotation(self, params);

    (function assignExtrema() {
        var price = params.price;
        if (price) {
            self.priceGap = params.price.priceGap;
            self.peakByClose = params.price.PeakByClose;
            self.peakByHigh = params.price.PeakByHigh;
            self.troughByClose = params.price.TroughByClose;
            self.troughByLow = params.price.TroughByLow;
        }
    })();



    //API.
    self.hasAnyExtremum = function () {
        return (self.peakByClose || self.peakByHigh || self.troughByClose || self.troughByLow);
    }

    self.hasPeak = function () {
        return self.peakByClose || self.peakByHigh;
    }

    self.hasTrough = function () {
        return self.troughByClose || self.troughByLow;
    }

    self.getDate = function () {
        return self.dataItem.date;
    }

    self.isPriceUp = function () {
        return self.quotation.priceUp;
    }

    self.isPriceDown = function () {
        return self.quotation.priceDown();
    }

}

function Quotation(parent, params) {

    'use strict';

    var self = this;
    self.Quotation = true;
    self.dataItem = params.dataItem;
    self.price = parent;

    self.open = params.quotation.Open;
    self.high = params.quotation.High;
    self.low = params.quotation.Low;
    self.close = params.quotation.Close;
    self.volume = params.quotation.Volume;
    self.priceUp = self.close > self.open;
    self.priceDown = self.open < self.close;

    //API.
    self.getDate = function() {
        return self.dataItem.date;
    }

}




function Macd(params){

    'use strict';

    var self = this;
    self.Macd = true;
    self.id = params.MacdId;
    self.histogram = params.Histogram;
    self.signal = params.SignalLine;
    self.macd = params.MacdLine;

    self.strMacd = function () {
        return self.macd.toFixed(4);
    };
    self.strSignal = function () {
        return self.signal.toFixed(4);
    };
    self.strHistogram = function () {
        return self.histogram.toFixed(4);
    };

}


function Adx(params) {

    'use strict';

    var self = this;
    self.Adx = true;

}