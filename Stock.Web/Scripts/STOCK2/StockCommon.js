/*
 * Common objects used in whole STOCK project.
 */

Date.prototype.isHoliday = function () {
    if (this.getMonth() === 11){
        if (this.getDate() === 25) return true;
        if (this.getDate() === 24 && this.getHours() >= 21) return true;
        if (this.getDate() === 31 && this.getHours() >= 21) return true;
    } else if (this.getMonth() === 0) {
        return (this.getDate() === 1);
    } else {
        return false;
    }
};

(function(window){

    'use strict';

    var timeframes = {
        M5: {
            id: 1, name: '5 minutes', symbol: 'M5', period: 5, selectable: true,
            next: function (date) {
                var d = mielk.dates.addMinutes(date, date.getDay() === 5 && date.getHours() === 23 && date.getMinutes() === 55 ? 2885 : 5);
                if (d.isHoliday()) {
                    d = this.next(d);
                }
                return d;
            }
        },
        M15: {
            id: 2, name: '15 minutes', symbol: 'M15', period: 15, selectable: true,
            next: function (date) {
                var d = mielk.dates.addMinutes(date, date.getDay() === 5 && date.getHours() === 23 && date.getMinutes() === 45 ? 2895 : 15);
                if (d.isHoliday()) {
                    d = this.next(d);
                }
                return d;
            }
        },
        M30: {
            id: 3, name: '30 minutes', symbol: 'M30', period: 30, selectable: true,
            next: function (date) {
                var d = mielk.dates.addMinutes(date, date.getDay() === 5 && date.getHours() === 23 && date.getMinutes() === 30 ? 2910 : 30);
                if (d.isHoliday()) {
                    d = this.next(d);
                }
                return d;
            }
        },
        H1: {
            id: 4, name: '1 hour', symbol: 'H1', period: 60, selectable: true,
            next: function (date) {
                var d = mielk.dates.addHours(date, date.getDay() === 5 && date.getHours() === 23 ? 49 : 1);
                if (d.isHoliday()) {
                    d = this.next(d);
                }
                return d;
            }
        },
        H4: {
            id: 5, name: '4 hours', symbol: 'H4', period: 240, selectable: true,
            next: function (date) {
                var d = mielk.dates.addHours(date, date.getDay() === 5 && date.getHours() === 20 ? 52 : 4);
                if (d.isHoliday()) {
                    d = this.next(d);
                }
                return d;
            }
        },
        D1: {
            id: 6, name: 'daily', symbol: 'D1', period: 1440, selectable: true,
            next: function (date) {
                var d = mielk.dates.addDays(date, date.getDay() === 5 ? 3 : 1);
                if (d.isHoliday()) {
                    d = this.next(d);
                }
                return d;
            }
        },
        W1: {
            id: 7, name: 'weekly', symbol: 'W1', period: 7000, selectable: true,
            next: function (date) {
                return mielk.dates.addWeeks(date, 1);
            }
        },
        MN1: {
            id: 8, name: 'monthly', symbol: 'MN1', period: 30000, selectable: true,
            next: function (date) {
                return mielk.dates.addMonths(date, 1);
            }
        },
        //D: { id: 1, name: 'daily', symbol: '1D', period: 1, selectable: true },
        //W: { id: 2, name: 'weekly', symbol: '1W', period: 7, selectable: true },
        //M: { id: 3, name: 'monthly', symbol: '1M', period: 30, selectable: true },
        //Y: { id: 4, name: 'yearly', symbol: '1Y', period: 365, selectable: false },
        getItem: function (value) {
            for (var key in this) {
                if (this.hasOwnProperty(key)) {
                    var object = this[key];
                    if (object.symbol === value) {
                        return object;
                    }
                }
            }
            return null;
        },
        getValues: function () {
            var array = [];
            for (var key in this) {
                if (this.hasOwnProperty(key)) {
                    var item = this[key];
                    if (item && typeof (item) !== 'function' && item.selectable) {
                        var object = {
                            id: key,
                            name: item.name,
                            symbol: item.symbol,
                            object: item
                        };
                        array.push(object);
                    }
                }
            }
            return array;
        },
        defaultValue: function () {
            return this.D1;
        }
    };

    var indicators = {
        PRICE: {
            id: 1,
            name: 'prices',
            analyzer: function () {
                return new PriceAnalyzer({ type: this });
            },
            svgRenderer: function (params) {
                return new PriceSvgRenderer(params);
            },
            labelFactory: function() {
                return new PriceLabelFactory();
            },
            runWhenComplete: false,
            initialHeight: 600,
            minValue: 0,
            maxValue: null,
            color: 'blue'
        },
        MACD: {
            id: 2,
            name: 'MACD',
            analyzer: function () {
                return new MacdAnalyzer({ type: this });
            },
            svgRenderer: function (params) {
                return new MacdSvgRenderer(params);
            },
            labelFactory: function () {
                return new MacdLabelFactory();
            },
            initialHeight: 200,
            negativeAllowed: true,
            minValue: null,
            maxValue: null,
            color: 'yellow'
        },
        ADX: {
            id: 3,
            name: 'ADX',
            analyzer: function () {
                return new AdxAnalyzer({ type: this });
            },
            svgRenderer: function (params) {
                return new AdxSvgRenderer(params);
            },
            labelFactory: function () {
                return new AdxLabelFactory();
            },
            initialHeight: 200,
            negativeAllowed: false,
            minValue: 0,
            maxValue: 100,
            color: 'brown'
        },
        getItem: function (value) {
            for (var key in this) {
                if (this.hasOwnProperty(key)) {
                    var object = this[key];
                    if (key === value || object.id === value) {
                        return object;
                    }
                }
            }
            return null;
        },
        getValues: function () {
            var array = [];
            for (var key in this) {
                if (this.hasOwnProperty(key)) {
                    var item = this[key];
                    if (item && typeof (item) !== 'function') {
                        var object = {
                            id: key,
                            name: item.name,
                            object: item
                        };
                        array.push(object);
                    }
                }
            }
            return array;
        },
        defaultValue: function () {
            return this.PRICE;
        }
    };

    var config = {
        loading: {
            packageSize: 1000
        },
        chart: {
            margin: 0.1
        },
        candle: {
            svgLevelsZoom: 1.04,
            defaultWidth: 8,
            maxWidth: 50,
            minWidth: 1,
            modifyWidth: function (width) {
                this.width = Math.min(Math.max(width, this.minWidth), this.maxWidth);
            },
            space: 0.4,
            color: {
                ascendingBody: '#6BA583', //'white',
                ascendingLine: '#225437',
                descendingBody: '#D75442',
                descendingLine: '#5B1A13',
                shadow: '#000000' //'#737375'
            },
            strokeWidth: 1
        },

        macd: {
            color: {
                histogramLine: '#666',
                macdLine: '#222',
                signalLine: '#999',
                histogramAscFill: '#0F0',
                histogramDescFill: '#F00'
            }
        }

        , timeScale: {
            height: 50
        }
        , valueScale: {
            width: 100
        }
        , svgPanel: {
            height: 400,
            width: 25000
        }
        , peaks: {
            distance: 35
        }
        , trendlines: {
            color: 'black'
        }

        , modify: function (param, value) {

        }


        //chartMargin: 0.1,
        //candleSpace: 0.35,
        //ascCandleColor: 'white',
        //descCandleColor: 'black',
        //verticalWeeksLinesColor: '#EEE',
        //verticalMonthsLinesColor: '#CCC',
        //verticalYearsLinesColor: '#18285C',
        //transparent: 'rgba(255, 255, 255, 0)',
        
        //valuesScaleWidth: 50,
        //dateRangeIndicator: 2,
        //dataLabelColor: '#777',
        //valueLabelColor: '#777',
        //horizontalLinesColor: '#EEE',
        //valueScaleLineWidth: 5,
        //valueScaleIndicatorLength: '#777',
        //valueLabelLeft: 17,
        

        
        //ADX
        //diPlusLineColor: 'green',
        //diMinusLineColor: 'red',
        //adxLineColor: 'blue',
        
        //Peaks & Troughs
        //extremaCircleStrokeColor: '#CCC'

    };

    var stock = {
        TIMEFRAMES: timeframes,
        INDICATORS: indicators,
        CONFIG: config
    };


    // Expose STOCK to the global object
    window.STOCK = stock;


})(window);