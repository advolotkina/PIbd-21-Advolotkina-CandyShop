using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CandyShopView
{
    public partial class FormCandyIngredient : Form
    {

        public CandyIngredientViewModel Model { set { model = value; }  get { return model; } }

        private CandyIngredientViewModel model;

        public FormCandyIngredient()
        {
            InitializeComponent();
        }

        private void FormCandyIngredient_Load(object sender, EventArgs e)
        {
            try
            {
                var response = ClientAPI.GetRequest("api/Ingredient/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    comboBoxComponent.DisplayMember = "IngredientName";
                    comboBoxComponent.ValueMember = "Id";
                    comboBoxComponent.DataSource = ClientAPI.GetElement<List<IngredientViewModel>>(response);
                    comboBoxComponent.SelectedItem = null;
                }
                else
                {
                    throw new Exception(ClientAPI.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (model != null)
            {
                comboBoxComponent.Enabled = false;
                comboBoxComponent.SelectedValue = model.IngredientId;
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxComponent.SelectedValue == null)
            {
                MessageBox.Show("Выберите ингредиент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new CandyIngredientViewModel
                    {
                        IngredientId = Convert.ToInt32(comboBoxComponent.SelectedValue),
                        IngredientName = comboBoxComponent.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
                }
                MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
