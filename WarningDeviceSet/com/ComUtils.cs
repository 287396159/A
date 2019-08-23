using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarningDeviceSet.com
{
    class ComUtils
    {

        private Com com = new Com();

        /**格式为E1+type[1byte]+data[type]+check[1byte]+1E**/
        public void setComE1(byte type,byte[] data) {
            com.setStartCom(new byte[] { 0xE1});
            com.setComType(new byte[] { type });
            com.setComData(data);
            int checkCom = type;
            foreach(byte by in data){
                checkCom += by;
            }
            com.setCheckCom(new byte[] { (byte)checkCom });
            com.setEndCom(new byte[] { 0x1E });
        }

        

        public byte[] getComE1()
        {
            byte[] startCom = com.getStartCom();
            byte[] comType = com.getComType();
            byte[] comData = com.getComData();
            byte[] checkCom = com.getCheckCom();
            byte[] endCom = com.getEndCom();
            if (startCom == null|| endCom==null|| comType==null||checkCom==null)
            {
                return null;
            }
            bool isOK = false;
            if (startCom.Length==1&& startCom[0] == 0xE1&&endCom.Length==1&&endCom[0]==0x1E&&comType.Length==1 && checkCom.Length == 1)
            {
                int checkSum = comType[0];
                foreach (byte by in comData)
                {
                    checkSum += by;
                }
                if ((byte)checkSum == checkCom[0])
                {
                    isOK = true;
                }
            }
            if (isOK)
            {
                byte[] comBytes = new byte[startCom.Length + comType.Length + comData.Length + checkCom.Length + endCom.Length];
                int i = 0;
                for (; i < startCom.Length; i++)
                {
                    comBytes[i] = startCom[i];
                }
                for (; i < startCom.Length + comType.Length; i++)
                {
                    comBytes[i] = comType[i-startCom.Length];
                }

                for (; i < startCom.Length + comType.Length + comData.Length; i++)
                {
                    comBytes[i] = comData[i - startCom.Length- comType.Length];
                }

                for (; i < startCom.Length + comType.Length + comData.Length + checkCom.Length; i++)
                {
                    comBytes[i] = checkCom[i - startCom.Length - comType.Length - comData.Length];
                }

                for (; i < startCom.Length + comType.Length + comData.Length + checkCom.Length + endCom.Length; i++)
                {
                    comBytes[i] = endCom[i - startCom.Length - comType.Length - comData.Length - checkCom.Length];
                }

                return comBytes;
            }
            return null;
        }
    }
}
