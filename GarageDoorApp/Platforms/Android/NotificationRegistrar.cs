using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Gms.Tasks;
using Firebase.Messaging;
using Microsoft.Azure.NotificationHubs;

namespace GarageDoorApp
{
    public class NotificationRegistrar
    {
        public event EventHandler<NotificationRegisteredEventArgs> RegisteredEvent;

        public NotificationRegistrar(Context context)
        {
            var channel = new NotificationChannel(
                "GarageDoorChannel",
                "Firebase Notifications",
                NotificationImportance.Default);

            var notificationManager = (NotificationManager)context.GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        public void Register()
        {
            var sender = this;

            var tokenTask = FirebaseMessaging.Instance.GetToken();
            tokenTask.AddOnCompleteListener(new OnFirebaseTokenCompleteListener(async token =>
            {
                var notificationHub = new NotificationHubClient(
                    "",
                    "");

                var fcmNativeRegistration = await notificationHub.CreateFcmNativeRegistrationAsync(token);
                var registrationId = fcmNativeRegistration.FcmRegistrationId;
                RegisteredEvent?.Invoke(sender, new NotificationRegisteredEventArgs(registrationId));
            }));
        }
    }

    internal class OnFirebaseTokenCompleteListener : Java.Lang.Object, IOnCompleteListener
    {
        public Action<string> CompleteAction { get; private set; }

        public OnFirebaseTokenCompleteListener(Action<string> completeAction)
        {
            CompleteAction = completeAction;
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            CompleteAction.Invoke(task.Result.ToString());
        }
    }
}
