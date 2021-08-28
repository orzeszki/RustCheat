using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;
using System.Windows.Forms;

namespace GorgaTech
{
    public partial class RadarForm : SharpDX.Windows.RenderForm
    {
        public RadarForm()
        {
            //this.ClientSize = new System.Drawing.Size(300, 300);
            this.TopMost = true;
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Opacity = 0.7D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;

            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            if (Form1.settings != null)
            {
                this.Opacity = Form1.settings.RadarOpacity!=0?Form1.settings.RadarOpacity:0.7D;
                this.Location = new System.Drawing.Point(Form1.settings.RadarPosX, Form1.settings.RadarPosY);
            }
            else
            {
                this.Location = new System.Drawing.Point(0, 0);
                this.Opacity = 0.7D;
            }
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RadarRenderDX.zoom += 0.2f;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (RadarRenderDX.zoom - 0.2f > 0)
                RadarRenderDX.zoom -= 0.2f;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RadarRenderDX.zoom = 1.0f;
        }

    }

}
