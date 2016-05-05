using Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeaConnect_mobile.Views;

namespace BeaConnect_mobile.ViewModels
{
    class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            // Build the menu
            Menu.Add(new MenuItem() {  Text = "Main Page", NavigationDestination = typeof(MainPage) });
            Menu.Add(new MenuItem() { Text = "Setting", NavigationDestination = typeof(SettingPage) });
            
        }
    }
}
