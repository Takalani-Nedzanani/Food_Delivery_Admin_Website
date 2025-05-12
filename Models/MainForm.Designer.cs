//namespace FoodDeliveryAdminWebsite.Models
//{
//    partial class MainForm
//    {
//        private System.ComponentModel.IContainer components = null;

//        private DataGridView ordersDataGridView;
//        private DataGridView menuItemsDataGridView;
//        private ComboBox statusComboBox;
//        private Button updateStatusButton;
//        private Button addMenuItemButton;
//        private Button deleteMenuItemButton;
//        private TabControl tabControl1;
//        private TabPage ordersTabPage;
//        private TabPage menuTabPage;

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private void InitializeComponent()
//        {
//            this.ordersDataGridView = new System.Windows.Forms.DataGridView();
//            this.statusComboBox = new System.Windows.Forms.ComboBox();
//            this.updateStatusButton = new System.Windows.Forms.Button();
//            this.menuItemsDataGridView = new System.Windows.Forms.DataGridView();
//            this.addMenuItemButton = new System.Windows.Forms.Button();
//            this.deleteMenuItemButton = new System.Windows.Forms.Button();
//            this.tabControl1 = new System.Windows.Forms.TabControl();
//            this.ordersTabPage = new System.Windows.Forms.TabPage();
//            this.menuTabPage = new System.Windows.Forms.TabPage();
//            ((System.ComponentModel.ISupportInitialize)(this.ordersDataGridView)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.menuItemsDataGridView)).BeginInit();
//            this.tabControl1.SuspendLayout();
//            this.ordersTabPage.SuspendLayout();
//            this.menuTabPage.SuspendLayout();
//            this.SuspendLayout();

//            // ordersDataGridView
//            this.ordersDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.ordersDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "OrderId", HeaderText = "Order ID" },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "UserName", HeaderText = "Customer" },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Timestamp", HeaderText = "Time" },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "ItemCount", HeaderText = "Items" },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Total", HeaderText = "Total" },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Status", HeaderText = "Status" }
//        });
//            this.ordersDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.ordersDataGridView.Location = new System.Drawing.Point(3, 3);
//            this.ordersDataGridView.Name = "ordersDataGridView";
//            this.ordersDataGridView.Size = new System.Drawing.Size(786, 369);
//            this.ordersDataGridView.TabIndex = 0;

//            // statusComboBox
//            this.statusComboBox.FormattingEnabled = true;
//            this.statusComboBox.Items.AddRange(new object[] {
//            "pending",
//            "preparing",
//            "ready",
//            "collected"
//        });
//            this.statusComboBox.Location = new System.Drawing.Point(10, 380);
//            this.statusComboBox.Name = "statusComboBox";
//            this.statusComboBox.Size = new System.Drawing.Size(150, 21);
//            this.statusComboBox.TabIndex = 1;

//            // updateStatusButton
//            this.updateStatusButton.Location = new System.Drawing.Point(166, 378);
//            this.updateStatusButton.Name = "updateStatusButton";
//            this.updateStatusButton.Size = new System.Drawing.Size(120, 23);
//            this.updateStatusButton.TabIndex = 2;
//            this.updateStatusButton.Text = "Update Status";
//            this.updateStatusButton.UseVisualStyleBackColor = true;
//            this.updateStatusButton.Click += new System.EventHandler(this.updateStatusButton_Click);

//            // menuItemsDataGridView
//            this.menuItemsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
//            this.menuItemsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "MenuItemId", HeaderText = "ID", Visible = false },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Name", HeaderText = "Name" },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Description", HeaderText = "Description" },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Price", HeaderText = "Price" },
//            new System.Windows.Forms.DataGridViewTextBoxColumn() { Name = "Category", HeaderText = "Category" }
//        });
//            this.menuItemsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.menuItemsDataGridView.Location = new System.Drawing.Point(3, 3);
//            this.menuItemsDataGridView.Name = "menuItemsDataGridView";
//            this.menuItemsDataGridView.Size = new System.Drawing.Size(786, 369);
//            this.menuItemsDataGridView.TabIndex = 0;

//            // addMenuItemButton
//            this.addMenuItemButton.Location = new System.Drawing.Point(10, 380);
//            this.addMenuItemButton.Name = "addMenuItemButton";
//            this.addMenuItemButton.Size = new System.Drawing.Size(120, 23);
//            this.addMenuItemButton.TabIndex = 1;
//            this.addMenuItemButton.Text = "Add Menu Item";
//            this.addMenuItemButton.UseVisualStyleBackColor = true;
//            this.addMenuItemButton.Click += new System.EventHandler(this.addMenuItemButton_Click);

//            // deleteMenuItemButton
//            this.deleteMenuItemButton.Location = new System.Drawing.Point(136, 380);
//            this.deleteMenuItemButton.Name = "deleteMenuItemButton";
//            this.deleteMenuItemButton.Size = new System.Drawing.Size(120, 23);
//            this.deleteMenuItemButton.TabIndex = 2;
//            this.deleteMenuItemButton.Text = "Delete Selected";
//            this.deleteMenuItemButton.UseVisualStyleBackColor = true;
//            this.deleteMenuItemButton.Click += new System.EventHandler(this.deleteMenuItemButton_Click);

//            // tabControl1
//            this.tabControl1.Controls.Add(this.ordersTabPage);
//            this.tabControl1.Controls.Add(this.menuTabPage);
//            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tabControl1.Location = new System.Drawing.Point(0, 0);
//            this.tabControl1.Name = "tabControl1";
//            this.tabControl1.SelectedIndex = 0;
//            this.tabControl1.Size = new System.Drawing.Size(800, 450);
//            this.tabControl1.TabIndex = 3;

//            // ordersTabPage
//            this.ordersTabPage.Controls.Add(this.ordersDataGridView);
//            this.ordersTabPage.Controls.Add(this.statusComboBox);
//            this.ordersTabPage.Controls.Add(this.updateStatusButton);
//            this.ordersTabPage.Location = new System.Drawing.Point(4, 22);
//            this.ordersTabPage.Name = "ordersTabPage";
//            this.ordersTabPage.Padding = new System.Windows.Forms.Padding(3);
//            this.ordersTabPage.Size = new System.Drawing.Size(792, 424);
//            this.ordersTabPage.TabIndex = 0;
//            this.ordersTabPage.Text = "Orders";
//            this.ordersTabPage.UseVisualStyleBackColor = true;

//            // menuTabPage
//            this.menuTabPage.Controls.Add(this.menuItemsDataGridView);
//            this.menuTabPage.Controls.Add(this.addMenuItemButton);
//            this.menuTabPage.Controls.Add(this.deleteMenuItemButton);
//            this.menuTabPage.Location = new System.Drawing.Point(4, 22);
//            this.menuTabPage.Name = "menuTabPage";
//            this.menuTabPage.Padding = new System.Windows.Forms.Padding(3);
//            this.menuTabPage.Size = new System.Drawing.Size(792, 424);
//            this.menuTabPage.TabIndex = 1;
//            this.menuTabPage.Text = "Menu Items";
//            this.menuTabPage.UseVisualStyleBackColor = true;

//            // MainForm
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(800, 450);
//            this.Controls.Add(this.tabControl1);
//            this.Name = "MainForm";
//            this.Text = "Food Order Admin";
//            ((System.ComponentModel.ISupportInitialize)(this.ordersDataGridView)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.menuItemsDataGridView)).EndInit();
//            this.tabControl1.ResumeLayout(false);
//            this.ordersTabPage.ResumeLayout(false);
//            this.menuTabPage.ResumeLayout(false);
//            this.ResumeLayout(false);
//        }
//    }
//}
