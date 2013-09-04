using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using CCMS.UI.Features.Reports;
using Microsoft.Reporting;
using Microsoft.Reporting.WinForms;

namespace CCMS.UI.Features.Reports
{
    /// <summary>
    /// Interaction logic for StatementSummaryRerportView.xaml
    /// </summary>
    public partial class StatementSummaryView : Window
    {
        private readonly BindingSource _bindingSource;
        private readonly IContainer _container;  

        public StatementSummaryView()
        {
            InitializeComponent();
            
            // initialize report viewer
            _container = new Container();
            _bindingSource = new BindingSource(_container);

            ((ISupportInitialize)(_bindingSource)).BeginInit();
            _reportViewer.LocalReport.DataSources.Add(new ReportDataSource()
            {
                Name = "ItemDataSet",
                Value = _bindingSource,
            });
            _reportViewer.LocalReport.ReportEmbeddedResource = "CCMS.UI.Features.Reports.StatementSummaryReport.rdlc";
            ((ISupportInitialize)(_bindingSource)).EndInit();

            // load report
            this.Loaded += (sender, e) =>
            {
                _bindingSource.DataSource = ((StatementSummaryViewModel)this.DataContext).Items;
                _reportViewer.RefreshReport();
            };
        }
    }
}
