using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace CandyShopView
{
    public partial class FormTakePurchaseOrderInWork : Form
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
                List<ConfectionerViewModel> listI = serviceI.GetList();
                if (listI != null)
                {
                    comboBoxImplementer.DisplayMember = "ConfectionerFIO";
                    comboBoxImplementer.ValueMember = "Id";
                    comboBoxImplementer.DataSource = listI;
                    comboBoxImplementer.SelectedItem = null;
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
                MessageBox.Show("Выберите кондитера", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                serviceM.TakePurchaseOrderInWork(new PurchaseOrderBindingModel
                {
                    Id = id.Value,
                    ConfectionerId = Convert.ToInt32(comboBoxImplementer.SelectedValue)
                });
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
