using MSUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace ASPJ.Admin
{
    public class DataHolder
    {
        public List<ILogRecord> dataSet { get; set; }
        public List<string> column { get; set; }
        public int index = 0;
        public DataHolder()
        {
            dataSet = new List<ILogRecord>();
            column = new List<string>();
        }
        public void addColumn(string columnName)
        {
            column.Add(columnName);
        }
        public void addData(ILogRecord data)
        {

            dataSet.Add(data);
            Debug.WriteLine(dataSet.Count);
        }
        public int getColumnCount()
        {
            return column.Count();

        }
        public string getColumnName(int index)
        {
            return column[index];
        }
        public bool atEnd()
        {
            if (index==dataSet.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void moveNext()
        {
            index++;
        }
        public ILogRecord getRecord()
        {
            return dataSet[index];
        }
    }
}