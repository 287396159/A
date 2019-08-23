using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarningDeviceSet.model
{
    class WarningDevice : Differentiable
    {
        private byte[] linkTime = new byte[2];
        private byte[] safeDistance = new byte[2];
        private byte volume;
        private byte[] cards = new byte[300];

        public void setLinkTime(byte[] linkTime)
        {
            this.linkTime = linkTime;
        }

        public byte[] getLinkTime()
        {
            return linkTime;
        }

        public void setSafeDistance(byte[] safeDistance)
        {
            this.safeDistance = safeDistance;
        }

        public byte[] getSafeDistance()
        {
            return safeDistance;
        }

        public void setVolume(byte volume)
        {
            this.volume = volume;
        }

        public byte getRangeTime()
        {
            return volume;
        }

        public void setCards(byte[] cards)
        {
            this.cards = cards;
        }

        public byte[] getCards()
        {
            return cards;
        }


    }
}
