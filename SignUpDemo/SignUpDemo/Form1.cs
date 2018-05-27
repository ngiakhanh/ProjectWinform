using PagedList;
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
    public partial class Form1 : Form
    {
        int page = 1;
        IPagedList<tbl_SinhVien> List;
        public Form1()
        {
            InitializeComponent();
        }
    }
}
