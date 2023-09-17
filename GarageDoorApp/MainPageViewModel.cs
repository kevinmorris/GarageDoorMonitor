using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageDoorApp.Api;
using GarageDoorApp.Model;

namespace GarageDoorApp
{
    public class MainPageViewModel
    {
        private int _count;
        private readonly IGarageDoorApi _api;

        public ObservableCollection<GarageDoorStatus> GarageDoorStatuses { get; } = new ObservableCollection<GarageDoorStatus>();

        public MainPageViewModel()
        {
            _api = App.Current.Services.GetService<IGarageDoorApi>();
        }

        public async void Action()
        {
            await _api.GetAsync("1");
        }
    }
}
