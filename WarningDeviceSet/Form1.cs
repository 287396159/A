using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WarningDeviceSet.com;
using WarningDeviceSet.model;

namespace WarningDeviceSet
{
    public partial class Form1 : Form
    {

        private SerialPort serialPort = new SerialPort();
        private DeviceCom deviceCom = new DeviceCom();
        private ReceivedData received;
        private List<Differentiable> devices;
        private Differentiable device;
        private delegate void ShowView(object data);
        private ShowView showView;
        private CLICKEVENTNUM clickEventNum = CLICKEVENTNUM.NONE;

        public Form1()
        {
            InitializeComponent();
            redresh();
            received = new ReceivedData();
            received.setForm(this);
            setDeviceCom();
        }

        private void setDeviceCom()
        {
            if (device is CardDevice)
            {
                deviceCom = new CardDeviceCom();
            }
            else if (device is WarningDevice)
            {
                deviceCom = new WarningDeviceCom();
            }
            deviceCom.setSerialPort(serialPort);
            deviceCom.setDevice(device);
            deviceCom.setReceivedData(received);
            deviceCom.setSerialPortReceivedData();
        }

        private void redresh()
        {
            comboBox1.Items.Clear();
            string[] str = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(str);
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = 0;
            }

        }

        private void Label1_Click(object sender, EventArgs e)
        {
            CardDevice cardDevice = new CardDevice();
            //cardDevice.setId(111);
            //label1.Text = "" + cardDevice.getId();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
            {
                clickEventNum = CLICKEVENTNUM.SEARCH;
                deviceCom.sendSearchDeviceNumCom();
            }
            else
            {
                MessageBox.Show("请打开端口");
            }
        }


        public void receivedData(byte comType, object data)
        {
            switch (comType)
            {
                case 0x00:
                    showView = new ShowView(receivedData00);
                    break;
                case 0x10:
                    showView = new ShowView(receivedData18);
                    break;
                case 0x12:
                    showView = new ShowView(receivedData12);
                    break;
                case 0x13:
                    showView = new ShowView(receivedData12);
                    break;
                case 0x14:
                    showView = new ShowView(receivedData14);
                    break;
                case 0x15:
                    showView = new ShowView(receivedData14);
                    break;
                case 0x16:
                    showView = new ShowView(receivedData16);
                    break;
                case 0x17:
                    showView = new ShowView(receivedData16);
                    break;
                case 0x18:
                    showView = new ShowView(receivedData18);
                    break;
                case 0x40:
                    showView = new ShowView(receivedData40);
                    break;
                case 0x41:
                    showView = new ShowView(receivedData41);
                    break;
                case 0x43:
                    showView = new ShowView(receivedData41);
                    break;
                case 0x44:
                    showView = new ShowView(receivedData44);
                    break;
                case 0x45:
                    showView = new ShowView(receivedData44);
                    break;
                case 0x46:
                    showView = new ShowView(receivedData46);
                    break;
                case 0x47:
                    showView = new ShowView(receivedData46);
                    break;
                case 0x48:
                    showView = new ShowView(receivedData48);
                    break;
                case 0x49:
                    showView = new ShowView(receivedData49);
                    break;
                default:
                    break;
            }
            BeginInvoke(showView, data);
        }

        
        private void receivedData49(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.SETWARNCARD|| clickEventNum == CLICKEVENTNUM.ADDWARNCARD || clickEventNum == CLICKEVENTNUM.REMOVEWARNCARD)
            {
                if(data is byte[])
                {
                    byte[] revflagBytes = data as byte[];
                    if(revflagBytes.Length== 1)
                    {
                        if (revflagBytes[0] == 0)
                        {
                            showResult();
                        }
                        else
                        {
                            switch (clickEventNum)
                            {
                                case CLICKEVENTNUM.GETWARNCARD:
                                    label12.Text = "Read card failed";
                                    break;
                                case CLICKEVENTNUM.SETWARNCARD:
                                    label12.Text = "Set card failed";
                                    break;
                                case CLICKEVENTNUM.ADDWARNCARD:
                                    label12.Text = "Add card failed";
                                    break;
                                case CLICKEVENTNUM.REMOVEWARNCARD:
                                    label12.Text = "Remove card failed";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                clickEventNum = CLICKEVENTNUM.NONE;
            }
        }

        private void receivedData48(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.GETWARNCARD|| clickEventNum == CLICKEVENTNUM.ADDWARNCARD || clickEventNum == CLICKEVENTNUM.LISTVIEWITEMWARNCLICK || clickEventNum == CLICKEVENTNUM.SEARCHWARNID)
            {
                listView3.Items.Clear();
                if (data is List<Differentiable>)
                {
                    List<Differentiable> warnCardDevices = data as List<Differentiable>;
                    foreach(Differentiable warnCardDevice in warnCardDevices)
                    {
                        if(warnCardDevice is CardDevice)
                        {
                            ListViewItem listViewItem = new ListViewItem();
                            listViewItem.Text = getID(warnCardDevice.getId());
                            listViewItem.BackColor = Color.DeepSkyBlue;
                            listView3.Items.Add(listViewItem);
                        }
                    }
                }
                if (clickEventNum == CLICKEVENTNUM.ADDWARNCARD)
                {
                    byte[] idBytes = getID(textBox5.Text, 3);
                    bool isExist = false;
                    if(data is List<Differentiable>)
                    {
                        List<Differentiable> warnCardDevices = data as List<Differentiable>;
                        foreach (Differentiable warnCardDevice in warnCardDevices)
                        {
                            if (warnCardDevice is CardDevice)
                            {
                                byte[] warnCardDeviceID = warnCardDevice.getId();
                                bool[] isExistBytes = new bool[idBytes.Length];
                                bool isOk = true;
                                for (int i=0;i< idBytes.Length; i++)
                                {
                                    if (idBytes[i] == warnCardDeviceID[i])
                                    {
                                        isExistBytes[i] = true;
                                    }
                                    else
                                    {
                                        isOk = false;
                                        isExistBytes[i] = false;
                                    }
                                }
                                if (isOk)
                                {
                                    isExist = true;
                                }
                                //foreach(bool isExistByte in isExistBytes)
                                //{
                                //    if (!isExistByte)
                                //    {
                                        
                                //    }
                                //}
                            }
                        }
                    }
                    if (isExist)
                    {

                    }
                    else
                    {
                        byte[] setCardBytes = new byte[300];
                        ListViewItem addListViewItem = new ListViewItem();
                        addListViewItem.Text = getID(getID(textBox5.Text, 3));
                        addListViewItem.BackColor = Color.DeepSkyBlue;
                        listView3.Items.Add(addListViewItem);
                        for (int i = 0; i < listView3.Items.Count; i++)
                        {
                            string cardIDStr = listView3.Items[i].Text;
                            byte[] cardIDBytes = getID(cardIDStr, 3);
                            setCardBytes[i * 3] = cardIDBytes[0];
                            setCardBytes[i * 3 + 1] = cardIDBytes[1];
                            setCardBytes[i * 3 + 2] = cardIDBytes[2];
                        }
                        WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                        warningDeviceCom.sendSetWarningCardsCom(setCardBytes);
                    }
                }
                else
                {
                    showResult();
                }
            }
        }

        private void receivedData46(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.SETVOLUME || clickEventNum == CLICKEVENTNUM.GETVOLUME || clickEventNum == CLICKEVENTNUM.LISTVIEWITEMWARNCLICK || clickEventNum == CLICKEVENTNUM.SEARCHWARNID)
            {
                byte[] volume = data as byte[];
                trackBar1.Value = volume[0];
                showResult();
            }
        }

        private void receivedData44(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.SETSAFEDISTANCE || clickEventNum == CLICKEVENTNUM.GETSAFEDISTANCE || clickEventNum == CLICKEVENTNUM.LISTVIEWITEMWARNCLICK || clickEventNum == CLICKEVENTNUM.SEARCHWARNID)
            {
                byte[] safeDistanceBytes = data as byte[];
                textBox3.Text = "" + (((int)safeDistanceBytes[0]) * 256 + safeDistanceBytes[1]);
                showResult();
            }
        }

        private void receivedData41(object data) {
            if (clickEventNum == CLICKEVENTNUM.SETLINKTIME || clickEventNum == CLICKEVENTNUM.GETLINKTIME || clickEventNum == CLICKEVENTNUM.LISTVIEWITEMWARNCLICK || clickEventNum == CLICKEVENTNUM.SEARCHWARNID)
            {
                byte[] linkTimeBytes = data as byte[];
                textBox2.Text = "" + (((int)linkTimeBytes[0]) * 256 + linkTimeBytes[1]);
                textBox1.Text = getID(device.getId());
                showResult();
            }
        }

        private void receivedData40(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.SETWARNID)
            {
                if (data is byte[])
                {
                    byte[] idBytes = data as byte[];
                    if (idBytes.Length == 3)
                    {
                        device.setId(idBytes);
                        deviceCom.sendSearchDeviceNumCom();
                        showResult();
                    }
                }
            }
        }

        private void receivedData18(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.SETCARDID || clickEventNum == CLICKEVENTNUM.LISTVIEWITEMCARDCLICK || clickEventNum == CLICKEVENTNUM.SEARCHCARDID)
            {
                byte[] idBytes = data as byte[];
                textBox1.Text = "" + getID(idBytes);
                device.setId(idBytes);
                if (clickEventNum == CLICKEVENTNUM.SETCARDID)
                {
                    deviceCom.sendSearchDeviceNumCom();
                }
                showResult();
                //if (clickEventNum == CLICKEVENTNUM.SETCARDID)
                //{
                //    clickEventNum = CLICKEVENTNUM.NONE;
                //}
            }
        }
        private void receivedData16(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.SETACCRANGE || clickEventNum == CLICKEVENTNUM.GETACCRANGE || clickEventNum == CLICKEVENTNUM.LISTVIEWITEMCARDCLICK || clickEventNum == CLICKEVENTNUM.SEARCHCARDID)
            {
                byte[] accRangeBytes = data as byte[];
                textBox4.Text = "" + accRangeBytes[0];
                showResult();
                clickEventNum = CLICKEVENTNUM.NONE;
            }
        }

        private void showResult()
        {
            string idStr = getID(device.getId());
            switch (clickEventNum)
            {
                case CLICKEVENTNUM.LISTVIEWITEMCARDCLICK:
                    //label12.Text = "Search " + idStr + " success";
                    break;
                case CLICKEVENTNUM.SETCARDID:
                    label12.Text = "Set card id(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.SEARCHCARDID:
                    label12.Text = "Search " + idStr + " success";
                    break;
                case CLICKEVENTNUM.SETRANGETIME:
                    label12.Text = "Set range time(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.GETRANGETIME:
                    label12.Text = "Read range time(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.SETCARDSTILLTIME:
                    label12.Text = "Set card still time(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.GETCARDSTILLTIME:
                    label12.Text = "Read card still time(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.SETACCRANGE:
                    label12.Text = "Set acc range("+ idStr + ") success";
                    break;
                case CLICKEVENTNUM.GETACCRANGE:
                    label12.Text = "Read acc range("+ idStr + ") success";
                    break;
                case CLICKEVENTNUM.LISTVIEWITEMWARNCLICK:
                    break;
                case CLICKEVENTNUM.SETWARNID:
                    label12.Text = "Set id(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.SEARCHWARNID:
                    label12.Text = "Search " + idStr + " success";;
                    break;
                case CLICKEVENTNUM.SETLINKTIME:
                    label12.Text = "Set line time(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.GETLINKTIME:
                    label12.Text = "Read line time(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.SETSAFEDISTANCE:
                    label12.Text = "Set safa distance(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.GETSAFEDISTANCE:
                    label12.Text = "Read safa distance(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.SETVOLUME:
                    label12.Text = "Set volume(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.GETVOLUME:
                    label12.Text = "Read volume(" + idStr + ") success";
                    break;
                case CLICKEVENTNUM.GETWARNCARD:
                    label12.Text = "Read card success";
                    break;
                case CLICKEVENTNUM.SETWARNCARD:
                    label12.Text = "Set card success";
                    break;
                case CLICKEVENTNUM.ADDWARNCARD:
                    label12.Text = "Add card success";
                    break;
                case CLICKEVENTNUM.REMOVEWARNCARD:
                    label12.Text = "Remove card success";
                    break;
                default:
                    break;
            }
            if (clickEventNum != CLICKEVENTNUM.SETCARDID && clickEventNum != CLICKEVENTNUM.SETWARNID && clickEventNum != CLICKEVENTNUM.LISTVIEWITEMCARDCLICK && clickEventNum != CLICKEVENTNUM.SEARCHCARDID&& clickEventNum != CLICKEVENTNUM.LISTVIEWITEMWARNCLICK && clickEventNum != CLICKEVENTNUM.SEARCHWARNID)
            {
                clickEventNum = CLICKEVENTNUM.NONE;
            }
            
        }
        private void receivedData14(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.SETCARDSTILLTIME || clickEventNum == CLICKEVENTNUM.GETCARDSTILLTIME || clickEventNum == CLICKEVENTNUM.LISTVIEWITEMCARDCLICK || clickEventNum == CLICKEVENTNUM.SEARCHCARDID)
            {
                byte[] cardStillTimeBytes = data as byte[];
                textBox3.Text = "" + (((int)cardStillTimeBytes[0])*256+ cardStillTimeBytes[1]);
                showResult();
                //if (clickEventNum == CLICKEVENTNUM.SETCARDSTILLTIME || clickEventNum == CLICKEVENTNUM.GETCARDSTILLTIME)
                //{
                //    clickEventNum = CLICKEVENTNUM.NONE; 
                //}
            }
                
        }
        private void receivedData12(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.GETRANGETIME || clickEventNum == CLICKEVENTNUM.SETRANGETIME|| clickEventNum==CLICKEVENTNUM.LISTVIEWITEMCARDCLICK || clickEventNum == CLICKEVENTNUM.SEARCHCARDID)
            {
                if(data is byte[])
                {
                    byte[] rangeTimeBytes = data as byte[];
                    if (rangeTimeBytes.Length == 1)
                    {
                        textBox2.Text = ""+rangeTimeBytes[0];
                    }
                }
                showResult();
                //if (clickEventNum == CLICKEVENTNUM.GETRANGETIME || clickEventNum == CLICKEVENTNUM.SETRANGETIME)
                //{
                //    clickEventNum = CLICKEVENTNUM.NONE;
                //}
            }
        }
        private void receivedData00(object data)
        {
            if (clickEventNum == CLICKEVENTNUM.SEARCH||clickEventNum==CLICKEVENTNUM.SETCARDID||clickEventNum==CLICKEVENTNUM.SETWARNID)
            {
                listView1.Items.Clear();
                listView2.Items.Clear();
                if (data is List<Differentiable>)
                {
                    devices = data as List<Differentiable>;

                    foreach (Differentiable device in devices)
                    {
                        byte[] id = device.getId();
                        ListViewItem listViewItem = new ListViewItem();
                        listViewItem.Text = getID(id);
                        listViewItem.BackColor = Color.DeepSkyBlue;
                        if (device is CardDevice)
                        {
                            listView1.Items.Add(listViewItem);
                        }
                        else if (device is WarningDevice)
                        {
                            listView2.Items.Add(listViewItem);
                        }
                    }

                }
                clickEventNum = CLICKEVENTNUM.NONE;
            }
        }


        private string getID(byte[] id)
        {
            string idStr = "";
            foreach(byte idItem in id)
            {
                idStr += Convert.ToString(idItem, 16).PadLeft(2,'0');
            }
            return idStr;
        }
        private byte[] getID(string idStr,int idLength)
        {
            byte[] id = new byte[idLength];

            if (idStr.Length < idLength*2)
            {
                idStr=idStr.PadLeft(idLength * 2, '0');
            }else if (idStr.Length > idLength * 2)
            {
                idStr = idStr.Substring(0, idLength * 2);
            }

            char[] idStrArr=idStr.ToArray();
            bool isOK = true;
            foreach(char ch in idStrArr)
            {
                byte by = (byte)ch;
                if(!((by >= 48 && by <= 57) || (by >= 65 && by <= 70)|| (by >= 97 && by <= 102)))
                {
                    isOK = false;
                }
            }
            if (isOK)
            {
                for(int i = 0; i < id.Length; i++)
                {
                    id[i] = Convert.ToByte(idStr.Substring(i * 2, 2), 16);
                }

                return id;
            }
            return null;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            redresh();
        }

        private void Button2_Click(object sender, EventArgs e)
        {

            if ("Connect".Equals(button2.Text))
            {
                try
                {
                    serialPort.PortName = comboBox1.SelectedItem.ToString();
                    serialPort.BaudRate = 115200;
                    serialPort.Parity = Parity.None;
                    serialPort.StopBits = StopBits.One;
                    //serialPort.DataBits = (int)SerialPortDataBits.EightBits;
                    serialPort.Open();
                    if (serialPort.IsOpen)
                    {
                        button2.Text = "DisConnect";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("端口占用或配置错误");
                }

            }
            else
            {
                serialPort.Close();
                button2.Text = "Connect";
            }
        }

        private void ListView1_Click(object sender, EventArgs e)
        {
            clickEventNum = CLICKEVENTNUM.LISTVIEWITEMCARDCLICK;
            listClick(listView1);
        }
        private void ListView2_Click(object sender, EventArgs e)
        {
            clickEventNum = CLICKEVENTNUM.LISTVIEWITEMWARNCLICK;
            listClick(listView2);
            textBox1.Text = "" + getID(device.getId());
        }
        private void listClick(ListView listView)
        {
            foreach (ListViewItem item in listView.SelectedItems)
            {
                string itemText = item.Text;
                byte[] id = new byte[itemText.Length / 2];
                for (int i = 0; i < id.Length; i++)
                {
                    id[i] = Convert.ToByte(itemText.Substring(i * 2, 2), 16);
                }
                Differentiable device = getDevice(id);
                this.device = device;
                setDeviceCom();
                showDevice(this.device);
                searchDeviceAllInfo();
            }
        }


        private void showDevice(Differentiable device)
        {
            splitContainer1.Panel2.Controls.Clear();
            setTextBoxValue();
            if (device is CardDevice)
            {
                label3.Location = new Point(52, 120);
                label3.Text = "Range Time:";
                label4.Location = new Point(52, 180);
                label4.Text = "Still Time:";
                label5.Location = new Point(58, 240);
                label5.Text = "Acc Range:";
                label7.Text = "Type:Card Device";

                label9.Text = "(range:1~255,unit:s)";
                label10.Text = "(range:0~65535)";
                label11.Text = "(range:10~86)";
                splitContainer1.Panel2.Controls.Add(label11);
                splitContainer1.Panel2.Controls.Add(textBox4);
            }
            else if (device is WarningDevice)
            {
                label3.Location = new Point(52, 120);
                label3.Text = "Link Time:";
                label4.Location = new Point(34, 180);
                label4.Text = "Safe Distance:";
                label5.Location = new Point(70, 240);
                label5.Text = "Volume:";
                label6.Location = new Point(82, 300);
                label6.Text = "Card:";
                label7.Text = "Type:Warning Device";
                label9.Text = "(range:0~65535,unit:s)";
                label10.Text = "(range:0~65535,unit:cm)";
                label11.Text = "(range:0~10)";
                splitContainer1.Panel2.Controls.Add(label6);
                splitContainer1.Panel2.Controls.Add(button15);
                splitContainer1.Panel2.Controls.Add(button14);
                splitContainer1.Panel2.Controls.Add(button13);
                //splitContainer1.Panel2.Controls.Add(button12);//setCard
                splitContainer1.Panel2.Controls.Add(listView3);

                splitContainer1.Panel2.Controls.Add(textBox5);

                splitContainer1.Panel2.Controls.Add(trackBar1);
                label13.Location = new Point(50, 30);
                trackBar1.Controls.Add(label13);
                //splitContainer1.Panel2.Controls.Add(label13);
            }
            addView();
        }

        //private string[][] textBoxBuf = new string[2][];

        private void setTextBoxValue()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            label12.Text = "";
            textBox4.Text = "";
            label13.Text = "0";
            trackBar1.Value = 0;
        }

        private void addView()
        {

            label8.Text = "(range:000001~FFFFFE,error:xx0000 or xxFFFF)";
            splitContainer1.Panel2.Controls.Add(label7);
            splitContainer1.Panel2.Controls.Add(button11);
            splitContainer1.Panel2.Controls.Add(button10);
            splitContainer1.Panel2.Controls.Add(button9);
            splitContainer1.Panel2.Controls.Add(button8);
            splitContainer1.Panel2.Controls.Add(button7);
            splitContainer1.Panel2.Controls.Add(button6);
            splitContainer1.Panel2.Controls.Add(button5);
            splitContainer1.Panel2.Controls.Add(button4);
            splitContainer1.Panel2.Controls.Add(textBox3);
            splitContainer1.Panel2.Controls.Add(textBox2);
            splitContainer1.Panel2.Controls.Add(textBox1);
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(label12);
            splitContainer1.Panel2.Controls.Add(label10);
            splitContainer1.Panel2.Controls.Add(label9);
            splitContainer1.Panel2.Controls.Add(label8);
        }

        private Differentiable getDevice(byte[] id)
        {
            if (id != null)
            {
                for (int i = 0; i < devices.Count; i++)
                {
                    Differentiable device = devices[i];
                    byte[] ID = device.getId();
                    if (id.Length == ID.Length)
                    {
                        bool isOk = true;
                        for (int index = 0; index < ID.Length; index++)
                        {
                            if (id[index] != ID[index])
                            {
                                isOk = false;
                            }
                        }
                        if (isOk)
                        {
                            if(this.device is CardDevice&&device is CardDevice)
                            {
                                return device;
                            }
                            else if(this.device is WarningDevice&&device is WarningDevice)
                            {
                                return device;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Button4_Click");
            //set id
            string idStr = textBox1.Text;
            byte[] id = getID(idStr, 3);
            if (id != null)
            {
                string endId = idStr.PadLeft(6, '0').Substring(2, 4);
                if ("0000" != endId && !"FFFF".Equals(endId.ToUpper()))
                {
                    if (device is CardDevice)
                    {
                        if (deviceCom is CardDeviceCom)
                        {
                            CardDeviceCom cardDeviceCom = deviceCom as CardDeviceCom;
                            clickEventNum = CLICKEVENTNUM.SETCARDID;
                            cardDeviceCom.sendSetCardIdCom(id);
                            //cardDeviceCom.sendSearchRangeTime();


                        }
                    }
                    else if (device is WarningDevice)
                    {
                        if (deviceCom is WarningDeviceCom)
                        {
                            WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                            clickEventNum = CLICKEVENTNUM.SETWARNID;
                            warningDeviceCom.sendSetWarningIdCom(id);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect format!");
                }
            }
            else
            {
                MessageBox.Show("Incorrect format!");
            }

        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //search id
            string textStr=textBox1.Text;
            string idStr = textStr.Replace(" ", "");
            byte[] id = getID(idStr, 3);
            if (id != null)
            {
                string endId = idStr.PadLeft(6, '0').Substring(2, 4);
                if ("0000" != endId && !"FFFF".Equals(endId.ToUpper()))
                {
                    //device.setId(id);
                    if (device is CardDevice)
                    {
                        clickEventNum = CLICKEVENTNUM.SEARCHCARDID;
                    }
                    else if (device is WarningDevice)
                    {
                        clickEventNum = CLICKEVENTNUM.SEARCHWARNID;
                    }
                    searchDeviceAllInfo();
                }
                else
                {
                    MessageBox.Show("Incorrect format!");
                }
                
            }
            else
            {
                MessageBox.Show("Incorrect format!");
            }
            //if (idStr.Length >= 6)
            //{
            //    device.setId(new byte[] {Convert.ToByte(idStr.Substring(0,2),16), Convert.ToByte(idStr.Substring(2, 2), 16), Convert.ToByte(idStr.Substring(4, 2), 16) });
            //}

        }

        private void searchDeviceAllInfo()
        {
            if(device is CardDevice)
            {
                if(deviceCom is CardDeviceCom)
                {
                    CardDeviceCom cardDeviceCom = deviceCom as CardDeviceCom;

                    cardDeviceCom.sendShowCardDeviceCom();
                    Thread.Sleep(120);
                    cardDeviceCom.sendSearchRangeTime();
                    Thread.Sleep(120);
                    cardDeviceCom.sendSearchStillTime();
                    Thread.Sleep(120);
                    cardDeviceCom.sendSearchAccRange();
                }
            }else if(device is WarningDevice)
            {
                if (deviceCom is WarningDeviceCom)
                {
                    WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;

                    warningDeviceCom.sendSerachWarningLinkTimeCom();
                    Thread.Sleep(120);
                    warningDeviceCom.sendSerachWarningSafeDistanceCom();
                    Thread.Sleep(120);
                    warningDeviceCom.sendSerachWarningVolumeCom();
                    Thread.Sleep(120);
                    warningDeviceCom.sendSerachWarningCardsCom();
                }
            }
        }


        private void Button6_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Button6_Click");
            //set Link Time
            if (device is CardDevice)
            {
                if (deviceCom is CardDeviceCom)
                {
                    CardDeviceCom cardDeviceCom = deviceCom as CardDeviceCom;
                    clickEventNum = CLICKEVENTNUM.SETRANGETIME;
                    //cardDeviceCom.sendSearchRangeTime();
                    //string rangeTimeText = textBox2.Text;
                    try
                    {
                        byte rangeTime = Convert.ToByte(textBox2.Text);
                        if (rangeTime>0)
                        {
                            cardDeviceCom.sendSetRangeTime(new byte[] { rangeTime });
                        }
                        else
                        {
                            MessageBox.Show("Incorrect format!");
                        }
                        
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    
                }
            }
            else if (device is WarningDevice)
            {
                if (deviceCom is WarningDeviceCom)
                {
                    WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                    clickEventNum = CLICKEVENTNUM.SETLINKTIME;
                    //string linkTimeText = textBox2.Text;
                    try
                    {
                        byte[] linkTimeBytes = new byte[2];
                        int linkTime = Convert.ToInt32(textBox2.Text);
                        if (linkTime >= 0 && linkTime <= 65535)
                        {
                            linkTimeBytes[0] = (byte)(linkTime / 256);
                            linkTimeBytes[1] = (byte)(linkTime % 256);
                            warningDeviceCom.sendSetWarningLinkTimeCom(linkTimeBytes);
                        }
                        else
                        {
                            MessageBox.Show("Incorrect format!");
                        }
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                }
            }
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            ////MessageBox.Show("Button7_Click");
            //read Link time
            if (device is CardDevice)
            {
                if (deviceCom is CardDeviceCom)
                {
                    CardDeviceCom cardDeviceCom = deviceCom as CardDeviceCom;
                    clickEventNum = CLICKEVENTNUM.GETRANGETIME;
                    cardDeviceCom.sendSearchRangeTime();
                }
            }else if (device is WarningDevice)
            {
                if (deviceCom is WarningDeviceCom)
                {
                    WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                    clickEventNum = CLICKEVENTNUM.GETLINKTIME;
                    warningDeviceCom.sendSerachWarningLinkTimeCom();
                }
            }

        }

        private void Button8_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Button8_Click");
            //set safe Distance
            if (device is CardDevice)
            {
                if (deviceCom is CardDeviceCom)
                {
                    CardDeviceCom cardDeviceCom = deviceCom as CardDeviceCom;
                    clickEventNum = CLICKEVENTNUM.SETCARDSTILLTIME;
                    //string stillTimeText = textBox3.Text;
                    try
                    {
                        byte[] stillTimeBytes = new byte[2];
                        int stillTime = Convert.ToInt32(textBox3.Text);
                        if (stillTime >= 0&&stillTime<=65535)
                        {
                            stillTimeBytes[0] = (byte)(stillTime / 256);
                            stillTimeBytes[1] = (byte)(stillTime % 256);
                            cardDeviceCom.sendSetStillTime(stillTimeBytes);
                        }
                        else
                        {
                            MessageBox.Show("Incorrect format!");
                        }
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    //cardDeviceCom.sendSearchRangeTime();
                }
            }
            else if (device is WarningDevice)
            {
                if (deviceCom is WarningDeviceCom)
                {
                    WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                    clickEventNum = CLICKEVENTNUM.SETSAFEDISTANCE;
                    try
                    {
                        byte[] safeDistanceBytes = new byte[2];
                        int safeDistance = Convert.ToInt32(textBox3.Text);
                        if (safeDistance >= 0 && safeDistance <= 65535)
                        {
                            safeDistanceBytes[0] = (byte)(safeDistance / 256);
                            safeDistanceBytes[1] = (byte)(safeDistance % 256);
                            warningDeviceCom.sendSetWarningSafeDistanceCom(safeDistanceBytes);
                        }
                        else
                        {
                            MessageBox.Show("Incorrect format!");
                        }
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    //warningDeviceCom.sendSetWarningSafeDistanceCom();
                }
            }
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            //read safe Distance
            if (device is CardDevice)
            {
                if (deviceCom is CardDeviceCom)
                {
                    CardDeviceCom cardDeviceCom = deviceCom as CardDeviceCom;
                    clickEventNum = CLICKEVENTNUM.GETCARDSTILLTIME;
                    cardDeviceCom.sendSearchStillTime();
                }
            }
            else if (device is WarningDevice)
            {
                if (deviceCom is WarningDeviceCom)
                {
                    WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                    clickEventNum = CLICKEVENTNUM.GETSAFEDISTANCE;
                    warningDeviceCom.sendSerachWarningSafeDistanceCom();
                }
            }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Button10_Click");
            //set volume
            if (device is CardDevice)
            {
                if (deviceCom is CardDeviceCom)
                {
                    CardDeviceCom cardDeviceCom = deviceCom as CardDeviceCom;
                    clickEventNum = CLICKEVENTNUM.SETACCRANGE;
                    //cardDeviceCom.sendSetAccRange();
                    //string accRangeText = textBox4.Text;
                    try
                    {
                        byte accRange = Convert.ToByte(textBox4.Text);
                        if (accRange >= 10 && accRange <= 86)
                        {
                            cardDeviceCom.sendSetAccRange(new byte[] { accRange});
                        }
                        else
                        {
                            MessageBox.Show("Incorrect format!");
                        }
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    catch (OverflowException)
                    {
                        MessageBox.Show("Incorrect format!");
                    }
                    //cardDeviceCom.sendSearchAccRange();
                }
            }
            else if (device is WarningDevice)
            {
                if (deviceCom is WarningDeviceCom)
                {
                    WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                    clickEventNum = CLICKEVENTNUM.SETVOLUME;
                    //byte volume = trackBar1.Value;
                    warningDeviceCom.sendSetWarningVolumeCom(new byte[] { (byte)trackBar1.Value });
                }
            }
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            //read volume
            if (device is CardDevice)
            {
                if (deviceCom is CardDeviceCom)
                {
                    CardDeviceCom cardDeviceCom = deviceCom as CardDeviceCom;
                    clickEventNum = CLICKEVENTNUM.GETACCRANGE;
                    cardDeviceCom.sendSearchAccRange();
                }
            }
            else if (device is WarningDevice)
            {
                if (deviceCom is WarningDeviceCom)
                {
                    WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                    clickEventNum = CLICKEVENTNUM.GETVOLUME;
                    warningDeviceCom.sendSerachWarningVolumeCom();
                }
            }
        }

        private void Button15_Click(object sender, EventArgs e)
        {
            //remove card
            //MessageBox.Show("Button15_Click");
            int selectCount = 0;
            foreach (ListViewItem item in listView3.SelectedItems)
            {
                selectCount++;
                listView3.Items.Remove(item);
            }
            if (selectCount == 0)
            {
                MessageBox.Show("Select the object to remove");
            }
            else
            {
                if (device is WarningDevice && deviceCom is WarningDeviceCom)
                {
                    WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                    clickEventNum = CLICKEVENTNUM.REMOVEWARNCARD;
                    warningDeviceCom.sendSetWarningCardsCom(getListViewItemsToCardID(listView3));
                }
            }
            

        }


        private byte[] getListViewItemsToCardID(ListView listView)
        {
            byte[] setCardBytes = new byte[300];
            for (int i = 0; i < listView.Items.Count; i++)
            {
                string cardIDStr = listView.Items[i].Text;
                byte[] cardIDBytes = getID(cardIDStr, 3);
                setCardBytes[i * 3] = cardIDBytes[0];
                setCardBytes[i * 3 + 1] = cardIDBytes[1];
                setCardBytes[i * 3 + 2] = cardIDBytes[2];
            }
            return setCardBytes;
        }

        private void Button14_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Button14_Click");
            //Add card

            string idStr = textBox5.Text;
            byte[] id = getID(idStr, 3);
            if (id != null)
            {
                string endId = idStr.PadLeft(6, '0').Substring(2, 4);
                if ("0000" != endId && !"FFFF".Equals(endId.ToUpper()))
                {
                    if (device is WarningDevice && deviceCom is WarningDeviceCom)
                    {
                        WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                        clickEventNum = CLICKEVENTNUM.ADDWARNCARD;
                        warningDeviceCom.sendSerachWarningCardsCom();
                        //byte[] setCardBytes = new byte[300];
                        //ListViewItem listViewItem = new ListViewItem();
                        //listViewItem.Text = getID(id);
                        //listViewItem.BackColor = Color.DeepSkyBlue;
                        //listView3.Items.Add(listViewItem);
                        //for (int i = 0; i < listView3.Items.Count; i++)
                        //{
                        //    string cardIDStr = listView3.Items[i].Text;
                        //    byte[] cardIDBytes = getID(cardIDStr, 3);
                        //    setCardBytes[i * 3] = cardIDBytes[0];
                        //    setCardBytes[i * 3 + 1] = cardIDBytes[1];
                        //    setCardBytes[i * 3 + 2] = cardIDBytes[2];
                        //}
                        //warningDeviceCom.sendSetWarningCardsCom(setCardBytes);
                    }
                }
                else
                {
                    MessageBox.Show("Incorrect format!");
                }
            }
            else
            {
                MessageBox.Show("Incorrect format!");
            }
            
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Button12_Click");
            //set card
            if (device is WarningDevice && deviceCom is WarningDeviceCom)
            {
                WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                clickEventNum = CLICKEVENTNUM.SETWARNCARD;
                //warningDeviceCom.sendSetWarningCardsCom();
            }
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Button13_Click");
            //read card
            if (device is WarningDevice && deviceCom is WarningDeviceCom)
            {
                WarningDeviceCom warningDeviceCom = deviceCom as WarningDeviceCom;
                clickEventNum = CLICKEVENTNUM.GETWARNCARD;
                warningDeviceCom.sendSerachWarningCardsCom();
            }
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            listView2.Items.Clear();
            if (serialPort.IsOpen)
            {
                clickEventNum = CLICKEVENTNUM.SEARCH;
                deviceCom.sendSearchDeviceNumCom();
            }
            if ("Tag".Equals(tabControl1.SelectedTab.Text))
            {
                device = new CardDevice();
                showDevice(device);
            }
            else if ("Warn".Equals(tabControl1.SelectedTab.Text))
            {
                device = new WarningDevice();
                showDevice(device);
            }
            setDeviceCom();
        }

        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(""+ trackBar1.Value);
            label13.Text = trackBar1.Value.ToString();
        }
    }

    public enum SerialPortDataBits : int
    {
        FiveBits = 5,
        SixBits = 6,
        SevenBits = 7,
        EightBits = 8,
    }
    public enum SerialPortBaudRates : int
    {
        BaudRate_75 = 75,
        BaudRate_110 = 110,
        BaudRate_150 = 150,
        BaudRate_300 = 300,
        BaudRate_600 = 600,
        BaudRate_1200 = 1200,
        BaudRate_2400 = 2400,
        BaudRate_4800 = 4800,
        BaudRate_9600 = 9600,
        BaudRate_14400 = 14400,
        BaudRate_19200 = 19200,
        BaudRate_28800 = 28800,
        BaudRate_38400 = 38400,
        BaudRate_56000 = 56000,
        BaudRate_57600 = 57600,
        BaudRate_115200 = 115200,
        BaudRate_128000 = 128000,
        BaudRate_230400 = 230400,
        BaudRate_256000 = 256000,
    }

    public enum CLICKEVENTNUM : int
    {
        NONE,
        SEARCH,
        LISTVIEWITEMCARDCLICK,
        LISTVIEWITEMWARNCLICK,
        SEARCHCARDID,
        SETCARDID,
        GETRANGETIME,
        SETRANGETIME,
        GETCARDSTILLTIME,
        SETCARDSTILLTIME,
        GETACCRANGE,
        SETACCRANGE,
        GETCARDID,
        SEARCHWARNID,
        SETWARNID,
        GETLINKTIME,
        SETLINKTIME,
        GETSAFEDISTANCE,
        SETSAFEDISTANCE,
        GETVOLUME,
        SETVOLUME,
        GETWARNCARD,
        SETWARNCARD,
        ADDWARNCARD,
        REMOVEWARNCARD,
    }
}
