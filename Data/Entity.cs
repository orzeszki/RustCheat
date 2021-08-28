using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ProtoBuf;
using GorgaTech.Entities;
namespace GorgaTech.Data
{

    public class Entity
    {

        public enum Stone:byte{
            stone=0,
            metal=1,
            sulfur=2
        }
        private static void PlayerSound()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = @"c:\Windows\Media\tada.wav";
            player.Load();
            player.Play();
        }

        public static bool CheckPlayer(ulong id)
        {
            try
            {
                lock (RadarRenderDX.players)
                {
                    return RadarRenderDX.players.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static int CheckStones(ulong id)
        {
                lock (RadarRenderDX.stones)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            if (RadarRenderDX.stones[i].ContainsKey(id))
                                return i;
                        }
                        catch (Exception ex) { System.Windows.Forms.MessageBox.Show("Check stones: "+ex.Message); }
                        /*foreach(var item in RadarRenderDX.stones[i])
                        {
                            if (item.Key == id)
                            {
                                return i;
                            }
                        }*/
                        
                    }
                    return -1;
                }
            
        }

        public static bool CheckPlant(ulong id)
        {
            try
            {
                lock (RadarRenderDX.plants)
                {
                    return RadarRenderDX.plants.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckCorpse(ulong id)
        {
            try
            {
                lock (RadarRenderDX.lootcorpses)
                {
                    return RadarRenderDX.lootcorpses.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckWeapon(ulong id)
        {
            try
            {
                lock (RadarRenderDX.weapons)
                {
                    return RadarRenderDX.weapons.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckAutoturret(ulong id)
        {
            try
            {
                lock (RadarRenderDX.autoturret)
                {
                    return RadarRenderDX.autoturret.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckAnimal(ulong id)
        {
            try
            {
                lock (RadarRenderDX.animals)
                {
                    return RadarRenderDX.animals.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckDecayCorpse(ulong id)
        {
            try
            {
                lock (RadarRenderDX.decaycorpse)
                {
                    return RadarRenderDX.decaycorpse.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckLootboxes(ulong id)
        {
            try
            {
                lock (RadarRenderDX.lootboxes)
                {
                    return RadarRenderDX.lootboxes.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckRaidboxes(ulong id)
        {
            try
            {
                lock (RadarRenderDX.raidboxes)
                {
                    return RadarRenderDX.raidboxes.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckRTloot(ulong id)
        {
            try
            {
                lock (RadarRenderDX.rtloot)
                {
                    return RadarRenderDX.rtloot.ContainsKey(id);
                }
            }
            catch
            {
                return false;
            }
        }

        public static void UpdateCorpse(ulong id)
        {
            lock (RadarRenderDX.decaycorpse)
            {
                TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks);
                try
                {
                    if (CheckDecayCorpse(id))
                        RadarRenderDX.decaycorpse[id] = elapsedSpan;
                    else
                        RadarRenderDX.decaycorpse.Add(id, elapsedSpan);
                }
                catch { }
            }
        }

        public static void UpdatePosition(Packet p)
        {
            
            /* EntityPosition packets may contain multiple positions */
            while (p.unread >= 28L)
            {
                /* Entity UID */
                var id = p.EntityID();
                /* Read 2 Vector3 in form of 3 floats each, Position and Rotation */
                Vector3 Position = new Vector3(p.Float(), p.Float(), p.Float());
                Vector3 Rotation = new Vector3(p.Float(), p.Float(), p.Float());
                

                if (CheckPlayer(id))
                {
                    lock (RadarRenderDX.players)
                    {
                        RadarRenderDX.players[id].Position = Position;
                        RadarRenderDX.players[id].Rotation = Rotation;

                        if (RadarRenderDX.players[id].isLocalPlayer)
                        {
                            cPlayers.localPosition = Position;
                            cPlayers.localCamera = Rotation;
                        }
                    }

                    UpdateCorpse(id);
                    
                }
                if (CheckAnimal(id))
                {
                    lock (RadarRenderDX.animals)
                    {
                        try
                        {
                            RadarRenderDX.animals[id].Position = Position;
                        }
                        catch { }
                    }
                }

                
                if (CheckStones(id) != -1)
                {
                    lock (RadarRenderDX.stones)
                    {
                        try{
                            RadarRenderDX.stones[CheckStones(id)][id].Set(Position.x, Position.y, Position.z);
                        }
                        catch { }
                    }
                }

                if (CheckCorpse(id))
                {
                    lock (RadarRenderDX.lootcorpses)
                    {
                        try{
                            RadarRenderDX.lootcorpses[id].Position = Position;
                        }
                        catch { }
                    }
                }
                if (CheckPlant(id))
                {
                    lock (RadarRenderDX.plants)
                    {
                        try{
                            RadarRenderDX.plants[id].Position = Position;
                        }
                        catch { }
                    }
                }

                if (CheckWeapon(id))
                {
                    lock (RadarRenderDX.weapons)
                    {
                        try
                        {
                            RadarRenderDX.weapons[id].Position = Position;
                        }
                        catch { }
                    }
                }

                
                if (CheckLootboxes(id))
                {
                    lock (RadarRenderDX.lootboxes)
                    {
                        try{
                            RadarRenderDX.lootboxes[id].Set(Position.x, Position.y, Position.z);
                        }
                        catch { }
                    }
                }
                if (CheckAutoturret(id))
                {
                    lock (RadarRenderDX.autoturret)
                    {
                        try
                        {
                            RadarRenderDX.autoturret[id].Set(Position.x, Position.y, Position.z);
                        }
                        catch { }
                    }
                }
                if (CheckRaidboxes(id))
                {
                    lock (RadarRenderDX.raidboxes)
                    {
                        try
                        {
                            RadarRenderDX.raidboxes[id].Set(Position.x, Position.y, Position.z);
                        }
                        catch { }
                    }
                }
                if (CheckRTloot(id))
                {
                    lock (RadarRenderDX.rtloot)
                    {
                        try{
                            RadarRenderDX.rtloot[id].Set(Position.x, Position.y, Position.z);
                        }
                        catch { }
                    }
                }
               /* if (CheckSmallStashes(id))
                {
                    lock (RadarRenderDX.smallstash)
                    {
                        RadarRenderDX.smallstash[id].Set(Postion.x, Postion.y, Postion.z);
                    }
                }*/




            }

        }

        public static void CreateOrUpdate(Packet p)
        {
            /* Entity Number/Order, for internal use */
            var num = p.UInt32();
            
            //uint uid = proto.baseNetworkable.uid;
            ProtoBuf.Entity proto = null;
            
            try
            {
                proto = global::ProtoBuf.Entity.Deserialize(p);
                //global::ProtoBuf.Entity.Serialize(p, proto);
            }
            catch { }

            if (proto != null)
            {
                var id_entity = proto.baseNetworkable.uid;
                var ent_prefab_id = proto.baseNetworkable.prefabID;

                

                try
                {
                    var player = proto.basePlayer;

                    if (player != null)
                    {
                        lock (RadarRenderDX.players)
                        {

                            if (CheckPlayer(id_entity))
                                RadarRenderDX.players[id_entity].Update(proto);
                            else
                                RadarRenderDX.players.Add(id_entity, new cPlayers(proto));
                        }
                        UpdateCorpse(id_entity);
                    }
                }
                catch (Exception ex) { System.Windows.Forms.MessageBox.Show(ex.Message); }

                try
                {
                    var animal = proto.baseNPC;

                    if (animal != null)
                    {

                        lock (RadarRenderDX.animals)
                        {
                            if (!CheckAnimal(id_entity))
                                RadarRenderDX.animals.Add(id_entity, new cAnimals(proto));
                            else
                                RadarRenderDX.animals[id_entity] = new cAnimals(proto);
                        }
                    }
                }
                catch { }

                try
                {
                    //var lootcorpse = proto.lootableCorpse;
                    var nazwa = RadarRenderDX.prefab_names.Get(ent_prefab_id);
                    if (nazwa.Contains("misc/item drop/item_drop"))
                    {
                        /*SZUKANIE BACKPACKOW*/
                        lock (RadarRenderDX.lootcorpses)
                        {
                            if (!CheckCorpse(id_entity))
                            {
                                RadarRenderDX.lootcorpses.Add(id_entity, new cLootcorpse(proto, true));
                            }

                        }
                    }
                }
                catch { }

                try
                {
                    var nazwa = RadarRenderDX.prefab_names.Get(ent_prefab_id);
                    if (nazwa.Contains("radtown/loot") || nazwa.Contains("autospawn/resource/loot/"))
                    {
                        lock (RadarRenderDX.lootboxes)
                        {

                            if (!CheckLootboxes(id_entity))
                            {
                                RadarRenderDX.lootboxes.Add(id_entity, proto.baseEntity.pos);

                            }
                        }
                    }
                }
                catch { }

                try
                {
                    if (proto.autoturret != null)
                    {
                        lock (RadarRenderDX.autoturret)
                        {

                            if (!CheckAutoturret(id_entity))
                            {
                                RadarRenderDX.autoturret.Add(id_entity, proto.baseEntity.pos);
                                //Console.WriteLine(nazwa);

                            }
                        }
                    }
                }
                catch { }  

                try
                {
                    var nazwa = RadarRenderDX.prefab_names.Get(ent_prefab_id);
                    if (nazwa.Contains("radtown/crate") || nazwa.Contains("radtown/dmloot"))
                    {
                        lock (RadarRenderDX.rtloot)
                        {

                            if (!CheckRTloot(id_entity))
                            {
                                RadarRenderDX.rtloot.Add(id_entity, proto.baseEntity.pos);
                                //Console.WriteLine(nazwa);

                            }
                        }
                    }
                }
                catch { }

                /*if (animal != null)
                {
                    lock (RadarRenderDX.animals)
                    {
                        if (!CheckAnimal(id_entity))
                            RadarRenderDX.animals.Add(id_entity, proto);
                        else
                            RadarRenderDX.animals[id_entity] = proto;
                    }
                }*/

                try
                {
                    var spawner = proto.resource;
                    var nazwa = RadarRenderDX.prefab_names.Get(ent_prefab_id);
                    if (spawner != null)
                    {
                        lock (RadarRenderDX.stones)
                        {
                            if (CheckStones(id_entity)==-1)
                            {
                                if(nazwa.Contains("metal"))
                                    RadarRenderDX.stones[(int)Stone.metal].Add(id_entity, proto.baseEntity.pos);
                                else if(nazwa.Contains("sulfur"))
                                    RadarRenderDX.stones[(int)Stone.sulfur].Add(id_entity, proto.baseEntity.pos);
                                else if(nazwa.Contains("stone"))
                                    RadarRenderDX.stones[(int)Stone.stone].Add(id_entity, proto.baseEntity.pos);
                            }
                        }
                    }
                }
                catch{  }

                try
                {
                    var lootcorpse = proto.lootableCorpse;
                    if (lootcorpse != null)
                    {
                        lock (RadarRenderDX.lootcorpses)
                        {
                            if (!CheckPlayer(id_entity) && !CheckCorpse(id_entity))
                            {
                                RadarRenderDX.lootcorpses.Add(id_entity, new cLootcorpse(proto, false));

                                TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks);
                                RadarRenderDX.decaycorpse.Add(lootcorpse.playerID, elapsedSpan);
                            }

                        }
                    }
                }
                catch { }
                /*else if (proto.baseEntity.flags == 2048)
                {
                    lock (RadarRenderDX.smallstash)
                    {

                        if (!CheckSmallStashes(id_entity))
                        {
                            RadarRenderDX.smallstash.Add(id_entity, proto.baseEntity.pos);
                            //Console.WriteLine(nazwa);

                        }
                    }
                }*/
                try
                {
                    var nazwa = RadarRenderDX.prefab_names.Get(ent_prefab_id);
                    lock (RadarRenderDX.plants)
                    {
                        if (!CheckPlant(id_entity))
                        {
                            if (nazwa.Contains("autospawn/collectable/hemp"))
                                RadarRenderDX.plants.Add(id_entity, new cPlants(proto));
                            else if (nazwa.Contains("hemp"))
                            {
                                if (proto.plantEntity.state > 2 && proto.plantEntity.state < 5)
                                    RadarRenderDX.plants.Add(id_entity, new cPlants(proto));
                            }
                        }
                        else
                            RadarRenderDX.plants[id_entity].Update(proto);

                    }
                }
                catch { }

                try
                {
                    var wep = proto.worldItem;
                    var nazwa = RadarRenderDX.prefab_names.Get(ent_prefab_id);
                    lock (RadarRenderDX.weapons)
                    {
                        if (!CheckWeapon(id_entity))
                        {
                            if (RadarRenderDX.items_names.Get(wep.item.itemid) != null)
                            {
                                RadarRenderDX.weapons.Add(id_entity, new cWeapons(proto, true));
                                if (wep.item.itemid == -1716193401 || wep.item.itemid == 193190034 || wep.item.itemid == -1461508848)
                                    PlayerSound();
                            }
                            else if (nazwa.Contains("supply drop/supply_drop.prefab"))
                            {
                                RadarRenderDX.weapons.Add(id_entity, new cWeapons(proto, false));
                            }
                        }
                        else
                        {
                            RadarRenderDX.weapons[id_entity].Update(proto);
                        }
                    }
                }
                catch { }

                try
                {
                    var nazwa = RadarRenderDX.prefab_names.Get(ent_prefab_id);
                   
                    lock (RadarRenderDX.raidboxes)
                    {
                        if (!CheckRaidboxes(id_entity))
                        {
                            if (nazwa.Contains("box.wooden.large"))
                            {
                                RadarRenderDX.raidboxes.Add(id_entity, proto.baseEntity.pos);
                            }
                        }
                    }
                }
                catch { }
                // Console.WriteLine(uid);
            }
        }

    }
}