using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaConnect_mobile.Beacons
{
    #region BeaconManager definition
    class BeaconManager
    {
        private static BeaconManager instance;

        private BeaconManager() { }

        public static BeaconManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BeaconManager();
                }
                return instance;
            }

        }

        public double getCalculateAccuracy(int txPower, double rssi)
        {
            if (rssi == 0)
            {
                return -1.0; // if we cannot determine accuracy, return -1.
            }

            double ratio = rssi * 1.0 / txPower;
            if (ratio < 1.0)
            {
                return Math.Pow(ratio, 10);
            }
            else {
                double accuracy = (0.89976) * Math.Pow(ratio, 7.7095) + 0.111;
                return accuracy;
            }
        }
    }
    #endregion
}
