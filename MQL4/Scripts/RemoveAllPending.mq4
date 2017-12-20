//+------------------------------------------------------------------+
//|                                             RemoveAllPending.mq4 |
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
   
   //Count orders.
   int total = OrdersTotal();
   
   
   for(int i = total - 1; i >= 0; i--)
   {
      OrderSelect(i, SELECT_BY_POS);
      int type = OrderType();
   
      if (type != OP_BUY && type != OP_SELL){
         OrderDelete(OrderTicket());
      }
   
   }
    
 }
//+------------------------------------------------------------------+
