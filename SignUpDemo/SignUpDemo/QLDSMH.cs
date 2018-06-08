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
    public partial class QLDSMH : Form
    {
        SinhVienEntities1 SV = new SinhVienEntities1();

        public bool First { get; set; } = false;

        public QLDSMH()
        {
            InitializeComponent();
        }
        public void LoadFullDS()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.AutoGenerateColumns = false;
            var DS = (from s in SV.tbl_DMMonHoc
                      select s).ToArray();
            dataGridView1.DataSource = DS;
            label1.Text = dataGridView1.RowCount.ToString();
        }

        public void LoadPartDS(string k)
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.AutoGenerateColumns = false;
            var DS = (from s in SV.tbl_DMMonHoc
                      where s.MaKhoa == k
                      select s).ToArray();
            dataGridView1.DataSource = DS;
            label1.Text = dataGridView1.RowCount.ToString();
        }

        private void QLDSMH_Load(object sender, EventArgs e)
        {
            ReloadDS();
        }

        private void ReloadDS()
        {
            if (Login.MaKhoa == "QT")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                var p = (from s in SV.tbl_Khoa
                                    select s).ToList();
                comboBox1.DataSource = p;
                comboBox1.DisplayMember = "TenKhoa";
                comboBox1.ValueMember = "MaKhoa";
                comboBox1.SelectedIndex = 1;
                First = true;
                LoadFullDS();
            }
            else
            {
                comboBox1.Enabled = false;
                LoadPartDS(Login.MaKhoa);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ReloadDS();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue.ToString() != "QT")
            {
                LoadPartDS(comboBox1.SelectedValue.ToString());
            }
            else
            {
                LoadFullDS();
            }
        }
    }
}
