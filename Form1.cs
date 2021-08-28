using GorgaTech.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GorgaTech
{
    public partial class Form1 : Form
    {
        public static Thread firstThread;
        public static Thread thirdThread;
        public static CheckedListBox listAnimals;
        public static CheckedListBox listLoot;
        //public static RustWork ooo;
        public static Settings settings;
        public Form1()
        {
            
            InitializeComponent();
            try
            {
                settings = Settings.Load();
                if(settings.Nicks == null)
                    settings.Nicks = new string[0];
                if(settings.Enemys == null)
                    settings.Enemys = new string[0];
                if (settings!=null)
                {
                    textBox1.Text = settings.IPport;
                    foreach (var name in settings.Nicks)
                    {
                        lock (listBox1)
                        {
                            listBox1.Items.Add(name);
                        }
                    }
                    foreach (var name in settings.Enemys)
                    {
                        lock (listBox2)
                        {
                            listBox2.Items.Add(name);
                        }
                    }
                    numericUpDown1.Value = settings.RadarPosX;
                    numericUpDown2.Value = settings.RadarPosY;
                    numericUpDown3.Value = (decimal)settings.RadarOpacity!=0?(decimal)settings.RadarOpacity:(decimal)0.7;
                }

                checkedListBox1.Items.AddRange(Enum.GetNames(typeof(Entities.cAnimals.Animal)));
               // checkedListBox2.Items.AddRange(Enum.GetNames(typeof(Entities.cPlants.Plants)));
                listAnimals = this.checkedListBox1;
               // listLoot = this.checkedListBox2;
            }
            catch { MessageBox.Show("Blad ladowania ustawien"); }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //ooo = new RustWork();
            if (settings != null)
            {
                settings.IPport = textBox1.Text;
                settings.Nicks = listBox1.Items.Cast<string>().ToArray();
                settings.Enemys = listBox2.Items.Cast<string>().ToArray();
                settings.Save();
            }

            firstThread = new Thread(new ThreadStart(PergenWork.ReadRustPackets));
            thirdThread = new Thread(new ThreadStart(PergenWork.DrawForm));

            firstThread.Start();
            thirdThread.Start();
            Form2 lol = new Form2();
            lol.Show();

            btn_uruchom.Enabled = false;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (firstThread != null && thirdThread != null)
            {
                firstThread.Abort();
                thirdThread.Abort();
            }
            var current = Process.GetCurrentProcess();
            Process.GetProcessesByName(current.ProcessName)
                .Where(t => t.Id != current.Id)
                .ToList()
                .ForEach(t => t.Kill());

            current.Kill();
            /*foreach (var process in Process.GetProcessesByName("chromex"))
            {
                process.Kill();
            }*/
            this.Dispose();

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            settings.IPport = textBox1.Text;
            lock (settings.Nicks)
            {
                settings.Nicks = listBox1.Items.Cast<string>().ToArray();
            }
            lock (PergenWork.nicks)
            {
                PergenWork.nicks = listBox1.Items.Cast<string>().ToArray();
            }
            lock (settings.Enemys)
            {
                settings.Enemys = listBox2.Items.Cast<string>().ToArray();
            }
            lock (PergenWork.enemys)
            {
                PergenWork.enemys = listBox2.Items.Cast<string>().ToArray();
            }
            settings.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lock (listBox1)
            {
                listBox1.Items.Add(textBox3.Text);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            lock (listBox1)
            {
                if(listBox1.SelectedItem != null)
                    listBox1.Items.Remove(listBox1.SelectedItem);
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            settings.RadarPosX = (short)numericUpDown1.Value;
            if (thirdThread != null)
            {
                RadarRenderDX.form.Location = new Point(settings.RadarPosX, settings.RadarPosY);
            }

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            settings.RadarPosY = (short)numericUpDown2.Value;
            if (thirdThread != null)
                RadarRenderDX.form.Location = new Point(settings.RadarPosX, settings.RadarPosY);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            settings.RadarOpacity = (double)numericUpDown3.Value;
            if (thirdThread != null)
                RadarRenderDX.form.Opacity = settings.RadarOpacity;
        }

        private void checkBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Form2.isCelownik)
                Form2.isCelownik = false;
            else
                Form2.isCelownik = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lock (RadarRenderDX.weapons)
            {
                try
                {
                    RadarRenderDX.weapons.Clear();
                }
                catch { }
            }
        }

        private void checkBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (Form2.showTurret)
                Form2.showTurret = false;
            else
                Form2.showTurret = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lock (listBox2)
            {
                listBox2.Items.Add(textBox4.Text);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            lock (listBox2)
            {
                if (listBox2.SelectedItem != null)
                    listBox2.Items.Remove(listBox2.SelectedItem);
            }
        }

    }
}
