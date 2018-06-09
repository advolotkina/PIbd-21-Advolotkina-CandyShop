using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using Microsoft.Reporting.WinForms;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Unity;
using Unity.Attributes;

namespace CandyShopWpfView
{
    /// <summary>
    /// Логика взаимодействия для FormCustomerOrders.xaml
    /// </summary>
    public partial class FormCustomerOrders : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IReportService service;

        public FormCustomerOrders(IReportService service)
        {
            InitializeComponent();
            this.service = service;
            reportViewer.Load += FormCustomerOrders_Load;
        }

        private void FormCustomerOrders_Load(object sender, EventArgs e)
        {
            reportViewer.RefreshReport();
            this.reportViewer.LocalReport.ReportEmbeddedResource = "CandyShopWpfView.ReportCustomerPurchaseOrders.rdlc";
        }

        private void buttonMake_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate.Value.Date >= dateTimePickerTo.SelectedDate.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                ReportParameter parameter = new ReportParameter("ReportParameterPeriod",

                                            "c " + dateTimePickerFrom.ToString() +

                                            " по " + dateTimePickerTo.ToString());

                reportViewer.LocalReport.SetParameters(parameter);
                var dataSource = service.GetCustomerOrders(new ReportBindingModel

                {

                    DateFrom = dateTimePickerFrom.SelectedDate,

                    DateTo = dateTimePickerTo.SelectedDate

                });

                ReportDataSource source = new ReportDataSource("DataSetOrders", dataSource);

                reportViewer.LocalReport.DataSources.Add(source);

                reportViewer.RefreshReport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonToPdf_Click(object sender, EventArgs e)
        {
            if (dateTimePickerFrom.SelectedDate.Value.Date >= dateTimePickerTo.SelectedDate.Value.Date)
            {
                MessageBox.Show("Дата начала должна быть меньше даты окончания", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "pdf|*.pdf"
            };
            if (sfd.ShowDialog() == true)
            {
                try
                {
                    service.SaveCustomerOrders(new ReportBindingModel
                    {
                        FileName = sfd.FileName,
                        DateFrom = dateTimePickerFrom.SelectedDate.Value,
                        DateTo = dateTimePickerTo.SelectedDate.Value
                    });
                    MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
