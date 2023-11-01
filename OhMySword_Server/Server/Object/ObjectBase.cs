using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public abstract class ObjectBase
    {
        public ClientSession session;
        public ushort objectID;
        public Vector3 position;
        public Vector3 rotation;
    }
}
