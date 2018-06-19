using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CandyShopView
{
    public partial class FormConfectioner : Form
    {

        public int Id { set { id = value; } }

        private int? id;

        public FormConfectioner()
        {
            InitializeComponent();
        }

        private void FormConfectioner_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = ClientAPI.GetRequest("api/Confectioner/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var confectioner = ClientAPI.GetElement<ConfectionerViewModel>(response);
                        textBoxFIO.Text = confectioner.ConfectionerFIO;
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
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = ClientAPI.PostRequest("api/Confectioner/UpdElement", new ConfectionerBindingModel
                    {
                        Id = id.Value,
                        ConfectionerFIO = textBoxFIO.Text
                    });
                }
                else
                {
                    response = ClientAPI.PostRequest("api/Confectioner/AddElement", new ConfectionerBindingModel
                    {
                        ConfectionerFIO = textBoxFIO.Text
                    });
                }
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
