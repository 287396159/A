using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarningDeviceSet.com
{
    class Com
    {

        private byte[] startCom;
        private byte[] comType;
        private byte[] comData;
        private byte[] checkCom;
        private byte[] endCom;

        public Com(byte[] startCom,byte[] comType,byte[] comData,byte[] checkCom,byte[] endCom) {
            this.startCom = startCom;
            this.comType = comType;
            this.comData = comData;
            this.checkCom = checkCom;
            this.endCom = checkCom;

        }

        public Com(byte[] startCom, byte[] comType, byte[] checkCom, byte[] endCom)
        {
            this.startCom = startCom;
            this.comType = comType;
            this.checkCom = checkCom;
            this.endCom = checkCom;
        }

        public Com() { }

        public void setStartCom(byte[] startCom)
        {
            this.startCom = startCom;
        }

        public void setComType(byte[] comType)
        {
            this.comType = comType;
        }

        public void setComData(byte[] comData)
        {
            this.comData = comData;
        }

        public void setCheckCom(byte[] checkCom)
        {
            this.checkCom = checkCom;
        }

        public void setEndCom(byte[] endCom)
        {
            this.endCom = endCom;
        }

        public byte[] getStartCom()
        {
            return startCom;
        }

        public byte[] getComType()
        {
            return comType;
        }

        public byte[] getComData()
        {
            return comData;
        }

        public byte[] getCheckCom()
        {
            return checkCom;
        }

        public byte[] getEndCom()
        {
            return endCom;
        }
    }
}
