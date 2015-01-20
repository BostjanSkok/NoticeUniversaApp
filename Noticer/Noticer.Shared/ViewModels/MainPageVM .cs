using System;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Noticer.Services;
using PropertyChanged;

namespace Noticer.ViewModels
{
    [ImplementPropertyChanged]
    public class MainPageVM : ViewModelBase, IMainViewModel
    {
        private readonly INoticeListener _noiticeListener;

        public MainPageVM(INoticeListener noiticeListener)
        {
            LinkedDevices = new ObservableCollection<DeviceEntry>();
            NotificationHistoryList = new ObservableCollection<SaveNotification>();
            _noiticeListener = noiticeListener;
            _noiticeListener.DeviceLinkedMsg = NewDevicePacketReceived;
            _noiticeListener.NewNotification = NewNotification;

            noiticeListener.StartListening();
            NewDeviceCmd = new RelayCommand(AddNewDevice);
            ClosePopUpCmd = new RelayCommand(()=>QrPopUp=false);
        }

        public RelayCommand NewDeviceCmd { get; private set; }
        public RelayCommand ClosePopUpCmd { get; private set; }


        public int OffsetX
        {
            get
            {
                var t = (int) (Window.Current.Bounds.Width/2);
                return t - 100;
            }
        }

        public int OffsetY
        {
            get
            {
                var t = (int) (Window.Current.Bounds.Height/2);
                return t - 100;
            }
        }

        public BitmapImage QrImage { get; set; }
        public ObservableCollection<DeviceEntry> LinkedDevices { get; set; }
        public ObservableCollection<SaveNotification> NotificationHistoryList { get; set; }
        public DeviceEntry SelectedDevice { get; set; }
        public bool QrPopUp { get; set; }

        public Visibility ShowWhenDeviceSelected
        {
            get { return SelectedDevice == null ? Visibility.Collapsed : Visibility.Visible; }
        }

        public string Text
        {
            get { return "Text from view model"; }
        }

        private async void NewDevicePacketReceived(string obj)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            await
                dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        LinkedDevices.Add(new DeviceEntry {AddedOn = DateTime.Now, DeviceName = obj , Key = "test"});
                    });
        }

        private async void NewNotification(SaveNotification obj)
        {
            var dispatcher = CoreApplication.MainView.CoreWindow.Dispatcher;

            await
                dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    () =>
                    {
                        NotificationHistoryList.Add(obj);
                    });
        }

        private async void AddNewDevice()
        {
            var gen = new QrGenerator();
            var img = gen.GenerateQr(200, 200, "Test qr code");
            QrImage = new BitmapImage(await gen.SaveImage(img));
            QrPopUp = true;
        }
    }
}