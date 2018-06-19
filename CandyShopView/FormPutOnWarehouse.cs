using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CandyShopView
{
    public partial class FormPutOnWarehouse : Form
    {
        public FormPutOnWarehouse()
        {
            InitializeComponent();
        }

        private void FormPutOnWarehouse_Load(object sender, EventArgs e)
        {
            try
            {
                var responseI = ClientAPI.GetRequest("api/Ingredient/GetList");
                if (responseI.Result.IsSuccessStatusCode)
                {
                    List<IngredientViewModel> list = ClientAPI.GetElement<List<IngredientViewModel>>(responseI);
                    if (list != null)
                    {
                        comboBoxComponent.DisplayMember = "IngredientName";
                        comboBoxComponent.ValueMember = "Id";
                        comboBoxComponent.DataSource = list;
                        comboBoxComponent.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(ClientAPI.GetError(responseI));
                }
                var responseW = ClientAPI.GetRequest("api/Warehouse/GetList");
                if (responseW.Result.IsSuccessStatusCode)
                {
                    List<WarehouseViewModel> list = ClientAPI.GetElement<List<WarehouseViewModel>>(responseW);
                    if (list != null)
                    {
                        comboBoxStock.DisplayMember = "WarehouseName";
                        comboBoxStock.ValueMember = "Id";
                        comboBoxStock.DataSource = list;
                        comboBoxStock.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(ClientAPI.GetError(responseI));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxStock.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = ClientAPI.PostRequest("api/Main/PutIngredientOnStock", new WarehouseIngredientBindingModel
                {
                    IngredientId = Convert.ToInt32(comboBoxComponent.SelectedValue),
                    WarehouseId = Convert.ToInt32(comboBoxStock.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DialogResult = DialogResult.OK;
                    Close();
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
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
