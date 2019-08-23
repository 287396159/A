using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarningDeviceSet.model;

namespace WarningDeviceSet.com
{
    class ReceivedData
    {
        private List<byte[]> list = new List<byte[]>();
        private byte[] buf=new byte[0];
        private Form form;
        //private List<byte> bytes = new List<byte>();
        //private List<byte> bytesDec = new List<byte>();
        public void receivedData(byte[] buffer)
        {
            addBytes(buffer);
        }

        internal void clearBuf()
        {
            buf = new byte[0];
        }
        public void setForm(Form form)
        {
            this.form = form;
        }
        private void addBytes(byte[] buffer)
        {
            if (buf != null && buf.Length == 0)
            {
                buf = buffer;
            }
            else if (buf != null && buf.Length > 0)
            {
                setBuf(buf, buffer);
            }
            if (buf != null&& buf.Length > 0)
            {
                if (buf[0] == 0xF1&& buf.Length>1)
                {
                    switch (buf[1])
                    {
                        case 0x00:
                            List<Differentiable> devices = getDifferentiablesCom00();
                            if (devices != null)
                            {
                                com(0x00, devices);
                            }
                            break;
                        case 0x10:
                            byte[] IDBytes = getDataCom(10, 3, 5);
                            if (IDBytes != null)
                            {
                                com(0x10, IDBytes);
                            }
                            break;
                        case 0x12:
                            byte[] rangeTimeBytes = getDataCom(8, 1, 5);
                            if (rangeTimeBytes != null)
                            {
                                com(0x12, rangeTimeBytes);
                            }
                            break;
                        case 0x13:
                            byte[] setRangeTimeBytes = getDataCom(8, 1, 5);
                            if (setRangeTimeBytes != null)
                            {
                                com(0x13, setRangeTimeBytes);
                            }
                            break;
                        case 0x14:
                            byte[] cardStillTimeBytes = getDataCom(9, 2, 5);//7+dataCount
                            if (cardStillTimeBytes != null)
                            {
                                com(0x14, cardStillTimeBytes);
                            }
                            break;
                        case 0x15:
                            byte[] setCardStillTimeBytes = getDataCom(9, 2, 5);
                            if (setCardStillTimeBytes != null)
                            {
                                com(0x15, setCardStillTimeBytes);
                            }
                            break;
                        case 0x16:
                            byte[] accRangeBytes = getDataCom(8, 1, 5);
                            if (accRangeBytes != null)
                            {
                                com(0x16, accRangeBytes);
                            }
                            break;
                        case 0x17:
                            byte[] setAccRangeBytes = getDataCom(8, 1, 5);
                            if (setAccRangeBytes != null)
                            {
                                com(0x17, setAccRangeBytes);
                            }
                            break;
                        case 0x18:
                            byte[] id = getDataCom(7, 3, 2);
                            if (id != null)
                            {
                                com(0x18, id);
                            }
                            break;
                        case 0x40:
                            byte[] newID = getDataCom(10, 3, 5);
                            if (newID != null)
                            {
                                com(0x40, newID);
                            }
                            break;
                        case 0x41:
                            byte[] linkTime = getDataCom(9, 2, 5);
                            if (linkTime != null)
                            {
                                com(0x41, linkTime);
                            }
                            break;
                        case 0x43:
                            byte[] setLinkTime = getDataCom(9, 2, 5);
                            if (setLinkTime != null)
                            {
                                com(0x43, setLinkTime);
                            }
                            break;
                        case 0x44:
                            byte[] safeDistance = getDataCom(9,2,5);
                            if (safeDistance != null)
                            {
                                com(0x44, safeDistance);
                            }
                            break;
                        case 0x45:
                            byte[] setSafeDistance = getDataCom(9, 2, 5);
                            if (setSafeDistance != null)
                            {
                                com(0x45, setSafeDistance);
                            }
                            break;
                        case 0x46:
                            byte[] volume = getDataCom(8, 1, 5);
                            if (volume != null)
                            {
                                com(0x46, volume);
                            }
                            break;
                        case 0x47:
                            byte[] setVolume = getDataCom(8, 1, 5);
                            if (setVolume != null)
                            {
                                com(0x47, setVolume);
                            }
                            break;
                        case 0x48:
                            List<Differentiable> cardDevices = getDifferentiablesCom48();
                            if (cardDevices != null)
                            {
                                com(0x48, cardDevices);
                            }
                            break;
                        case 0x49:
                            byte[] revflag = getDataCom(8, 1, 5);
                            if (revflag != null)
                            {
                                com(0x49, revflag);
                            }
                            break;
                        default:
                            //string s = "";
                            //foreach (byte b in buf)
                            //{
                            //    s += b;
                            //}
                            //MessageBox.Show(s);
                            //setBuf(buf, buffer);
                            break;
                    }
                }
            }
        }

        #region
        private byte[] getIDCom10(int comLength)
        {
            if (buf.Length == comLength && checkBuf())
            {
                return new byte[] { buf[5], buf[6], buf[7] };
            }else if (buf.Length> comLength)
            {
                buf = new byte[0];
            }
            return null;
        }

        private byte[] getRangeTimeCom12(int comLength)//,int dataLength)
        {
            if (buf.Length == comLength && checkBuf())
            {
                //byte[] bytes = new byte[dataLength];
                //Array.Copy(buf, 5, bytes, 0, bytes.Length);
                return new byte[] { buf[5] };
            }
            else if (buf.Length > comLength)
            {
                buf = new byte[0];
            }
            return null;
        }

        private byte[] getCardStillTimeCom14(int comLength)
        {
            if (buf.Length == comLength && checkBuf())
            {
                return new byte[] { buf[5],buf[6] };
            }
            else if (buf.Length > comLength)
            {
                buf = new byte[0];
            }
            return null;
        }

        private byte[] getAccRangeCom16(int comLength)
        {
            if (buf.Length == comLength && checkBuf())
            {
                return new byte[] { buf[5] };
            }
            else if (buf.Length > comLength)
            {
                buf = new byte[0];
            }
            return null;
        }

        private byte[] getIDCom18(int comLength)
        {
            if (buf.Length == comLength && checkBuf())
            {
                return new byte[] { buf[2],buf[3],buf[4] };
            }
            else if (buf.Length > comLength)
            {
                buf = new byte[0];
            }
            return null;
        }

        private byte[] getLinkTimeCom41(int comLength)
        {
            if (buf.Length == comLength && checkBuf())
            {
                return new byte[] { buf[5], buf[6] };
            }
            else if (buf.Length > comLength)
            {
                buf = new byte[0];
            }
            return null;
        }

        private byte[] getLinkTimeCom44(int comLength)
        {
            if (buf.Length == comLength && checkBuf())
            {
                return new byte[] { buf[5], buf[6] };
            }
            else if (buf.Length > comLength)
            {
                buf = new byte[0];
            }
            return null;
        }
        #endregion

        private byte[] getDataCom(int comLength,int fixedComLength,int dataStartIndex)
        {
            if (comLength < fixedComLength+dataStartIndex)
            {
                return null;
            }
            if (buf.Length == comLength && checkBuf())
            {
                byte[] bytes = new byte[fixedComLength];
                for(int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = buf[dataStartIndex + i];
                }
                return bytes;
            }
            else if (buf.Length > comLength)
            {
                buf = new byte[0];
            }
            return null;
        }

        private List<Differentiable> getDifferentiablesCom00()
        {
            if (buf.Length == 50 * 7 + 5&& checkBuf())
            {
                List<Differentiable> devices = new List<Differentiable>();
                byte devicesNum = buf[2];
                for (int i = 0; i < devicesNum; i++)
                {
                    Differentiable device = null;
                    byte[] id = new byte[3];
                    for (int j = 0; j < 7; j++)
                    {
                        if (j < 3)
                        {
                            id[j] = buf[3 + i * 7 + j];
                        }
                        if (j == 6)
                        {
                            switch (buf[3 + i * 7 + j])
                            {
                                case 0x00:
                                    device = new CardDevice();
                                    break;
                                case 0x01:
                                    device = new WarningDevice();
                                    break;
                                case 0x02:
                                    break;
                                case 0x03:
                                    break;
                                default:
                                    break;

                            }
                            device.setId(id);
                        }
                    }
                    devices.Add(device);
                }
                return devices;
            }else if(buf.Length > 50 * 7 + 5)
            {
                buf = new byte[0];
            }
            return null;
        }


        private List<Differentiable> getDifferentiablesCom48()
        {
            if (buf.Length == 100 * 3 + 7 && checkBuf())
            {
                List<Differentiable> devices = new List<Differentiable>();
                for(int i = 0; i < 100; i++)
                {
                    Differentiable device = new CardDevice();
                    byte[] id = new byte[3];
                    for(int index = 0; index < id.Length; index++)
                    {
                        id[index] = buf[5 + index + i * 3];
                    }
                    device.setId(id);
                    if (id[0] == 0 && id[1] == 0 && id[2] == 0)
                    {
                        return devices;
                    }
                    else
                    {
                        devices.Add(device);
                    }
                }
                return devices;
            }
            else if (buf.Length > 100 * 3 + 7)
            {
                buf = new byte[0];
            }
            return null;
        }

        private void com(byte comType,object data)
        {
            Form1 form1 = form as Form1;
            form1.receivedData(comType,data);
            buf = new byte[0];
        }

        private void setBuf(byte[] oldBuf,byte[] buffer)
        {
            //Console.WriteLine(oldBuf.Length + "XXXXX" + buffer.Length);
            buf = new byte[oldBuf.Length + buffer.Length];
            int i = 0;
            for (; i < oldBuf.Length; i++)
            {
                buf[i] = oldBuf[i];
            }
            for (; i < oldBuf.Length + buffer.Length; i++)
            {
                buf[i] = buffer[i - oldBuf.Length];
            }
        }

        private bool checkBuf()
        {
            if (buf.Length > 4 && buf[0] == 0xF1 && buf[buf.Length - 1] == 0x1F)
            {
                
                int checkSum = 0;
                for(int i = 1; i < buf.Length - 2; i++)
                {
                    checkSum += buf[i];
                }
                if ((byte)checkSum == buf[buf.Length - 2])
                {
                    return true;
                }
            }
            return false;
        }

    }
}
