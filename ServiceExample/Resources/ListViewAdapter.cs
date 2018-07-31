using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.Lang;
using ServiceExample.Model;

namespace ServiceExample.Resources
{
    public class ViewHolder : Java.Lang.Object
    {
        public TextView txt_Subject { get; set; }
        public TextView txt_Email { get; set; }
        public TextView txt_Message { get; set; }
    }
    public class ListViewAdapter:BaseAdapter
    {
        private Activity activity;
        private List<Listfeedback> lstFeedback;

        public ListViewAdapter(Activity activity, List<Listfeedback> lstFeedback)
        {
            this.activity = activity;
            this.lstFeedback = lstFeedback;
        }

        public override int Count => lstFeedback.Count;

        public override Java.Lang.Object GetItem(int position)
        {
            //throw new NotImplementedException();
            return null;
        }

        public override long GetItemId(int position)
        {
            //throw new NotImplementedException();
            return lstFeedback[position].Id;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            //throw new NotImplementedException();
            var view = convertView ?? activity.LayoutInflater.Inflate(Resource.Layout.ListViewTemplate, parent, false);

            var txtSubject = view.FindViewById<TextView>(Resource.Id.txt_Subject);
            var txtEmail = view.FindViewById<TextView>(Resource.Id.txt_Email);
            var txtMessage = view.FindViewById<TextView>(Resource.Id.txt_Message);

            txtSubject.Text = lstFeedback[position].Subject;
            txtEmail.Text = lstFeedback[position].Email;
            txtMessage.Text = lstFeedback[position].Message;

            return view;
        }
    }
}