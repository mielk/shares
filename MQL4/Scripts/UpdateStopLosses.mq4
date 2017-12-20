//+------------------------------------------------------------------+
//|                                             UpdateStopLosses.mq4 |
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
   
   Alert("Tightening stop losses");
   
   const double SL = 100;
   
   //Count orders.
   int total = OrdersTotal();
   
   //Iterate through all orders.
   for(int i = total - 1; i >= 0; i--)
   {
   
      OrderSelect(i, SELECT_BY_POS);
      
      int type = OrderType();
      
      
      if (type == OP_BUY || type == OP_SELL){
      
         string ticket = OrderTicket();
         string symbol = OrderSymbol();
         double sl = OrderStopLoss();
         double point = MarketInfo(symbol, MODE_POINT);
         
         
         
         if (type == OP_BUY){
         
            double newSl = MarketInfo(symbol, MODE_ASK) - (point * SL);
            if (sl == 0 || newSl > sl){
               OrderModify(ticket, OrderOpenPrice(), newSl, OrderTakeProfit(), OrderExpiration(), clrNONE);
               Alert("SL for trade " + ticket + " modified from " + sl + " to " + newSl + " (current market price: " + MarketInfo(symbol, MODE_ASK) + ")");
            } else {
               Alert("SL for trade " + ticket + " not modified, since the actual one is closer to the market price.");
            }
         
            
         
         } else if (type == OP_SELL){
         
            double newSl = MarketInfo(symbol, MODE_BID) + (point * SL);
            if (sl == 0 || newSl < sl){
               OrderModify(ticket, OrderOpenPrice(), newSl, OrderTakeProfit(), OrderExpiration(), clrNONE);
               Alert("SL for trade " + ticket + " modified from " + sl + " to " + newSl + " (current market price: " + MarketInfo(symbol, MODE_BID) + ")");
            } else {
               Alert("SL for trade " + ticket + " not modified, since the actual one is closer to the market price.");
            }
                     
         }
         
         
      
      }
    
   }   
   
   
  }
//+------------------------------------------------------------------+