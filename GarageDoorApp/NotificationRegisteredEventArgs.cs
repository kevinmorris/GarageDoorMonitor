using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorApp
{
    public class NotificationRegisteredEventArgs : EventArgs
    {
        public string RegistrationId { get; set; }
        public NotificationRegisteredEventArgs(string registrationId)
        {
            RegistrationId = registrationId;
        }
    }
}
