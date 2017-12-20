//+------------------------------------------------------------------+
//|                                                    MySQL-003.mq5 |
//|                                   Copyright 2014, Eugene Lugovoy |
//|                                              http://www.mql5.com |
//| Inserting data with multi-statement (DEMO)                       |
//+------------------------------------------------------------------+
#property copyright "Copyright 2014, Eugene Lugovoy."
#property link      "http://www.mql5.com"
#property version   "1.00"
#property strict

#include <hash.mqh>
#include "..\libraries\functions.mq4"
#include "..\libraries\mysql.mq4"


void OnStart()
{
   double processStartTimestamp = TimeLocal();
   Alert("========================================================");
   Alert("[Start price update]");
   quotationsFullUpdate();
   
   double updateEndTimestamp = TimeLocal();
   double secondsElapsed = updateEndTimestamp - processStartTimestamp;
   Alert("Quotations downloaded | " + secondsElapsed + "s");
   
   
   checkDataConsistency();
   secondsElapsed = (double) TimeLocal() - updateEndTimestamp;
   Alert("Consistency checked | " + secondsElapsed + "s");
   Alert("========================================================");
}



void quotationsFullUpdate(){
   string pairs[][4];
   
   fetchArrayByTableName("v_symbols_last_updates", pairs);
   
   for (int i = 0; i < ArrayRange(pairs, 0); i++){
      int assetId = pairs[i, 0];
      int timeframeId = pairs[i, 1];
      string symbol = pairs[i, 2];
      string pair = StringSubstr(symbol, 0, 6);
      datetime lastUpdate = convertStringToDate(pairs[i, 3]);
      ENUM_TIMEFRAMES timeframe = stringToTimeframe(StringSubstr(symbol, 7, StringLen(symbol) - 7));

      Alert("--------------------------------------------------------");
      Alert("[Symbol] " + symbol);
      Alert("[Last quotation before update] " + dateToTime(lastUpdate));
      
      RemoveLastQuotation(assetId, timeframeId, lastUpdate);
      int inserted = InsertSingleQuotationSet(assetId, timeframeId, pair, timeframe, lastUpdate);
     
      Alert("[Added] " + inserted + " record(s)");
      Alert("[Last quotation after update] " + getLastUpdate(symbol));
      
   }
   
}

string getLastUpdate(string symbol){
   string sql = "SELECT lastUpdate FROM v_symbols_last_updates WHERE symbol = '" + symbol + "';";
   string date[][1];
   fetchArrayBySql(sql, date);
   return date[0, 0];
}


void RemoveLastQuotation(int assetId, int timeframeId, datetime lastUpdate){
   string query = "DELETE FROM quotations " + 
                  "WHERE AssetId = " + assetId + " AND " + 
                        "TimeframeId = " + timeframeId + " AND " + 
                        "PriceDate >= '" + dateToTime(lastUpdate) + "';";
   insert(query);
}


int InsertSingleQuotationSet(int assetId, int timeframeId, string symbol, ENUM_TIMEFRAMES timeframe, datetime lastUpdate){
   MqlRates rates[];
   int copied = 0;
   datetime lastRate = 0;
   
   if (TimeYear(lastUpdate) < 2010){
      copied = getAllAvailableRates(symbol, timeframe, 0, rates);         
   } else {
      copied = getRates(symbol, timeframe, lastUpdate, rates);
   }
   
   
   
	if(copied > 0) {
      int size = copied;

      for (int i = 0; i < size; i++)
      {
         datetime date = rates[i].time;
         
         if (lastRate == 0 || lastRate < date){
            lastRate = date;
         }

         string query = "INSERT INTO quotations" +
                        "(PriceDate, AssetId, TimeframeId, OpenPrice, HighPrice, LowPrice, ClosePrice, Volume) " +
                        "VALUES (" + 
                        "'" + dateToTime(rates[i].time) + "', " + 
                        (string) assetId + ", " + 
                        (string) timeframeId + ", " + 
                        (string) rates[i].open + ", " + 
                        (string) rates[i].high + ", " + 
                        (string) rates[i].low + ", " + 
                        (string) rates[i].close + ", " + 
                        (string) rates[i].tick_volume + ");";
         insert(query);

      }

      return copied;
      
     }
  else Print("Failed to get history data for the symbol ", Symbol());
  
  return 0;
   
}




int getRates(string symbol, ENUM_TIMEFRAMES timeframe, datetime startDate, MqlRates& rates[]){
   int copied = CopyRates(symbol, timeframe, startDate, TimeCurrent(), rates); //);
   return ArrayRange(rates, 0);
   
}


int getRatesForTimeRange(string symbol, ENUM_TIMEFRAMES timeframe, datetime startDate, datetime endDate, MqlRates& rates[]){
   int copied = CopyRates(symbol, timeframe, startDate, endDate, rates);
   return ArrayRange(rates, 0);
}


int getAllAvailableRates(string symbol, ENUM_TIMEFRAMES timeframe, int startPos, MqlRates& rates[]){
   int count = 5000;
   int copied = CopyRates(symbol, timeframe, startPos, count, rates);
   return ArrayRange(rates, 0);
}

void checkDataConsistency(){
   string query = "CALL _checkQuotationsConsistency();";
   insert(query);   
}