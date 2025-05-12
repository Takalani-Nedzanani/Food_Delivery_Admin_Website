//using System;
//using System.Collections.Generic;
//using System.Windows.Forms;
//using System.Linq;
//using Firebase.Database;
//using Firebase.Database.Query;


//namespace FoodDeliveryAdminWebsite.Controllers
//{
//    public partial class MainForm : Form
//    {
//        private FirebaseClient firebase;
//        private const string FirebaseUrl = "YOUR_FIREBASE_URL";
//        private const string FirebaseSecret = "YOUR_FIREBASE_SECRET";

//        public MainForm()
//        {
//            InitializeComponent();
//            firebase = new FirebaseClient(
//                FirebaseUrl,
//                new FirebaseOptions
//                {
//                    AuthTokenAsyncFactory = () => Task.FromResult(FirebaseSecret)
//                });

//            LoadOrders();
//            LoadMenuItems();
//        }

//        private async void LoadOrders()
//        {
//            try
//            {
//                var orders = await firebase
//                    .Child("orders")
//                    .OrderByKey()
//                    .OnceAsync<Dictionary<string, object>>();

//                ordersDataGridView.Rows.Clear();

//                foreach (var order in orders)
//                {
//                    var orderData = order.Object;
//                    string orderId = order.Key;
//                    string status = orderData["status"].ToString();
//                    string userName = orderData.ContainsKey("userName") ? orderData["userName"].ToString() : "Unknown";
//                    double total = Convert.ToDouble(orderData["total"]);
//                    DateTime timestamp = DateTimeOffset.FromUnixTimeMilliseconds(
//                        Convert.ToInt64(orderData["timestamp"])).DateTime;

//                    // Count items
//                    int itemCount = 0;
//                    if (orderData.ContainsKey("items") && orderData["items"] is Dictionary<string, object> items)
//                    {
//                        itemCount = items.Count;
//                    }

//                    ordersDataGridView.Rows.Add(
//                        orderId,
//                        userName,
//                        timestamp.ToString("g"),
//                        itemCount,
//                        total.ToString("C"),
//                        status
//                    );
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error loading orders: {ex.Message}");
//            }
//        }

//        private async void LoadMenuItems()
//        {
//            try
//            {
//                var menuItems = await firebase
//                    .Child("menu")
//                    .OnceAsync<Dictionary<string, object>>();

//                menuItemsDataGridView.Rows.Clear();

//                foreach (var item in menuItems)
//                {
//                    var itemData = item.Object;
//                    menuItemsDataGridView.Rows.Add(
//                        item.Key,
//                        itemData["name"],
//                        itemData["description"],
//                        Convert.ToDouble(itemData["price"]).ToString("C"),
//                        itemData["category"]
//                    );
//                }
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error loading menu items: {ex.Message}");
//            }
//        }

//        private async void updateStatusButton_Click(object sender, EventArgs e)
//        {
//            if (ordersDataGridView.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("Please select an order first.");
//                return;
//            }

//            string orderId = ordersDataGridView.SelectedRows[0].Cells["OrderId"].Value.ToString();
//            string newStatus = statusComboBox.SelectedItem?.ToString();

//            if (string.IsNullOrEmpty(newStatus))
//            {
//                MessageBox.Show("Please select a status.");
//                return;
//            }

//            try
//            {
//                await firebase
//                    .Child("orders")
//                    .Child(orderId)
//                    .Child("status")
//                    .PutAsync(newStatus);

//                MessageBox.Show("Order status updated successfully.");
//                LoadOrders();
//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show($"Error updating order status: {ex.Message}");
//            }
//        }

//        private async void addMenuItemButton_Click(object sender, EventArgs e)
//        {
//            var addForm = new AddMenuItemForm();
//            if (addForm.ShowDialog() == DialogResult.OK)
//            {
//                try
//                {
//                    var newItem = new Dictionary<string, object>
//                    {
//                        { "name", addForm.ItemName },
//                        { "description", addForm.Description },
//                        { "price", addForm.Price },
//                        { "category", addForm.Category },
//                        { "imageUrl", addForm.ImageUrl }
//                    };

//                    await firebase
//                        .Child("menu")
//                        .PostAsync(newItem);

//                    MessageBox.Show("Menu item added successfully.");
//                    LoadMenuItems();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Error adding menu item: {ex.Message}");
//                }
//            }
//        }

//        private async void deleteMenuItemButton_Click(object sender, EventArgs e)
//        {
//            if (menuItemsDataGridView.SelectedRows.Count == 0)
//            {
//                MessageBox.Show("Please select a menu item first.");
//                return;
//            }

//            string itemId = menuItemsDataGridView.SelectedRows[0].Cells["MenuItemId"].Value.ToString();

//            if (MessageBox.Show("Are you sure you want to delete this menu item?",
//                "Confirm Delete", MessageBoxButtons.YesNo) == DialogResult.Yes)
//            {
//                try
//                {
//                    await firebase
//                        .Child("menu")
//                        .Child(itemId)
//                        .DeleteAsync();

//                    MessageBox.Show("Menu item deleted successfully.");
//                    LoadMenuItems();
//                }
//                catch (Exception ex)
//                {
//                    MessageBox.Show($"Error deleting menu item: {ex.Message}");
//                }
//            }
//        }
//    }
//}
