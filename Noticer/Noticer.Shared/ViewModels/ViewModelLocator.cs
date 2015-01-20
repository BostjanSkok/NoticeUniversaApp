using System;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using Noticer.Services;

namespace Noticer.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<MainPageVM>();
           SimpleIoc.Default.Register<INoticeListener,NoticeListener>();
        }

        public IMainViewModel MainPageVM
        {
            get
            {
                if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                    return new MockMainVm();
               return ServiceLocator.Current.GetInstance<MainPageVM>(); }
        }
    }
}