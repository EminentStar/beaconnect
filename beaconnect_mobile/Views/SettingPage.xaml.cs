using Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//Shared Preferences in Windows 8 : http://stackoverflow.com/questions/12759641/windows-8-androids-sharedpreferences-in-windows-8

namespace BeaConnect_mobile.Views
{
    public sealed partial class SettingPage : Page
    {
        private GlobalVariables globalVariables = GlobalVariables.Instance;
        public SettingPage()
        {
            this.InitializeComponent();
            this.SetInfo();
        }

        private void SetInfo()
        {
            if(globalVariables.FiremanName != null)
            {
                firemanName.Text = globalVariables.FiremanName;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           
        }

        private void registerFiremanBtn_Click(object sender, RoutedEventArgs e)
        {
            //모든 정보가 채워졌을때,
            if (firemanName.Text.ToString() != null)
            {
                SaveInfo("firemanName", firemanName.Text);
                globalVariables.FiremanName = firemanName.Text;   
            }
        }

        public void SaveInfo(string key, string value)
        {
            //Windows.Storage에 소방대원의 정보를 수정 혹은 삽입한다.
            if (Windows.Storage.ApplicationData.Current.LocalSettings.Values.ContainsKey(key))
            {
                if(Windows.Storage.ApplicationData.Current.LocalSettings.Values[key].ToString() != null)
                {
                    //do update
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] = value;
                }
            }
            else
            {
                // do create key and save value, first time only.
                Windows.Storage.ApplicationData.Current.LocalSettings.CreateContainer(key, Windows.Storage.ApplicationDataCreateDisposition.Always);
                if(Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] == null)
                {
                    Windows.Storage.ApplicationData.Current.LocalSettings.Values[key] = value;
                }
            }
        }
    }
}
