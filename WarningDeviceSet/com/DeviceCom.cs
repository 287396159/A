using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarningDeviceSet.model;

namespace WarningDeviceSet.com
{
    class DeviceCom
    {
        private Differentiable device;

        private SerialPort serialPort;

        private ComUtils comUtils=new ComUtils();

        private ReceivedData receivedData;
        public void setSerialPortReceivedData()
        {
            if (serialPort != null)
            {
                serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            int bytesToReadCount = serialPort.BytesToRead;
            byte[] buffer = new byte[bytesToReadCount];
            if (bytesToReadCount != 0)
            {
                serialPort.Read(buffer, 0, bytesToReadCount);
                receivedData.receivedData(buffer);
            }
            
        }

        public DeviceCom() { }

        public DeviceCom(SerialPort serialPort) {
            this.serialPort = serialPort;
        }
        public DeviceCom(SerialPort serialPort,Differentiable device)
        {
            this.serialPort = serialPort;
            this.device = device;
        }

        public void  setReceivedData(ReceivedData receivedData)
        {
            this.receivedData = receivedData;
        }

        public ReceivedData getReceivedData()
        {
            return receivedData;
        }
        public void setSerialPort(SerialPort serialPort)
        {
            this.serialPort = serialPort;
        }
        public SerialPort getSerialPort()
        {
            return serialPort;
        }

        public void setComUtils(ComUtils comUtils)
        {
            this.comUtils = comUtils;
        }
        public ComUtils getComUtils()
        {
            return comUtils;
        }

        public void setDevice(Differentiable device)
        {
            this.device = device;
        }
        public Differentiable getDevice()
        {
            return device;
        }

        private void setCom(byte type, byte[] data)
        {
            comUtils.setComE1(type, data);
            byte[] comBytes = comUtils.getComE1();
            receivedData.clearBuf();
            if (serialPort.IsOpen)
            {
                serialPort.Write(comBytes, 0, comBytes.Length);
            }
        }

        protected void setSearchCom(byte type)
        {
            setCom(type, getData());
        }

        protected void setSetCom(byte type,int lenght, byte[] data)
        {
            setCom(type, getData(lenght,data));
        }

        private byte[] getData(int length, byte[] appData)
        {
            int index = 0;
            byte[] data = new byte[length];
            byte[] id = getDevice().getId();
            for (; index < id.Length; index++)
            {
                data[index] = id[index];
            }
            for (; index < id.Length + appData.Length; index++)
            {
                //Console.WriteLine(index);
                data[index] = appData[index - id.Length];
            }
            return data;
        }

        private byte[] getData()
        {
            return device.getId();
        }

        /**
         * PC端 ：
         * E1 + 00 + CS + 1E
         * 设置端：      
         * F1+00+Num[1byte]+Id[3byte]+uid[3 type]+Type[1byte]+...+CS+1F(长度固定，为50*7+5字节)
         */
        public void sendSearchDeviceNumCom()
        {
            setCom(0x00, new byte[0]);

        }


        


    }
}
