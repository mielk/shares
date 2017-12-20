function QuotationsAnalyzer(params) {

    'use strict';

    var self = this;
    self.QuotationsAnalyzer = true;
    self.type = params.type;

    //TODO - przenieść funkcję run do prototypów.

    self.run = function (quotations, params) {
        self.quotations = quotations;

        //Set params if applicable for this type of chart.
        //if (self.setParams && typeof(self.setParams) === "function") {
        self.setParams(params);
        //}
        

        quotations.forEach(function (quotation) {
            quotation[self.type.name] = self.process(quotation);
        });

        //var items = [];
        //for (var i = 0; i < quotations.length; i++) {
        //    var quotation = quotations[i];
        //    var item = this.process(quotation);
        //    items[i] = item;
        //}

        //return items;

    };

}


function PriceAnalyzer(params) {
    QuotationsAnalyzer.call(this, params);
    var self = this;
    self.PriceAnalyzer = true;
}
//mielk.objects.extend(QuotationsAnalyzer, PriceAnalyzer);
extend(QuotationsAnalyzer, PriceAnalyzer);
PriceAnalyzer.prototype = {
    setParams: function(params) {
        
    },
    process: function (quotation) {
        return {
            date: quotation.date,
            open: quotation.open,
            high: quotation.high,
            low: quotation.low,
            close: quotation.close,
            volume: quotation.volume,
            peakByClose: quotation.peakByClose,
            peakByLow: quotation.peakByLow,
            troughByClose: quotation.troughByClose,
            troughByLow: quotation.troughByLow
        };
    }
};


function MacdAnalyzer(params) {
    QuotationsAnalyzer.call(this, params);
    var self = this;
    self.MacdAnalyzer = true;
    self.index = 0;
    self.indicators = {
        shortMa: 0,
        longMa: 0,
        shortEma: 0,
        longEma: 0,
        signal: 0
    };

}
mielk.objects.extend(QuotationsAnalyzer, MacdAnalyzer);
MacdAnalyzer.prototype = {
    
    setParams: function (params) {
        var p = params || {};
        this.short = p.short || 12;
        this.long = p.long || 26;
        this.signal = p.signal || 9;
        this.kShort = 2 / (this.short + 1);
        this.kLong = 2 / (this.long + 1);
        this.kSignal = 2 / (this.signal + 1);
    },

    calculateShortEma: function(closePrice) {
        var prev = this.indicators.shortEma;
        var k = this.kShort;

        return k * (closePrice - prev) + prev;

    },
    
    calculateLongEma: function(closePrice) {
        var prev = this.indicators.longEma;
        var k = this.kLong;

        return k * (closePrice - prev) + prev;

    },
    
    calculateSignal: function(macd) {
        var prev = this.indicators.signal;
        var k = this.kSignal;

        return k * (macd - prev) + prev;

    },

    process: function (quotation) {
        var self = this;

        //Short EMA.
        if (self.index < (self.short - 1)) {
            self.indicators.shortMa = ((self.indicators.shortMa * self.index) + quotation.close) / (self.index + 1);
            self.indicators.shortEma = self.indicators.shortMa;
        } else {
            self.indicators.shortEma = self.calculateShortEma(quotation.close);
        }
        
        
        //Long EMA.
        if (self.index < (self.long - 1)) {
            self.indicators.longMa = ((self.indicators.longMa * self.index) + quotation.close) / (self.index + 1);
            self.indicators.longEma = self.indicators.longMa;
        } else {
            self.indicators.longEma = self.calculateLongEma(quotation.close);
        }
        
        

        //Iterate to the next item.
        self.indicators.macd = self.indicators.shortEma - self.indicators.longEma;
        self.indicators.signal = self.index < self.signal ? self.indicators.macd : self.calculateSignal(self.indicators.macd);
        self.indicators.histogram = self.indicators.macd - self.indicators.signal;
        
        self.index++;
        
        return {
            date: quotation.date,
            macd: self.indicators.macd,
            signal: self.indicators.signal,
            histogram: self.indicators.histogram
        };
        
    }
    
};



function AdxAnalyzer(params) {
    QuotationsAnalyzer.call(this, params);
    var self = this;
    self.AdxAnalyzer = true;
    self.index = 0;
    self.indicators = {        
        tr: 0,
        dmPlus1: 0,
        dmMinus1: 0,
        tr14: 0,
        dmPlus14: 0,
        dmMinus14: 0,
        diPlus14: 0,
        diMinus14: 0,
        diDiff: 0,
        diSum: 0,
        dx: 0,
        adx: 0
    };

}
mielk.objects.extend(QuotationsAnalyzer, AdxAnalyzer);
AdxAnalyzer.prototype = {
    setParams: function(params) {
        this.period = params ? params.period || 13 : 13;
    },
    process: function (quotation) {
        var self = this;
        
        if (self.previous) {
            self.calculateTr(quotation);
            self.calculateDm1(quotation);
            self.calculate14();
            self.calculateDi();
            self.calculateAdx();
        }

        self.index++;
        self.previous = quotation;

        return {
            date: quotation.date,
            diPlus: self.indicators.diPlus14,
            diMinus: self.indicators.diMinus14,
            adx: self.indicators.adx
        };

    },
    
    calculateTr: function (quotation) {
        var self = this;
        var tr = Math.max(quotation.high - quotation.low,
            Math.abs(quotation.high - self.previous.close),
            Math.abs(quotation.low - self.previous.close));
        self.indicators.tr = tr;
    },
    
    calculateDm1: function (quotation) {
        var self = this;
        var highsDifference = quotation.high - self.previous.high;
        var lowsDifference = self.previous.low - quotation.low;

        if (highsDifference === lowsDifference) return;
        
        if (highsDifference > lowsDifference) {
            self.indicators.dmPlus1 = Math.max(highsDifference, 0);
            self.indicators.dmMinus1 = 0;
        } else {
            self.indicators.dmPlus1 = 0;
            self.indicators.dmMinus1 = Math.max(lowsDifference, 0);
        }


    },
    
    calculate14: function () {
        var self = this;
        var i = self.indicators;
        
        if (self.index >= self.period) {
            i.tr14 = i.tr14 * (1 - 1 / self.period) + i.tr;
            i.dmPlus14 = i.dmPlus14 * (1 - 1 / self.period) + i.dmPlus1;
            i.dmMinus14 = i.dmMinus14 * (1 - 1 / self.period) + i.dmMinus1;
        } else {
            i.tr14 += i.tr;
            i.dmPlus14 += i.dmPlus1;
            i.dmMinus14 += i.dmMinus1;
        }


    },
    
    calculateDi: function() {
        var i = this.indicators;

        if (i.tr14 === 0) {
            i.diPlus14 = 0;
            i.diMinus14 = 0;
        } else {
            i.diPlus14 = 100 * i.dmPlus14 / i.tr14;
            i.diMinus14 = 100 * i.dmMinus14 / i.tr14;
        }

        i.diDiff = Math.abs(i.diPlus14 - i.diMinus14);
        i.diSum = i.diPlus14 + i.diMinus14;

    },
    
    calculateAdx: function() {
        var i = this.indicators;
        
        if (i.diSum === 0) {
            i.dx = 0;
            i.adx = 0;
        } else {
            i.dx = 100 * i.diDiff / i.diSum;
            i.adx = (i.adx * (this.period - 1) + i.dx) / this.period;
        }

    }

};