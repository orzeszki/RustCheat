using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GorgaTech.Entities
{
    class cLootcorpse
    {
        public Vector3 Position { get; set; }
        public bool isBackpack { get; set; }
        public ulong ID { get; set; }

        public cLootcorpse(ProtoBuf.Entity data, bool isBackpack)
        {
            this.Position = data.baseEntity.pos;
            this.isBackpack = isBackpack;
            this.ID = data.lootableCorpse.playerID;
        }

    }
}
