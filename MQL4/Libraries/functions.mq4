//+------------------------------------------------------------------+
//|                                                    functions.mq4 |
//|                                                Tomasz Mielniczek |
//|                                             https://www.mql5.com |
//+------------------------------------------------------------------+
#property library
#property copyright "Tomasz Mielniczek"
#property link      "https://www.mql5.com"
#property version   "1.00"
#property strict
//+------------------------------------------------------------------+
//| My function                                                      |
//+------------------------------------------------------------------+
// int MyCalculator(int value,int value2) export
//   {
//    return(value+value2);
//   }
//+------------------------------------------------------------------+


string addLeadingZero(string value, int length){

   if (StringLen(value) >= length){
      return value;
   } else {
      string result = "";
      for (int i = 0; i < (length - StringLen(value)); i++){
         result = result + "0";
      }
      
      result = result + value;
      
      return result;
   }

}

string dateToTime(datetime date){
   string strDate = (string) TimeYear(date) + "-" + 
                    addLeadingZero((string) TimeMonth(date), 2) + "-" + 
                    addLeadingZero((string) TimeDay(date), 2) + " " + 
                    addLeadingZero((string) TimeHour(date), 2) + ":" + 
                    addLeadingZero((string) TimeMinute(date), 2) + ":" + 
                    addLeadingZero((string) TimeSeconds(date), 2);
   return strDate;
}


ENUM_TIMEFRAMES stringToTimeframe(string str){

   if (str == "D1")
   {
      return PERIOD_D1;
   } 
   else if (str == "H1")
   {
      return PERIOD_H1;
   }
   else if (str == "H4")
   {
      return PERIOD_H4;
   }
   else if (str == "M15")
   {
      return PERIOD_M15;
   }
   else if (str == "M30")
   {
      return PERIOD_M30;
   }
   else if (str == "M5")
   {
      return PERIOD_M5;
   }
   else if (str == "MN1")
   {
      return PERIOD_MN1;
   }
   else if (str == "W1")
   {
      return PERIOD_W1;
   } 
   else 
   {
      return PERIOD_M1;
   }
   
}


string periodToString(ENUM_TIMEFRAMES timeframe){
   
   switch(timeframe){
      case PERIOD_D1: return "D1";
      case PERIOD_H1: return "H1";
      case PERIOD_H4: return "H4";
      case PERIOD_M15: return "M15";
      case PERIOD_M30: return "M30";
      case PERIOD_M5: return "M5";
      case PERIOD_MN1: return "MN1";
      case PERIOD_W1: return "W1";
   }
   
   return "XX";
   
}




datetime convertStringToDate(string str){

   //Check if str is not null.
   if (str == NULL){
      return 0;
   }

   int year = (int) StringSubstr(str, 0, 4);
   int month = (int) StringSubstr(str, 5, 2);
   int day = (int) StringSubstr(str, 8, 2);
   int hour = (int) StringSubstr(str, 11, 2);
   int minute = (int) StringSubstr(str, 14, 2);
   int second = (int) StringSubstr(str, 17, 2);
   
   string strDate = (string)year + "/" + (string)month + "/" + (string)day + " " + 
                    (string)hour + ":" + (string)minute + ":" + (string)second;
   
   return StrToTime(strDate);      

}

