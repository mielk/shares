function CompanyManager() {

    'use strict';

    var items = mielk.hashTable(null);


    (function initialize() {
        var company = getStubCompany(1);
        items.setItem(company.id, company);
        return company;
    })();

    function loadCompany(id) {
    }

    function getStubCompany(shareId)
    {
        return new Company({
            Id: 1,
            Name: 'Carillion',
            Short: 'CLLN.L'
        });
    }

    function getList() {
        return items;
    }

    function getAll() {
        return items.values();
    }

    function getCompany(id) {
        return items.getItem(id) || loadCompany(id);
    }

    return {
        getAll: getAll,
        getCompany: getCompany,
        getList: getList
    };

};




/*
 * Company object used in whole application.
 */
function Company(params) {
    var self = this;
    this.Company = true;

    //[Company data].
    this.id = params.Id || 0;
    this.name = params.Name || '';
    this.idMarket = params.IdMarket || 0;
    this.symbol = params.Short || params.Name;
    this.isFx = params.IsFx || false;
    this.displayed = this.name + (this.isFx ? '' : ' (' + this.symbol + ')');

    //[Status & Timestamp variables].
    this.lastUpdate = params.LastPriceUpdate;
    this.lastCalculation = params.LastCalculation;
    this.lastTrendlinesReview = params.LastTrendlinesReview;
    this.pricesChecked = params.PricesChecked;

    //[Data sets].
    this.dataSets = {};

}

Company.prototype = {
    bind: function(e){
        $(this).bind(e);
    },
    trigger: function (e) {
        $(this).trigger(e);
    },
    toString: function () {
        return this.name + ' (' + this.id + ')';
    },
    getSymbol: function (timeframe) {
        this.name + '_' + timeframe.symbol;
    },
    getDataSet: function (timeframe, simulationId) {
        //Create reference to this [Company] object.
        var self = this;

        //Check if the sub-object for this timeframe already exists.
        var dataSetCol = self.dataSets[timeframe.symbol];

        //If [dataSet] for this timeframe doesn't exist, 
        //create it, add to the collection of data sets...
        if (!dataSetCol) {
            dataSetCol = new DataSetCollection({ company: self, timeframe: timeframe });
            self.dataSets[timeframe.symbol] = dataSetCol;
        }

        return dataSetCol;

    }
};


function DataSetCollection(params) {
    //[Meta].
    var self = this;
    self.DataSetCollection = true;
    var company = params.company;
    var timeframe = params.timeframe;
    var size = STOCK.CONFIG.loading.packageSize;

    //[DataSetCollection] objects are user for two purposes - showing current charts and simulation charts.
    //Simulation charts works different and [company] and [timeframe] can be empty in this object.
    var simulationId = params.simulationId || 0;


    //[Loading status].
    var quotationsLoaded = false;
    var propertiesLoaded = false;

    //[Properties].
    var quotations = {};
    var quotationsArray;
    var quotationsDates = [];
    var firstDate;
    var lastDate;
    var minLevel;
    var maxLevel;
    var actualQuotationsCounter;
    var realQuotationsCounter;
    

    function fetchQuotations(fn, params) {
        var initialized = params.initialized || false;
        var endIndex = params.endIndex || quotationsDates.length - 1;
        var startIndex = Math.max(endIndex - size + 1, 0);
        var endDate = quotationsDates[endIndex];
        var startDate = quotationsDates[startIndex];
        var params = { 
            shareId: company.id
        };

        mielk.db.fetch(
            'Data',
            'GetDataSets',
            params,
            {
                async: true,
                callback: function (res) {

                    var _quotations = res.quotations;
                    var _trendlines = res.trendlines;


                    //Populate quotations collection.
                    assignQuotations(_quotations);

                    //If function has been passed as a parameter, call it.
                    if (mielk.objects.isFunction(fn)) {
                        fn({
                            initial: !initialized,
                            obj: quotations,
                            arr: quotationsArray,
                            trendlines: _trendlines,
                            complete: (startIndex === 0)
                        });
                    }

                    if (startIndex > 0) {
                        fetchQuotations(fn, { initialized: true, endIndex: startIndex - 1 });
                    }

                }
            }
        );
    }


    function loadQuotations(fn, counter, force) {

        //Make sure that metadata about those quotations are already loaded.
        if (!propertiesLoaded) loadProperties(null);

        if (!quotationsLoaded || force || simulationId) {
            fetchQuotations(fn, { });
        } else {
            if (mielk.objects.isFunction(fn)) {
                fn({ obj: quotations, arr: quotationsArray });
            }
        }

    }

    function assignQuotations(data) {
        data.forEach(function (item) {
            if (item) {
                var date = mielk.dates.fromCSharpDateTime(item.Date);
                var dataItem = quotations[date];

                //There are some quotations in the database with Saturday or Sunday date.
                //For such quotations the operation above will return null, since there are no
                //items in [quotations] object with Saturday or Sunday date as a key.
                if (dataItem) {
                    var quotation = new Quotation(item);
                    dataItem.quotation = quotation;
                    dataItem.index = quotation.quotation.DateIndex; //IndexNumber;
                    quotationsArray[dataItem.index] = quotation;
                }

            }
        });

    }



    //Funkcja pobierająca właściwości dla danego timeframeu z bazy danych.
    function loadProperties(fn, simulationId, startIndex, endIndex) {

        var properties = {
            shareId: company.id
        };
        //simulation ? {} : { pairSymbol: company.id, timeframe: timeframe.id },
        mielk.db.fetch(
            'Data',
            'GetDataSetsInfo',
            properties,
            {
                async: false,
                callback: function (res) {

                    if (res == null) {
                        res = {
                              firstDate: mielk.dates.fromString('2005-01-04')
                            , lastDate: mielk.dates.fromString('2017-12-08')
                            , minLevel: 14.75
                            , maxLevel: 456.5
                            , counter: 3342
                        };
                    }

                    var arr = { firstDate: res.firstDate, lastDate: res.lastDate, minLevel: res.minLevel * 1, maxLevel: res.maxLevel * 1, counter: res.counter * 1 };

                    firstDate = arr.firstDate; //mielk.dates.fromCSharpDateTime(arr.firstDate);
                    lastDate = arr.lastDate; //mielk.dates.fromCSharpDateTime(arr.lastDate);
                    minLevel = arr.minLevel;
                    maxLevel = arr.maxLevel;


                    actualQuotationsCounter = arr.counter;
                    
                    //Create object for quotations (with slot for each date
                    //between [firstDate] and [lastDate].
                    createQuotationsSets();
                    realQuotationsCounter = Object.keys(quotations).length;

                    //Flag this data set as having properties already loaded.
                    propertiesLoaded = true;

                    //Create [properties] object to be returns.
                    properties = {
                        firstDate: firstDate,       //The date of the first quotation
                        lastDate: lastDate,         //The date of the last quotation
                        minLevel: minLevel,         //The minimum level of the price
                        maxLevel: maxLevel,         //The maximum level of the price
                        actualQuotationsCounter: actualQuotationsCounter,   //The number of quotations in the database
                        realQuotationsCounter: realQuotationsCounter        //The expected number of quotations
                    };

                    //If function has been passed as a parameter, call it.
                    if (mielk.objects.isFunction(fn)) {
                        fn(properties);
                    }

                },
                err: function (msg) {
                    alert(msg.status + ' | ' + msg.statusText);
                }
            }
        );

    }

    function createQuotationsSets() {

        //Check if properties are already loaded.
        //If not, this function cannot continue.
        //if (!propertiesLoaded) return;   -- commented out; this method is now invoked together with loading properties.

        //Initial values.
        quotations = {};
        quotationsDates = [];
        var index = 0;
        var date = new Date(firstDate);

        while (date <= lastDate) {

            var strDate = mielk.dates.toString(date, true);

            quotations[date] = {
                index: index,
                date: strDate,
                quotation: null
            }

            quotationsDates[index] = strDate;
            index++;


            //Calculate the next date, using method [next] of Timeframe object.
            date = timeframe.next(date);

        }

        //Resize quotations array.
        quotationsArray = new Array(index);

    }


    //Public API.
    self.loadQuotations = loadQuotations;
    self.loadProperties = loadProperties;

}



//Add the instance of company manager as an item of STOCK library.
$(function () {
    STOCK.COMPANIES = CompanyManager();
});