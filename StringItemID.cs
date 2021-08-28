using System.Collections.Generic;

namespace GorgaTech {
    class StringItemID {

        public Dictionary<int, string> idToString = new Dictionary<int, string>();

        public string Get(int id) {
            string name = "";
            if (idToString.TryGetValue(id, out name))
            {
                return name;
            }
            else
                return null;
            
        }

        public void Fill() {
            idToString.Add(109552593,"SMG");
            idToString.Add(-2094080303,"MP5");
            idToString.Add(456448245,"Tomek");
            idToString.Add(-1461508848,"AK");
            idToString.Add(-55660037,"Bolt");
            idToString.Add(-1716193401,"LR-300");
            idToString.Add(-1745053053,"SR");
            idToString.Add(-1379225193,"Eoka");
            idToString.Add(371156815,"P3");
            idToString.Add(2033918259,"P RV");
            idToString.Add(-930579334,"RV");
            idToString.Add(548699316,"P2");
            idToString.Add(193190034,"M249");
            idToString.Add(191795897,"2x ShotG");
            idToString.Add(-1009492144,"6x ShotG");
            idToString.Add(2077983581,"1x ShotG");
            idToString.Add(-853695669, "B");
            idToString.Add(2123300234, "CB");
            idToString.Add(-2118132208, "W");
            idToString.Add(-1127699509, "W");
            idToString.Add(649603450, "RL");
            idToString.Add(1045869440, "Flame");
        }

    }
}
