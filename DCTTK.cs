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
    public partial class DCTTK : Form
    {
        SinhVienEntities1 SV = new SinhVienEntities1();
        public DCTTK()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string MaKhoa = label4.Text;
            if (MaKhoa != "Them")
            {
                //Check 
                var k = (from s in SV.tbl_Khoa
                         where s.MaKhoa == textBox1.Text && s.TenKhoa == textBox2.Text
                         select s).ToList();
                if (k.Count == 0)
                {
                    //Change TenKhoa
                    tbl_Khoa theOne = (from s in SV.tbl_Khoa
                                       where s.MaKhoa == MaKhoa
                                       select s).FirstOrDefault<tbl_Khoa>();
                    theOne.TenKhoa = textBox2.Text;

                    //Check if MaKhoa was changed
                    if (theOne.MaKhoa != textBox1.Text)
                    {
                        tbl_Khoa dn = new tbl_Khoa();
                        dn.MaKhoa = textBox1.Text;
                        dn.TenKhoa = textBox2.Text;
                        SV.tbl_Khoa.Add(dn);
                        SV.SaveChanges();

                        var o = (from s in SV.tbl_DangNhap
                                 where s.MaKhoa == MaKhoa
                                 select s).ToList();

                        for (int i = 0; i < o.Count; i++)
                        {
                            o[i].MaKhoa = textBox1.Text;
                        }

                        var p = (from s in SV.tbl_SinhVien
                                 where s.MaKhoa == MaKhoa
                                 select s).ToList();

                        for (int i = 0; i < p.Count; i++)
                        {
                            p[i].MaKhoa = textBox1.Text;
                        }

                        SV.tbl_Khoa.Remove((from s in SV.tbl_Khoa
                                            where s.MaKhoa == MaKhoa
                                            select s).FirstOrDefault());



                        /*
                        tbl_Khoa theOne = (from s in SV.tbl_Khoa
                                           where s.MaKhoa == MaKhoa
                                           select s).FirstOrDefault<tbl_Khoa>();
                        theOne.MaKhoa = textBox1.Text;
                        theOne.TenKhoa = textBox2.Text;
                        SV.SaveChanges();
                        */
                    }
                    SV.SaveChanges();
                    MessageBox.Show("Edited successfully!");
                    
                }

                else
                {
                    MessageBox.Show("You didn't edit anything!");
                }

            }
            else
            {
                var o = (from s in SV.tbl_Khoa
                         where s.MaKhoa == textBox1.Text && s.TenKhoa == textBox2.Text
                         select s).ToList();
                if (o.Count == 0)
                {
                    tbl_Khoa dn = new tbl_Khoa();
                    dn.MaKhoa = textBox1.Text;
                    dn.TenKhoa = textBox2.Text;

                    SV.tbl_Khoa.Add(dn);
                    SV.SaveChanges();
                    MessageBox.Show("Added successfully!");
                }
                else
                {
                    MessageBox.Show("Added unsuccessfully!");
                }
            }
            Main.ReloadKhoa();
            this.Close();
        }

        private void DCTTK_Load(object sender, EventArgs e)
        {
            label4.Text = QLDSK.QLDSK_MaKhoa.ToString();
            string MaKhoa = label4.Text;
            if (MaKhoa != "Them")
            {
                tbl_Khoa theOne = (from s in SV.tbl_Khoa
                                       where s.MaKhoa == MaKhoa
                                       select s).FirstOrDefault<tbl_Khoa>();
                textBox1.Text = theOne.MaKhoa.ToString();
                textBox2.Text = theOne.TenKhoa.ToString();
            }
            else
            {
                button1.Text = "Add";
            }
        }
    }
}
