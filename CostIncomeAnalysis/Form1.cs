using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CostIncomeAnalysis
{
    public partial class Form1 : Form
    {
        decimal AvSale, TurnOver, VarCost, OHCost, OHRent, OHWages, OHOther = 0.00M;
        decimal s;          // temp field for tryparse op
        decimal BESalesPerMonth = 0.00M;
        int OHPerMonth = 0;
        bool DataValidated = false;
        int[] monthlyVariable = new int[8];
        int[] monthlyTotalCost = new int[8];
        int[] monthlyProfitLoss = new int[8];
        int[] monthlyFixedCost = new int[8];

        public Form1()
        {
            InitializeComponent();
        }

        //  Check our input fields to ensure they are not zero length and
        //      are valid numeric data

        private void buttonCalculateBE_Click(object sender, EventArgs e)
        {
            bool validData = true;

            if ((textAvSale.Text.Length > 0) && (decimal.TryParse(textAvSale.Text, out s)))
            {
                AvSale = decimal.Parse(textAvSale.Text);
                textAvSale.BackColor = Color.White;
            }
            else
            {
                textAvSale.BackColor = Color.Red;
                validData = false;
            }

            if ((textTurnOver.Text.Length > 0) && (decimal.TryParse(textTurnOver.Text, out s)) && (s > 0.00M))
            {
                TurnOver = decimal.Parse(textTurnOver.Text);
                textTurnOver.BackColor = Color.White;
            }
            else
            {
                textTurnOver.BackColor = Color.Red;
                validData = false;
            }

            if ((textVarCost.Text.Length > 0) && (decimal.TryParse(textVarCost.Text, out s)))
            {
                VarCost = decimal.Parse(textVarCost.Text);
                textVarCost.BackColor = Color.White;
            }
            else
            {
                textVarCost.BackColor = Color.Red;
                validData = false;
            }

            if ((textOHCost.Text.Length > 0) && (decimal.TryParse(textOHCost.Text, out s)))
            {
                OHCost = decimal.Parse(textOHCost.Text);
                textOHCost.BackColor = Color.White;
            }
            else
            {
                textOHCost.BackColor = Color.Red;
                validData = false;
            }

            if (validData)
            {
                DataValidated = true;
                CalculateNetProfit();
                CalculateContributionMargins();
            }
            else
            {
                DataValidated = false;
                MessageBox.Show("Invalid numeric data entered!!!", "Please correct");
            }

        }

        private void generateGraph_Click(object sender, EventArgs e)
        {
            if (!DataValidated)
            {
                MessageBox.Show("You must enter valid data first!!!");
                return;
            }

            //  Calculate monthly revenues for graph
            int[] monthlyRevenue = new int[8];
            monthlyRevenue[3] = (int)(BESalesPerMonth / 1000) * 1000;

            monthlyRevenue[2] = (int)(Math.Round((monthlyRevenue[3] * 0.9) / 1000)) * 1000;
            monthlyRevenue[1] = (int)(Math.Round((monthlyRevenue[2] * 0.9) / 1000)) * 1000;
            monthlyRevenue[0] = (int)(Math.Round((monthlyRevenue[1] * 0.9) / 1000)) * 1000;

            monthlyRevenue[4] = (int)(Math.Round((monthlyRevenue[3] * 1.1) / 1000)) * 1000;
            monthlyRevenue[5] = (int)(Math.Round((monthlyRevenue[4] * 1.1) / 1000)) * 1000;
            monthlyRevenue[6] = (int)(Math.Round((monthlyRevenue[5] * 1.1) / 1000)) * 1000;
            monthlyRevenue[7] = (int)(Math.Round((monthlyRevenue[6] * 1.1) / 1000)) * 1000;

            //  Calculate variable costs based on monthly revenue and margin, and store total costs and net profit/loss for graph

            
            for (int i = 0; i < 8; i++)
            {
                monthlyVariable[i] = (int)((VarCost / TurnOver) * monthlyRevenue[i]);
                monthlyTotalCost[i] = monthlyVariable[i] + (int)OHPerMonth;
                monthlyFixedCost[i] = (int)OHPerMonth;
                monthlyProfitLoss[i] = monthlyRevenue[i] - monthlyTotalCost[i];
            }

            //  Setup graph display

            var form = new FormGraphPopUp(monthlyVariable, monthlyTotalCost, monthlyProfitLoss, monthlyFixedCost, monthlyRevenue);
            form.Show(); 

        }

        private void CalculateContributionMargins()
        {
            decimal CostsPerDollar = VarCost / TurnOver;
            labelCostPerDollarSales.Text = CostsPerDollar.ToString("0.##");
            labelVariableCost.Text = VarCost.ToString();
            labelSales.Text = TurnOver.ToString();

            decimal ContributionMargin = 1 - CostsPerDollar;
            labelContribMargin.Text = ContributionMargin.ToString("0.##");
            labelContrib.Text = "1 - " + CostsPerDollar.ToString("0.##");

            decimal BreakEvenTurnover = OHCost / ContributionMargin;
            labelBETurnover.Text = BreakEvenTurnover.ToString("c2");

            decimal BEUnits = BreakEvenTurnover / AvSale;
            labelBEUnits.Text = BEUnits.ToString("n2");

            decimal BEUnitsPerMonth = BEUnits / (decimal)numericUpDownMonths.Value;
            labelBEUnitsPerMonth.Text = BEUnitsPerMonth.ToString("n2");

            BESalesPerMonth = BreakEvenTurnover / (decimal)numericUpDownMonths.Value;
            labelBESalesPerMonth.Text = BESalesPerMonth.ToString("c2");

            decimal BESalesPerWeek = BreakEvenTurnover / (((decimal)numericUpDownMonths.Value * (decimal)46.0) / (decimal)12.0);
            labelBESalesPerWeek.Text = BESalesPerWeek.ToString("c2");

            decimal BESalesPerDay = BESalesPerWeek / (decimal)numericUpDownDaysWeek.Value;
            labelBEsalesPerDay.Text = BESalesPerDay.ToString("c2");

            OHPerMonth = (int)(OHCost / (decimal)numericUpDownMonths.Value);
            labelOHPerMonth.Text = OHPerMonth.ToString("c2");

            decimal OHPerWeek = OHPerMonth / ((decimal)46.0 / (decimal)12.0);
            labelOHPerWeek.Text = OHPerWeek.ToString("c2");

            decimal OHPerDay = OHPerWeek / (decimal)numericUpDownDaysWeek.Value;
            labelOHPerDay.Text = OHPerDay.ToString("c2");

            decimal SalesPerMonth = TurnOver / (decimal)numericUpDownMonths.Value;
            labelSalesPerMonth.Text = SalesPerMonth.ToString("c2");

            decimal SalesPerWeek = SalesPerMonth / ((decimal)46.0 / (decimal)12.0);
            labelSalesPerWeek.Text = SalesPerWeek.ToString("c2");

            decimal SalesPerDay = SalesPerWeek / (decimal)numericUpDownDaysWeek.Value;
            labelSalesPerDay.Text = SalesPerDay.ToString("c2");

            decimal DaysToBreakEven = BESalesPerMonth / SalesPerDay;
            labelDaysToBreakEven.Text = DaysToBreakEven.ToString("n2");
        }

        private void textOHRent_Leave(object sender, EventArgs e)
        {
            if ((textOHRent.Text.Length > 0) && (decimal.TryParse(textOHRent.Text, out s)))
            {
                OHRent = decimal.Parse(textOHRent.Text);
                textOHRent.BackColor = Color.White;
                CalculateOHCost();
            }
            else
                textOHRent.BackColor = Color.Red;
        }

        private void textOHWages_Leave(object sender, EventArgs e)
        {
            if ((textOHWages.Text.Length > 0) && (decimal.TryParse(textOHWages.Text, out s)))
            {
                OHWages = decimal.Parse(textOHWages.Text);
                textOHWages.BackColor = Color.White;
                CalculateOHCost();
            }
            else
                textOHWages.BackColor = Color.Red;
        }

        private void textOHOther_Leave(object sender, EventArgs e)
        {
            if ((textOHOther.Text.Length > 0) && (decimal.TryParse(textOHOther.Text, out s)))
            {
                OHOther = decimal.Parse(textOHOther.Text);
                textOHOther.BackColor = Color.White;
                CalculateOHCost();
            }
            else
                textOHOther.BackColor = Color.Red;
        }

        private void CalculateOHCost()
        {
            OHCost = OHRent + OHWages + OHOther;
            textOHCost.Text = OHCost.ToString();
        }

        private void CalculateNetProfit()
        {
            decimal NetProfit = TurnOver - VarCost - OHCost;
            textNetProfit.Text = NetProfit.ToString();
        }

    }
}
