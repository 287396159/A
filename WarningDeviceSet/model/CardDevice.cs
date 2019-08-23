using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarningDeviceSet.model
{
    class CardDevice : Differentiable
    {
        private byte rangeTime;
        private byte[] stillTime = new byte[2];
        private byte accRange;

        public void setRangeTime(byte rangeTime) {
            this.rangeTime = rangeTime;
        }

        public byte getRangeTime()
        {
            return rangeTime;
        }


        public void setStillTime(byte[] stillTime)
        {
            this.stillTime = stillTime;
        }

        public byte[] getStillTime()
        {
            return stillTime;
        }

        public void setAccRange(byte accRange)
        {
            this.accRange = accRange;
        }

        public byte getAccRange()
        {
            return accRange;
        }

    }
}
