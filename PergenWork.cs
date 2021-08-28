using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;



namespace GorgaTech
{
    public class PergenWork
    {
        public static string[] nicks = Form1.settings.Nicks;
        public static string[] enemys = Form1.settings.Enemys;

        internal static void ReadRustPackets()
        {

            //string ip = "149.202.88.62";
            string ip = Form1.settings.IPport;
            string[] address = ip.Split(':');
            GorgaMem rusti;
            int port = 28026;
            Int32.TryParse(address[1], out port);
            ip = address[0];

            //MessageBox.Show(ip + port);
           

            rusti = new GorgaMem(server: ip, port: port);
            rusti.ClientPackets = false;
            rusti.RememberPackets = false;
            Packet packet;
            
            rusti.Start();
            while (rusti.IsAlive)
            {
                rusti.GetPacket(out packet);
                
                    switch ((Packet.Rust)packet.packetID)
                    {
                        case Packet.Rust.Entities:
                            Data.Entity.CreateOrUpdate(packet);
                            break;
                        case Packet.Rust.EntityPosition:
                            Data.Entity.UpdatePosition(packet);
                            break;
                        case Packet.Rust.EntityDestroy:
                            Data.EntityDestroy.Destroy(packet);
                            break;
                        /*case Packet.Rust.Tick:
                            Data.Tick.UpdateTick(packet);
                            break;*/
                    }
                
            }
        }

        internal static void DrawForm()
        {
            //test.ShowDialog();
            RadarRenderDX radar = new RadarRenderDX();
            
        }
    }
}
