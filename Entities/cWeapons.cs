using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GorgaTech.Entities
{
    class cWeapons
    {
        public Vector3 Position { get; set; }
        public string name { get; set; }
        //public bool isAirdrop { get; set; }

        public cWeapons(ProtoBuf.Entity data, bool isWeapon){
            this.Position = data.baseEntity.pos;
            this.name = isWeapon ? RadarRenderDX.items_names.Get(data.worldItem.item.itemid) : "Z";
            //this.isAirdrop = isAirdrop;
        }
        public void Update(ProtoBuf.Entity data)
        {
            if(data.baseEntity.pos !=null)
                this.Position = data.baseEntity.pos;
        }
    }
}
