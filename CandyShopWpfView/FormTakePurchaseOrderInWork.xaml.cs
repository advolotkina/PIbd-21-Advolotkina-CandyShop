using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для FormTakePurchaseOrderInWork.xaml
    /// </summary>
    public partial class FormTakePurchaseOrderInWork : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IConfectionerService serviceI;

        private readonly IMainService serviceM;

        private int? id;

        public FormTakePurchaseOrderInWork(IConfectionerService serviceI, IMainService serviceM)
        {
            InitializeComponent();
            this.serviceI = serviceI;
            this.serviceM = serviceM;
            Loaded += FormTakePurchaseOrderInWork_Load;
        }

        private void FormTakePurchaseOrderInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Close();
                }
                List<ConfectionerViewModel> listI = serviceI.GetList();
                if (listI != null)
                {
                    comboBoxConfectioner.DisplayMemberPath = "ConfectionerFIO";
                    comboBoxConfectioner.SelectedValuePath = "Id";
                    comboBoxConfectioner.ItemsSource = listI;
                    comboBoxConfectioner.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxConfectioner.SelectedValue == null)
            {
                MessageBox.Show("Выберите кондитера", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                serviceM.TakePurchaseOrderInWork(new PurchaseOrderBindingModel
                {
                    Id = id.Value,
                    ConfectionerId = Convert.ToInt32(comboBoxConfectioner.SelectedValue)
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
