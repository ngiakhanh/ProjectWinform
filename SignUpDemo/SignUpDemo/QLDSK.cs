using ClosedXML.Excel;
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
    public partial class QLDSK : Form
    {
        SinhVienEntities1 SV = new SinhVienEntities1();

        public static string QLDSK_MaKhoa;

        public QLDSK()
        {
            InitializeComponent();
        }

        public void LoadDS()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.AutoGenerateColumns = false;
            var DS = (from s in SV.tbl_Khoa
                      select s).ToArray();
            dataGridView1.DataSource = DS;
            label1.Text = dataGridView1.RowCount.ToString();
        }

        private void QLDSK_Load(object sender, EventArgs e)
        {
            LoadDS();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 2)
            {
                DCTTK edit = new DCTTK();
                QLDSK_MaKhoa = dataGridView1.Rows[e.RowIndex].Cells["MaKhoa"].Value.ToString();
                edit.Show();
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 3)
            {
                DialogResult confirm = MessageBox.Show("Are you fucking sure - This will delete the corresponding Sinh Vien, Diem and Login!", "Warning", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string MaKhoa = dataGridView1.Rows[e.RowIndex].Cells["MaKhoa"].Value.ToString();

                    var o = (from s in SV.tbl_DangNhap
                             where s.MaKhoa == MaKhoa
                             select s).ToList();
                    for (int i = 0; i < o.Count; i++)
                    {
                        SV.tbl_DangNhap.Remove(o[i]);
                    }

                    var p = (from s in SV.tbl_SinhVien
                             where s.MaKhoa == MaKhoa
                             select s).ToList();
                    for (int i = 0; i < p.Count; i++)
                    {
                        string mssv = p[i].MSSV;
                        var l = (from s in SV.tbl_Diem
                                 where s.MSSV == mssv
                                 select s).ToList();
                        for (int g = 0; g < l.Count; g++)
                        {
                            SV.tbl_Diem.Remove(l[g]);
                        }
                        SV.tbl_SinhVien.Remove(p[i]);
                    }
                    SV.tbl_Khoa.Remove((from s in SV.tbl_Khoa
                                            where s.MaKhoa == MaKhoa
                                            select s).FirstOrDefault());
                    SV.SaveChanges();
                    MessageBox.Show("Deleted successfully!");
                    Main.ReloadKhoa();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QLDSK_MaKhoa = "Them";
            DCTTK edit = new DCTTK();
            edit.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadDS();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var query = from BangSinhVien in SV.tbl_SinhVien
                        join BangKhoa in SV.tbl_Khoa
                        on BangSinhVien.MaKhoa equals BangKhoa.MaKhoa
                        select new
                        {
                            BangSinhVien.MSSV,
                            BangSinhVien.Ho,
                            BangSinhVien.Ten,
                            BangSinhVien.GioiTinh,
                            BangSinhVien.NTNS,
                            BangSinhVien.NoiSinh,
                            BangSinhVien.MaKhoa,
                            BangKhoa.TenKhoa,
                            BangSinhVien.KetQuaTN,
                            BangSinhVien.DTB,
                            BangSinhVien.XepLoaiTN
                        };
            DataTable dt = ConvertDatatable.LINQToDataTable(query);
            saveFileDialog1.FileName = "DanhSachTemp";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FolderPath = saveFileDialog1.FileName;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //Ten sheet
                    wb.Worksheets.Add(dt, "Temp");
                    //Duong dan chua ten file excel
                    wb.SaveAs(FolderPath + ".xlsx");
                }
                MessageBox.Show("Saved succesfully at "+ FolderPath);
            }
        }

    }
}
