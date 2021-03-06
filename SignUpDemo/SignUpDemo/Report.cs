﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Drawing.Printing;

namespace SignUpDemo
{
    public partial class Report : Form
    {
        public Report()
        {
            InitializeComponent();
        }

        private void Report_Load(object sender, EventArgs e)
        {
            
            SinhVienEntities1 SV = new SinhVienEntities1();
            DataTable dt;
            if (Login.MaKhoa == "QT")
            {
                var DS = (from s in SV.tbl_SinhVien
                          select s).ToList();
                dt = ConvertDatatable.LINQToDataTable(DS);
            }
            else
            {
                var DS = (from s in SV.tbl_SinhVien
                          where s.MaKhoa == Login.MaKhoa
                          select s).ToList();
                dt = ConvertDatatable.LINQToDataTable(DS);
            }
            
            PageSettings pg = new PageSettings();

            pg.Margins = new Margins(0, 0, 0, 0);
            pg.PaperSize = new PaperSize("A4", 1169, 827);
            pg.PaperSize.RawKind = (int)PaperKind.A4;

            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource rpds = new ReportDataSource("SinhVienReport", dt);
            reportViewer1.ProcessingMode = ProcessingMode.Local;
            reportViewer1.LocalReport.EnableExternalImages = true;
            reportViewer1.LocalReport.DataSources.Add(rpds);

            ReportParameter[] parameters = new ReportParameter[1];    // So luong parameters
            parameters[0] = new ReportParameter("Signature", "WhiteK");
            this.reportViewer1.LocalReport.SetParameters(parameters);

            this.reportViewer1.SetPageSettings(pg);
            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            this.reportViewer1.RefreshReport();
        }
    }
}
