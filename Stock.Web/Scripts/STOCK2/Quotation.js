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
            self.peakByClose = (params.price.PeakByClose ? new Extremum(self, params.price.PeakByClose) : null);
            self.peakByHigh = (params.price.PeakByHigh ? new Extremum(self, params.price.PeakByHigh) : null);
            self.troughByClose = (params.price.TroughByClose ? new Extremum(self, params.price.TroughByClose) : null);
            self.troughByLow = (params.price.TroughByLow ? new Extremum(self, params.price.TroughByLow) : null);
        }
    })();



    //API.
    self.hasAnyExtremum = function () {
        return (self.peakByClose || self.peakByHigh || self.troughByClose || self.troughByLow);
    }

    self.getAllExtrema = function () {
        var arr = [];
        if (self.peakByClose) arr.push(self.peakByClose);
        if (self.peakByHigh) arr.push(self.peakByHigh);
        if (self.troughByClose) arr.push(self.troughByClose);
        if (self.troughByLow) arr.push(self.troughByLow);
        return arr;
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

function Extremum(price, params) {

    'use strict';

    var self = this;
    self.Extremum = true;
    self.price = price;

    self.id = params.ExtremumId;
    self.extremumType = params.ExtremumTypeId;
    self.value = params.Value
    self.stats = {
        earlier: {
            amplitude: params.EarlierAmplitude,
            averageArea: params.EarlierAverageArea,
            change1: params.EarlierChange1,
            change2: params.EarlierChange2,
            change3: params.EarlierChange3,
            change5: params.EarlierChange5,
            change10: params.EarlierChange10,
            counter: params.EarlierCounter,
            totalArea: params.EarlierTotalArea
        },
        later: {
            amplitude: params.LaterAmplitude,
            averageArea: params.LaterAverageArea,
            change1: params.LaterChange1,
            change2: params.LaterChange2,
            change3: params.LaterChange3,
            change5: params.LaterChange5,
            change10: params.LaterChange10,
            counter: params.LaterCounter,
            totalArea: params.LaterTotalArea
        },
        isOpen: params.IsEvaluationOpen
    };

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







function Trendline(params, extremumGroups) {

    'use strict';

    var self = this;
    self.Trendline = true;

    self.id = params.Id;
    self.slope = params.Slope;
    self.edgePoints = {
        base: {
            index: params.BaseDateIndex,
            level: params.BaseLevel
        },
        counter: {
            index: params.CounterDateIndex,
            level: params.CounterLevel
        }
    }
    self.range = {
        start: params.StartIndex,
        end: params.EndIndex
    }
    self.value = params.Value;
    self.isClosed = !params.IsOpenFromRight;
    self.trendRanges = (function ($trendRanges, $extremumGroups) {
        var arr = [];
        $trendRanges.forEach(function (item) {
            var trendRange = new TrendRange(self, item, $extremumGroups);
            arr.push(trendRange);
        });
        return arr;
    })(params.TrendRanges, extremumGroups);

    self.countPriceForDateIndex = function (dateIndex) {
        return (dateIndex - self.edgePoints.base.index) * self.slope + self.edgePoints.base.level;
    };

    self.getAllTrendHits = function () {
        var arr = [];
        self.trendRanges.forEach(function (tr) {
            if (tr.base && tr.base.TrendHit) {
                arr.push(tr.base);
            }
            if (tr.counter && tr.counter.TrendHit) {
                arr.push(tr.counter);
            }
        });
        return arr;
    };

    self.getAllTrendBreaks = function () {
        var arr = [];
        self.trendRanges.forEach(function (tr) {
            if (tr.base && tr.base.TrendBreak) {
                arr.push(tr.base);
            }
            if (tr.counter && tr.counter.TrendBreak) {
                arr.push(tr.counter);
            }
        });
        return arr;
    };

}

function TrendHit(trendRange, params, extremumGroups) {

    'use strict';

    var self = this;
    self.TrendHit = true;
    self.trendRange = trendRange;

    self.id = params.TrendHitId;
    self.value = params.Value;
    self.extremumGroup = (extremumGroups && extremumGroups[params.ExtremumGroupId] ? extremumGroups[params.ExtremumGroupId] : params.ExtremumGroupId);
    self.evaluation = {
        gap: params.Gap,
        relativeGap: params.RelativeGap,
        pointsForDistance: params.PointsForDistance,
        pointsForValue: params.PointsForValue
    };
}

function TrendBreak(trendRange, params) {

    'use strict';

    var self = this;
    self.TrendBreak = true;
    self.trendRange = trendRange;

    self.id = params.TrendBreakId;
    self.index = params.DateIndex;
    self.fromAbove = params.BreakFromAbove;
    self.value = params.Value;
    self.evaluation = {
        breakDayAmplitude: params.BreakDayAmplitudePoints,
        previousDayPoints: params.PreviousDayPoints,
        nextDaysMinDistancePoints: params.NextDaysMinDistancePoints,
        nextDaysMaxVariancePoints: params.NextDaysMaxVariancePoints
    };
}

function TrendRange(trendline, params, extremumGroups) {

    'use strict';

    var self = this;
    self.TrendRange = true;
    self.trendline = trendline;

    self.id = params.TrendRangeId;
    self.isPeak = params.IsPeak;
    self.value = params.Value;
    self.base = params.BaseIsHit ? new TrendHit(self, params.StartDelimiter, extremumGroups) : new TrendBreak(self, params.StartDelimiter);
    self.counter = params.CounterIsHit ? new TrendHit(self, params.EndDelimiter, extremumGroups) : new TrendBreak(self, params.EndDelimiter);
    self.stats = {
        extremumPriceCross: {
            penaltyPoints: params.ExtremumPriceCrossPenaltyPoints,
            counter: params.ExtremumPriceCrossCounter,
        },
        openClosePriceCross: {
            penaltyPoints: params.OCPriceCrossPenaltyPoints,
            counter: params.OCPriceCrossCounter,
        },
        totalCandles: params.TotalCandles,
        variation: {
            average: params.AverageVariation,
            extremum: params.ExtremumVariation,
            openClose: params.OpenCloseVariation
        }

    }

}

function ExtremumGroup(params, extremaArray) {

    'use strict';

    var self = this;
    self.ExtremumGroup = true;

    self.id = params.ExtremumGroupId;
    self.isPeak = params.IsPeak;
    self.master = (extremaArray && extremaArray[params.MasterExtremumId] ?
                        extremaArray[params.MasterExtremumId] :
                        {
                            extremumId: params.MasterExtremumId,
                            dateIndex: params.MasterDateIndex
                        });
    self.slave = (extremaArray && extremaArray[params.SlaveExtremumId] ?
                        extremaArray[params.SlaveExtremumId] :
                        {
                            extremumId: params.SlaveExtremumId,
                            dateIndex: params.SlaveDateIndex
                        });
    self.dates = {
        start: params.StartDateIndex,
        end: params.EndDateIndex
    }
    self.levels = {
        openClose: params.OCPriceLevel,
        extremum: params.ExtremumPriceLevel,
        middle: params.MiddlePriceLevel
    }

    self.getValue = function () {
        var points = self.master.value;
        if (self.slave && self.slave.value > points) {
            return self.slave.value;
        } else {
            return points;
        }
    }

}