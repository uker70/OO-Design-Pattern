using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;
using OO_Design_Pattern.ChipScannerPackage.ChipScannerManager;

namespace OO_Design_Pattern
{
    class OMNIKEY5021CL : OMNIKEYCardreader
    {
        public OMNIKEY5021CL(SmartCardReader chipScanner, ICSDeviceListener csManager)
        {
            this.chipScanner = chipScanner;
            this.csManager = csManager;
            this.timePerTimeout = TimeSpan.FromHours(2); //change this if you want another value, to be the standard
            this.nextTimeout = DateTime.Now + this.timePerTimeout;
        }

        public OMNIKEY5021CL(SmartCardReader chipScanner, ICSDeviceListener csManager, TimeSpan timeoutTimer)
        {
            this.chipScanner = chipScanner;
            this.csManager = csManager;
            this.timePerTimeout = timeoutTimer;
            this.nextTimeout = DateTime.Now + this.timePerTimeout;
        }
    }
}
