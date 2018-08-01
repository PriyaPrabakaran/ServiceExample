using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;

namespace ServiceExample.Activities
{
    [Activity(Label = "DeleteFeedback")]
    public class DeleteFeedback : Activity
    {
        private EditText edtId;
        private Button btnDel;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.DeleteFeedback);
            edtId = FindViewById<EditText>(Resource.Id.edtId);
            btnDel = FindViewById<Button>(Resource.Id.btnDel);

            var progress = new ProgressDialog(this)
            {
                Indeterminate = true
            };
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Deleting Feedback, Please wait...");
            progress.SetCancelable(false);

            btnDel.Click += async delegate
             {
                 Feedback feedback = new Feedback
                 {
                     Id = int.Parse(edtId.Text)
                 };
                 progress.Show();
                 HttpClient client = new HttpClient();
                 string url = "http://devdemand.firestreamonline.com/transpac/delete?id=" + feedback.Id.ToString();
                 client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                 var uri = new Uri(url);
                 client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 var json = JsonConvert.SerializeObject(feedback);
                 var content = new StringContent(json, Encoding.UTF8, "application/json");
                 var response = await client.PostAsync(uri, content);
                 var contents = await response.Content.ReadAsStringAsync();
                 progress.Hide();
                 if (response.IsSuccessStatusCode)
                 {
                     Toast.MakeText(this, "Feedback delete successfully!", ToastLength.Long).Show();
                 }
                 else
                 {
                     Toast.MakeText(this, "ERROR!!! Feedback not deleted!", ToastLength.Long).Show();
                 }
             };
        }
    }
}