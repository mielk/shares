//+------------------------------------------------------------------+
//|                                                       DBTest.mq4 |
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

#include <MQLMySQL.mqh>

//#import "libmysql.dll"
//int mysql_init(int db);
//int mysql_errno(int TMYSQL);
//int mysql_real_connect( int TMYSQL,string host,string user,string password, string DB,int port,int socket,int clientflag);
//int mysql_real_query(int TMSQL,string query,int length);
//void mysql_close(int TMSQL);
//#import

string INI;

int mysql;
int init(){
//----

   mysql = mysql_init(mysql);
   Alert("MySQL: " + mysql);
   
   if (mysql != 0){
      Print("allocated");
   }
   
   string host = "localhost";
   string user = "root";
   string password = "79tmkmlk33";
   string DB = "mielk";
   int clientflag = 0;
   int port = 3306;
   string socket = "";
   int res = mysql_real_connect(mysql, host, user, password, DB, port, socket, clientflag);
   int err = GetLastError();
   
   
   if (res == mysql){
      Print("connected");
   }
   else
   {
      Alert("Connection error");
      //Print("error=",mysql," ",mysql_errno(mysql)," ");
   }
   
   return(0);
   
  }
  
  
//+------------------------------------------------------------------+
//| expert deinitialization function                                 |
//+------------------------------------------------------------------+
int deinit()
  {
//----
   mysql_close(mysql);
//----
   return(0);
   
  }


void OnStart()
//int start()
  {
  
   Alert("BeforeInit");
   
   init();
   
   Alert("AfterInit");
   
   string query = "";
   int length = 0;
   
   //query = StringConcatenate("insert into ticks(margin,freemargin,date,ask,bid,symbol,equity) values(",AccountMargin(),",",AccountFreeMargin(),",\"",TimeToStr(CurTime(),TIME_DATE|TIME_SECONDS),"\",",NormalizeDouble(Ask,4),",",NormalizeDouble(Bid,4),",\"",Symbol(),"\",",AccountEquity(),");");
   query = ("INSERT INTO test(v) VALUES('a');");
   Alert(query);
   
   length = StringLen(query);
   
   Alert("length: " + length);
   mysql_real_query(mysql, query, length);
   int myerr = mysql_errno(mysql);
   if (myerr > 0){
      Alert("error = " + myerr);
   }
   
   //return(0);

  }
 
