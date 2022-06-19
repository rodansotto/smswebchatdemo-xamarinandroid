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

namespace MySMSWebChat
{
    class MyListViewAdapter : BaseAdapter<string>
    {
        string[] items;
        Context context;

        public MyListViewAdapter(Context context, string[] items)
        {
            this.context = context;
            this.items = items;
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position] => items[position];

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            string fromUser = items[position].Split(':')[0];
            string msg = items[position].Substring(fromUser.Length + 2);
            bool isSentLayout = fromUser.Equals("You", StringComparison.OrdinalIgnoreCase);

            View view = convertView; // re-use an existing view, if one is available

            if (view == null || (bool)view.Tag != isSentLayout) // otherwise create a new one
            {
                int layoutID;
                int iconID;
                if (isSentLayout)
                {
                    layoutID = Resource.Layout.SentMessageItem;
                    iconID = Resource.Drawable.ic_support7;
                }
                else
                {
                    layoutID = Resource.Layout.ReceivedMessageItem;
                    iconID = Resource.Drawable.ic_user7;
                }
                view = LayoutInflater.From(context).Inflate(layoutID, null);
                view.FindViewById<ImageView>(Resource.Id.imageView1).SetImageResource(iconID);
                view.Tag = isSentLayout;
            }

            view.FindViewById<TextView>(Resource.Id.textView1).Text = msg;
            view.FindViewById<TextView>(Resource.Id.textView2).Text = fromUser;

            return view;
        }

        public override int Count
        {
            get
            {
                return items.Length;
            }
        }
    }
}