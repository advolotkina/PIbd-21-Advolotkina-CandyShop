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
    /// Логика взаимодействия для FormCandy.xaml
    /// </summary>
    public partial class FormCandy : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly ICandyService service;

        private int? id;

        private List<CandyIngredientViewModel> candyIngredients;

        public FormCandy(ICandyService service)
        {
            InitializeComponent();
            this.service = service;
            Loaded += FormCandy_Load;
        }

        private void FormCandy_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    CandyViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.CandyName;
                        textBoxPrice.Text = view.Price.ToString();
                        candyIngredients = view.CandyIngredients;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                candyIngredients = new List<CandyIngredientViewModel>();
            }
        }

        private void LoadData()
        {
            try
            {
                if (candyIngredients != null)
                {
                    dataGridView.ItemsSource = null;
                    dataGridView.ItemsSource = candyIngredients;
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

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCandyIngredient>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                    {
                        form.Model.CandyId = id.Value;
                    }
                    candyIngredients.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedItem!=null)
            {
                var form = Container.Resolve<FormCandyIngredient>();
                form.Model = candyIngredients[dataGridView.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    candyIngredients[dataGridView.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        candyIngredients.RemoveAt(dataGridView.SelectedIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (candyIngredients == null || candyIngredients.Count == 0)
            {
                MessageBox.Show("Заполните ингредиенты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<CandyIngredientBindingModel> productComponentBM = new List<CandyIngredientBindingModel>();
                for (int i = 0; i < candyIngredients.Count; ++i)
                {
                    productComponentBM.Add(new CandyIngredientBindingModel
                    {
                        Id = candyIngredients[i].Id,
                        CandyId = candyIngredients[i].CandyId,
                        IngredientId = candyIngredients[i].IngredientId,
                        Count = candyIngredients[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new CandyBindingModel
                    {
                        Id = id.Value,
                        CandyName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CandyIngredients = productComponentBM
                    });
                }
                else
                {
                    service.AddElement(new CandyBindingModel
                    {
                        CandyName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CandyIngredients = productComponentBM
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
