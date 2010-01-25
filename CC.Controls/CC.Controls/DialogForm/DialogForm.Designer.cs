namespace CC.Controls
{
    partial class DialogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this._TableLayoutPanelButtons = new System.Windows.Forms.TableLayoutPanel();
            this._ButtonCancel = new System.Windows.Forms.Button();
            this._ButtonOk = new System.Windows.Forms.Button();
            this._TableLayoutPanelButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // _TableLayoutPanelButtons
            // 
            this._TableLayoutPanelButtons.ColumnCount = 6;
            this._TableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this._TableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this._TableLayoutPanelButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelButtons.Controls.Add(this._ButtonCancel, 4, 1);
            this._TableLayoutPanelButtons.Controls.Add(this._ButtonOk, 2, 1);
            this._TableLayoutPanelButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._TableLayoutPanelButtons.Location = new System.Drawing.Point(0, 165);
            this._TableLayoutPanelButtons.Margin = new System.Windows.Forms.Padding(0);
            this._TableLayoutPanelButtons.Name = "_TableLayoutPanelButtons";
            this._TableLayoutPanelButtons.RowCount = 3;
            this._TableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelButtons.Size = new System.Drawing.Size(324, 42);
            this._TableLayoutPanelButtons.TabIndex = 0;
            // 
            // _ButtonCancel
            // 
            this._ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._ButtonCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ButtonCancel.Location = new System.Drawing.Point(240, 9);
            this._ButtonCancel.Margin = new System.Windows.Forms.Padding(0);
            this._ButtonCancel.Name = "_ButtonCancel";
            this._ButtonCancel.Size = new System.Drawing.Size(75, 24);
            this._ButtonCancel.TabIndex = 1;
            this._ButtonCancel.Text = "&Cancel";
            this._ButtonCancel.UseVisualStyleBackColor = true;
            this._ButtonCancel.Click += new System.EventHandler(this._ButtonCancel_Click);
            // 
            // _ButtonOk
            // 
            this._ButtonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this._ButtonOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this._ButtonOk.Location = new System.Drawing.Point(156, 9);
            this._ButtonOk.Margin = new System.Windows.Forms.Padding(0);
            this._ButtonOk.Name = "_ButtonOk";
            this._ButtonOk.Size = new System.Drawing.Size(75, 24);
            this._ButtonOk.TabIndex = 0;
            this._ButtonOk.Text = "&OK";
            this._ButtonOk.UseVisualStyleBackColor = true;
            this._ButtonOk.Click += new System.EventHandler(this._ButtonOk_Click);
            // 
            // DialogForm
            // 
            this.AcceptButton = this._ButtonOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._ButtonCancel;
            this.ClientSize = new System.Drawing.Size(324, 207);
            this.Controls.Add(this._TableLayoutPanelButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "DialogForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "DialogForm";
            this._TableLayoutPanelButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _TableLayoutPanelButtons;
        private System.Windows.Forms.Button _ButtonCancel;
        private System.Windows.Forms.Button _ButtonOk;
    }
}