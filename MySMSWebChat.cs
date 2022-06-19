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
    [Activity(Label = "My SMS Web Chat", WindowSoftInputMode = SoftInput.AdjustResize)]
    public class MySMSWebChat : Activity
    {
        SMSReceived smsReceiver;
        public static MySMSWebChat Instance;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            MySMSWebChat.Instance = this;
            smsReceiver = new SMSReceived();

            SetContentView(Resource.Layout.MySMSWebChat);

            Button buttonSend = FindViewById<Button>(Resource.Id.buttonSend);
            buttonSend.Click += ButtonSend_Click;

            if (!string.IsNullOrEmpty(Intent.Extras?.GetString("message")))
            {
                // if we have extras data then this was launched from the notification

                Toast.MakeText(this, "MySMSWebChat received intent!", ToastLength.Long).Show();

                // display the message received
                var fromUser = Intent.Extras.GetString("fromUser");
                var msg = Intent.Extras.GetString("message");
                DisplayMessage(fromUser, msg);

                // close notification
                var notificationId = Intent.Extras.GetInt("notificationId");
                NotificationManager notificationManager =
                    GetSystemService(Context.NotificationService) as NotificationManager;
                notificationManager.Cancel(notificationId);
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            IntentFilter intentFilter = new IntentFilter("MySMSWebChat.SmsReceived")
            {
                Priority = 2
            };
            RegisterReceiver(smsReceiver, intentFilter);
        }

        private void ButtonSend_Click(object sender, EventArgs e)
        {
            EditText editTextMsg = FindViewById<EditText>(Resource.Id.editTextMsg);
            DisplayMessage("You", editTextMsg.Text);
            editTextMsg.Text = "";
        }

        private List<string> _messages = new List<string>();
        private void DisplayMessage(string fromUser, string msg)
        {
            //TextView textViewMsgs = FindViewById<TextView>(Resource.Id.textViewMsgs);
            //textViewMsgs.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();
            //textViewMsgs.Append($"{fromUser}: {msg}\n");

            _messages.Add($"{ fromUser}: { msg}");
            ListView listViewMsgs = FindViewById<ListView>(Resource.Id.listViewMsgs);
            //listViewMsgs.Adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, _messages.ToArray());
            listViewMsgs.Adapter = new MyListViewAdapter(this, _messages.ToArray());
        }

        public class SMSReceived : BroadcastReceiver
        {
            public override void OnReceive(Context context, Intent intent)
            {
                InvokeAbortBroadcast();

                Toast.MakeText(context, "SMS Received!", ToastLength.Long).Show();

                var fromUser = intent.Extras.GetString("fromUser");
                var msg = intent.Extras.GetString("message");
                MySMSWebChat.Instance.DisplayMessage(fromUser, msg);
            }
        }
    }
}