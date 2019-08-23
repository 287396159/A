using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarningDeviceSet.model;

namespace WarningDeviceSet.com
{
    class CardDeviceCom:DeviceCom
    {

        public CardDeviceCom()
        {
            setDevice(new CardDevice());
        }


        

        //public void sendSearchDeviceNumCom()
        //{
        //    setCom(0x00, new byte[0]);
        //}

        /**
         * 设置卡片ID
         * 发送端       E1 + 0x10 + ID[3 byte] + NewId[3 byte] + CS + 1E
         * 接收端       F1 + 0x10 + ID[3 byte] + NewId[3 byte] + CS + 1F
         * */
        public void sendSetCardIdCom(byte[] newId)
        {
            setSetCom(0x10, 6, newId);
        }
        /**
        * 显示卡片
        * 发送端       E1 + 0x18 + ID[3 byte] + CS + 1E
        * 接收端       F1 + 0x18 + ID[3 byte] + CS + 1F
        * */
        public void sendShowCardDeviceCom()
        {
            setSearchCom(0x18);
            //setCom(0x18, getDevice().getId());
        }

        /**
        * 查询测距时间
        * 发送端       E1 + 0x12 + ID[3 byte] + CS + 1E
        * 接收端       F1 + 0x12 + ID[3 byte] + RangeTime[1 byte] + CS + 1F
        * rangetime :间隔时间   单位为1s
        * */
        public void sendSearchRangeTime()
        {
            setSearchCom(0x12);
        }

        /**
        * 设置测距时间 
        * 发送端       E1 + 0x13 + ID[3 byte] + RangeTime[1 byte] + CS + 1E
        * 接收端       F1 + 0x13 + ID[3 byte] + RangeTime[1 byte] + CS + 1F
        * */
        public void sendSetRangeTime(byte[] rangeTime)
        {
            setSetCom(0x13, 4, rangeTime);
            //setCom(0x12, getData(4, rangeTime));
        }


        /**
        * 查询卡片静止不动报警时间 
        * 发送端       E1 + 0x14 + ID[3 byte] + CS + 1E
        * 接收端       F1 + 0x14 + ID[3 byte] + CardStillTime[2 byte] + CS + 1F
        * CardStillTime：卡片静止不动报警时间
        * */
        public void sendSearchStillTime()
        {
            setSearchCom(0x14);
        }

        /**
        * 设置卡片静止不动报警时间 
        * 发送端       E1 + 0x15 + ID[3 byte] + CardStillTime[2 byte] + CS + 1E
        * 接收端       F1 + 0x15 + ID[3 byte] + CardStillTime[2 byte] + CS + 1F
        * */
        public void sendSetStillTime(byte[] cardStillTime)
        {
            setSetCom(0x15, 5, cardStillTime);
            //setCom(0x15, getData(5, cardStillTime));
        }

        /**
        * 查询加速度阙值报警 
        * 发送端       E1 + 0x16 + ID[3 byte]  + CS + 1E
        * 接收端       F1 + 0x16 + ID[3 byte]  + AccRange[1 byte] + CS + 1F
        * AccRange单位为186mg，0~127
        * */
        public void sendSearchAccRange()
        {
            setSearchCom(0x16);
        }

        /**
        * 设置加速度阙值报警 
        * 发送端       E1 + 0x17 + ID[3 byte]  + AccRange[1 byte] + CS + 1E
        * 接收端       F1 + 0x17 + ID[3 byte]  + AccRange[1 byte] + CS + 1F
        * */

        public void sendSetAccRange(byte[] accRange)
        {
            setSetCom(0x17, 4, accRange);
            //setCom(0x17, getData(4, accRange));
        }
    }
}
