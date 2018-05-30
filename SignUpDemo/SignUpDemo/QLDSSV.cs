using ClosedXML.Excel;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignUpDemo
{
    public partial class QLDSSV : Form
    {

        private bool first = false;

        private bool first2 = false;

        private string path;
        SinhVienEntities1 SV = new SinhVienEntities1();

        public QLDSSV()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Login.MaKhoa == "QT")
            {
                if (comboBox2.SelectedValue.ToString() == "QT")
                {
                    var DS = (from s in SV.tbl_SinhVien
                              join d in SV.tbl_Diem
                              on s.MSSV equals d.MSSV into temp1
                              from t in temp1.DefaultIfEmpty()
                              join m in SV.tbl_DMMonHoc
                              on t.MaMH equals m.MaMH into temp2
                              from p in temp2.DefaultIfEmpty()
                              orderby s.MSSV
                              select new
                              {
                                  s.MSSV,
                                  s.Ho,
                                  s.Ten,
                                  s.MaKhoa,
                                  t.MaMH,
                                  p.TenMH,
                                  t.Diem,
                                  s.KetQuaTN,
                              });
                    DataTable dt = ConvertDatatable.LINQToDataTable(DS);
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
                        MessageBox.Show("Saved succesfully at " + FolderPath);
                    }

                }
                else
                {
                    var DS = (from s in SV.tbl_SinhVien
                              join d in SV.tbl_Diem
                              on s.MSSV equals d.MSSV into temp1
                              from t in temp1.DefaultIfEmpty()
                              join m in SV.tbl_DMMonHoc
                              on t.MaMH equals m.MaMH into temp2
                              from p in temp2.DefaultIfEmpty()
                              where s.MaKhoa == comboBox2.SelectedValue.ToString()
                              orderby s.MSSV
                              select new
                              {
                                  s.MSSV,
                                  s.Ho,
                                  s.Ten,
                                  s.MaKhoa,
                                  t.MaMH,
                                  p.TenMH,
                                  t.Diem,
                                  s.KetQuaTN,
                              });
                    DataTable dt = ConvertDatatable.LINQToDataTable(DS);
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
                        MessageBox.Show("Saved succesfully at " + FolderPath);
                    }
                }
            }
            else
            {
                var DS = (from s in SV.tbl_SinhVien
                          join d in SV.tbl_Diem
                          on s.MSSV equals d.MSSV into temp1
                          from t in temp1.DefaultIfEmpty()
                          join m in SV.tbl_DMMonHoc
                          on t.MaMH equals m.MaMH into temp2
                          from p in temp2.DefaultIfEmpty()
                          where s.MaKhoa == Login.MaKhoa
                          orderby s.MSSV
                          select new
                          {
                              s.MSSV,
                              s.Ho,
                              s.Ten,
                              s.MaKhoa,
                              t.MaMH,
                              p.TenMH,
                              t.Diem,
                              s.KetQuaTN,
                          });
                DataTable dt = ConvertDatatable.LINQToDataTable(DS);
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
                    MessageBox.Show("Saved succesfully at " + FolderPath);
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.Filter = "XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|XLS files (*.xls, *.xlt)|*.xls;*.xlt";
                path = openFileDialog1.FileName;
                first2 = false;
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {

                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // 2. Use the AsDataSet extension method
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = true,
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                        comboBox1.Items.Clear();
                        foreach (DataTable dt in result.Tables)
                        {
                            comboBox1.Items.Add(dt.TableName);
                        }
                        comboBox1.SelectedIndex = 0;
                        first2 = true;

                        if (result.Tables[0].Columns.Contains("MaKhoa"))
                        {
                            if (Login.MaKhoa != "QT")
                            {
                                for (int i = result.Tables[0].Rows.Count - 1; i >= 0; i--)
                                {
                                    DataRow dr = result.Tables[0].Rows[i];
                                    if (Login.MaKhoa != dr["MaKhoa"].ToString())
                                    {
                                        result.Tables[0].Rows.Remove(dr);
                                    }
                                }
                            }
                            else
                            {
                                comboBox2.SelectedIndex = 1;
                                comboBox2.Enabled = false;
                            }

                            dataGridView1.DataSource = null;
                            dataGridView1.Rows.Clear();

                            dataGridView1.DataSource = result.Tables[0];
                            label1.Text = dataGridView1.Rows.Count.ToString();
                        }
                        // The result of each spreadsheet is in result.Tables
                    }
                }

                comboBox1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool success = true;
            foreach (DataGridViewRow rw in this.dataGridView1.Rows)
            {
                tbl_SinhVien SinhVien = new tbl_SinhVien();
                tbl_Diem Diem = new tbl_Diem();
                if (rw.Cells[0].Value == null || rw.Cells[0].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[0].Value.ToString()))
                {
                    continue;
                }

                //Check ton tai trong tbl_SinhVien
                string valueCheck = rw.Cells[0].Value.ToString();

                var check = (from s in SV.tbl_SinhVien
                             where s.MSSV == valueCheck
                             select s).FirstOrDefault();

                if (check == null)
                {
                    //Check MSSV
                    bool kh = false;

                    string temp = rw.Cells[0].Value.ToString();
                    string temp2 = "";
                    int i = 0;
                    do
                    {
                        temp2 += temp[i];
                        i++;
                    } while (i < temp.Length && !Char.IsDigit(temp[i]));

                    var khoa = (from s in SV.tbl_Khoa
                                select s.MaKhoa).ToList();
                    if (Login.MaKhoa == "QT")
                    {
                        for (int r = 0; r < khoa.Count; r++)
                        {
                            if (khoa[r] == temp2)
                            {
                                kh = true;
                                break;
                            }
                        }
                    }
                    else
                    {
                        if (Login.MaKhoa == temp2)
                        {
                            kh = true;
                        }
                    }

                    //Add
                    if (kh == true)
                    {
                        SinhVien.MSSV = rw.Cells[0].Value.ToString();
                        SinhVien.Ho = rw.Cells[1].Value.ToString();
                        SinhVien.Ten = rw.Cells[2].Value.ToString();

                        SinhVien.MaKhoa = temp2;
                        SV.tbl_SinhVien.Add(SinhVien);

                        //Check MaMH
                        var mamh = (from s in SV.tbl_DMMonHoc
                                    where s.MaKhoa == temp2
                                    select s.MaMH).ToList();
                        bool checkMaMH = false;
                        for (int r = 0; r < mamh.Count; r++)
                        {
                            if (mamh[r] == rw.Cells[4].Value.ToString())
                            {
                                checkMaMH = true;
                                break;
                            }
                        }

                        if (checkMaMH == true)
                        {
                            Diem.MSSV = rw.Cells[0].Value.ToString();
                            Diem.MaMH = rw.Cells[4].Value.ToString();

                            Double k;
                            if (Double.TryParse(rw.Cells[6].Value.ToString(), out k))
                            {
                                Diem.Diem = k;
                            }
                            else if (rw.Cells[6].Value.ToString() != "")
                            {
                                MessageBox.Show("Student " + rw.Cells[0].Value.ToString() + " does not have valid score!");
                                success = false;
                            }
                            SV.tbl_Diem.Add(Diem);
                        }
                        else
                        {
                            MessageBox.Show("Student " + rw.Cells[0].Value.ToString() + " does not have valid MaMH!");
                            success = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Student " + rw.Cells[0].Value.ToString() + " does not have valid MSSV!");
                        success = false;
                    }
                }
                else
                {
                    //Update
                    check.Ho = rw.Cells[1].Value.ToString();
                    check.Ten = rw.Cells[2].Value.ToString();
                    bool result;
                    if (Boolean.TryParse(rw.Cells[7].Value.ToString(), out result))
                    {
                        check.KetQuaTN = result;
                    }
                    else if (rw.Cells[7].Value.ToString() != "")
                    {
                        MessageBox.Show("Student " + rw.Cells[0].Value.ToString() + " does not have valid KetQuaTN!");
                        success = false;
                    }
                    //Check ton tai trong tbl_Diem
                    string valueCheck2 = rw.Cells[0].Value.ToString();
                    string MaMH = rw.Cells[4].Value.ToString();
                    var check2 = (from s in SV.tbl_Diem
                                  where s.MSSV == valueCheck2 && s.MaMH == MaMH
                                  select s).FirstOrDefault();
                    //Update
                    if (check2 != null)
                    {
                        double score;
                        if (Double.TryParse(rw.Cells[6].Value.ToString(), out score))
                        {
                            check2.Diem = score;
                        }
                        else if (rw.Cells[6].Value.ToString() != "")
                        {
                            MessageBox.Show("Student " + rw.Cells[0].Value.ToString() + " does not have valid score!");
                            success = false;
                        }
                    }

                    else
                    {
                        //Check MaMh
                        string temp = rw.Cells[0].Value.ToString();
                        string temp2 = "";
                        int i = 0;
                        do
                        {
                            temp2 += temp[i];
                            i++;
                        } while (i < temp.Length && !Char.IsDigit(temp[i]));
                        var mamh = (from s in SV.tbl_DMMonHoc
                                    where s.MaKhoa == temp2
                                    select s.MaMH).ToList();
                        bool checkMaMH = false;
                        for (int r = 0; r < mamh.Count; r++)
                        {
                            if (mamh[r] == rw.Cells[4].Value.ToString())
                            {
                                checkMaMH = true;
                                break;
                            }
                        }
                        //Add
                        if (checkMaMH == true)
                        {
                            Diem.MSSV = rw.Cells[0].Value.ToString();
                            Diem.MaMH = rw.Cells[4].Value.ToString();

                            Double k;
                            if (Double.TryParse(rw.Cells[6].Value.ToString(), out k))
                            {
                                Diem.Diem = k;
                            }
                            else if (rw.Cells[6].Value.ToString() != "")
                            {
                                MessageBox.Show("Student " + rw.Cells[0].Value.ToString() + " does not have valid score!");
                                success = false;
                            }
                            SV.tbl_Diem.Add(Diem);
                        }
                        else
                        {
                            MessageBox.Show("Student " + rw.Cells[0].Value.ToString() + " does not have valid MaMH!");
                            success = false;
                        }
                    }
                }
            }

            SV.SaveChanges();
            if (success == true)
            {
                MessageBox.Show("Saved successfully");
            }
            if (Login.MaKhoa == "QT")
            {
                comboBox2.Enabled = true;
            }
            ReloadSV();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ReloadSV();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow rw in this.dataGridView1.Rows)
            {
                if (!string.IsNullOrWhiteSpace(rw.Cells["MSSV"].Value as string))
                {
                    string MSSV = rw.Cells["MSSV"].Value.ToString();
                    var sinhvien = (from s in SV.tbl_Diem
                                    where s.MSSV == MSSV
                                    select s).ToList();
                    bool TN = true;
                    int sotinchi = 0;
                    if (sinhvien.Count > 0)
                    {
                        sotinchi = (int)(from d in SV.tbl_Diem
                                        join t in SV.tbl_DMMonHoc
                                        on d.MaMH equals t.MaMH into temp1
                                        from q in temp1.DefaultIfEmpty()
                                        where d.MSSV == MSSV
                                        select q.SoTinChi).Sum(); 
                    }
                    
                    if (sotinchi>=6)
                    {
                        foreach (var item in sinhvien)
                        {
                            if (item.Diem < 5)
                            {
                                TN = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        TN = false;
                    }

                    var sv = (from s in SV.tbl_SinhVien
                                    where s.MSSV == MSSV
                                    select s).FirstOrDefault();

                    sv.KetQuaTN = TN;

                    double tongdiem = 0;
                    
                    if (TN==true)
                    {
                        var diemtp =    (from d in SV.tbl_Diem
                                         join t in SV.tbl_DMMonHoc
                                         on d.MaMH equals t.MaMH into temp1
                                         from q in temp1.DefaultIfEmpty()
                                         where d.MSSV == MSSV
                                         select new
                                         {
                                             num = q.SoTinChi,
                                             score = d.Diem
                                         }).ToList();
                        foreach (var item in diemtp)
                        {
                            tongdiem += (double)item.num * (double)item.score;
                        }
                        sv.DTB = Math.Round((double)(tongdiem / sotinchi),2);
                        if (sv.DTB >= 8)
                        {
                            sv.XepLoaiTN = "Gioi";
                        }
                        else if (sv.DTB >=7)
                        {
                            sv.XepLoaiTN = "Kha";
                        }
                        else
                        {
                            sv.XepLoaiTN = "TB";
                        }
                    }
                }
            }
            SV.SaveChanges();
            MessageBox.Show("Xet TN 's done!");
            ReloadSV();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (first2 == true)
            {
                using (var stream = File.Open(path, FileMode.Open, FileAccess.Read))
                {

                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx)
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // 2. Use the AsDataSet extension method
                        var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                        {
                            UseColumnDataType = true,
                            ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                            {
                                UseHeaderRow = true
                            }
                        });
                        if (result.Tables[0].Columns.Contains("MaKhoa"))
                        {
                            if (Login.MaKhoa != "QT")
                            {
                                for (int i = result.Tables[0].Rows.Count - 1; i >= 0; i--)
                                {
                                    DataRow dr = result.Tables[0].Rows[i];
                                    if (Login.MaKhoa != dr["MaKhoa"].ToString())
                                    {
                                        result.Tables[0].Rows.Remove(dr);
                                    }
                                }
                            }
                            else
                            {
                                comboBox2.SelectedIndex = 1;
                                comboBox2.Enabled = false;
                            }

                            dataGridView1.DataSource = null;
                            dataGridView1.Rows.Clear();
                            dataGridView1.DataSource = result.Tables[comboBox1.SelectedIndex];
                            label1.Text = dataGridView1.Rows.Count.ToString();
                            // The result of each spreadsheet is in result.Tables
                        }
                    }
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedValue != null)
            {

                if (first == true)
                {
                    if (comboBox2.SelectedValue.ToString() != "QT")
                    {
                        LoadPartDS(comboBox2.SelectedValue.ToString());
                    }
                    else
                    {
                        LoadFullDS();
                    }
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 8)
            {
                DialogResult confirm = MessageBox.Show("Are you fucking sure ?", "Warning", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    string MSSV = dataGridView1.Rows[e.RowIndex].Cells["MSSV"].Value.ToString();
                    SV.tbl_SinhVien.Remove((from s in SV.tbl_SinhVien
                                            where s.MSSV == MSSV
                                            select s).FirstOrDefault());
                    var o = (from s in SV.tbl_Diem
                             where s.MSSV == MSSV
                             select s).ToList();
                    for (int i = 0; i < o.Count; i++)
                    {
                        SV.tbl_Diem.Remove(o[i]);
                    }

                    SV.SaveChanges();
                    MessageBox.Show("Deleted successfully!");
                    ReloadSV();
                }
            }
        }

        private void QLDSSV_Load(object sender, EventArgs e)
        {
            ReloadSV();
        }

        private void ReloadSV()
        {
            comboBox1.Items.Clear();
            comboBox1.DataSource = null;
            comboBox1.Enabled = false;
            if (Login.MaKhoa == "QT")
            {
                first = false;
                comboBox2.DataSource = null;
                comboBox2.Items.Clear();
                List<tbl_Khoa> p = (from s in SV.tbl_Khoa
                                    select s).ToList();
                comboBox2.DataSource = p;
                comboBox2.DisplayMember = "TenKhoa";
                comboBox2.ValueMember = "MaKhoa";
                comboBox2.SelectedIndex = 1;
                first = true;
                LoadFullDS();
            }
            else
            {
                comboBox2.Enabled = false;
                LoadPartDS(Login.MaKhoa);
            }
        }

        public void LoadFullDS()
        {
            dataGridView1.AutoGenerateColumns = false;
            var DS = (from s in SV.tbl_SinhVien
                      join d in SV.tbl_Diem
                      on s.MSSV equals d.MSSV into temp1
                      from t in temp1.DefaultIfEmpty()
                      join m in SV.tbl_DMMonHoc
                      on t.MaMH equals m.MaMH into temp2
                      from p in temp2.DefaultIfEmpty()
                      orderby s.MSSV
                      select new
                      {
                          s.MSSV,
                          s.Ho,
                          s.Ten,
                          s.MaKhoa,
                          t.MaMH,
                          p.TenMH,
                          t.Diem,
                          s.KetQuaTN,
                      });

            DataTable dt = ConvertDatatable.LINQToDataTable(DS);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.DataSource = dt;
            label1.Text = dataGridView1.RowCount.ToString();
        }

        public void LoadPartDS(string k)
        {
            dataGridView1.AutoGenerateColumns = false;
            var DS = (from s in SV.tbl_SinhVien
                      join d in SV.tbl_Diem
                      on s.MSSV equals d.MSSV into temp1
                      from t in temp1.DefaultIfEmpty()
                      join m in SV.tbl_DMMonHoc
                      on t.MaMH equals m.MaMH into temp2
                      from p in temp2.DefaultIfEmpty()
                      where s.MaKhoa == k
                      orderby s.MSSV
                      select new
                      {
                          s.MSSV,
                          s.Ho,
                          s.Ten,
                          s.MaKhoa,
                          t.MaMH,
                          p.TenMH,
                          t.Diem,
                          s.KetQuaTN,
                      });
            DataTable dt = ConvertDatatable.LINQToDataTable(DS);
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.DataSource = dt;
            label1.Text = dataGridView1.RowCount.ToString();
        }
    }
}
