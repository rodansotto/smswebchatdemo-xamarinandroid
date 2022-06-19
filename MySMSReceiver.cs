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
using static Android.Provider.Telephony;

namespace MySMSWebChat
{
    [BroadcastReceiver(Label = "My SMS Receiver", Enabled = true, Exported = true)]
    [IntentFilter(new[] { Sms.Intents.SmsReceivedAction })]
    public class MySMSReceiver : BroadcastReceiver
    {
        public override void OnReceive(Context context, Intent intent)
        {
            var msgs = Sms.Intents.GetMessagesFromIntent(intent);
            var msgBody = msgs[0].MessageBody;
            var originatingAddress = msgs[0].OriginatingAddress;

            if (msgBody.StartsWith("rs.c:wc"))
            {
                Toast.MakeText(context, "MySMSReceiver received intent!", ToastLength.Long).Show();

                var fromUser = msgBody.Split(':')[2];
                var msg = msgBody.Substring("rs.c:wc:".Length + fromUser.Length + 1); // + 1 to exclude ':'

                Intent smsReceived = new Intent("MySMSWebChat.SmsReceived");
                smsReceived.PutExtra("fromUser", fromUser);
                smsReceived.PutExtra("message", msg);

                context.SendOrderedBroadcast(smsReceived, null);
            }
        }
    }
}