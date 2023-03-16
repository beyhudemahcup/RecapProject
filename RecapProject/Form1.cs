using System;
using System.Linq;
using System.Windows.Forms;

namespace RecapProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ListCategories();
            ListProducts();
        }

        private void ListProducts()
        {
            using (NorthwindContext northwindContext = new NorthwindContext())
            {
                dgwProduct.DataSource = northwindContext.Products.ToList();
            }
        }
        private void ListProductsByCategory(int categoryId)
        {
            using (NorthwindContext northwindContext = new NorthwindContext())
            {
                dgwProduct.DataSource =
                    northwindContext.Products.Where(p => p.CategoryId == categoryId).ToList();
            }
        }

        private void ListProductsByName(string productName)
        {
            using (NorthwindContext northwindContext = new NorthwindContext())
            {
                //I add ToLower in order to use this project in OracleDB
                dgwProduct.DataSource =
                    northwindContext.Products.Where(p => p.ProductName.ToLower().Contains(productName.ToLower())).ToList();
            }
        }

        private void ListCategories()
        {
            using (NorthwindContext northwindContext = new NorthwindContext())
            {
                cbxCategory.DataSource = northwindContext.Categories.ToList();
                cbxCategory.DisplayMember = "CategoryName";
                cbxCategory.ValueMember = "CategoryId";
            }
        }

        private void cbxCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            TryCatchMethod(() =>
            {
                ListProductsByCategory(Convert.ToInt32(cbxCategory.SelectedValue));
            });
        }

        private void TryCatchMethod(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch
            {
                //we do not do anything in the catch block to give a chance to ListProductsByCategory
                //for creating a list that have all the constraints
            }
        }

        private void tbxSearch_TextChanged(object sender, EventArgs e)
        {
            string key = tbxSearch.Text;
            if (string.IsNullOrEmpty(key))
            {
                ListProducts();
            }
            else
            {
                ListProductsByName(key);
            }
        }
    }
}
