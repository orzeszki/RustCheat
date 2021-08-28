using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using SharpDX.Direct3D;
using SharpDX.Direct2D1;
using System.Text;
using System.Windows.Forms;
using SharpDX.DXGI;
using SharpDX;
using SharpDX.Windows;
using System.Globalization;
using UnityEngine;


using SharpDX.DirectWrite;
using System.Collections.Generic;
using GorgaTech.Data;
using GorgaTech.Entities;
using SharpDX.Mathematics.Interop;

namespace GorgaTech
{
    class RadarRenderDX
    {
        public static float zoom = 1.0f;
        public static float moja_poz_x = 0;
        public static float moja_poz_y = 0;
        public static double moja_poz_oko = 0;
        public static Dictionary<ulong, cPlayers> players = new Dictionary<ulong, cPlayers>();
        //public static Dictionary<ulong, UnityEngine.Vector3> stones = new Dictionary<ulong, UnityEngine.Vector3>();
        public static Dictionary<ulong, UnityEngine.Vector3>[] stones = new Dictionary<ulong, UnityEngine.Vector3>[] 
        {
            new Dictionary<ulong, UnityEngine.Vector3>(),
            new Dictionary<ulong, UnityEngine.Vector3>(),
            new Dictionary<ulong, UnityEngine.Vector3>()
        };
        public static Dictionary<ulong, cPlants> plants = new Dictionary<ulong, cPlants>();
        public static Dictionary<ulong, cLootcorpse> lootcorpses = new Dictionary<ulong, cLootcorpse>();
        public static Dictionary<ulong, cAnimals> animals = new Dictionary<ulong, cAnimals>();
        public static Dictionary<ulong, cWeapons> weapons = new Dictionary<ulong, cWeapons>();
        public static Dictionary<ulong, UnityEngine.Vector3> lootboxes = new Dictionary<ulong, UnityEngine.Vector3>();
        public static Dictionary<ulong, UnityEngine.Vector3> raidboxes = new Dictionary<ulong, UnityEngine.Vector3>();
        public static Dictionary<ulong, UnityEngine.Vector3> autoturret = new Dictionary<ulong, UnityEngine.Vector3>();
       // public static Dictionary<ulong, UnityEngine.Vector3> smallstash = new Dictionary<ulong, UnityEngine.Vector3>();
        public static Dictionary<ulong, UnityEngine.Vector3> rtloot = new Dictionary<ulong, UnityEngine.Vector3>();
        public static Dictionary<ulong, TimeSpan> decaycorpse = new Dictionary<ulong, TimeSpan>();
        public static GorgaTech.StringPool prefab_names = new GorgaTech.StringPool();
        public static StringItemID items_names = new StringItemID();

        WindowRenderTarget wndRender = null;
        SharpDX.Direct2D1.Factory fact = new SharpDX.Direct2D1.Factory(SharpDX.Direct2D1.FactoryType.SingleThreaded);
        SharpDX.DirectWrite.Factory fact_txt = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);
        private SolidColorBrush solidcolorbrush;
        RenderTargetProperties rndTargProperties;
        HwndRenderTargetProperties hwndProperties;
        public static RadarForm form = new RadarForm();
        SharpDX.Mathematics.Interop.RawVector2 l1_point1 = new SharpDX.Mathematics.Interop.RawVector2(150f, 0f);
        SharpDX.Mathematics.Interop.RawVector2 l1_point2 = new SharpDX.Mathematics.Interop.RawVector2(150f, 300f);

        SharpDX.Mathematics.Interop.RawVector2 l2_point1 = new SharpDX.Mathematics.Interop.RawVector2(0f, 150f);
        SharpDX.Mathematics.Interop.RawVector2 l2_point2 = new SharpDX.Mathematics.Interop.RawVector2(300f, 150f);
        RenderLoop.RenderCallback callback;

        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        public RadarRenderDX()
        {
                prefab_names.Fill();
                items_names.Fill();
                test();
                solidcolorbrush = new SolidColorBrush(wndRender, SharpDX.Color.White);
                callback = new RenderLoop.RenderCallback(Render);

                RenderLoop.Run(form, callback);
        }

        private void test()
        {
            rndTargProperties = new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, SharpDX.Direct2D1.AlphaMode.Premultiplied));
 
            hwndProperties = new HwndRenderTargetProperties();
            hwndProperties.Hwnd = form.Handle;
            hwndProperties.PixelSize = new SharpDX.Size2(form.ClientSize.Width, form.ClientSize.Height);
            hwndProperties.PresentOptions = PresentOptions.None;
            wndRender = new WindowRenderTarget(fact, rndTargProperties, hwndProperties);
           // SharpDX.Mathematics.Interop.RawColor4 Colors = new SharpDX.Mathematics.Interop.RawColor4(0f, 0f, 0f, 1f);
            form.Show();
        }

        public static UnityEngine.Vector3 GetEntityPosition(UnityEngine.Vector3 ent)
        {
             float odl_x = (ent.x - moja_poz_x);
             float odl_y = (ent.z - moja_poz_y);
             double odl = Math.Round(Math.Sqrt(Math.Pow(odl_x, 2) + Math.Pow(odl_y, 2)));

            double wsp_x = (odl_x * Math.Cos(DegreeToRadian((moja_poz_oko)))) - (odl_y * Math.Sin(DegreeToRadian((moja_poz_oko))));
            double wsp_y = (odl_x * Math.Sin(DegreeToRadian((moja_poz_oko)))) + (odl_y * Math.Cos(DegreeToRadian((moja_poz_oko))));

            wsp_x *= zoom;
            wsp_y *= zoom;

            return new UnityEngine.Vector3((float)wsp_x, (float)wsp_y, (float)odl);
        }

        public static UnityEngine.Vector3 RevertYZ(UnityEngine.Vector3 ent)
        {
            return new UnityEngine.Vector3(ent.x, ent.z, ent.y);
        }


        public void DrawPlayers()
        {
            lock (players)
            {
                foreach (var player in players.Values)
                {
                    try
                    {

                        if (player.isLocalPlayer)
                        {
                            moja_poz_x = player.Position.x;
                            moja_poz_y = player.Position.z;
                            moja_poz_oko = player.Rotation.y;
                            
                        }
                        else if (form.checkBox5.Checked)
                        {
                            UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(player.Position); 

                            if (entity_pos_onradar.z < (150 / zoom) && player.health > 0)
                            {
                                string wep = "";
                                bool friend = false;

                                foreach (var itemek in player.Items)
                                {
                                    if (items_names.Get(itemek.itemid) != null)
                                    {
                                        wep += items_names.Get(itemek.itemid);
                                        wep += " | ";
                                    }
                                }
     

                                if (!player.isSleeping)
                                {
                                    solidcolorbrush.Color = SharpDX.Color.Blue;

                                    foreach (var name in PergenWork.nicks)
                                    {
                                        if (player.name.Contains(name))
                                        {
                                            solidcolorbrush.Color = SharpDX.Color.Red;
                                            friend = true;
                                        }
                                    }

                                    //test.DrawString(info, Convert.ToInt32((150 + wsp_x)), Convert.ToInt32((150 - wsp_y)));


                                }
                                else if (form.checkBox6.Checked)
                                {
                                    solidcolorbrush.Color = SharpDX.Color.Black;

                                }
                                else
                                    continue;

                                foreach (var name in PergenWork.enemys)
                                {
                                    if (player.name.Equals(name))
                                    {
                                        solidcolorbrush.Color = SharpDX.Color.Silver;
                                    }
                                }
                                
                                wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 2.00F);
                                solidcolorbrush.Color = SharpDX.Color.Black;
                                if (!friend)
                                {
                                    string info = string.Format("HP: {0}{1}{2}m{3}{4}", Math.Round(player.health, 1), Environment.NewLine, entity_pos_onradar.z, Environment.NewLine, wep);
                                    using (TextFormat _textformat = new TextFormat(fact_txt, "Arial", FontWeight.Light, SharpDX.DirectWrite.FontStyle.Normal, 7.0f))
                                    {
                                        wndRender.DrawText(info, _textformat, new SharpDX.RectangleF(Convert.ToInt32((153 + entity_pos_onradar.x)), Convert.ToInt32((153 - entity_pos_onradar.y)), 100f, 30f), solidcolorbrush);
                                    }
                                }
                                
                            }
                            
                        }

                    }
                    catch { }

                }
            }

          
        }

        public void DrawWeapons()
        {
            lock (weapons)
            {
                foreach (var weapon in weapons.Values)
                {
                    try
                    {
                        UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(weapon.Position);

                        if (entity_pos_onradar.z < (150 / zoom))
                        {
                            solidcolorbrush.Color = SharpDX.Color.LimeGreen;
                            wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 1F);
                            solidcolorbrush.Color = SharpDX.Color.Black;
                            using (TextFormat _textformat = new TextFormat(fact_txt, "Arial", FontWeight.Light, SharpDX.DirectWrite.FontStyle.Normal, 7.0f))
                            {
                                wndRender.DrawText(weapon.name + " (" + entity_pos_onradar.z + "m)", _textformat, new SharpDX.RectangleF(Convert.ToInt32((153 + entity_pos_onradar.x)), Convert.ToInt32((153 - entity_pos_onradar.y)), 100f, 30f), solidcolorbrush);
                            }
                        }
                    }
                    catch { }
                    
                       // test.DrawString(items_names.Get(item.worldItem.item.itemid) + " (" + odl + "m)", , Convert.ToInt32((150 - wsp_y)));


                    
                }
            }
        }

        public void DrawAnimals()
        {
            lock (animals)
            {
                foreach (var animal in animals.Values)
                {
                    try
                    {
                        if (!Form1.listAnimals.GetItemChecked((int)animal.Type))
                            continue;

                        UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(animal.Position);

                        if (entity_pos_onradar.z < (150 / zoom))
                        {
                            solidcolorbrush.Color = SharpDX.Color.Pink;

                            if (animal.isDangerous)
                                solidcolorbrush.Color = SharpDX.Color.Black;

                            wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 2F);

                        }
                    }
                    catch { }

                }
            }
        }

        public void DrawStones()
        {
            lock (stones)
            {
                for(int i=0; i<stones.Length; i++)
                {
                    var color = SharpDX.Color.Purple;
                    try
                    {
                        switch (i)
                        {
                            case (int)Entity.Stone.metal:
                                color = SharpDX.Color.LightGray;
                                break;
                            case (int)Entity.Stone.stone:
                                break;
                            case (int)Entity.Stone.sulfur:
                                color = SharpDX.Color.Red;
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception ex) { MessageBox.Show(ex.Message); }
                
                    foreach (var item in stones[i].Values)
                    {
                        try
                        {
                            UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(item);

                            if (entity_pos_onradar.z < (150 / zoom))
                            {
                                solidcolorbrush.Color = SharpDX.Color.Purple;
                                wndRender.FillRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 3, 3), solidcolorbrush);
                                solidcolorbrush.Color = color;
                                wndRender.FillRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 2, 2), solidcolorbrush);
                            }
                            //test.DrawStones(Convert.ToInt32((150 + wsp_x)), Convert.ToInt32((150 - wsp_y)));
                        }
                        catch { }
                    }
                }
            }
        }

        public void DrawCorpses()
        {
            lock (lootcorpses)
            {
                foreach (var lootcorpse in lootcorpses.Values)
                {
                    try
                    {
                        UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(lootcorpse.Position);
                        solidcolorbrush.Color = SharpDX.Color.LimeGreen;

                        if (form.checkBox7.Checked && lootcorpse.isBackpack)
                        {
                            solidcolorbrush.Color = SharpDX.Color.Aqua;
                            wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 2F);
                            continue;
                        }
                        else if (!lootcorpse.isBackpack)
                        {
                            TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks);
                            if (entity_pos_onradar.z < (150 / zoom) && (elapsedSpan.TotalSeconds - decaycorpse[lootcorpse.ID].TotalSeconds) < 300)
                                wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 1F);
                        }
                    }
                    catch { }
                }
            }
        }

        public void DrawPlants()
        {
            lock (plants)
            {
                foreach (var item in plants.Values)
                {
                    try
                    {
                        UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(item.Position);
                        solidcolorbrush.Color = SharpDX.Color.OrangeRed;
                        if (entity_pos_onradar.z < (150 / zoom))
                            wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 2F);
                        //test.DrawStones(Convert.ToInt32((150 + wsp_x)), Convert.ToInt32((150 - wsp_y)));
                    }
                    catch { }
                }
            }
        }

        public void DrawLootboxes()
        {
            lock (lootboxes)
            {
                foreach (var item in lootboxes.Values)
                {
                    try
                    {
                        UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(item);
                        solidcolorbrush.Color = SharpDX.Color.White;
                        
                        if (entity_pos_onradar.z < (150 / zoom))
                            wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 2F);
                        //test.DrawStones(Convert.ToInt32((150 + wsp_x)), Convert.ToInt32((150 - wsp_y)));
                    }
                    catch { }
                }
            }
        }

        /*public void DrawSmallStashes()
        {
            lock (smallstash)
            {
                foreach (var item in smallstash.Values)
                {
                    UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(item);
                    solidcolorbrush.Color = SharpDX.Color.Cyan;
                    if (entity_pos_onradar.z < (150 / zoom) && form.checkBox1.Checked)
                        wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 2F);
                    //test.DrawStones(Convert.ToInt32((150 + wsp_x)), Convert.ToInt32((150 - wsp_y)));
                }
            }
        }*/

        public void DrawRTloot()
        {
            lock (rtloot)
            {
                foreach (var item in rtloot.Values)
                {
                    try
                    {
                        UnityEngine.Vector3 entity_pos_onradar = GetEntityPosition(item);
                        solidcolorbrush.Color = SharpDX.Color.Goldenrod;
                        if (entity_pos_onradar.z < (150 / zoom))
                            wndRender.DrawRectangle(new SharpDX.RectangleF(Convert.ToInt32(150 + entity_pos_onradar.x), Convert.ToInt32(150 - entity_pos_onradar.y), 1, 1), solidcolorbrush, 2F);
                        //test.DrawStones(Convert.ToInt32((150 + wsp_x)), Convert.ToInt32((150 - wsp_y)));
                    }
                    catch { };
                }
            }
        }

        public void ClearPlayers()
        {
            lock (decaycorpse)
            {
                TimeSpan elapsedSpan = new TimeSpan(DateTime.Now.Ticks);
                foreach(KeyValuePair<ulong, TimeSpan> entry in decaycorpse)
                {
                    try
                    {
                        if (Entity.CheckPlayer(entry.Key))
                        {
                            int totalMinutes = (int)(elapsedSpan - entry.Value).Minutes;
                            if (totalMinutes > 3)
                            {
                                lock (players)
                                {
                                    if (!players[entry.Key].isSleeping && !players[entry.Key].isLocalPlayer)
                                        players[entry.Key].Position = new UnityEngine.Vector3(9999f, 9999f, 9999f);
                                }
                            }
                        }
                    }
                    catch { }
                }
                
            }
        }

        

        public void Render()
        {

            wndRender.BeginDraw();
            //SharpDX.Mathematics.Interop.RawColor4 Colors = new SharpDX.Mathematics.Interop.RawColor4(1f, 1f, 1f, 1f);
            wndRender.Clear(SharpDX.Color.DimGray);
            
            //ME
            //wndRender.DrawRectangle(new SharpDX.RectangleF(150f, 150f, 1, 1), brush_me, 2F);
            
            //World
            
            DrawPlayers();
            if(form.checkBox3.Checked)
                DrawStones();
            if(form.checkBox2.Checked)
                DrawPlants();
            if(form.checkBox1.Checked)
                DrawAnimals();
            if(form.checkBox4.Checked)
                DrawWeapons();
            if(form.checkBox7.Checked)
                DrawLootboxes();
            if(form.checkBox8.Checked)
                DrawRTloot();

            DrawCorpses();
            ClearPlayers();

            solidcolorbrush.Color = SharpDX.Color.Red;
            wndRender.DrawLine(l1_point1, l1_point2, solidcolorbrush, 0.4f);
            wndRender.DrawLine(l2_point1, l2_point2, solidcolorbrush, 0.4f);

            wndRender.Flush();
            wndRender.EndDraw();
        }
    }
}
