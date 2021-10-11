﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;

namespace FinCalc
{
    public partial class Form_calc : Form
    {
        int all_periods;
        int annual_period = 12;
        string[] duration = new string[2] { "Месяц", "Год" };

        public Form_calc()
        {
            InitializeComponent();
            cmb_duration.SelectedIndex = 0;
            cmb_periods.SelectedIndex = 0;
            date_credit.Value = DateTime.Today;
        }

        public void keypress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        public void keypress_float(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && !(e.KeyChar == ','))
            {
                e.Handled = true;
            }
            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(','))
            {
                e.Handled = true;
            }
        }

        private void txt_duration_TextChanged(object sender, EventArgs e)
        {

            if (txt_duration.Text.Length > 0)
            {
                switch (txt_duration.Text[txt_duration.Text.Length - 1])
                {
                    case '1':
                        {
                            cmb_duration.Items[0] = "Месяц";
                            cmb_duration.Items[1] = "Год";
                            break;
                        }
                    case '2':
                    case '3':
                    case '4':
                        {
                            cmb_duration.Items[0] = "Месяца";
                            cmb_duration.Items[1] = "Года";
                            break;
                        }
                    default:
                        {
                            cmb_duration.Items[0] = "Месяцев";
                            cmb_duration.Items[1] = "Лет";
                            break;
                        }
                }
                if (Int32.Parse(txt_duration.Text) > 10 && Int32.Parse(txt_duration.Text) < 20)
                {
                    cmb_duration.Items[0] = "Месяцев";
                    cmb_duration.Items[1] = "Лет";
                }
                cmb_duration.Enabled = true;
                if (cmb_duration.SelectedIndex == -1)
                    cmb_duration.SelectedIndex = 0;
                cmb_periods.Enabled = true;
                if (cmb_periods.SelectedIndex == -1)
                    cmb_periods.SelectedIndex = 0;
            }
            else
            {
                cmb_duration.Enabled = false;
                cmb_periods.Enabled = false;
                cmb_periods.SelectedIndex = -1;
                cmb_duration.SelectedIndex = -1;
            }
        }

        public bool Check()
        {
            if (txt_credit_sum.TextLength > 0 && txt_duration.TextLength > 0 && txt_procent.TextLength > 0)
                return true;
            else return false;
        }

        private void date_credit_ValueChanged(object sender, EventArgs e)
        {
            if (date_credit.Value < DateTime.Today)
            {
                date_credit.Value = DateTime.Today;
            }
        }

        private void cmb_duration_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmb_duration.SelectedIndex)
            {
                case 0:
                    {
                        cmb_periods.Enabled = false;
                        break;
                    }
                case 1:
                    {
                        cmb_periods.Enabled = true;
                        break;
                    }
            }
        }

        private void btn_calculation_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                chart_results.Series.Clear();
                double all_pays = 0;
                double over_pay = 0;
                int credit_sum = Int32.Parse(txt_credit_sum.Text);
                double ostatok = Double.Parse(txt_credit_sum.Text);
                double procent = Double.Parse(txt_procent.Text) / 100;
                switch (cmb_duration.SelectedIndex)
                {
                    case 0:
                        {
                            all_periods = Int32.Parse(txt_duration.Text);
                            cmb_periods.Enabled = false;
                            break;
                        }
                    case 1:
                        {
                            all_periods = Int32.Parse(txt_duration.Text) * 12;
                            cmb_periods.Enabled = true;
                            break;
                        }
                }
                switch (cmb_periods.SelectedIndex)
                {
                    case 0:
                        {
                            annual_period = 12;
                            break;
                        }
                    case 1:
                        {
                            all_periods /= 3;
                            annual_period = 4;
                            break;
                        }
                    case 2:
                        {
                            all_periods /= 12;
                            annual_period = 1;
                            break;
                        }
                }
                double payment;
                while (ostatok > double.Epsilon)
                {
                    payment = Math.Round(((double)credit_sum / all_periods) + ostatok * ((double)procent / annual_period), 2);
                    all_pays += payment;
                    over_pay = Math.Round(over_pay + ostatok * ((double)procent / annual_period), 2);
                    ostatok = Math.Round(ostatok - payment + ostatok * ((double)procent / annual_period), 2);
                }
                chart_results.Series.Add("Results");
                chart_results.Series[0].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
                chart_results.Series[0].Points.AddY((double)credit_sum);
                chart_results.Series[0].Points[0].LegendText = "Тело кредита";
                chart_results.Series[0].Points.AddY(over_pay);
                chart_results.Series[0].Points[1].LegendText = "Переплата";
                lbl_results_procent.Text = over_pay.ToString();
                lbl_results_all.Text = (credit_sum + over_pay).ToString();
                pnl_results.Visible = true;
            }
            else MessageBox.Show("Проверьте корректность введенных данных");
        }
    }
}
