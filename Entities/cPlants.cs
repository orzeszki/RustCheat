using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GorgaTech.Entities
{
    class cPlants
    {
        public Vector3 Position { get; set; }

        public cPlants(ProtoBuf.Entity data)
        {
            this.Position = data.baseEntity.pos;
         
        }
        public void Update(ProtoBuf.Entity data)
        {
            this.Position = data.baseEntity.pos;
            
        }
    }
}
