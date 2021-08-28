using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GorgaTech.Entities
{
    class cPlayers
    {
        public static Vector3 localPosition { get; set; }
        public static Vector3 localCamera { get; set; }

        public Vector3 Position {get; set;}
        public Vector3 Rotation { get; set; }
        public string name { get; set; }
        public List<ProtoBuf.Item> Items;
        public bool isSleeping = true;
        public bool isWounded = false;
        public float health { get; set; }
        public bool isLocalPlayer = false;
        
        public cPlayers(ProtoBuf.Entity data){
            this.Position = data.baseEntity.pos;
            this.Rotation = data.baseEntity.rot;
            this.name = data.basePlayer.name;
            this.Items = data.basePlayer.inventory.invBelt.contents;
            this.health = data.baseCombat.health;
            this.isSleeping = data.basePlayer.modelState.sleeping;
            this.isWounded = (data.basePlayer.playerFlags & 64) == 64 ? true : false;
            if (data.basePlayer.metabolism != null)
            {
                this.isLocalPlayer = true;
                localPosition = this.Position;
                localCamera = this.Rotation;
            }
        }

        public void Update(ProtoBuf.Entity data)
        {
            //this.Position = data.baseEntity.pos;
            //this.Rotation = data.baseEntity.rot;
            this.name = data.basePlayer.name;
            this.Items = data.basePlayer.inventory.invBelt.contents;
            this.health = data.baseCombat.health;
            this.isSleeping = data.basePlayer.modelState.sleeping;
            this.isWounded = (data.basePlayer.playerFlags & 64) == 64 ? true : false;
            if (data.basePlayer.metabolism != null)
            {
                this.isLocalPlayer = true;
                localPosition = this.Position;
                localCamera = this.Rotation;
            }
        }
    }
}
