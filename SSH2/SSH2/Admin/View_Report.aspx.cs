using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ASPJ.Admin
{
    public partial class View_Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //       LineChart.Series["LineSeries"].Points.DataBindXY();
            string[] x = { "BOB","test"};
            decimal[] y = { 10,20};
            lineChart.Series.Add(new AjaxControlToolkit.LineChartSeries {Name = "BOB", Data = y });
            lineChart.CategoriesAxis = string.Join(",",x);
        }
    }
}