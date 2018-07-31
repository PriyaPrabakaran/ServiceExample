using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using ServiceExample.Activities;
using System;
using Android.Content;
using System.Collections.Generic;
using ServiceExample.Resources;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Collections;
using System.Linq;
using ServiceExample.Model;

namespace ServiceExample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button btnAdd, btnUpdate, btnDelete;
        private TextView txt_Subject, txt_Email, txt_Message;
        private ListView lstData;
        private List<Listfeedback> lstSource = new List<Listfeedback>();
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            lstData = FindViewById<ListView>(Resource.Id.listView);

            btnAdd = FindViewById<Button>(Resource.Id.btnAdd);
            btnUpdate = FindViewById<Button>(Resource.Id.btnUpdate);
            btnDelete = FindViewById<Button>(Resource.Id.btnDelete);

            txt_Subject = FindViewById<TextView>(Resource.Id.txt_Subject);
            txt_Email = FindViewById<TextView>(Resource.Id.txt_Email);
            txt_Message = FindViewById<TextView>(Resource.Id.txt_Message);

            btnAdd.Click += delegate { GoToActivity(typeof(AddFeedback));};
            btnUpdate.Click += delegate {GoToActivity(typeof(UpdateFeedback)); };
            btnDelete.Click += delegate { GoToActivity(typeof(DeleteFeedback));};

            LoadListViewAsync();
        }
        public void GoToActivity(Type myActivity)
        {
            StartActivity(myActivity);
        }
        private async System.Threading.Tasks.Task LoadListViewAsync()
        {
            var progress = new ProgressDialog(this)
            {
                Indeterminate = true
            };
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading data, Please wait...");
            progress.SetCancelable(false);
            progress.Show();
            List<Listfeedback> feedback = new List<Listfeedback>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
            string url = "http://devdemand.firestreamonline.com/transpac/getList";
            var result = await client.GetAsync(url);
            var json = await result.Content.ReadAsStringAsync();
            try
            {
                lstSource = JsonConvert.DeserializeObject<List<Listfeedback>>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            if (lstSource == null)
            {
                Toast.MakeText(this, json, ToastLength.Short).Show();
            }
            else
            {
                progress.Hide();
                var adapter = new ListViewAdapter(this, lstSource);
                lstData.Adapter = adapter;
            }
        }
    }
}

