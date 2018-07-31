using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    [Activity(Label = "AddFeedback")]
    public class AddFeedback : Activity
    {
        private EditText edtEmail, edtSubject, edtMessage;
        private Button btnSend;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.AddFeedback);
            edtEmail = FindViewById<EditText>(Resource.Id.edtEmail);
            edtSubject = FindViewById<EditText>(Resource.Id.edtSubject);
            edtMessage = FindViewById<EditText>(Resource.Id.edtMessage);
            btnSend = FindViewById<Button>(Resource.Id.btnSend);

            var progress = new ProgressDialog(this)
            {
                Indeterminate = true
            };
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Adding Feedback, Please wait...");
            progress.SetCancelable(false);

            btnSend.Click += async delegate
            {
                Feedback feedback = new Feedback
                {
                    Email = edtEmail.Text,
                    Subject = edtSubject.Text,
                    Message = edtMessage.Text
                };
                progress.Show();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                string url = $"http://devdemand.firestreamonline.com/transpac/insert?email={feedback.Email}&subject={feedback.Subject}&message={feedback.Message}";
                var uri =new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(feedback);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                //var contents = await response.Content.ReadAsStringAsync();
                Clear();
                progress.Hide();
                if (response.IsSuccessStatusCode)
                {
                    Toast.MakeText(this, "Feedback saved successfully!", ToastLength.Long).Show();
                }
                else
                {
                    Toast.MakeText(this, "ERROR!!! Feedback not saved!", ToastLength.Long).Show();
                }
            };
        }
        void Clear()
        {
            edtEmail.Text="";
            edtSubject.Text = "";
            edtMessage.Text = "";
        }
    }
}