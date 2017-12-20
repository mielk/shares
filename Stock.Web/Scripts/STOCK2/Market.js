/*
 * Market object used in whole application.
 */
function Market(params) {
    var self = this;
    self.Market = true;
    self.id = params.Id || 0;
    self.name = params.Name || '';
    self.short = params.Short;
    self.startTime = params.StartTime;
    self.endTime = params.EndTime;
}





function MarketManager() {

    'use strict';

    var items = mielk.hashTable(null);


    function loadItems() {
        var objects = mielk.db.fetch('Market', 'GetMarkets', {}, {});

        for (var i = 0; i < objects.length; i++) {
            var market = new Market(objects[i]);
            items.setItem(market.id, market);
        }

    }


    function getAll() {
        return items.values();
    }


    function getCssClass(id) {
        var market = items.getItem(id);
        return market ? market.short.toLowerCase() : '';
    }


    (function initialize() {
        loadItems();
    })();


    return {
        getAll: getAll,
        getCssClass: getCssClass
    };

}



//Add the instance of market manager as an item of STOCK library.
$(function () {
    STOCK.MARKETS = MarketManager();
});
