using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;
using OO_Design_Pattern.ChipScannerPackage.ChipScannerManager;

namespace OO_Design_Pattern
{
    static class ChipScannerManager // : ICSDeviceListener
    {
        private static List<AbstractChipScanner> chipScanners = new List<AbstractChipScanner>();
        private static readonly TimeSpan waitAfterScan = new TimeSpan(0, 0, 0, 0, 500);
        private static DateTime readyForScan;

        public static void RecieveUIDFromChipScanner(string chipUID)
        {
            if (readyForScan < DateTime.Now)
            {
                readyForScan = DateTime.Now + waitAfterScan;

                //færdiggør
            }
        }

        public static void test()
        {
            
        }
    }
}
