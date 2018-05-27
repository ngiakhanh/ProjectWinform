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
    public partial class DCTTTK : Form
    {
        SinhVienEntities1 SV = new SinhVienEntities1();

        public DCTTTK()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DCTTTK_Load(object sender, EventArgs e)
        {
            LoadCode();
            label4.Text = QLDSND.QLDSND_ID.ToString();
            int ID = Int32.Parse(label4.Text);
            if (ID != -1)
            {
                tbl_DangNhap theOne = (from s in SV.tbl_DangNhap
                                       where s.ID == ID
                                       select s).FirstOrDefault<tbl_DangNhap>();
                textBox1.Text = theOne.TenDangNhap.ToString();
                textBox2.Text = theOne.MatKhau.ToString();
                comboBox1.SelectedValue = theOne.MaKhoa.ToString();
            }
            else
            {
                button1.Text = "Add";
            }
        }

        private void LoadCode()
        {
            var theOne = (from s in SV.tbl_Khoa
                          select s).ToList();
            comboBox1.DataSource = theOne;
            comboBox1.DisplayMember = "TenKhoa";
            comboBox1.ValueMember = "MaKhoa";
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ID = Int32.Parse(label4.Text);
            if (ID != -1)
            {
                tbl_DangNhap theOne = (from s in SV.tbl_DangNhap
                                       where s.ID == ID
                                       select s).FirstOrDefault<tbl_DangNhap>();
                theOne.TenDangNhap = textBox1.Text;
                theOne.MatKhau = textBox2.Text;
                theOne.MaKhoa = comboBox1.SelectedValue.ToString();
                SV.SaveChanges();
                MessageBox.Show("Edited successfully!");
            }
            else
            {
                tbl_DangNhap dn = new tbl_DangNhap();
                dn.MaKhoa = comboBox1.SelectedValue.ToString();
                dn.MatKhau = textBox2.Text;
                dn.TenDangNhap = textBox1.Text;
                SV.tbl_DangNhap.Add(dn);
                SV.SaveChanges();
                MessageBox.Show("Added successfully!");
            }
            Main.ReloadND();
            this.Close();
        }
    }
}
