//+------------------------------------------------------------------+
//|                                                         test.mq4 |
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

   Alert(GetLastError());
   //OrderSend("AUDUSD", OP_SELLSTOP, 0.1, 0.7650, 10, 0.7690, 0.7620);
   Alert(GetLastError());
   
  }
//+------------------------------------------------------------------+
