using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading;
using Android.Bluetooth;
using Java.Lang;

namespace Stewart_Control
{
    public delegate void RawDataHandler(byte[] data);

    public class BluetoothConnection
    {
        private const int BUFFER_SIZE = 255;

        public BluetoothSocket _socket = null;
        private BluetoothAdapter _adapter;
        private BluetoothDevice _device;
        public BluetoothDecoder _decoder;

        private System.Threading.Thread readThread;
        private Activity mainActivity;

        public event RawDataHandler NewRawDataArrived;

        private void getAdapter() { _adapter = BluetoothAdapter.DefaultAdapter; }
        private void getDevice() { _device = (from bd in _adapter.BondedDevices where bd.Name == "HC-05" select bd).FirstOrDefault(); }

        public byte[] _data;

        public BluetoothConnection(Activity parent)
        {
            mainActivity = parent;
            //We creating connection when constructing new object

            //Get the adapter
            getAdapter();
            if (_adapter == null)
            {
                throw new System.Exception("No default bluetooth adapter found!");
            }

            //Check if bluetooth is turned on
            if (_adapter.IsEnabled == false)
            {
                throw new System.Exception("Turn on bluetooth!");
            }


            //Get the device from paired devices
            getDevice();
            if (_device == null)
            {
                throw new System.Exception("Cannot found HC-05 device!");
            }

            //Create communication socket
            _socket = _device.CreateRfcommSocketToServiceRecord(Java.Util.UUID.FromString("00001101-0000-1000-8000-00805f9b34fb"));
            if (_socket == null)
            {
                throw new System.Exception("Error while creating RfcommSocket");
            }

            //Simply connect to the device
            //btConnect(_socket);
            _socket.Connect();

            if (_socket.IsConnected == false)
            {
                throw new System.Exception("Error while establishing connection!");
            }

            _data = new byte[BUFFER_SIZE];

            _decoder = new BluetoothDecoder(this);

        }

        public void SendMessage(byte[] buffer)
        {
            for (int i = 0; i < buffer.Length; i++)
            {
                _socket.OutputStream.WriteByte(buffer[i]);
            }
            _socket.OutputStream.WriteByte(13);
            _socket.OutputStream.WriteByte(10);
            _socket.OutputStream.Close();
        }

        public void StartReading()
        {
            //Launch the reading listener
            try
            {
                readThread = new System.Threading.Thread(ReadBluetoothListener);
            }
            catch
            {
                throw new System.Exception("Cannot create new Thread");
            }

            try
            {
                readThread.Start();
            }
            catch
            {
                throw new System.Exception("Cannot start thread function");
            }

        }

        public void StopReading()
        {

        }

        public void ReadBluetoothListener()
        {
            byte[] data = new byte[BUFFER_SIZE];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = 0;
            }

            int ctr = 0;

            while (true)
            {
                try
                {
                    _socket.InputStream.Read(data, ctr++, 1);
                    _socket.InputStream.Close();
                }
                catch
                { }
                data.CopyTo(_data, 0);
                _decoder.Decode(_data);

            }
        }
        
        ~BluetoothConnection()
        {
            readThread.Abort();
            _socket.Dispose();
            _socket.Close();
            _adapter.Disable();
        }
    }

    public class BluetoothDecoder
    {
        public event RawDataHandler NewRawDataArrived;
        private BluetoothConnection parent;

        public BluetoothDecoder(BluetoothConnection _parent)
        {
            parent = _parent;
        }

        public void Decode(byte[] data)
        {

        }


    }


}
