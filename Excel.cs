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
    public partial class Excel : Form
    {
        SinhVienEntities1 SV = new SinhVienEntities1();

        string path;

        bool first = false;
        public Excel()
        {
            InitializeComponent();
        }

        private void Export_Click(object sender, EventArgs e)
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
                MessageBox.Show("Saved succesfully at " + FolderPath);
            }
        }

        private void Import_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|XLS files (*.xls, *.xlt)|*.xls;*.xlt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                first = false;
                path = openFileDialog1.FileName;
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
                        first = true;
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();
                        dataGridView1.DataSource = result.Tables[0];
                        label1.Text = dataGridView1.Rows.Count.ToString();
                        // The result of each spreadsheet is in result.Tables
                    }
                }

            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow rw in this.dataGridView1.Rows)
                {
                    tbl_Khoa khoa = new tbl_Khoa();
                    if (rw.Cells[0].Value == null || rw.Cells[0].Value == DBNull.Value || String.IsNullOrWhiteSpace(rw.Cells[0].Value.ToString()))
                    {
                        continue;
                    }
                    khoa.MaKhoa = rw.Cells[0].Value.ToString();
                    khoa.TenKhoa = rw.Cells[1].Value.ToString();
                    SV.tbl_Khoa.Add(khoa);
                }
                SV.SaveChanges();
                MessageBox.Show("Added successfully");
            }
            catch (Exception)
            {
                MessageBox.Show("Added unsuccessfully");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (first == true)
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
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();
                        dataGridView1.DataSource = result.Tables[comboBox1.SelectedIndex];
                        label1.Text = dataGridView1.Rows.Count.ToString();
                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "XLSX files (*.xlsx, *.xlsm, *.xltx, *.xltm)|*.xlsx;*.xlsm;*.xltx;*.xltm|XLS files (*.xls, *.xlt)|*.xls;*.xlt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                using (XLWorkbook workBook = new XLWorkbook(path))
                {
                    //Read the first Sheet from Excel file.
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        //Use the first row to add columns to DataTable.
                        if (firstRow)
                        {
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            firstRow = false;
                        }
                        else
                        {
                            //Add rows to DataTable.
                            dt.Rows.Add();
                            int i = 0;
                            foreach (IXLCell cell in row.Cells())
                            {
                                dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                i++;
                            }
                        }
                        dataGridView1.DataSource = null;
                        dataGridView1.Rows.Clear();
                        dataGridView1.DataSource = dt;
                        label1.Text = dataGridView1.Rows.Count.ToString();
                    }
                }
            }
        }
    }
}
