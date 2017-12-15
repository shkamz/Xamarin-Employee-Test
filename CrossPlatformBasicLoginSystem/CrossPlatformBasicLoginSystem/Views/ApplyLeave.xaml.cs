using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossPlatformBasicLoginSystem.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ApplyLeave : ContentPage
    {
        public partial class leavework
        {
            public int Leave_Id { get; set; }
            public string Leave_Status { get; set; }
            public System.DateTime Leave_Date { get; set; }
            public System.DateTime Leave_EndDate { get; set; }
            public string Leave_Desc { get; set; }
            public string Emp_name { get; set; }
            public string Leave_Type { get; set; }
        }

        public class employee
        {

            public string Emp_Id { get; set; }
            public string Emp_Name { get; set; }
            public string Password { get; set; }
            public string Emp_LeaveBalance { get; set; }
        }

        public ApplyLeave()
        {
            InitializeComponent();
            //EntryName.Text = "Fernando";
            CheckBalanceAsync();
            EntryName.Text = App.Current.Properties["Name"].ToString();

        }



        private async void ButtonLogin_Clicked(object sender, EventArgs e)
        {
            //get how many day selected...
            DateTime sdt = datepicker.Date;
            DateTime edt = dateendpicker.Date;
            int f = (edt - sdt).Days;
            EntryDay.Text = f.ToString();

            using (var httpClient = new HttpClient())
            {

                // Create a new post  
                var lol = new leavework
                {
                    Leave_Date = datepicker.Date,
                    Leave_EndDate = dateendpicker.Date,
                    Leave_Status = "Pending",
                    Leave_Desc = EntryReason.Text,
                    Emp_name = App.Current.Properties["Name"].ToString(),
                    Leave_Type = pickertype.Items[pickertype.SelectedIndex]
                };

                // create the request content and define Json  
                var json = JsonConvert.SerializeObject(lol);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                // var httpClient = new HttpClient();
                //  send a POST request  
                var uri = "http://nudgeme-001-site2.etempurl.com/api/leaveworks1";
                //httpClient.DefaultRequestHeaders.ExpectContinue = false;
                var result = await httpClient.PostAsync(uri, content);

                // on error throw a exception  
                result.EnsureSuccessStatusCode();

                // handling the answer  
                var resultString = await result.Content.ReadAsStringAsync();
                var post = JsonConvert.DeserializeObject<leavework>(resultString);

                await DisplayAlert("Success", "Leave Request is posted.", "OK");
                //await Navigation.PushAsync(new PostAnnouncement());
            }
        }

        //check days left...
        private void DateLeft(object sender, EventArgs e)
        {
            DateTime sdt = datepicker.Date;
            DateTime edt = dateendpicker.Date;
            int f = (edt - sdt).Days;
            EntryDay.Text = f.ToString();
        }


        

        private const string WebServiceUrl = "http://nudgeme-001-site2.etempurl.com/api/employees/getname/";

        //show employee leave balance.......
        public async void CheckBalanceAsync()
        {
            string ff = App.Current.Properties["Name"].ToString();

             employee bsObj = new employee();

            var httpClient = new HttpClient();

            var json = await httpClient.GetStringAsync(WebServiceUrl + ff);

            dynamic bsObject = JsonConvert.DeserializeObject<dynamic>(json);
            
            //string nan = bsObject["Nationality"];
            string emp = bsObject["Emp_LeaveBalance"];
           
            EntryBalance.Text = emp;
            
        }

    }
}