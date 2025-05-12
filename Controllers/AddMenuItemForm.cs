//using System;
//using System.Windows.Forms;

//namespace FoodDeliveryAdminWebsite.Controllers
//{
//    public partial class AddMenuItemForm : Form
//    {
//        public string ItemName => nameTextBox.Text;
//        public string Description => descriptionTextBox.Text;
//        public double Price => double.Parse(priceTextBox.Text);
//        public string Category => categoryComboBox.SelectedItem.ToString();
//        public string ImageUrl => imageUrlTextBox.Text;

//        public AddMenuItemForm()
//        {
//            InitializeComponent();

//            // Add sample categories
//            categoryComboBox.Items.AddRange(new object[] {
//                "Appetizers",
//                "Main Courses",
//                "Desserts",
//                "Drinks",
//                "Specials"
//            });
//            categoryComboBox.SelectedIndex = 0;
//        }

//        private void saveButton_Click(object sender, EventArgs e)
//        {
//            if (ValidateForm())
//            {
//                DialogResult = DialogResult.OK;
//                Close();
//            }
//        }

//        private bool ValidateForm()
//        {
//            if (string.IsNullOrWhiteSpace(ItemName))
//            {
//                MessageBox.Show("Please enter a name for the menu item.");
//                return false;
//            }

//            if (!double.TryParse(priceTextBox.Text, out double price) || price <= 0)
//            {
//                MessageBox.Show("Please enter a valid price.");
//                return false;
//            }

//            return true;
//        }

//        private void cancelButton_Click(object sender, EventArgs e)
//        {
//            DialogResult = DialogResult.Cancel;
//            Close();
//        }
//    }
//}
