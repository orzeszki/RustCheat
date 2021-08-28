using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GorgaTech.Entities
{
    class cAnimals
    {
        //List<string> names = new List<string> { "wolf", "bear"};
        public Vector3 Position {get; set;}
        public bool isDangerous { get; set; }
        public byte Type { get; set; }

        byte WhatAnimal(string name)
        {
            foreach (var anim in Enum.GetNames(typeof(Animal)))
            {
                if (name.Contains(anim.ToLower()))
                {
                    return (byte)Enum.Parse(typeof(Animal), anim);
                }
            }
            return (byte)Animal.Unknown;

            /*if (name.Contains("bear"))
                return (byte)Animal.Bear;
            if(name.Contains("wolf"))
                return (byte)Animal.Wolf;
            if(name.Contains("boar"))
                return (byte)Animal.Boar;
            if(name.Contains("horse"))
                return (byte)Animal.Horse;
            if(name.Contains("stag"))
                return (byte)Animal.Stag;
            if (name.Contains("chicken"))
                return (byte)Animal.Chicken;
            
            return (byte)Animal.Unknown;*/
        }

        bool Dangerous(byte _type)
        {
            if (_type == (byte)Animal.Bear || _type == (byte)Animal.Wolf)
                return true;
            return false;
        }

        public cAnimals(ProtoBuf.Entity data){
            this.Position = data.baseEntity.pos;
            this.Type = WhatAnimal(RadarRenderDX.prefab_names.Get(data.baseNetworkable.prefabID));
            this.isDangerous = Dangerous(this.Type);
        }

        public enum Animal:byte
        {
            Bear = 0,
            Wolf,
            Boar,
            Horse,
            Stag,
            Chicken,
            Unknown
        }
    }
}
