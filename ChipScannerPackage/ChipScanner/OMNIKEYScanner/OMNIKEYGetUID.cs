using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.SmartCards;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;

namespace OO_Design_Pattern
{
    class OMNIKEYGetUID
    {
        public static void GetCardUID(SmartCardReader sender, CardAddedEventArgs args)
        {
            byte[] byteCardUID = Get7ByteCardUID(args).Result;

            string cardUID = Convert7ByteCardUIDToDecimal(byteCardUID);

            ChipScannerManager.RecieveUIDFromChipScanner(cardUID);
        }

        private static async Task<byte[]> Get7ByteCardUID(CardAddedEventArgs args)
        {
            byte[] byteReadUID = new byte[] { (byte) 0xFF, (byte) 0xCA,
                (byte) 0x00, (byte) 0x00, (byte) 0x00 };

            SmartCardConnection cardConnection = await args.SmartCard.ConnectAsync();

            IBuffer bufferResultUID = await cardConnection.TransmitAsync(byteReadUID.AsBuffer());

            CryptographicBuffer.CopyToByteArray(bufferResultUID, out byte[] byteResultUID);

            return byteResultUID;
        }

        private static string Convert7ByteCardUIDToDecimal(byte[] byteCardID)
        {
            string hexCardUID = ByteArrayToHex(byteCardID);

            hexCardUID = hexCardUID.Substring(0, 8);

            string cardUID = HexToDecimal(hexCardUID);

            return cardUID;
        }

        private static string ByteArrayToHex(byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        private static string HexToDecimal(string hex)
        {
            string stringDecimal = long.Parse(hex, System.Globalization.NumberStyles.HexNumber).ToString();

            return stringDecimal;
        }
    }
}
