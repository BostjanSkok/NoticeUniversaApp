using System;
using Windows.Networking.Sockets;
using Windows.UI.Notifications;
using Newtonsoft.Json;
using Noticer.ViewModels;

namespace Noticer.Services
{
    public class NoticeListener : INoticeListener
    {
        private DatagramSocket _listener;

        public Action<String> DeviceLinkedMsg { get; set; }
        public Action<SaveNotification> NewNotification { get; set; }

    

        public async void StartListening()
        {
            _listener = new DatagramSocket();
            _listener.MessageReceived += SendToast;


            // Start listen operation.
            try
            {
                 _listener.BindServiceNameAsync("55555");
                SendToast(null, null);
                //rootPage.NotifyUser("Listening", NotifyType.StatusMessage);
            }
            catch (Exception exception)
            {
                // If this is an unknown status it means that the error is fatal and retry will likely fail.
                if (SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
                {
                    throw;
                }
                _listener.Dispose();
                //       rootPage.NotifyUser("Start listening failed with error: " + exception.Message, NotifyType.ErrorMessage);
            }
        }

        public async void StopService()
        {
            _listener.Dispose();
        }

        private  void SendToast(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {
            var str = args == null ? "Empty Udp" : GetString(args);
            if (str.StartsWith("*"))
            {
                str = str.Trim();
                DeviceLinkedMsg(str.Substring(1, str.Length - 1));
                return;
            }

            SaveNotification result = GetNotificationFormJson(str);
            if (result == null)
                return;

             NewNotification(result);


            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastImageAndText04);
            var toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(str));

            var toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        private SaveNotification GetNotificationFormJson(string str)
        {
            try
            {
                var a = JsonConvert.DeserializeObject<SaveNotification>(str);
                return a;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GetString(DatagramSocketMessageReceivedEventArgs args)
        {
            try
            {
                var stringLength = args.GetDataReader().UnconsumedBufferLength;

                return args.GetDataReader().ReadString(stringLength);
            }
            catch (Exception exception)
            {
                var socketError = SocketError.GetStatus(exception.HResult);
                if (socketError == SocketErrorStatus.ConnectionResetByPeer)
                {
                }
                else if (socketError != SocketErrorStatus.Unknown)
                {
                }
                else
                {
                    throw;
                }
                return "error";
            }
        }
    }

    public interface INoticeListener
    {
        void StartListening();
        void StopService();
        Action<String> DeviceLinkedMsg { get; set; }
    }
}