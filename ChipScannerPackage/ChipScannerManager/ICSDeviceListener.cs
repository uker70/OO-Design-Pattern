using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OO_Design_Pattern.ChipScannerPackage.ChipScannerManager
{
    interface ICSDeviceListener
    {
        void CSDeviceListener(string chipUID);
    }
}
