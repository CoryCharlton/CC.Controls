namespace CC.Controls
{
    partial class InputBoxForm
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
            this._TableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this._TextBoxInput = new System.Windows.Forms.TextBox();
            this._LabelPrompt = new System.Windows.Forms.Label();
            this._TableLayoutPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // _TableLayoutPanelMain
            // 
            this._TableLayoutPanelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._TableLayoutPanelMain.ColumnCount = 3;
            this._TableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelMain.Controls.Add(this._TextBoxInput, 1, 3);
            this._TableLayoutPanelMain.Controls.Add(this._LabelPrompt, 1, 1);
            this._TableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this._TableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0);
            this._TableLayoutPanelMain.Name = "_TableLayoutPanelMain";
            this._TableLayoutPanelMain.RowCount = 4;
            this._TableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this._TableLayoutPanelMain.Size = new System.Drawing.Size(296, 65);
            this._TableLayoutPanelMain.TabIndex = 0;
            // 
            // _TextBoxInput
            // 
            this._TextBoxInput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._TextBoxInput.Location = new System.Drawing.Point(9, 42);
            this._TextBoxInput.Margin = new System.Windows.Forms.Padding(0);
            this._TextBoxInput.Name = "_TextBoxInput";
            this._TextBoxInput.Size = new System.Drawing.Size(278, 20);
            this._TextBoxInput.TabIndex = 1;
            // 
            // _LabelPrompt
            // 
            this._LabelPrompt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._LabelPrompt.AutoSize = true;
            this._LabelPrompt.Location = new System.Drawing.Point(9, 13);
            this._LabelPrompt.Margin = new System.Windows.Forms.Padding(0);
            this._LabelPrompt.Name = "_LabelPrompt";
            this._LabelPrompt.Size = new System.Drawing.Size(278, 13);
            this._LabelPrompt.TabIndex = 0;
            this._LabelPrompt.Text = "Prompt";
            // 
            // InputBoxForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 107);
            this.Controls.Add(this._TableLayoutPanelMain);
            this.Name = "InputBoxForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "InputBoxForm";
            this.Controls.SetChildIndex(this._TableLayoutPanelMain, 0);
            this._TableLayoutPanelMain.ResumeLayout(false);
            this._TableLayoutPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _TableLayoutPanelMain;
        private System.Windows.Forms.TextBox _TextBoxInput;
        private System.Windows.Forms.Label _LabelPrompt;
    }
}