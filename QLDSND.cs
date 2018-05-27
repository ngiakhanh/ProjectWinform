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
    public partial class QLDSND : Form
    {
        SinhVienEntities1 SV = new SinhVienEntities1();

        public static int QLDSND_ID;

        public QLDSND()
        {
            InitializeComponent();
        }

        public void LoadDS()
        {
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            dataGridView1.AutoGenerateColumns = false;
            var DS = (from s in SV.tbl_DangNhap
                      select s).ToArray();
            dataGridView1.DataSource = DS;
            label1.Text = dataGridView1.RowCount.ToString();
        }

        private void QLDSND_Load(object sender, EventArgs e)
        {
            LoadDS();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 4)
            {
                DCTTTK edit = new DCTTTK();
                QLDSND_ID = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                edit.Show();
            }
            else if (dataGridView1.CurrentCell.ColumnIndex == 5)
            {
                DialogResult confirm = MessageBox.Show("Are you fucking sure ?", "Warning", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    int ID = Int32.Parse(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value.ToString());
                    SV.tbl_DangNhap.Remove((from s in SV.tbl_DangNhap
                                            where s.ID == ID
                                            select s).FirstOrDefault());
                    SV.SaveChanges();
                    MessageBox.Show("Deleted successfully!");
                    Main.ReloadND();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QLDSND_ID = -1;
            DCTTTK edit = new DCTTTK();
            edit.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadDS();
        }
    }
}
