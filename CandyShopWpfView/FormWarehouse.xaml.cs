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
    /// Логика взаимодействия для FormWarehouse.xaml
    /// </summary>
    public partial class FormWarehouse : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IWarehouseService service;

        private int? id;

        public FormWarehouse(IWarehouseService service)
        {
            InitializeComponent();
            this.service = service;
            Loaded += FormWarehouse_Load;
        }

        private void FormWarehouse_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    WarehouseViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.WarehouseName;
                        dataGridView.ItemsSource = view.WarehouseIngredients;
                        dataGridView.Columns[0].Visibility = Visibility.Hidden;
                        dataGridView.Columns[1].Visibility = Visibility.Hidden;
                        dataGridView.Columns[2].Visibility = Visibility.Hidden;
                        dataGridView.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new WarehouseBindingModel
                    {
                        Id = id.Value,
                        WarehouseName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new WarehouseBindingModel
                    {
                        WarehouseName = textBoxName.Text
                    });
                }
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
