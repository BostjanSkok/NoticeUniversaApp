using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;

namespace Noticer.ViewModels
{
    public  class MockMainVm:ViewModelBase,IMainViewModel
    {
        
        public  List<DeviceEntry> LinkedDevices
        {
            get
            {
                return new List<DeviceEntry>
                {
                    new DeviceEntry {DeviceName = "Test device 1", AddedOn = new DateTime(2014, 12, 1)},
                    new DeviceEntry {DeviceName = "Test device 3", AddedOn = new DateTime(2015, 1, 1)}
                };
            }
        }

        public  List<SaveNotification> NotificationHistoryList
        {
            get
            {
                return new List<SaveNotification>
                {
                    new SaveNotification
                    {
                        ReceivedOn = new DateTime(2015, 1, 1, 5, 1, 2),
                        Title = "Test",
                        Text = "Notification test"
                    },
                    new SaveNotification
                    {
                        ReceivedOn = new DateTime(2015, 1, 5, 5, 1, 2),
                        Title = "Test 2",
                        Text = "Notification test"
                    }
                };
            }
        }
    }

    public interface IMainViewModel
    {
    }

    public class SaveNotification
    {
        public DateTime ReceivedOn { get; set; }
        public String Title { get; set; }
        public String Text { get; set; }
    }

    public class DeviceEntry
    {
        public string DeviceName { get; set; }
        public DateTime AddedOn { get; set; }
        public string Key { get; set; }
    }
}