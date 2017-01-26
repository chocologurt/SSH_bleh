using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MSUtil;
using System.Diagnostics;
using ASPJ.Admin;

namespace ASPJ
{
    public class Logger
    {
        private static readonly string currentDir="C:\\Users\\Guangyao\\Documents\\Visual Studio 2015\\Projects\\ASPJ\\ASPJ";
        static readonly string[] logList = {"access.log","auth.log" };
        public string cmd { get; set; }
        public Logger(string cmd)
        {
            this.cmd = cmd;
        }
        public ILogRecordset executeCommand()
        {
            // prepare LogParser Recordset & Record objects
            ILogRecordset rsLP = null;
            ILogRecord rowLP = null;

            LogQueryClassClass LogParser = null;
            COMCSVInputContextClass W3Clog = null;


            LogParser = new LogQueryClassClass();
            
            W3Clog = new COMCSVInputContextClass();
            
            try
            {
                //W3C Logparsing SQL. Replace this SQL query with whatever 
                //you want to retrieve. The example below 
                //will sum up all the bandwidth
                //Usage of a specific folder with name 
                //"userID". Download Log Parser 2.2 
                //from Microsoft and see sample queries.
                

                foreach(string s in logList)
                {
                    cmd = Utils.ReplaceFirst(cmd, s, "'" + currentDir + "\\Logs\\" + s + "'");
                }
                
                Debug.WriteLine(cmd);
                
                // run the query against W3C log
                rsLP = LogParser.Execute(cmd, W3Clog);              
                


            }
            catch
            {
                
            }
            return rsLP;
        }
        public static List<string> retrieveColumns(string logName)
        {
            List<string> columnList = new List<string>();
            if(logList.Contains(logName)){
                ILogRecordset rsLP = null;
                ILogRecord rowLP = null;

                LogQueryClassClass LogParser = null;
                COMCSVInputContextClass W3Clog = null;


                LogParser = new LogQueryClassClass();

                W3Clog = new COMCSVInputContextClass();
                
                try
                {
                    //W3C Logparsing SQL. Replace this SQL query with whatever 
                    //you want to retrieve. The example below 
                    //will sum up all the bandwidth
                    //Usage of a specific folder with name 
                    //"userID". Download Log Parser 2.2 
                    //from Microsoft and see sample queries.
                    string cmd = "SELECT * FROM " + logName;

                    foreach (string s in logList)
                    {
                        cmd = Utils.ReplaceFirst(cmd, s, "'" + currentDir + "\\Logs\\" + s + "'");
                    }

                    Debug.WriteLine(cmd);

                    // run the query against W3C log
                    rsLP = LogParser.Execute(cmd, W3Clog);
                    for(int i = 0; i < rsLP.getColumnCount(); i++)
                    {
                        columnList.Add(rsLP.getColumnName(i));
                    }


                }
                catch
                {

                }
            }


            return columnList;
        }
        public static void errorLogging()
        {

        }
        public static void authLogging(string message)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(currentDir+"\\Logs\\auth.log", true))
            {
                file.WriteLine(message);

                file.Close();
            }
                
        }
        public static void accessLogging(string message)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(currentDir + "\\Logs\\access.log", true))
            {
                file.WriteLine(message);

                file.Close();
            }
        }
        public DataHolder executeCommandWithTime(int hourBefore)
        {
            DataHolder data = new DataHolder();
            ILogRecordset rsLP = null;

            LogQueryClassClass LogParser = null;
            COMCSVInputContextClass W3Clog = null;


            LogParser = new LogQueryClassClass();
             
            W3Clog = new COMCSVInputContextClass();
            try
            {
                //W3C Logparsing SQL. Replace this SQL query with whatever 
                //you want to retrieve. The example below 
                //will sum up all the bandwidth
                //Usage of a specific folder with name 
                //"userID". Download Log Parser 2.2 
                //from Microsoft and see sample queries.


                foreach (string s in logList)
                {
                    cmd = Utils.ReplaceFirst(cmd, s, "'" + currentDir + "\\Logs\\" + s + "'");
                }

                Debug.WriteLine(cmd);
                
                // run the query against W3C log
                rsLP = LogParser.Execute(cmd, W3Clog);
                for(int i=0; i < rsLP.getColumnCount(); i++)
                {
                    data.addColumn(rsLP.getColumnName(i));
                }
                while (!rsLP.atEnd())
                {
                    ILogRecord row = rsLP.getRecord();
                    
                    DateTime time = Convert.ToDateTime(row.getValue("timestamp").ToString());
                    if (time > (DateTime.Now.AddHours(-hourBefore)))
                    {
                        data.addData(row);
                    }
                    rsLP.moveNext();
                }

            }
            catch
            {

            }

            return data;
        }
    }
}