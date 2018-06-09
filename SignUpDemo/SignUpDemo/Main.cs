using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignUpDemo
{
    public partial class Main : Form
    {
        private static Main instance;

        public static string MaKhoa;

        public Main()
        {
            InitializeComponent();
            instance = this;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReloadND();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (MaKhoa == "QT")
            {
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReloadKhoa();
        }

        public static void ReloadKhoa()
        {
            instance.panel2.Controls.Clear();
            QLDSK dk = new QLDSK();
            dk.TopLevel = false;
            dk.Dock = DockStyle.Fill;
            instance.panel2.Controls.Add(dk);
            dk.Show();
        }

        public static void ReloadND()
        {
            instance.panel2.Controls.Clear();
            QLDSND dk = new QLDSND();
            dk.TopLevel = false;
            dk.Dock = DockStyle.Fill;
            instance.panel2.Controls.Add(dk);
            dk.Show();
        }

        public static void ReloadMH()
        {
            instance.panel2.Controls.Clear();
            QLDSMH dk = new QLDSMH();
            dk.TopLevel = false;
            dk.Dock = DockStyle.Fill;
            instance.panel2.Controls.Add(dk);
            dk.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Excel frm = new Excel();
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReloadSV();
        }

        public static void ReloadSV()
        {
            instance.panel2.Controls.Clear();
            QLDSSV dk = new QLDSSV();
            dk.TopLevel = false;
            dk.Dock = DockStyle.Fill;
            instance.panel2.Controls.Add(dk);
            dk.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ReloadMH();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Report frm = new Report();
            frm.Show();
        }
    }
}
