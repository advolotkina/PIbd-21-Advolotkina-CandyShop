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
                    var confectioner = Task.Run(() => ClientAPI.GetRequestData<ConfectionerViewModel>("api/Confectioner/Get/" + id.Value)).Result;
                    textBoxFIO.Text = confectioner.ConfectionerFIO;
                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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
            string fio = textBoxFIO.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => ClientAPI.PostRequestData("api/Confectioner/UpdElement", new ConfectionerBindingModel
                {
                    Id = id.Value,
                    ConfectionerFIO = fio
                }));
            }
            else
            {
                task = Task.Run(() => ClientAPI.PostRequestData("api/Confectioner/AddElement", new ConfectionerBindingModel
                {
                    ConfectionerFIO = fio
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
