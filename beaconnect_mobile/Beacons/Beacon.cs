using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.Advertisement;

namespace BeaConnect_mobile.Beacons
{
    class Beacon
    {
        // The type of advertisement
        private BluetoothLEAdvertisementType advertisementType;
        // The timestamp of the event
        private DateTimeOffset timestamp;
        //RSSI
        private Int16 rssi;
        // The local name of the advertising device contained within the payload, if any
        private string localName;
        private string uuidString;
        private int major;
        private int minor;
        private double distance;
        private DateTime receivedTime;
        
        
        public Beacon(BluetoothLEAdvertisementType advertisementType, DateTimeOffset timestamp, Int16 rssi, string localName, int major, int minor, DateTime receivedTime, string uuid)
        {
            this.advertisementType = advertisementType;
            this.timestamp = timestamp;
            this.rssi = rssi;
            this.localName = localName;
            this.major = major;
            this.minor = minor;
            this.receivedTime = receivedTime;
            this.uuidString = uuid;
        }
        

        public BluetoothLEAdvertisementType AdvertisementType
        {
            get { return this.advertisementType; }
            set { this.advertisementType = value; }
        }

        public DateTimeOffset Timestamp
        {
            get { return this.timestamp; }
            set { this.timestamp = value; }
        }

        public Int16 Rssi
        {
            get { return this.rssi; }
            set { this.rssi = value; }
        }
        public string LocalName
        {
            get { return this.localName; }
            set { this.localName = value; }
        }
        public int Major
        {
            get { return this.major; }
            set { this.major = value; }
        }

        public int Minor
        {
            get { return this.minor; }
            set { this.minor = value; }
        }

        public double Distance
        {
            get { return this.distance; }
            set { this.distance = value; }
        }
        public DateTime Receivedtime
        {
            get { return this.receivedTime; }
            set { this.receivedTime = value; }
        }
        public string UuidString
        {
            get { return this.uuidString; }
            set { this.uuidString = value; }
        }

    }
}
