namespace GorgaTech.Data
{
	public class EntityDestroy
    {
        internal static uint uid;
		public uint UID { get{ return uid; } }
		internal static byte destroyMode;
		public byte DestroyMode { get{ return destroyMode; } }

		public static void Destroy(Packet p) {
			uid = p.UInt32();
            destroyMode = p.UInt8();
            if (Entity.CheckPlayer(uid))
            {
                lock (RadarRenderDX.players)
                {
                    try
                    {
                        RadarRenderDX.players.Remove(uid);
                    }
                    catch { }
                }
            }
            if (Entity.CheckStones(uid)!=-1)
            {
                lock (RadarRenderDX.stones)
                {
                    try
                    {
                        RadarRenderDX.stones[Entity.CheckStones(uid)].Remove(uid);
                        /*foreach (var stone in RadarRenderDX.stones)
                        {
                            if(stone.ContainsKey(uid))
                                stone.Remove(uid);
                        }*/
                    }
                    catch { }
                }
            }
            if (Entity.CheckPlant(uid))
            {
                lock (RadarRenderDX.plants)
                {
                    try
                    {
                    RadarRenderDX.plants.Remove(uid);
                        }
                    catch { }
                }
            }
            if (Entity.CheckCorpse(uid))
            {
                lock (RadarRenderDX.lootcorpses)
                {
                    try
                    {
                    RadarRenderDX.lootcorpses.Remove(uid);}
                    catch { }
                }
            }
            if (Entity.CheckAnimal(uid))
            {
                lock (RadarRenderDX.animals)
                {
                    try
                    {
                    RadarRenderDX.animals.Remove(uid);}
                    catch { }
                }
            }
            if (Entity.CheckWeapon(uid))
            {
                lock (RadarRenderDX.weapons)
                {
                    try
                    {
                    RadarRenderDX.weapons.Remove(uid);}
                    catch { }
                }
            }
            if (Entity.CheckAutoturret(uid))
            {
                lock (RadarRenderDX.autoturret)
                {
                    try
                    {
                        RadarRenderDX.autoturret.Remove(uid);
                    }
                    catch { }
                }
            }
            if (Entity.CheckLootboxes(uid))
            {
                lock (RadarRenderDX.lootboxes)
                {
                    try
                    {
                    RadarRenderDX.lootboxes.Remove(uid);}
                    catch { }
                }
            }
            if (Entity.CheckRaidboxes(uid))
            {
                lock (RadarRenderDX.raidboxes)
                {
                    try
                    {
                        RadarRenderDX.raidboxes.Remove(uid);
                    }
                    catch { }
                }
            }
            if (Entity.CheckRTloot(uid))
            {
                lock (RadarRenderDX.rtloot)
                {
                    try
                    {
                    RadarRenderDX.rtloot.Remove(uid);}
                    catch { }
                }
            }
            /*if (Entity.CheckSmallStashes(uid))
            {
                lock (RadarRenderDX.smallstash)
                {
                    RadarRenderDX.smallstash.Remove(uid);
                }
            }*/
            if (Entity.CheckDecayCorpse(uid))
            {
                lock (RadarRenderDX.decaycorpse)
                {
                    try
                    {
                        RadarRenderDX.decaycorpse.Remove(uid);
                    }
                    catch { }
                }
            }
		}
	}
}
