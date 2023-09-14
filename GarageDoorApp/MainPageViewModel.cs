using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageDoorModels;

namespace GarageDoorApp
{
    public class MainPageViewModel
    {
        private int _count;

        public ObservableCollection<GarageDoorStatus> GarageDoorStatuses { get; } = new ObservableCollection<GarageDoorStatus>();

        public MainPageViewModel()
        {
        }

        public void Action()
        {
            GarageDoorStatuses.Add(new GarageDoorStatus()
            {
                Id = _count.ToString(),
                IsOpen = _count % 2,
                TimestampSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            });

            _count++;
        }
    }
}
