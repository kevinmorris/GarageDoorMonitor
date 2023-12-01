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
using Microsoft.Extensions.Configuration;

namespace GarageDoorApp
{
    /// <summary>
    /// Responsible for registering this device with Firebase Cloud Messaging in order
    /// to receive alerts that the garage door is open
    /// </summary>
    public class NotificationRegistrar
    {
        public event EventHandler<NotificationRegisteredEventArgs> RegisteredEvent;

        public string NotificationHubPath { get; set; }
        public string NotificationHubConnectionStr { get; set; }

        public NotificationRegistrar(Context context, IConfiguration config)
        {
            NotificationHubConnectionStr = config.GetValue<string>("NotificationHub:ConnectionString");
            NotificationHubPath = config.GetValue<string>("NotificationHub:Path");

            var channel = new NotificationChannel(
                "GarageDoorChannel",
                "Firebase Notifications",
                NotificationImportance.Default);

            var notificationManager = (NotificationManager)context.GetSystemService(Android.Content.Context.NotificationService);
            notificationManager.CreateNotificationChannel(channel);
        }

        /// <summary>
        /// Register this device with Azure Notification Hub
        /// </summary>
        public void Register()
        {
            var sender = this;

            var tokenTask = FirebaseMessaging.Instance.GetToken();
            tokenTask.AddOnCompleteListener(new OnFirebaseTokenCompleteListener(async token =>
            {
                var notificationHub = new NotificationHubClient(
                    NotificationHubConnectionStr,
                    NotificationHubPath);

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
