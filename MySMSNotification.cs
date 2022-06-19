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
    [BroadcastReceiver(Label = "My SMS Notification", Enabled = true)]
    [IntentFilter(new[] { "MySMSWebChat.SmsReceived" }, Priority = 1)]
    public class MySMSNotification : BroadcastReceiver
    {
        static int _notificationId = 0;

        public override void OnReceive(Context context, Intent intent)
        {
            Toast.MakeText(context, "MySMSNotification received intent!", ToastLength.Long).Show();

            var fromUser = intent.Extras.GetString("fromUser");
            var msg = intent.Extras.GetString("message");

            // Set up an intent so that tapping the notifications returns to this app:
            Intent webChat = new Intent(context, typeof(MySMSWebChat));

            // Pass some information to MySMSWebChat:
            webChat.PutExtra("fromUser", fromUser);
            webChat.PutExtra("message", msg);
            webChat.PutExtra("notificationId", _notificationId.ToString());

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent =
                PendingIntent.GetActivity(context, pendingIntentId, webChat, PendingIntentFlags.CancelCurrent);

            // Instantiate the builder and set notification elements:
            Notification.Builder builder = new Notification.Builder(context)
                .SetContentTitle(fromUser)
                .SetContentText(msg)
                .SetSmallIcon(Resource.Drawable.ic_notification_sms5)
                .SetDefaults(NotificationDefaults.Sound)
                .SetContentIntent(pendingIntent)
                .SetPriority((int)NotificationPriority.High)
                .SetVisibility(NotificationVisibility.Private)
                .SetCategory(Notification.CategoryMessage);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager =
                context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            notificationManager.Notify(_notificationId++, notification);
            _notificationId %= int.MaxValue;
        }
    }
}