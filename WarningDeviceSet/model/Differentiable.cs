using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarningDeviceSet.model
{
    abstract class Differentiable
    {

        private byte[] id=new byte[3];

        public byte[] getId() {
            return this.id;
        }

        public void setId(byte[] id) {
            this.id = id;
        }

        

    }
}
