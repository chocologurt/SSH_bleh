using MSUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPJ.Admin
{
    public partial class View_Log : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            retrieveSearch();
            retrieveReport();
            if (!IsPostBack)
            {
                if (Request.QueryString["view"] == "2")
                {
                    MultiView1.ActiveViewIndex = 1;
                }
                else
                {
                    MultiView1.ActiveViewIndex = 0;
                }
            }
        }
        public void getColumns(object sender, EventArgs e)
        {
            columns.Items.Clear();
            List<string> columnList = Logger.retrieveColumns(logList.SelectedValue);
            Debug.WriteLine("Iam here");
            foreach(string s in columnList)
            {
                if(s!="Filename")
                    columns.Items.Add(new ListItem(s));
            }
        }
        public void search_clicked(object sender,EventArgs e)
        {
            if (TimeList.SelectedIndex == 0)
            {
                getLogTable();
            }
            else
            {
                getLogTableWithTime();
            }
        }
        public void getLogTable()
        {
            LogTable.Rows.Clear();
            Logger l = new Logger(query.Text);
            ILogRecordset result=l.executeCommand();
            
            ILogRecord dataRow = null;
            TableHeaderRow header = new TableHeaderRow();
            TableHeaderCell headerCell;

            for (int i = 0; i < result.getColumnCount(); i++)
            {
                if (result.getColumnName(i) == "Filename")
                {
                    continue;
                }
                headerCell = new TableHeaderCell();
                headerCell.Text = result.getColumnName(i);
                headerCell.CssClass = "forumHeader";
                headerCell.Style.Add("border", "1px solid black");
                header.Cells.Add(headerCell);
                
            }
            LogTable.Rows.Add(header);

            while (!result.atEnd())
            {
                dataRow = result.getRecord();
                TableRow row = new TableRow();
                TableCell cell;
                for (int i = 0; i < result.getColumnCount(); i++)
                {
                    if (result.getColumnName(i) == "Filename")
                    {
                        continue;
                    }
                    cell = new TableCell();
                    cell.Text = dataRow.getValue(i).ToString();
                    cell.Style.Add("border", "1px solid black");
                    row.Cells.Add(cell);
                }
                LogTable.Rows.Add(row);
                result.moveNext();
            }
            pieChart.ImageUrl = "pieChart.cshtml?cmd="+query.Text+"&title="+ title.Value;
            barChart.ImageUrl = "barChart.cshtml?cmd=" + query.Text + "&title=" + title.Value;
            title.Value = "Custom Search";
            
        }
        public void getLogTableWithTime()
        {
            LogTable.Rows.Clear();
            Logger l = new Logger(query.Text);
            DataHolder result = l.executeCommandWithTime(Convert.ToInt32(TimeList.SelectedValue));

            ILogRecord dataRow = null;
            TableHeaderRow header = new TableHeaderRow();
            TableHeaderCell headerCell;

            for (int i = 0; i < result.getColumnCount(); i++)
            {
                if (result.getColumnName(i) == "Filename")
                {
                    continue;
                }
                headerCell = new TableHeaderCell();
                headerCell.Text = result.getColumnName(i);
                headerCell.CssClass = "forumHeader";
                headerCell.Style.Add("border", "1px solid black");
                header.Cells.Add(headerCell);

            }
            LogTable.Rows.Add(header);

            while (!result.atEnd())
            {
                dataRow = result.getRecord();
                TableRow row = new TableRow();
                TableCell cell;
                for (int i = 0; i < result.getColumnCount(); i++)
                {
                    if (result.getColumnName(i) == "Filename")
                    {
                        continue;
                    }
                    cell = new TableCell();
                    cell.Text = dataRow.getValue(i).ToString();
                    cell.Style.Add("border", "1px solid black");
                    row.Cells.Add(cell);
                }
                LogTable.Rows.Add(row);
                result.moveNext();
            }
            pieChart.ImageUrl = "pieChart.cshtml?cmd=" + query.Text + "&title=" + title.Value;
            barChart.ImageUrl = "barChart.cshtml?cmd=" + query.Text + "&title=" + title.Value;
            title.Value = "Custom Search";

        }
        public void saveSearches(object sender, EventArgs e)
        {
            string queryText = query.Text;
            string queryName = searchName.Text;

            Search s = new Search(queryName, queryText);
            s.addSearch();
        }
        public void retrieveSearch()
        {
            searchTable.Rows.Clear();
            List<Search> sList = Search.retrieveSearch();

            TableHeaderRow header = new TableHeaderRow();
            TableHeaderCell headerCell = new TableHeaderCell();
            headerCell.Text = "Search Name";
            header.Cells.Add(headerCell);
            headerCell = new TableHeaderCell();
            headerCell.Text = "Query";
            headerCell.Width = 450;
            header.Cells.Add(headerCell);
            headerCell = new TableHeaderCell();
            headerCell.Text = "Run/Delete";
            header.Cells.Add(headerCell);

            searchTable.Rows.Add(header);
            foreach (Search s in sList)
            {
                TableRow row = new TableRow();

                TableCell test = new TableCell();
                test.Text = s.searchName;
                row.Cells.Add(test);

                test = new TableCell();

                test.Text = s.query;
                row.Cells.Add(test);


                Button b = new Button();
                b.Text = "Run Search";
                //       b.PostBackUrl = "?search=" + s.query;
                b.CommandArgument = s.searchName+";"+s.query;
                b.Command += runSearch;

                test = new TableCell();
                test.Controls.Add(b);
                row.Cells.Add(test);

                searchTable.Rows.Add(row);            
            }

        }
        public void retrieveReport()
        {
            string[] reportList = { "SELECT result,count(result) FROM auth.log GROUP BY result"
                    ,"SELECT pageAccess,COUNT(pageAccess) FROM access.log GROUP BY pageAccess"};
            string[] reportName = { "Login Report","Access Report"};
            reportTable.Rows.Clear();

            TableHeaderRow header = new TableHeaderRow();
            TableHeaderCell headerCell = new TableHeaderCell();
            headerCell.Text = "Report Name";
            headerCell.Width = 450;
            header.Cells.Add(headerCell);
            headerCell = new TableHeaderCell();
            headerCell.Text = "Run/Delete";
            header.Cells.Add(headerCell);

            reportTable.Rows.Add(header);
            for (int i=0;i<reportList.Length;i++)
            {
                TableRow row = new TableRow();

                TableCell test = new TableCell();
                test.Text = reportName[i];
                row.Cells.Add(test);


                Button b = new Button();
                b.Text = "Run Report";
               //       b.PostBackUrl = "?search=" + s.query;
                b.CommandArgument = reportName[i]+";" + reportList[i];
                b.Command += runReport;

                test = new TableCell();
                test.Controls.Add(b);
                row.Cells.Add(test);

                reportTable.Rows.Add(row);

            }


        }
        public void runSearch(object sender,CommandEventArgs e)
        {
            string[] arg = e.CommandArgument.ToString().Split(';');
            query.Text = arg[1];
            //     Server.Transfer("View_Log.aspx",true);
            MultiView1.ActiveViewIndex = 0;
            title.Value = arg[0];
            getLogTable();
        }
        public void runReport(object sender, CommandEventArgs e)
        {
            string[] arg = e.CommandArgument.ToString().Split(';');
            query.Text = arg[1];
            //     Server.Transfer("View_Log.aspx",true);
            MultiView1.ActiveViewIndex = 0;
            title.Value = arg[0];
            getLogTable();
            DataView.ActiveViewIndex = 1;
        }


        public void switchDataView(object sender,CommandEventArgs e)
        {
            int view = Convert.ToInt32(e.CommandArgument);
            DataView.ActiveViewIndex = view;
        }
    }
}