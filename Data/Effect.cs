using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GorgaTech.Data
{
    class Effect
    {
        public static void ReceivedEffect(Packet p)
        {
            EffectData cos = global::EffectData.Deserialize(p);

            try{

                Console.WriteLine("Normal: "+cos.normal.x+" | Origin: "+cos.origin.x);
                //StringPool.Get(cos.pooledstringid);
            }catch{}
        }
    }
}
