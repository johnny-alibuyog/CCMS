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
using Microsoft.Reporting.WinForms;
using ReactiveUI;

namespace CCMS.UI.Features.Reports
{
    /// <summary>
    /// Interaction logic for AnnualSummaryView.xaml
    /// </summary>
    public partial class AnnualSummaryView : Window, IViewFor<AnnualSummaryViewModel>
    {
        private readonly BindingSource _bindingSource;
        private readonly IContainer _container;

        public AnnualSummaryView()
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
            _reportViewer.LocalReport.ReportEmbeddedResource = "CCMS.UI.Features.Reports.AnnualSummaryReport.rdlc";
            ((ISupportInitialize)(_bindingSource)).EndInit();

            // set loading action
            this.Loaded += (sender, e) =>
            {
                this.ViewModel.Render = () =>
                {
                    _bindingSource.DataSource = this.ViewModel.Items;
                    _reportViewer.LocalReport.SetParameters(new ReportParameter("Year", this.ViewModel.Year.ToString()));
                    _reportViewer.RefreshReport();
                };
            };
        }

        #region IViewFor<AnnualSummaryViewModel> Members

        public AnnualSummaryViewModel ViewModel
        {
            get { return this.DataContext as AnnualSummaryViewModel; }
            set { this.DataContext = value; }
        }

        object IViewFor.ViewModel
        {
            get { return this.DataContext; }
            set { this.DataContext = value; }
        }

        #endregion
    }
}
