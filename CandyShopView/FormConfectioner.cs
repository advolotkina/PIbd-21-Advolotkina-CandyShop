using CandyShopService.BindingModels;
using CandyShopService.Interfaces;
using CandyShopService.ViewModels;
using System;
using System.Windows.Forms;
using Unity;
using Unity.Attributes;

namespace CandyShopView
{
    public partial class FormConfectioner : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IConfectionerService service;

        private int? id;

        public FormConfectioner(IConfectionerService service)
        {
            InitializeComponent();
            this.service = service;
        }

        private void FormConfectioner_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    ConfectionerViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxFIO.Text = view.ConfectionerFIO;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxFIO.Text))
            {
                MessageBox.Show("Заполните ФИО", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new ConfectionerBindingModel
                    {
                        Id = id.Value,
                        ConfectionerFIO = textBoxFIO.Text
                    });
                }
                else
                {
                    service.AddElement(new ConfectionerBindingModel
                    {
                        ConfectionerFIO = textBoxFIO.Text
                    });
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
