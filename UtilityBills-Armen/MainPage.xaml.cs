using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UtilityBills_Armen
{
    public partial class MainPage : ContentPage
    {

        private List<string> provincesList = new List<string>();
        private double DayUsage;
        private double EveningUsage;
        private double ProvTax;

        private double DayCharge;
        private double NightCharge;
        private double TotalCharge;
        private double SalesTax;
        private double EnvRebate;
        private double ToPay;

        private bool Renewable;
       
        public MainPage()
        {
            InitializeComponent();

            ClearValues();

            provincesList.Add("AB");
            provincesList.Add("BC");
            provincesList.Add("ON");
            provincesList.Add("NL");

            pkr_Province.ItemsSource = provincesList;

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    sl_LabelSwitch.Orientation = StackOrientation.Vertical;
                    break;
                case Device.Android:
                    sl_LabelSwitch.Orientation = StackOrientation.Horizontal;
                    break;
            }

        }

        void btn_Reset_Clicked(System.Object sender, System.EventArgs e)
        {
            ClearValues();
        }

        void btn_Calculate_Clicked(System.Object sender, System.EventArgs e)
        {
            if (double.TryParse(txt_DayUsage.Text, out DayUsage) && double.TryParse(txt_NightUsage.Text, out EveningUsage) && pkr_Province.SelectedIndex >= 0)
            {
                //parse successful
                CalculateCharges();
                DisplayChargeBreakdown();
                err_MissingValues.IsVisible = false;
                err_UsageValues.IsVisible = false;

            } else if (txt_DayUsage.Text == "" || txt_NightUsage.Text == "" || pkr_Province.SelectedIndex < 0)
            {
                // parse fails because any input is empty
                err_MissingValues.IsVisible = true;
                err_UsageValues.IsVisible = false;
            } else
            {
                //parse fails because input is NaN
                err_UsageValues.IsVisible = true;
                err_MissingValues.IsVisible = false;
            }

        }

        void ClearValues()
        {
            pkr_Province.SelectedIndex = -1;
            txt_DayUsage.Text = "";
            txt_NightUsage.Text = "";
            sw_ReEn.IsToggled = false;
            sw_ReEn.IsEnabled = true;

            err_MissingValues.IsVisible = false;
            err_UsageValues.IsVisible = false;

            lbl_ChargeTitle.IsVisible = false;
            lbl_DaytimeCharge.IsVisible = false;
            lbl_EveningCharge.IsVisible = false;
            lbl_TotalCharge.IsVisible = false;
            lbl_SalesTax.IsVisible = false;
            lbl_EnvRebate.IsVisible = false;

            lbl_Pay.IsVisible = false;
        }

        void pkr_Province_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if(pkr_Province.SelectedIndex == 1) //selected BC
            {
                sw_ReEn.IsToggled = true;
                sw_ReEn.IsEnabled = false;
            } else
            {
                sw_ReEn.IsToggled = false;
                sw_ReEn.IsEnabled = true;
            }

            switch (pkr_Province.SelectedIndex)
            {
                case 0:
                    ProvTax = 0;
                    break;
                case 1:
                    ProvTax = 12;
                    break;
                case 2:
                    ProvTax = 13;
                    break;
                case 3:
                    ProvTax = 15;
                    break;
            }
        }

        void sw_ReEn_Toggled(System.Object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            Renewable = sw_ReEn.IsToggled;
        }

        void CalculateCharges()
        {
            DayCharge = DayUsage * 0.314;
            NightCharge = EveningUsage * 0.186;
            TotalCharge = DayCharge + NightCharge;
            SalesTax = (ProvTax / 100) * TotalCharge;
            if (Renewable == true) {
                EnvRebate = TotalCharge * (9.5 / 100);
            }else
            {
                EnvRebate = 0.00;
            }
            ToPay = TotalCharge + SalesTax - EnvRebate;

        }

        void DisplayChargeBreakdown()
        {
            lbl_DaytimeCharge.Text = $"Daytime Usage Charges: ${Math.Round(DayCharge, 2).ToString("0.00")}";
            lbl_EveningCharge.Text = $"Evening Usage Charges: ${Math.Round(NightCharge, 2).ToString("0.00")}";
            lbl_TotalCharge.Text = $"Total Charges: ${Math.Round(TotalCharge, 2).ToString("0.00")}";
            lbl_SalesTax.Text = $"Sales Tax ({ProvTax}%): ${Math.Round(SalesTax, 2).ToString("0.00")}";
            lbl_EnvRebate.Text = $"Environmental Rebate: -${Math.Round(EnvRebate, 2).ToString("0.00")}";
            lbl_Pay.Text = $"You Must Pay: ${Math.Round(ToPay, 2).ToString("0.00")}";

            lbl_ChargeTitle.IsVisible = true;
            lbl_DaytimeCharge.IsVisible = true;
            lbl_EveningCharge.IsVisible = true;
            lbl_TotalCharge.IsVisible = true;
            lbl_SalesTax.IsVisible = true;
            lbl_EnvRebate.IsVisible = true;
            lbl_Pay.IsVisible = true;
        }
    }
}

