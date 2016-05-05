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
    class ViewModelBase : BindableBase
    {
        private static ObservableCollection<MenuItem> menu = new ObservableCollection<MenuItem>();

        public ViewModelBase()
        {}

        public ObservableCollection<MenuItem> Menu {
            get { return menu; }
        }
    }
}
