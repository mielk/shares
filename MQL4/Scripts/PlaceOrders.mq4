//+------------------------------------------------------------------+
//|                                                  PlaceOrders.mq4 |
//|                                                Tomasz Mielniczek |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property copyright "Tomasz Mielniczek"
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict
//+------------------------------------------------------------------+
//| Script program start function                                    |
//+------------------------------------------------------------------+
void OnStart()
  {
//---

   string symbol = "AUDUSD";
   double volume = 0.1;
   double startMarginPips = 250;
   double difference = 50;
   double counter = 1;
   double sl = 100;
   bool clearPrevious = true;
   
   Alert("Placing pre-events orders");
   
   placeOrders(symbol, false, volume, startMarginPips, difference, counter, sl, clearPrevious);
      
  }
//+------------------------------------------------------------------+


   

bool placeOrders(string symbol, bool isBuy, double volume, double startMarginPips, double difference, double counter, double sl, 
                  bool clearPrevious){

   double ask = MarketInfo(symbol, MODE_ASK);   //kupno
   double bid = MarketInfo(symbol, MODE_BID);   //sprzeda¿
   double point = MarketInfo(symbol, MODE_POINT);
   double price;
   
   
   
   //Clear previous orders if [clearPrevious] parameter is set to True.
   
   
   //Place sell orders.
   price = bid - startMarginPips * point;
   Alert(price);
   for (int i = 0; i < counter; i++){
      bool result;
      result = OrderSend(symbol, OP_SELLSTOP, volume, price, 10, price + sl * point, 5);
      Alert("Result: " + result);
      price -= difference * point;   
   }
   
   
   //Place buy orders.
   price = ask + startMarginPips * point;
   for (int i = 0; i < counter; i++){
      bool result;
      result = OrderSend(symbol, OP_BUYSTOP, volume, price, 10, price - sl * point, 5);
      price += difference * point;
   }


   Alert("Action completed");
   return true;
   
}