using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CandyShopView
{
    public partial class FormTakePurchaseOrderInWork : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        public FormTakePurchaseOrderInWork()
        {
            InitializeComponent();
        }

        private void FormTakePurchaseOrderInWork_Load(object sender, EventArgs e)
        {
            try
            {
                if (!id.HasValue)
                {
                    MessageBox.Show("Не указан заказ", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                }
                var response = ClientAPI.GetRequest("api/Confectioner/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<ConfectionerViewModel> list = ClientAPI.GetElement<List<ConfectionerViewModel>>(response);
                    if (list != null)
                    {
                        comboBoxImplementer.DisplayMember = "ConfectionerFIO";
                        comboBoxImplementer.ValueMember = "Id";
                        comboBoxImplementer.DataSource = list;
                        comboBoxImplementer.SelectedItem = null;
                    }
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxImplementer.SelectedValue == null)
            {
                MessageBox.Show("Выберите исполнителя", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                var response = ClientAPI.PostRequest("api/Main/TakePurchaseOrderInWork", new PurchaseOrderBindingModel
                {
                    Id = id.Value,
                    ConfectionerId = Convert.ToInt32(comboBoxImplementer.SelectedValue)
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
