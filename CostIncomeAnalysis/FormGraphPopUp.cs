using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace CostIncomeAnalysis
{
    public partial class FormGraphPopUp : Form
    {
        int[] monthlyVariable = new int[8];
        int[] monthlyTotalCost = new int[8];
        int[] monthlyProfitLoss = new int[8];
        int[] monthlyFixedCost = new int[8];
        int[] monthlyTurnover = new int[8];

        public FormGraphPopUp()
        {
            InitializeComponent();
        }

        //  Our constructor for passing parameters to the form
        public FormGraphPopUp(int[] monthlyVariable, int[] monthlyTotalCost, int[] monthlyProfitLoss, int[] monthlyFixedCost, int[] monthlyTurnover)
        {
            InitializeComponent();
            this.monthlyVariable = monthlyVariable;
            this.monthlyTotalCost = monthlyTotalCost;
            this.monthlyProfitLoss = monthlyProfitLoss;
            this.monthlyFixedCost = monthlyFixedCost;
            this.monthlyTurnover = monthlyTurnover;   
        }

        private void FormGraphPopUp_Load(object sender, EventArgs e)
        {
            dataGridView1.ColumnCount = 9;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[0].Name = "Monthly Revenue";

            for (int i = 0; i < monthlyTurnover.Length; i++)
            {
                dataGridView1.Columns[i + 1].Name = monthlyTurnover[i].ToString();
            }

            chart1.Series.Clear();
            // chart.Size = new System.Drawing.Size(800, 300);
            chart1.Titles.Add("Break Even Analysis");

            chart1.ChartAreas[0].AxisX.Minimum = monthlyTurnover[0];
            chart1.ChartAreas[0].AxisX.Maximum = monthlyTurnover[7];
            // chart1.ChartAreas[0].AxisX.IntervalOffset = 5000;

            chart1.ChartAreas[0].AxisX.Title = "Monthly revenue";
            chart1.ChartAreas[0].AxisX.Interval = 3000;
            chart1.ChartAreas[0].AxisY.Title = "Costs and Profits";

            //var count = 0;
            //foreach (CustomLabel lbl in chart1.ChartAreas[0].AxisX.CustomLabels)
            //{
            //    lbl.Text = monthlyTurnover[count].ToString();
            //    count++;
            //}

            var row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = "Fixed Costs" });
            Series series = this.chart1.Series.Add("Fixed Costs");
            series.ChartType = SeriesChartType.StackedArea;

            for (int i = 0; i < monthlyFixedCost.Length; i++)
            {
                series.Points.AddXY(monthlyTurnover[i], monthlyFixedCost[i]);
                row.Cells.Add(new DataGridViewTextBoxCell { Value = monthlyFixedCost[i].ToString() });
            }
            dataGridView1.Rows.Add(row);
            chart1.Series["Fixed Costs"].Points[0].IsValueShownAsLabel = true;

            row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = "Variable Costs" });
            Series series2 = this.chart1.Series.Add("Variable Costs");
            series2.ChartType = SeriesChartType.StackedArea;
            for (int i = 0; i < monthlyVariable.Length; i++)
            {
                row.Cells.Add(new DataGridViewTextBoxCell { Value = monthlyVariable[i].ToString() });
                if (monthlyProfitLoss[i] < 0)
                    monthlyVariable[i] = monthlyVariable[i] + monthlyProfitLoss[i];   
                series2.Points.AddXY(monthlyTurnover[i], monthlyVariable[i]);
            }
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = "Total Costs" });
            Series series4 = this.chart1.Series.Add("Total Costs");
            series4.ChartType = SeriesChartType.Line;
            series4.Color = Color.Red;
            series4.BorderWidth = 4;
            for (int i = 0; i < monthlyTotalCost.Length; i++)
            {
                row.Cells.Add(new DataGridViewTextBoxCell { Value = monthlyTotalCost[i].ToString() });
                series4.Points.AddXY(monthlyTurnover[i], monthlyTotalCost[i]);
            }
            dataGridView1.Rows.Add(row);

            row = new DataGridViewRow();
            row.Cells.Add(new DataGridViewTextBoxCell { Value = "Surplus/Deficit" });
            Series series3 = this.chart1.Series.Add("Profit/Loss");
            series3.ChartType = SeriesChartType.StackedArea;
            series3.Color = Color.Yellow;
            for (int i = 0; i < monthlyProfitLoss.Length; i++)
            {
                row.Cells.Add(new DataGridViewTextBoxCell { Value = monthlyProfitLoss[i].ToString() });
                if (monthlyProfitLoss[i] < 0)
                    monthlyProfitLoss[i] = Math.Abs(monthlyProfitLoss[i]);
                series3.Points.AddXY(monthlyTurnover[i], monthlyProfitLoss[i]);
            }
            dataGridView1.Rows.Add(row);

        }

        int count;
        private void chart1_Customize(object sender, EventArgs e)
        {
            //count = 0;
            //foreach (CustomLabel lbl in chart1.ChartAreas[0].AxisX.CustomLabels)
            //{
            //    lbl.Text = monthlyTurnover[count].ToString();
            //    count++;
            //}
        }
    }
}
