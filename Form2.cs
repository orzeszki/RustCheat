using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpDX.Direct2D1;
using Factory = SharpDX.Direct2D1.Factory;
using FontFactory = SharpDX.DirectWrite.Factory;
using Format = SharpDX.DXGI.Format;
using SharpDX;
using SharpDX.DirectWrite;
using System.Threading;
using System.Runtime.InteropServices;
using GorgaTech.Data;

namespace GorgaTech
{
    public partial class Form2 : Form
    {
        private WindowRenderTarget device;
        private HwndRenderTargetProperties renderProperties;
        private SolidColorBrush solidColorBrush;
        //private Factory factory;

        SharpDX.Mathematics.Interop.RawVector2 celX_point1 = new SharpDX.Mathematics.Interop.RawVector2((float)(1920 / 2), (float)(1080 / 2) - 3);
        SharpDX.Mathematics.Interop.RawVector2 celX_point2 = new SharpDX.Mathematics.Interop.RawVector2((float)(1920 / 2), (float)(1080 / 2) + 3);

        SharpDX.Mathematics.Interop.RawVector2 celY_point1 = new SharpDX.Mathematics.Interop.RawVector2((float)(1920 / 2) - 3, (float)(1080 / 2));
        SharpDX.Mathematics.Interop.RawVector2 celY_point2 = new SharpDX.Mathematics.Interop.RawVector2((float)(1920 / 2) + 3, (float)(1080 / 2));

        public static bool isCelownik = false;
        public static bool showTurret = true;
       // public static string myModelState = "";

        //text fonts
        //private TextFormat font, fontSmall;
       // private FontFactory fontFactory;
       // private const string fontFamily = "Arial";//you can edit this of course
       // private const float fontSize = 12.0f;
       // private const float fontSizeSmall = 10.0f;

        private IntPtr handle;
        private Thread sDX = null;
        //DllImports
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        static extern bool SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        [DllImport("dwmapi.dll")]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, ref int[] pMargins);
        //Styles
        public const UInt32 SWP_NOSIZE = 0x0001;
        public const UInt32 SWP_NOMOVE = 0x0002;
        public const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        public static IntPtr HWND_TOPMOST = new IntPtr(-1);

        SharpDX.DirectWrite.Factory fact_txt = new SharpDX.DirectWrite.Factory(SharpDX.DirectWrite.FactoryType.Shared);
        

        public Form2()
        {
            this.handle = Handle;
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            OnResize(null);

            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;
            this.Width = 1920;// set your own size
            this.Height = 1080;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |// this reduce the flicker
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.DoubleBuffer |
                ControlStyles.UserPaint |
                ControlStyles.Opaque |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            this.TopMost = true;
            this.Visible = true;

            //factory = new Factory();
            //fontFactory = new FontFactory();
            renderProperties = new HwndRenderTargetProperties()
            {
                Hwnd = this.Handle,
                PixelSize = new Size2(1920, 1080),
                PresentOptions = PresentOptions.None
            };

            //SetLayeredWindowAttributes(this.Handle, 0, 255, Managed.LWA_ALPHA);// caution directx error

            //Init DirectX
            using (Factory factory = new Factory())
            {
                device = new WindowRenderTarget(factory, new RenderTargetProperties(new PixelFormat(Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied)), renderProperties);
            }

            solidColorBrush = new SolidColorBrush(device, Color.White);
            // Init font's
           // font = new TextFormat(fontFactory, fontFamily, fontSize);
           // fontSmall = new TextFormat(fontFactory, fontFamily, fontSizeSmall);
            //line = new device.DrawLine;

            sDX = new Thread(new ParameterizedThreadStart(sDXThread));

            sDX.Priority = ThreadPriority.Highest;
            sDX.IsBackground = true;
            sDX.Start();
        }
        protected override void OnPaint(PaintEventArgs e)// create the whole form
        {
            int[] marg = new int[] { 0, 0, Width, Height };
            DwmExtendFrameIntoClientArea(this.Handle, ref marg);
        }

        float HPbarSize(float odl)
        {
            if (odl > 40)
                return (40 / odl);
            return 1;
        }

        public void DrawESP()
        {
            int screenWidth = 1920;
            int screenHeight = 1080;
            float P = (float)Math.PI / 180;
            float focalLength = 540f / (float)Math.Tan(85f * P / 2f);
            try
            {
                lock (RadarRenderDX.players)
                {
                    foreach (var player in RadarRenderDX.players.Values)
                    {
                        
                        if (player.isLocalPlayer)
                        {
                           /* TextFormat _textformat = new TextFormat(fact_txt, "Arial", FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, 12f);
                            solidColorBrush.Color = Color.White;
                            device.DrawText(item.basePlayer.playerFlags+ "", _textformat, new SharpDX.RectangleF(900f, 20f, 900f, 30f), solidColorBrush);*/
                            continue;
                        }
                        //var target = new UnityEngine.Vector3(player.Position.x, player.Position.y, player.Position.z);
                        //if (item.basePlayer.modelState.flags == 1)
                        // target -= new UnityEngine.Vector3(0, 0.5f, 0);
                        var point = GorgaTech.ESP.W2S.WorldToScreen(Entities.cPlayers.localPosition, player.Position, Entities.cPlayers.localCamera);

                        var odl = RadarRenderDX.GetEntityPosition(player.Position).z;
                        if (point.z > 0 && odl < 150 && player.health != 0)
                        {
                            var color = SharpDX.Color.Aqua;
                            

                            if (player.isWounded)
                                color = SharpDX.Color.Purple;
                            foreach (var name in PergenWork.nicks)
                            {
                                if(player.name.Contains(name))
                                    color = SharpDX.Color.Red;
                            }

                            float screenX = (focalLength * point.x / point.z + screenWidth / 2);
                            float screenY = (focalLength * point.y / point.z + screenHeight / 2);
                            screenY = screenHeight - screenY;

                            screenX -= ((150 / odl) / 2);
                            screenY -= ((150 / odl) / 2);
                            //wndRender.DrawEllipse(new Ellipse(new RawVector2(screenX, screenY), 6, 6), greenBrush);
                            if (!player.isSleeping)
                            {
                                if (odl != 0)
                                {
                                    if (odl < 100)
                                    {
                                        solidColorBrush.Color = SharpDX.Color.Black;
                                        device.FillRectangle(new SharpDX.RectangleF(screenX, screenY - 25, 50f * HPbarSize(odl), 5f * HPbarSize(odl)), solidColorBrush);
                                        if (player.health > 70)
                                            solidColorBrush.Color = SharpDX.Color.LightGreen;
                                        else if (player.health > 35)
                                            solidColorBrush.Color = SharpDX.Color.Orange;
                                        else
                                            solidColorBrush.Color = SharpDX.Color.Red;
                                        device.FillRectangle(new SharpDX.RectangleF(screenX, screenY - 24, (player.health / 2) * HPbarSize(odl), 3f * HPbarSize(odl)), solidColorBrush);
                                    }

                                    using (TextFormat _textformat = new TextFormat(fact_txt, "Arial", FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, 12f - (4f * (odl / 150))))
                                    {
                                        solidColorBrush.Color = color;
                                        device.DrawRectangle(new SharpDX.RectangleF(screenX, screenY, (150 / odl), (150 / odl)), solidColorBrush, 1f - (1 * (odl / 150)));
                                        solidColorBrush.Color = Color.White;
                                        device.DrawText(odl + "", _textformat, new SharpDX.RectangleF(screenX, screenY - (30f - (29f * (odl / 150))), 100f, 30f), solidColorBrush);
                                    }
                                }
                            }
                            else if(RadarRenderDX.form.checkBox6.Checked)
                            {

                                using (TextFormat _textformat = new TextFormat(fact_txt, "Arial", FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, 12f - (4f * (odl / 150))))
                                {
                                    solidColorBrush.Color = Color.White;
                                    device.DrawRectangle(new SharpDX.RectangleF(screenX, screenY, (150 / odl), (150 / odl)), solidColorBrush, 1f - (1 * (odl / 150)));
                                    device.DrawText(odl + "", _textformat, new SharpDX.RectangleF(screenX, screenY - (30f - (29f * (odl / 150))), 100f, 30f), solidColorBrush);
                                }
                            }
                        }
                    }
                }
            }catch{}

            if (showTurret)
            {
                try
                {
                    foreach (var item in RadarRenderDX.autoturret.Values)
                    {
                        var weapon = new UnityEngine.Vector3(item.x, item.y - 1.0f, item.z);
                        var point = GorgaTech.ESP.W2S.WorldToScreen(Entities.cPlayers.localPosition, weapon, Entities.cPlayers.localCamera);

                        var odl = RadarRenderDX.GetEntityPosition(item).z;
                        if (point.z > 0 && odl < 100)
                        {
                            float screenX = focalLength * point.x / point.z + screenWidth / 2;
                            float screenY = focalLength * point.y / point.z + screenHeight / 2;
                            //wndRender.DrawEllipse(new Ellipse(new RawVector2(screenX, screenY), 6, 6), greenBrush);
                            if (odl != 0)
                            {
                                solidColorBrush.Color = Color.Yellow;
                                device.DrawRectangle(new SharpDX.RectangleF((float)(screenX - ((150 / odl) / 2)), (float)(screenHeight - screenY - ((150 / odl) / 2)), (150 / odl), (150 / odl)), solidColorBrush, 1F);

                                /*TextFormat _textformat = new TextFormat(fact_txt, "Arial", FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, 12f - (4f * (odl / 150)));
                                solidColorBrush.Color = Color.White;
                                device.DrawText(item.decayEntity.decayTimer + "", _textformat, new SharpDX.RectangleF(screenX, screenY - (30f - (29f * (odl / 150))), 100f, 30f), solidColorBrush);*/
                            }
                        }
                    }
                }
                catch { }
            }

            try
            {
                lock (RadarRenderDX.weapons)
                {
                    foreach (var weapon in RadarRenderDX.weapons.Values)
                    {
                        var target = new UnityEngine.Vector3(weapon.Position.x, weapon.Position.y - 1.5f, weapon.Position.z);
                        var point = GorgaTech.ESP.W2S.WorldToScreen(Entities.cPlayers.localPosition, target, Entities.cPlayers.localCamera);

                        var odl = RadarRenderDX.GetEntityPosition(weapon.Position).z;
                        if (point.z > 0 && odl < 50)
                        {
                            float screenX = focalLength * point.x / point.z + screenWidth / 2;
                            float screenY = focalLength * point.y / point.z + screenHeight / 2;
                            //wndRender.DrawEllipse(new Ellipse(new RawVector2(screenX, screenY), 6, 6), greenBrush);
                            if (odl != 0)
                            {
                                solidColorBrush.Color = Color.LimeGreen;
                                device.DrawRectangle(new SharpDX.RectangleF((float)(screenX - ((150 / odl) / 2)), (float)(screenHeight - screenY - ((150 / odl) / 2)), (150 / odl), (150 / odl)), solidColorBrush, 1F);

                                /*TextFormat _textformat = new TextFormat(fact_txt, "Arial", FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, 12f - (4f * (odl / 150)));
                                solidColorBrush.Color = Color.White;
                                device.DrawText(item.decayEntity.decayTimer + "", _textformat, new SharpDX.RectangleF(screenX, screenY - (30f - (29f * (odl / 150))), 100f, 30f), solidColorBrush);*/
                            }
                        }
                    }
                }
                if (RadarRenderDX.form.checkBox8.Checked)
                {
                    lock (RadarRenderDX.rtloot)
                    {
                        foreach (var item in RadarRenderDX.rtloot.Values)
                        {
                            var weapon = new UnityEngine.Vector3(item.x, item.y - 1.5f, item.z);
                            var point = GorgaTech.ESP.W2S.WorldToScreen(Entities.cPlayers.localPosition, weapon, Entities.cPlayers.localCamera);

                            var odl = RadarRenderDX.GetEntityPosition(item).z;
                            if (point.z > 0 && odl < 50)
                            {
                                float screenX = focalLength * point.x / point.z + screenWidth / 2;
                                float screenY = focalLength * point.y / point.z + screenHeight / 2;
                                //wndRender.DrawEllipse(new Ellipse(new RawVector2(screenX, screenY), 6, 6), greenBrush);
                                if (odl != 0)
                                {
                                    solidColorBrush.Color = Color.LimeGreen;
                                    device.DrawRectangle(new SharpDX.RectangleF((float)(screenX - ((150 / odl) / 2)), (float)(screenHeight - screenY - ((150 / odl) / 2)), (150 / odl), (150 / odl)), solidColorBrush, 1F);
                                }
                            }
                        }
                    }

                    lock (RadarRenderDX.raidboxes)
                    {
                        foreach (var item in RadarRenderDX.raidboxes.Values)
                        {
                            var weapon = new UnityEngine.Vector3(item.x, item.y - 1.5f, item.z);
                            var point = GorgaTech.ESP.W2S.WorldToScreen(Entities.cPlayers.localPosition, weapon, Entities.cPlayers.localCamera);

                            var odl = RadarRenderDX.GetEntityPosition(item).z;
                            if (point.z > 0 && odl < 150)
                            {
                                float screenX = focalLength * point.x / point.z + screenWidth / 2;
                                float screenY = focalLength * point.y / point.z + screenHeight / 2;
                                //wndRender.DrawEllipse(new Ellipse(new RawVector2(screenX, screenY), 6, 6), greenBrush);
                                if (odl != 0)
                                {
                                    solidColorBrush.Color = Color.LightGreen;
                                    using (TextFormat _textformat = new TextFormat(fact_txt, "Arial", FontWeight.Bold, SharpDX.DirectWrite.FontStyle.Normal, 10f - (4f * (odl / 150))))
                                    {
                                        device.DrawRectangle(new SharpDX.RectangleF((float)(screenX - ((150 / odl) / 2)), (float)(screenHeight - screenY - ((150 / odl) / 2)), (150 / odl), (150 / odl)), solidColorBrush, 1F);
                                    }
                                    //solidColorBrush.Color = Color.White;
                                   // device.DrawText(odl + "", _textformat, new SharpDX.RectangleF(screenX - ((150 / odl) / 2), (screenHeight - screenY - ((150 / odl) / 2)), 100f, 30f), solidColorBrush);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            /*if (RadarRenderDX.form.checkBox1.Checked)
            {
                lock (RadarRenderDX.smallstash)
                {
                    foreach (var item in RadarRenderDX.smallstash.Values)
                    {
                        var weapon = new UnityEngine.Vector3(item.x, item.y - 1.5f, item.z);
                        var point = Twojastara.ESP.W2S.WorldToScreen(Entities.cPlayers.localPosition, weapon, Entities.cPlayers.localCamera);

                        var odl = RadarRenderDX.GetEntityPosition(item).z;
                        if (point.z > 0 && odl < 50)
                        {
                            float screenX = focalLength * point.x / point.z + screenWidth / 2;
                            float screenY = focalLength * point.y / point.z + screenHeight / 2;
                            //wndRender.DrawEllipse(new Ellipse(new RawVector2(screenX, screenY), 6, 6), greenBrush);
                            if (odl != 0)
                            {
                                solidColorBrush.Color = Color.White;
                                device.DrawRectangle(new SharpDX.RectangleF((float)(screenX - ((150 / odl) / 2)), (float)(screenHeight - screenY - ((150 / odl) / 2)), (150 / odl), (150 / odl)), solidColorBrush, 1F);
                            }
                        }
                    }
                }
            }*/
        }

        private void sDXThread(object sender)
        {
            while (true)
            {
                device.BeginDraw();
                device.Clear(Color.Transparent);
                device.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Aliased;// you can set another text mode
                
               
                DrawESP();

                if (isCelownik)
                {
                    solidColorBrush.Color = Color.Lime;
                    device.DrawLine(celX_point1, celX_point2, solidColorBrush, 1.5f);
                    device.DrawLine(celY_point1, celY_point2, solidColorBrush, 1.5f);
                }
                device.Flush();
                device.EndDraw();
            }

            //whatever you want
        }
    }
}