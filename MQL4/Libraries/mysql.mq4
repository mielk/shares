//+------------------------------------------------------------------+
//|                                                    MySQL-003.mq5 |
//|                                   Copyright 2014, Eugene Lugovoy |
//|                                              http://www.mql5.com |
//| Inserting data with multi-statement (DEMO)                       |
//+------------------------------------------------------------------+
#property library
#property copyright "Copyright 2014, Eugene Lugovoy."
#property link      "http://www.mql5.com"
#property version   "1.00"
#property strict

#include <MQLMySQL.mqh>
#include <hash.mqh>
#include "..\libraries\functions.mq4"



string INI;
//+------------------------------------------------------------------+
//| Script program start function                                    |
//+------------------------------------------------------------------+



void insert(string Query){
 string Host, User, Password, Database, Socket; // database credentials
 int Port,ClientFlag;
 int DB; // database identifier

 Print (MySqlVersion());

 INI = TerminalInfoString(TERMINAL_PATH)+"\\MQL5\\Scripts\\MyConnection.ini";
 
 // reading database credentials from INI file
 Host = "localhost";//ReadIni(INI, "MYSQL", "Host");
 User = "root"; //ReadIni(INI, "MYSQL", "User");
 Password = "79tmkmlk33";//ReadIni(INI, "MYSQL", "Password");
 Database = "fx"; //ReadIni(INI, "MYSQL", "Database");
 Port     = 3306;//(int)StringToInteger(ReadIni(INI, "MYSQL", "Port"));
 Socket   = "";//ReadIni(INI, "MYSQL", "Socket");
 ClientFlag = CLIENT_MULTI_STATEMENTS; //(int)StringToInteger(ReadIni(INI, "MYSQL", "ClientFlag"));  

 Print ("Host: ",Host, ", User: ", User, ", Database: ",Database);
 
 // open database connection
 Print ("Connecting...");
 
 DB = MySqlConnect(Host, User, Password, Database, Port, Socket, ClientFlag);
 
 if (DB == -1) { Print ("Connection failed! Error: "+MySqlErrorDescription); } else { Print ("Connected! DBID#",DB);}
 
  // multi-insert
  if (MySqlExecute(DB, Query))
     {
      Print ("Succeeded! 3 rows has been inserted by one query.");
     }
  else
     {
      Print ("Error of multiple statements: ", MySqlErrorDescription);
     }
     
 MySqlDisconnect(DB);
 Print ("Disconnected. Script done!");
}




int fetchArray(string Query, string& data[][]){
   string Host, User, Password, Database, Socket; // database credentials
   int Port,ClientFlag;
   int DB; // database identifier
   string result;
   
   Print (MySqlVersion());
   
   INI = TerminalInfoString(TERMINAL_PATH)+"\\MQL5\\Scripts\\MyConnection.ini";
   
   // reading database credentials from INI file
   Host = "localhost";//ReadIni(INI, "MYSQL", "Host");
   User = "root"; //ReadIni(INI, "MYSQL", "User");
   Password = "79tmkmlk33";//ReadIni(INI, "MYSQL", "Password");
   Database = "fx"; //ReadIni(INI, "MYSQL", "Database");
   Port     = 3306;//(int)StringToInteger(ReadIni(INI, "MYSQL", "Port"));
   Socket   = "";//ReadIni(INI, "MYSQL", "Socket");
   ClientFlag = (int)StringToInteger(ReadIni(INI, "MYSQL", "ClientFlag"));  
   
   // open database connection
   Print ("Connecting...");
   
   DB = MySqlConnect(Host, User, Password, Database, Port, Socket, ClientFlag);
   
   if (DB == -1) { Print ("Connection failed! Error: "+MySqlErrorDescription); return -1; } else { Print ("Connected! DBID#",DB);}
   
   // executing SELECT statement
   int    i,Cursor,Rows;
   int      vId;
   string   vCode;
   
   Print ("SQL> ", Query);
   
   Cursor = MySqlCursorOpen(DB, Query);
   
   Print ("Done");
   
   //Convert result set to string.
   int columns = ArrayRange(data, 1);
   int counter = 0;
   
   if (Cursor >= 0)
    {
     Rows = MySqlCursorRows(Cursor);
     Print (Rows, " row(s) selected.");
     for (i=0; i<Rows; i++)
      {
         if (MySqlCursorFetchRow(Cursor))
            {
            
               ArrayResize(data, ++counter, 1000);
               for (int i = 0; i < columns; i++){
                  data[counter - 1, i] = (string) MySqlGetFieldAsString(Cursor, i);
               }
            }
      }      
      MySqlCursorClose(Cursor); // NEVER FORGET TO CLOSE CURSOR !!!
    }
   else
    {
     Print ("Cursor opening failed. Error: ", MySqlErrorDescription);
    }
   
   MySqlDisconnect(DB);
   Print ("Disconnected. Script done!");
   
   return(ArrayRange(data, 0));
   
}