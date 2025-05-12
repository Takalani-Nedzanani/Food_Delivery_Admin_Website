//namespace FoodDeliveryAdminWebsite.Models
//{
//    partial class AddMenuItemForm
//    {
//        private System.ComponentModel.IContainer components = null;

//        private Label nameLabel;
//        private TextBox nameTextBox;
//        private Label descriptionLabel;
//        private TextBox descriptionTextBox;
//        private Label priceLabel;
//        private TextBox priceTextBox;
//        private Label categoryLabel;
//        private ComboBox categoryComboBox;
//        private Label imageUrlLabel;
//        private TextBox imageUrlTextBox;
//        private Button saveButton;
//        private Button cancelButton;

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
//            this.nameLabel = new System.Windows.Forms.Label();
//            this.nameTextBox = new System.Windows.Forms.TextBox();
//            this.descriptionLabel = new System.Windows.Forms.Label();
//            this.descriptionTextBox = new System.Windows.Forms.TextBox();
//            this.priceLabel = new System.Windows.Forms.Label();
//            this.priceTextBox = new System.Windows.Forms.TextBox();
//            this.categoryLabel = new System.Windows.Forms.Label();
//            this.categoryComboBox = new System.Windows.Forms.ComboBox();
//            this.imageUrlLabel = new System.Windows.Forms.Label();
//            this.imageUrlTextBox = new System.Windows.Forms.TextBox();
//            this.saveButton = new System.Windows.Forms.Button();
//            this.cancelButton = new System.Windows.Forms.Button();
//            this.SuspendLayout();

//            // nameLabel
//            this.nameLabel.AutoSize = true;
//            this.nameLabel.Location = new System.Drawing.Point(12, 15);
//            this.nameLabel.Name = "nameLabel";
//            this.nameLabel.Size = new System.Drawing.Size(38, 13);
//            this.nameLabel.TabIndex = 0;
//            this.nameLabel.Text = "Name:";

//            // nameTextBox
//            this.nameTextBox.Location = new System.Drawing.Point(100, 12);
//            this.nameTextBox.Name = "nameTextBox";
//            this.nameTextBox.Size = new System.Drawing.Size(200, 20);
//            this.nameTextBox.TabIndex = 1;

//            // descriptionLabel
//            this.descriptionLabel.AutoSize = true;
//            this.descriptionLabel.Location = new System.Drawing.Point(12, 41);
//            this.descriptionLabel.Name = "descriptionLabel";
//            this.descriptionLabel.Size = new System.Drawing.Size(63, 13);
//            this.descriptionLabel.TabIndex = 2;
//            this.descriptionLabel.Text = "Description:";

//            // descriptionTextBox
//            this.descriptionTextBox.Location = new System.Drawing.Point(100, 38);
//            this.descriptionTextBox.Multiline = true;
//            this.descriptionTextBox.Name = "descriptionTextBox";
//            this.descriptionTextBox.Size = new System.Drawing.Size(200, 60);
//            this.descriptionTextBox.TabIndex = 3;

//            // priceLabel
//            this.priceLabel.AutoSize = true;
//            this.priceLabel.Location = new System.Drawing.Point(12, 107);
//            this.priceLabel.Name = "priceLabel";
//            this.priceLabel.Size = new System.Drawing.Size(34, 13);
//            this.priceLabel.TabIndex = 4;
//            this.priceLabel.Text = "Price:";

//            // priceTextBox
//            this.priceTextBox.Location = new System.Drawing.Point(100, 104);
//            this.priceTextBox.Name = "priceTextBox";
//            this.priceTextBox.Size = new System.Drawing.Size(100, 20);
//            this.priceTextBox.TabIndex = 5;

//            // categoryLabel
//            this.categoryLabel.AutoSize = true;
//            this.categoryLabel.Location = new System.Drawing.Point(12, 133);
//            this.categoryLabel.Name = "categoryLabel";
//            this.categoryLabel.Size = new System.Drawing.Size(52, 13);
//            this.categoryLabel.TabIndex = 6;
//            this.categoryLabel.Text = "Category:";

//            // categoryComboBox
//            this.categoryComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
//            this.categoryComboBox.FormattingEnabled = true;
//            this.categoryComboBox.Location = new System.Drawing.Point(100, 130);
//            this.categoryComboBox.Name = "categoryComboBox";
//            this.categoryComboBox.Size = new System.Drawing.Size(200, 21);
//            this.categoryComboBox.TabIndex = 7;

//            // imageUrlLabel
//            this.imageUrlLabel.AutoSize = true;
//            this.imageUrlLabel.Location = new System.Drawing.Point(12, 160);
//            this.imageUrlLabel.Name = "imageUrlLabel";
//            this.imageUrlLabel.Size = new System.Drawing.Size(61, 13);
//            this.imageUrlLabel.TabIndex = 8;
//            this.imageUrlLabel.Text = "Image URL:";

//            // imageUrlTextBox
//            this.imageUrlTextBox.Location = new System.Drawing.Point(100, 157);
//            this.imageUrlTextBox.Name = "imageUrlTextBox";
//            this.imageUrlTextBox.Size = new System.Drawing.Size(200, 20);
//            this.imageUrlTextBox.TabIndex = 9;

//            // saveButton
//            this.saveButton.Location = new System.Drawing.Point(144, 190);
//            this.saveButton.Name = "saveButton";
//            this.saveButton.Size = new System.Drawing.Size(75, 23);
//            this.saveButton.TabIndex = 10;
//            this.saveButton.Text = "Save";
//            this.saveButton.UseVisualStyleBackColor = true;
//            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);

//            // cancelButton
//            this.cancelButton.Location = new System.Drawing.Point(225, 190);
//            this.cancelButton.Name = "cancelButton";
//            this.cancelButton.Size = new System.Drawing.Size(75, 23);
//            this.cancelButton.TabIndex = 11;
//            this.cancelButton.Text = "Cancel";
//            this.cancelButton.UseVisualStyleBackColor = true;
//            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);

//            // AddMenuItemForm
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(312, 225);
//            this.Controls.Add(this.cancelButton);
//            this.Controls.Add(this.saveButton);
//            this.Controls.Add(this.imageUrlTextBox);
//            this.Controls.Add(this.imageUrlLabel);
//            this.Controls.Add(this.categoryComboBox);
//            this.Controls.Add(this.categoryLabel);
//            this.Controls.Add(this.priceTextBox);
//            this.Controls.Add(this.priceLabel);
//            this.Controls.Add(this.descriptionTextBox);
//            this.Controls.Add(this.descriptionLabel);
//            this.Controls.Add(this.nameTextBox);
//            this.Controls.Add(this.nameLabel);
//            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
//            this.MaximizeBox = false;
//            this.MinimizeBox = false;
//            this.Name = "AddMenuItemForm";
//            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
//            this.Text = "Add Menu Item";
//            this.ResumeLayout(false);
//            this.PerformLayout();
//        }
//    }
//}
