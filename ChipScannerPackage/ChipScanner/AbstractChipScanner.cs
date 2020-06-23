using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;
using OO_Design_Pattern.ChipScannerPackage.ChipScannerManager;

namespace OO_Design_Pattern
{
    abstract class AbstractChipScanner : IChipScanner
    {
        protected TimeSpan timePerTimeout;
        protected DateTime nextTimeout;
        protected SmartCardReader chipScanner;
        protected ICSDeviceListener csManager;

        public abstract void StartScan();
        public abstract void RestartScanner();
        public abstract void StopScan();

        /*public void NotifyChipScannerManager(string chipUID)
        {
            csManager.CSDeviceListener(chipUID);
        }*/
    }
}
