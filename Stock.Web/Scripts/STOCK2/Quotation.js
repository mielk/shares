function Quotation(params) {
    var self = this;
    self.Quotation = true;
    self.date = mielk.dates.fromCSharpDateTime(params.Date);
    self.assetId = params.ShareId; //AssetId;
    self.open = params.quotation.Open;
    self.high = params.quotation.High;
    self.low = params.quotation.Low;
    self.close = params.quotation.Close;
    self.volume = params.quotation.Volume;
    self.quotation = params.quotation;
    self.price = params.price;
    self.priceGap = params.price ? params.price.priceGap : 0;
    self.peakByClose = params.price ? params.price.PeakByClose : 0;
    self.peakByHigh = params.price ? params.price.PeakByHigh : 0;
    self.troughByClose = params.price ? params.price.TroughByClose : 0;
    self.troughByLow = params.price ? params.price.TroughByLow : 0;
    self.macd = (params.Macd ? new Macd(params.Macd) : null);
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
