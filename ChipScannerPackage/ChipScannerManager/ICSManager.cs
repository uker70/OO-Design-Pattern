using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OO_Design_Pattern.ChipScannerPackage.ChipScannerManager
{
    interface ICSManager
    {
        void AttachObserver();
        void DetachObserver();
        void NotifyObservers(CSManagerStatus status);
        void NotifyObservers(string chipUID);

        //ikke færdig, giv metoder deres variabler og implementer dem i ChipScannerManager
    }
}
