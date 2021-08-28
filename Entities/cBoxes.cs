using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GorgaTech.Entities
{
    class cBoxes
    {
        public UnityEngine.Vector3 Position { get; set; }
        public string name { get; set; }

        public static string[] BoxesNPC = {"bradley_crate", "heli_crate", "supply_drop"};
        public static string[] BoxesFood = {"crate_normal_2_food", "foodbox", "trash-pile-1", "crate_normal_2_medical"};
        public static string[] BoxesWep = {"crate_elite", "crate_normal", "crate_normal_2"};
        public static string[] BoxesShits = { "crate_mine", "crate_tools", "loot_barrel_1", "loot_barrel_2", "loot_trash", "loot-barrel-1", "loot-barrel-2", "minecart" };

        public cBoxes(ProtoBuf.Entity data)
        {
            this.Position = data.baseEntity.pos;
            this.name = ;
        }
    }
}
