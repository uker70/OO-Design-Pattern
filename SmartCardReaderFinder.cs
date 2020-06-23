using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.Enumeration;
using Windows.Devices.SmartCards;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using WindowsInput;
using WindowsInput.Native;

namespace OO_Design_Pattern
{
    class SmartCardReaderFinder
    {
        /*public static async void GetSmartCardReader()
        {
            while (true == true)
            {
                DeviceInformationCollection devices = await DeviceInformation.FindAllAsync
                    (SmartCardReader.GetDeviceSelector());

                if (devices.Count > 0)
                {
                    List<SmartCardReader> readers = new List<SmartCardReader>();

                    foreach (DeviceInformation device in devices)
                    {
                        readers.Add(await SmartCardReader.FromIdAsync(device.Id));
                    }

                    Thread scanSmartCard = new Thread(new ParameterizedThreadStart(ScanSmartCard));
                    scanSmartCard.Start(readers);
                    Console.WriteLine("Card Reader Ready");
                    Thread.CurrentThread.Abort();
                }

                Thread.Sleep(2000);
            }
        }*/


        public static void ScanSmartCard()
        {
            List<SmartCardReader> readers = new List<SmartCardReader>();

            DeviceWatcher smartCardReaderWatcher = DeviceInformation.CreateWatcher();

            smartCardReaderWatcher.Added += async (sender, args) =>
            {
                DeviceInformationCollection devices =
                    await DeviceInformation.FindAllAsync(SmartCardReader.GetDeviceSelector());

                foreach (DeviceInformation device in devices)
                {
                    if (device.Id == args.Id)
                    {
                        readers.Add(await SmartCardReader.FromIdAsync(args.Id));
                        readers[readers.Count - 1].CardAdded += GetCardID;
                        Console.WriteLine("{0} is running and ready", readers[readers.Count - 1].Name); //can be changed to output in a popup
                    }
                }
            };

            smartCardReaderWatcher.Removed += (sender, args) =>
            {
                
                for (int idCounter = 0; idCounter < readers.Count; idCounter++)
                {
                    if (readers[idCounter].DeviceId == args.Id)
                    {
                        readers[idCounter].CardAdded -= GetCardID;
                        Console.WriteLine("{0} has been removed", readers[idCounter].Name); //can be changed to output in a popup
                        readers.RemoveAt(idCounter);
                        break;
                    }
                }
            };
            smartCardReaderWatcher.Start();
        }

        private static void GetCardID(SmartCardReader sender, CardAddedEventArgs args)
        {
            byte[] byteCardID = Get7ByteCardID(args).Result;

            string decimalCardID = Convert7ByteCardIDToDecimal(byteCardID);

            CopyPaste(decimalCardID);
        }

        public static async Task<byte[]> Get7ByteCardID(CardAddedEventArgs args)
        {
            byte[] byteReadUID = new byte[] { (byte) 0xFF, (byte) 0xCA,
                (byte) 0x00, (byte) 0x00, (byte) 0x00 };

            SmartCardConnection cardConnection = await args.SmartCard.ConnectAsync();

            IBuffer bufferResultUID = await cardConnection.TransmitAsync(byteReadUID.AsBuffer());

            CryptographicBuffer.CopyToByteArray(bufferResultUID, out byte[] byteResultUID);

            return byteResultUID;
        }

        public static string Convert7ByteCardIDToDecimal(byte[] byteCardID)
        {
            string hexCardID = ByteArrayToHex(byteCardID);

            hexCardID = hexCardID.Substring(0, 8);

            string decimalCardID = HexToDecimal(hexCardID);


            //not needed
            Console.WriteLine("HEX ID {0}:{1}:{2}:{3}", hexCardID.Substring(0, 2).ToUpper(), hexCardID.Substring(2, 2).ToUpper(),
                hexCardID.Substring(4, 2).ToUpper(), hexCardID.Substring(6, 2).ToUpper());
            Console.WriteLine("DEC ID {0}\n", decimalCardID);
            //not needed


            return decimalCardID;
        }

        public static string ByteArrayToHex(byte[] byteArray)
        {
            StringBuilder hex = new StringBuilder(byteArray.Length * 2);
            foreach (byte b in byteArray)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public static string HexToDecimal(string hex)
        {
            string stringDecimal = long.Parse(hex, System.Globalization.NumberStyles.HexNumber).ToString();

            return stringDecimal;
        }

        public static void CopyPaste(string textToCopyPaste)
        {
            Thread copyToClipboard = new Thread(() => Clipboard.SetText(textToCopyPaste));
            copyToClipboard.SetApartmentState(ApartmentState.STA);
            copyToClipboard.Start();
            copyToClipboard.Join();

            InputSimulator keyBoard = new InputSimulator();
            keyBoard.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            keyBoard.Keyboard.KeyPress(VirtualKeyCode.VK_V);
            keyBoard.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            keyBoard.Keyboard.KeyPress(VirtualKeyCode.RETURN);
        }
    }
}
