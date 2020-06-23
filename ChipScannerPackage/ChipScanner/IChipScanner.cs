using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OO_Design_Pattern
{
    interface IChipScanner
    {
        void StartScan();
        void RestartScanner();
        void StopScan();
        //void NotifyChipScannerManager(string chipUID);
    }
}
