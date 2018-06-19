using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CandyShopView
{
    public partial class FormCreatePurchaseOrder : Form
    {

        public FormCreatePurchaseOrder()
        {
            InitializeComponent();
        }

        private void FormCreatePurchaseOrder_Load(object sender, EventArgs e)
        {
            try
            {
                var responseC = ClientAPI.GetRequest("api/Customer/GetList");
                if (responseC.Result.IsSuccessStatusCode)
                {
                    List<CustomerViewModel> list = ClientAPI.GetElement<List<CustomerViewModel>>(responseC);
                    if (list != null)
                    {
                        comboBoxClient.DisplayMember = "CustomerFIO";
                        comboBoxClient.ValueMember = "Id";
                        comboBoxClient.DataSource = list;
                        comboBoxClient.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(ClientAPI.GetError(responseC));
                }
                var responseP = ClientAPI.GetRequest("api/Candy/GetList");
                if (responseP.Result.IsSuccessStatusCode)
                {
                    List<CandyViewModel> list = ClientAPI.GetElement<List<CandyViewModel>>(responseP);
                    if (list != null)
                    {
                        comboBoxProduct.DisplayMember = "CandyName";
                        comboBoxProduct.ValueMember = "Id";
                        comboBoxProduct.DataSource = list;
                        comboBoxProduct.SelectedItem = null;
                    }
                }
                else
                {
                    throw new Exception(ClientAPI.GetError(responseP));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalcSum()
        {
            if (comboBoxProduct.SelectedValue != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxProduct.SelectedValue);
                    var responseP = ClientAPI.GetRequest("api/Candy/Get/" + id);
                    if (responseP.Result.IsSuccessStatusCode)
                    {
                        CandyViewModel product = ClientAPI.GetElement<CandyViewModel>(responseP);
                        int count = Convert.ToInt32(textBoxCount.Text);
                        textBoxSum.Text = (count * (int)product.Price).ToString();
                    }
                    else
                    {
                        throw new Exception(ClientAPI.GetError(responseP));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxClient.SelectedValue == null)
            {
                MessageBox.Show("Выберите покупателя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Выберите сладость", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = ClientAPI.PostRequest("api/Main/CreatePurchaseOrder", new PurchaseOrderBindingModel
                {
                    CustomerId = Convert.ToInt32(comboBoxClient.SelectedValue),
                    CandyId = Convert.ToInt32(comboBoxProduct.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToInt32(textBoxSum.Text)
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
