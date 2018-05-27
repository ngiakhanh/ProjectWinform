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
    public partial class Login : Form
    {
        public static string MaKhoa;

        public Login()
        {
            InitializeComponent();
        }
        SinhVienEntities1 sv = new SinhVienEntities1();

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var DS = (from s in sv.tbl_Khoa
                      join st in sv.tbl_SinhVien
                      on s.MaKhoa equals st.MaKhoa
                      select new
                      {
                          tenkhoa = s.TenKhoa,
                          tensv = st.Ho + " " + st.Ten
                      }).ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var o = (from s in sv.tbl_DangNhap
                     where s.TenDangNhap == bunifuMaterialTextbox1.Text&& s.MatKhau == bunifuMaterialTextbox2.Text
                     select s).ToList();
            if (o.Count==1 && bunifuCheckbox1.Checked==true)
            {
                MaKhoa = o[0].MaKhoa;
                var i = (from s in sv.tbl_DangNhap
                         where s.TenDangNhap == bunifuMaterialTextbox1.Text && s.MatKhau == bunifuMaterialTextbox2.Text
                         select s).FirstOrDefault();
                Main.MaKhoa = i.MaKhoa;
                Main frm = new Main();
                frm.FormClosed += new FormClosedEventHandler(FormClosed);
                frm.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Failed to login!");
            }
        }

        private new void FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();           
        }

        private void panelRight_MouseClick(object sender, MouseEventArgs e)
        {
            panelRight.Focus();
        }

        private void panelLeft_MouseClick(object sender, MouseEventArgs e)
        {
            panelLeft.Focus();
        }

        private void label2_MouseClick(object sender, MouseEventArgs e)
        {
            panelRight.Focus();
        }

        private void label3_MouseClick(object sender, MouseEventArgs e)
        {
            panelRight.Focus();
        }

        private void label4_MouseClick(object sender, MouseEventArgs e)
        {
            panelRight.Focus();
        }

        private void label7_MouseClick(object sender, MouseEventArgs e)
        {
            panelRight.Focus();
        }

        private void bunifuCheckbox1_MouseClick(object sender, MouseEventArgs e)
        {
            panelRight.Focus();
        }

        private void label6_MouseClick(object sender, MouseEventArgs e)
        {
            panelRight.Focus();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            panelLeft.Focus();
        }

        private void bunifuMaterialTextbox1_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void resetText()
        {
            bunifuMaterialTextbox1.Text = "";
            bunifuMaterialTextbox2.Text = "";
            bunifuMaterialTextbox3.Text = "";
            bunifuMaterialTextbox4.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            label1.Visible = true;            
            label5.Visible = true;
            label6.Visible = true;
            bunifuMaterialTextbox3.Visible = true;
            bunifuMaterialTextbox4.Visible = true;
            resetText();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            bunifuMaterialTextbox3.Visible = false;
            bunifuMaterialTextbox4.Visible = false;
            button1.Enabled = true;
            resetText();           
        }
    }
}
