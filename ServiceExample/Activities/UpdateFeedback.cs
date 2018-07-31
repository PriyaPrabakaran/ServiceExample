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
    [Activity(Label = "UpdateFeedback")]
    public class UpdateFeedback : Activity
    {
        private TextView hiddenID;
        private EditText edt_updateId, edt_updateEmail, edt_updateSubject, edt_updateMessage;
        private Button btngetID, btn_update;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.UpdateFeedback);
            hiddenID = FindViewById<TextView>(Resource.Id.hiddenID);
            edt_updateId = FindViewById<EditText>(Resource.Id.edt_updateId);
            edt_updateEmail = FindViewById<EditText>(Resource.Id.edt_updateEmail);
            edt_updateSubject = FindViewById<EditText>(Resource.Id.edt_updateSubject);
            edt_updateMessage = FindViewById<EditText>(Resource.Id.edt_updateMessage);
            btngetID = FindViewById<Button>(Resource.Id.btngetID);
            btn_update = FindViewById<Button>(Resource.Id.btn_update);

            var progress = new ProgressDialog(this)
            {
                Indeterminate = true
            };
            progress.SetProgressStyle(ProgressDialogStyle.Spinner);
            progress.SetMessage("Loading data, Please wait...");
            progress.SetCancelable(false);

            btngetID.Click += async delegate
            {
                progress.Show();
                Feedback feedback = new Feedback();
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                string url = "http://devdemand.firestreamonline.com/transpac/get?id=" + edt_updateId.Text.ToString();
                var result = await client.GetAsync(url);
                var json = await result.Content.ReadAsStringAsync();
                try
                {
                    feedback = Newtonsoft.Json.JsonConvert.DeserializeObject<Feedback>(json);
                }
                catch (Exception ex) { }

                if (feedback == null)
                {
                    Toast.MakeText(this,json, ToastLength.Short).Show();
                }
                else
                {
                    progress.Hide();
                    //hiddenID.SetTag(edt_updateId.Text);
                    edt_updateEmail.Text = feedback.Email;
                    edt_updateSubject.Text = feedback.Subject;
                    edt_updateMessage.Text = feedback.Message;
                }
            };
            btn_update.Click += async delegate
            {
                progress.SetMessage("Updating feedback, Please wait...");
                progress.Show();
                Feedback feedback = new Feedback();
                feedback.Id = int.Parse(edt_updateId.Text);
                feedback.Email= edt_updateEmail.Text;
                feedback.Subject = edt_updateSubject.Text;
                feedback.Message = edt_updateMessage.Text;

                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/60.0.3112.113 Safari/537.36");
                string url = $"http://devdemand.firestreamonline.com/transpac/update?id={feedback.Id}&email={feedback.Email}&subject={feedback.Subject}&message={feedback.Message}";
                var uri=new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(feedback);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(uri, content);
                var contents = await response.Content.ReadAsStringAsync();
                Clear();

                if (response.IsSuccessStatusCode)
                {
                    progress.Hide();
                    Toast.MakeText(this, "Feedback updated successfully!", ToastLength.Long).Show();
                }
                else
                {
                    progress.Hide();
                    Toast.MakeText(this, "ERROR!!! Feedback not updated!", ToastLength.Long).Show();
                }
            };
            void Clear()
            {
                edt_updateEmail.Text = "";
                edt_updateSubject.Text = "";
                edt_updateMessage.Text = "";
            }
        }
    }
}