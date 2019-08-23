using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarningDeviceSet.com
{
    class WarningDeviceCom:DeviceCom
    {


        /**
         * 设置报警ID
         * 发送端       E1 + 0x40 + ID[3 byte]  + NewId[3 byte] + CS + 1E
         * 接收端       F1 + 0x40 + ID[3 byte]  + NewId[3 byte] + CS + 1F
         * */
        public void sendSetWarningIdCom(byte[] newId)
        {
            setSetCom(0x40, 6, newId);
        }

        /**
         * 查询卡片未连接报警时间
         * 发送端       E1 + 0x41 + ID[3 byte]  + CS + 1E
         * 接收端       F1 + 0x41 + ID[3 byte]  + LinkTime[2 byte] + CS + 1F
         * LinkTime，超时时间，0~65535，单位为1s
         * 0为不对未连接时间报警
         * */
        public void sendSerachWarningLinkTimeCom()
        {
            setSearchCom(0x41);
        }


        /**
        * 设置卡片未连接报警时间
        * 发送端       E1 + 0x43 + ID[3 byte]  + LinkTime[2 byte] + CS + 1E
        * 接收端       F1 + 0x43 + ID[3 byte]  + LinkTime[2 byte] + CS + 1F
        * */
        public void sendSetWarningLinkTimeCom(byte[] linkTime)
        {
            setSetCom(0x43, 5, linkTime);
        }

        /**
         * 查询安全距离
         * 发送端       E1 + 0x44 + ID[3 byte]  + CS + 1E
         * 接收端       F1 + 0x44 + ID[3 byte]  + SafeDistance[2 byte] + CS + 1F
         * SafeDistance：安全距离，0~65535，单位为cm
         * */
        public void sendSerachWarningSafeDistanceCom()
        {
            setSearchCom(0x44);
        }

        /**
         * 设置安全距离
         * 发送端       E1 + 0x45 + ID[3 byte]  + SafeDistance[2 byte] + CS + 1E
         * 接收端       F1 + 0x45 + ID[3 byte]  + SafeDistance[2 byte] + CS + 1F
         * */
        public void sendSetWarningSafeDistanceCom(byte[] SafeDistance)
        {
            setSetCom(0x45, 5, SafeDistance);
        }

        /**
         * 查询报警器音量
         * 发送端       E1 + 0x46 + ID[3 byte] + CS + 1E
         * 接收端       F1 + 0x46 + ID[3 byte] + CS + 1F
         * */
        public void sendSerachWarningVolumeCom()
        {
            setSearchCom(0x46);
        }
        /**
         * 设置报警器音量
         * 发送端       E1 + 0x47 + ID[3 byte]  + Volume[1 byte] + CS + 1E
         * 接收端       F1 + 0x47 + ID[3 byte]  + Volume[1 byte] + CS + 1F
         * 0~10,0为静音，10位最大
         * */
        public void sendSetWarningVolumeCom(byte[] Volume)
        {
            setSetCom(0x47, 4, Volume);
        }

        /**
        * 查询当前连接设备
        * 发送端       E1 + 0x48 +报警器 ID[3 byte] + CS +1E
        * 接收端       F1 + 0x48 +报警器 ID[3 byte] + 卡片ID[3*100 byte] (不足补0)+ CS +1F
        * */
        public void sendSerachWarningCardsCom()
        {
            setSearchCom(0x48);
        }

        /**
         * 设置当前连接设备
         * 发送端       E1 + 0x49 +报警器 ID [3 byte]+ 卡片ID[3*100 byte] (不足补0) +CS +1E
         * 接收端       F1 + 0x49 +报警器 ID [3 byte]+ Revflag+ CS +1F
         * Revflag为0代表接收完整，不为0代表接收错误
         * */
        public void sendSetWarningCardsCom(byte[] cards)
        {
            setSetCom(0x49, 3 * 100 + 3, cards);
        }

    }
}
