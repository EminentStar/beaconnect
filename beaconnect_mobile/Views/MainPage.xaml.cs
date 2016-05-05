using BeaConnect_mobile.Beacons;
using Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Bluetooth.Advertisement;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//
using Windows.UI.Popups;
using System.Threading.Tasks;
using BeaConnect_mobile.DataModel;
using Microsoft.WindowsAzure.MobileServices;
using Windows.UI.Xaml.Media.Imaging;

namespace BeaConnect_mobile.Views
{
    public sealed partial class MainPage : Page
    {
        //Azure
        private MobileServiceCollection<TodoItem, TodoItem> items;
        private IMobileServiceTable<TodoItem> todoTable = App.MobileService.GetTable<TodoItem>();

        //regaring beacon
        DispatcherTimer timer = new DispatcherTimer();

        private static SolidColorBrush OnColor = new SolidColorBrush(Color.FromArgb(0xFF, 0xFF, 0xde, 0x00));
        private static SolidColorBrush OffColor = new SolidColorBrush(Color.FromArgb(0xFF, 0x9d, 0x9d, 0x9c));

        private GlobalVariables globalVariables = GlobalVariables.Instance;
        // Bluetooth Beacons
        private readonly BluetoothLEAdvertisementWatcher _watcher;

        private readonly BeaconManager _beaconManager = BeaconManager.Instance;

        private readonly int BEACON_MAJOR = 4660;

        // UI
        //Windows 런타임은 핵심 이벤트 메시지 디스패처를 제공합니다. 이 형식의 인스턴스는 창 메시지를 처리하고 이벤트를 클라이언트에 전달하는 작업을 담당합니다.
        private CoreDispatcher _dispatcher;

        List<Beacon> bList;

        public MainPage()
        {
            this.InitializeComponent();

            DataContext = this;
            _dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;

            // Create the Bluetooth LE watcher from the Windows 10 UWP
            _watcher = new BluetoothLEAdvertisementWatcher { ScanningMode = BluetoothLEScanningMode.Active };

            bList = new List<Beacon>();
            
            // Set the out-of-range timeout to be 2 seconds. Used in conjunction with OutOfRangeThresholdInDBm
            // to determine when an advertisement is no longer considered "in-range"
            _watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(2000);

            // End of watcher configuration. There is no need to comment out any code beyond this point.

            //Start_timer();
        }

        private async Task UpdateUserInfo(String name, String victim, String distance, String date)
        {

            MobileServiceInvalidOperationException exception = null;
            try
            {
                items = await todoTable
                     .Where(todoItem => todoItem.Name == name)
                     .Where(todoItem => todoItem.Victim == victim)
                     .ToCollectionAsync();


                if (items.Count == 0)
                {
                    var todoItem = new TodoItem { Name = name, Victim = victim, Distance = distance, Date = date };
                    await InsertTodoItem(todoItem);
                }
                else
                {
                    var userItem = items.ElementAt<TodoItem>(0);
                    userItem.Distance = distance;
                    userItem.Date = date;

                    await todoTable.UpdateAsync(userItem);
                }
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }


            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                //ListItems.ItemsSource = items;
            }
        }

        async void OnMessageDlg(String message)
        {
            var msgboxDlg = new MessageDialog(message, message);
            msgboxDlg.Commands.Add(new Windows.UI.Popups.UICommand("Ok") { Id = 0 });
            msgboxDlg.DefaultCommandIndex = 0;
            await msgboxDlg.ShowAsync();
        }

        private async Task InsertTodoItem(TodoItem todoItem)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile Services has assigned an Id, the item is added to the CollectionView
            await todoTable.InsertAsync(todoItem);
            items.Add(todoItem);

            //await SyncAsync(); // offline sync
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //대원 이름이 없을때,
            if (globalVariables.FiremanName != null)
            {
                fmanName.Text = globalVariables.FiremanName;
            }

            //비컨 신호 관련 핸들러 추가
            _watcher.Received += WatcherOnReceived;
            _watcher.Stopped += WatcherOnStopped;

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            //비컨 신호 관련 핸들러 제거
            _watcher.Received -= WatcherOnReceived;
            _watcher.Stopped -= WatcherOnStopped;
        }

        private async void WatcherOnReceived(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            int major = 0;
            int minor = 0;
            int txPower = 0;
            string uuid = null;
            double distance = 0;
            DateTime receivedTime = DateTime.Now;
            string distanceStr = null;


            // The timestamp of the event
            DateTimeOffset timestamp = eventArgs.Timestamp;

            // The type of advertisement
            BluetoothLEAdvertisementType advertisementType = eventArgs.AdvertisementType;

            // The received signal strength indicator (RSSI)
            Int16 rssi = eventArgs.RawSignalStrengthInDBm;

            // The local name of the advertising device contained within the payload, if any
            string localName = eventArgs.Advertisement.LocalName;

            var manufacturerSections = eventArgs.Advertisement.ManufacturerData;
            if (manufacturerSections.Count > 0)
            {
                // Only print the first one of the list
                var manufacturerData = manufacturerSections[0];
                var data = new byte[manufacturerData.Data.Length];
                using (var reader = DataReader.FromBuffer(manufacturerData.Data))
                {
                    reader.ReadBytes(data);
                }

                byte b = data[manufacturerData.Data.Length - 1];
                txPower = (int)(sbyte)b;


                Byte[] majorByteArray = { data[manufacturerData.Data.Length - 4], data[manufacturerData.Data.Length - 5], 0x00, 0x00 };
                Byte[] minorByteArray = { data[manufacturerData.Data.Length - 2], data[manufacturerData.Data.Length - 3], 0x00, 0x00 };
                major = BitConverter.ToInt32(majorByteArray, 0);
                minor = BitConverter.ToInt32(minorByteArray, 0);

                try
                {
                    uuid = BitConverter.ToString(data, 2, 16);
                }
                catch (Exception ex)
                {
                    //this.log.Trace("An error occured.: {0}", ex.StackTrace);
                }
            }
            else
            {
                return;
            }
            if (major != BEACON_MAJOR) // 등록되지 않은 비컨일 경우 
            {
                return;
            }
           
            distance = _beaconManager.getCalculateAccuracy(txPower, rssi);
            
            distanceStr = (distance <= 10) ? "Near" : "Far";

            await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                /*
                 *  서버로 해당 비컨의 정보를 보내는 메서드를 호출해야함. 
                 * 소방관 이름/비콘이름/소방관과의 거리/파악된 시간
                 */

                //위의 sendingInfo를 서버로 보내야함.
                //await UpdateUserInfo(globalVariables.FiremanName, receivedBeacon.UuidString, receivedBeacon.Distance.ToString(), receivedBeacon.Receivedtime.ToString());
                await UpdateUserInfo(globalVariables.FiremanName, uuid.Substring(42), distanceStr, receivedTime.ToString());
            });
        }

        private async void WatcherOnStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs eventArgs)
        {
        }

        private void rescue_mode_Checked(object sender, RoutedEventArgs e)
        {
            _watcher.Start();
            timer.Start();
            this.tb_onoff.Text = "ON";
            this.tb_onoff.Foreground = OnColor;
        }

        private void rescue_mode_Unchecked(object sender, RoutedEventArgs e)
        {
            _watcher.Stop();
            timer.Stop();
            this.tb_onoff.Text = "OFF";
            this.tb_onoff.Foreground = OffColor;
        }
    }
}
