﻿using System;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.Devices.SmartCards;
using Windows.Devices.Enumeration;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using WindowsInput;
using WindowsInput.Native;

namespace OO_Design_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            /*SmartCardReader reader = GetSmartCardReader().Result;
            Console.WriteLine(reader.Kind);
            reader.CardAdded += CardAdded;*/
            

            Thread findSmartCardReaders = new Thread(SmartCardReaderFinder.ScanSmartCard);
            findSmartCardReaders.Start();



            Console.ReadKey(false);


        }

        public static async Task<SmartCardReader> GetSmartCardReader()
        {
            DeviceInformationCollection devices = await DeviceInformation.FindAllAsync
                (SmartCardReader.GetDeviceSelector());

            return await SmartCardReader.FromIdAsync(devices[0].Id);
        }

        private static void CardAdded(SmartCardReader sender, CardAddedEventArgs args)
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
