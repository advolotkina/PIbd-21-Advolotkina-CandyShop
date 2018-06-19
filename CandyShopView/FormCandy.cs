using CandyShopService.BindingModels;
using CandyShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CandyShopView
{
    public partial class FormCandy : Form
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<CandyIngredientViewModel> candyIngredients;

        public FormCandy()
        {
            InitializeComponent();
        }

        private void FormProduct_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = ClientAPI.GetRequest("api/Candy/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var candy = ClientAPI.GetElement<CandyViewModel>(response);
                        textBoxName.Text = candy.CandyName;
                        textBoxPrice.Text = candy.Price.ToString();
                        candyIngredients = candy.CandyIngredients;
                        LoadData();
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
                    dataGridView.DataSource = null;
                    dataGridView.DataSource = candyIngredients;
                    dataGridView.Columns[0].Visible = false;
                    dataGridView.Columns[1].Visible = false;
                    dataGridView.Columns[2].Visible = false;
                    dataGridView.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormCandyIngredient();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if(form.Model != null)
                {
                    if(id.HasValue)
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
            if (dataGridView.SelectedRows.Count == 1)
            {
                var form = new FormCandyIngredient();
                form.Model = candyIngredients[dataGridView.SelectedRows[0].Cells[0].RowIndex];
                if (form.ShowDialog() == DialogResult.OK)
                {
                    candyIngredients[dataGridView.SelectedRows[0].Cells[0].RowIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        candyIngredients.RemoveAt(dataGridView.SelectedRows[0].Cells[0].RowIndex);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (candyIngredients == null || candyIngredients.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                List<CandyIngredientBindingModel> candyIngredientBM = new List<CandyIngredientBindingModel>();
                for (int i = 0; i < candyIngredients.Count; ++i)
                {
                    candyIngredientBM.Add(new CandyIngredientBindingModel
                    {
                        Id = candyIngredients[i].Id,
                        CandyId = candyIngredients[i].CandyId,
                        IngredientId = candyIngredients[i].IngredientId,
                        Count = candyIngredients[i].Count
                    });
                }
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = ClientAPI.PostRequest("api/Candy/UpdElement", new CandyBindingModel
                    {
                        Id = id.Value,
                        CandyName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CandyIngredients = candyIngredientBM
                    });
                }
                else
                {
                    response = ClientAPI.PostRequest("api/Candy/AddElement", new CandyBindingModel
                    {
                        CandyName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CandyIngredients = candyIngredientBM
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
