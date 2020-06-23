using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Usb;

namespace OO_Design_Pattern
{
    class OMNIKEYCardreader : AbstractChipScanner
    {

        public override void StartScan()
        {
            chipScanner.CardAdded += OMNIKEYGetUID.GetCardUID;
        }

        public override void RestartScanner()
        {
            chipScanner.CardAdded -= OMNIKEYGetUID.GetCardUID;
            this.nextTimeout = DateTime.Now + this.timePerTimeout;
            chipScanner.CardAdded += OMNIKEYGetUID.GetCardUID;
        }

        public override void StopScan()
        {
            chipScanner.CardAdded -= OMNIKEYGetUID.GetCardUID;
        }
    }
}
