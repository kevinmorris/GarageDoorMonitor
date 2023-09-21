using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GarageDoorApp.Api;
using GarageDoorApp.Model;

namespace GarageDoorApp
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IGarageDoorApi _api;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsBusy { get; private set; } = true;

        public bool RefreshEnabled => !IsBusy;

        public Command FetchStatusesCommand => new(FetchStatuses);

        private IEnumerable<GarageDoorStatus> _garageDoorStatuses;
        public IEnumerable<GarageDoorStatus> GarageDoorStatuses
        {
            get => _garageDoorStatuses;
            private set => SetField(ref _garageDoorStatuses, value);
        }

        public MainPageViewModel()
        {
            _api = App.Current.Services.GetService<IGarageDoorApi>();
        }

        public async void FetchStatuses()
        {
            IsBusy = true;
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(RefreshEnabled));

            var garageDoorStatuses = new List<GarageDoorStatus>(await _api.GetAsync());
            GarageDoorStatuses = garageDoorStatuses;

            IsBusy = false;
            OnPropertyChanged(nameof(IsBusy));
            OnPropertyChanged(nameof(RefreshEnabled));
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
