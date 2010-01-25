using System.Windows.Forms;

namespace CC.Controls
{
    partial class ParagraphDialog
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
            this._TableLayoutPanelInner = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._TableLayoutPanelIndent = new System.Windows.Forms.TableLayoutPanel();
            this._TextBoxHangingIndent = new CC.Controls.NumericTextBox();
            this._TextBoxRightIndent = new CC.Controls.NumericTextBox();
            this._LabelLeft = new System.Windows.Forms.Label();
            this._LabelRight = new System.Windows.Forms.Label();
            this._LabelFirstLine = new System.Windows.Forms.Label();
            this._TextBoxLeftIndent = new CC.Controls.NumericTextBox();
            this._TableLayoutPanelAlignment = new System.Windows.Forms.TableLayoutPanel();
            this._LabelAlignment = new System.Windows.Forms.Label();
            this._ComboBoxAlignment = new System.Windows.Forms.ComboBox();
            this._TableLayoutPanelMain.SuspendLayout();
            this._TableLayoutPanelInner.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this._TableLayoutPanelIndent.SuspendLayout();
            this._TableLayoutPanelAlignment.SuspendLayout();
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
            this._TableLayoutPanelMain.Controls.Add(this._TableLayoutPanelInner, 1, 1);
            this._TableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this._TableLayoutPanelMain.Margin = new System.Windows.Forms.Padding(0);
            this._TableLayoutPanelMain.Name = "_TableLayoutPanelMain";
            this._TableLayoutPanelMain.RowCount = 3;
            this._TableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelMain.Size = new System.Drawing.Size(171, 166);
            this._TableLayoutPanelMain.TabIndex = 0;
            // 
            // _TableLayoutPanelInner
            // 
            this._TableLayoutPanelInner.ColumnCount = 1;
            this._TableLayoutPanelInner.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelInner.Controls.Add(this.groupBox1, 0, 0);
            this._TableLayoutPanelInner.Controls.Add(this._TableLayoutPanelAlignment, 0, 2);
            this._TableLayoutPanelInner.Dock = System.Windows.Forms.DockStyle.Fill;
            this._TableLayoutPanelInner.Location = new System.Drawing.Point(9, 9);
            this._TableLayoutPanelInner.Margin = new System.Windows.Forms.Padding(0);
            this._TableLayoutPanelInner.Name = "_TableLayoutPanelInner";
            this._TableLayoutPanelInner.RowCount = 3;
            this._TableLayoutPanelInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 9F));
            this._TableLayoutPanelInner.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            this._TableLayoutPanelInner.Size = new System.Drawing.Size(153, 148);
            this._TableLayoutPanelInner.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._TableLayoutPanelIndent);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(153, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Indent";
            // 
            // _TableLayoutPanelIndent
            // 
            this._TableLayoutPanelIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._TableLayoutPanelIndent.ColumnCount = 2;
            this._TableLayoutPanelIndent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this._TableLayoutPanelIndent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelIndent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._TableLayoutPanelIndent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this._TableLayoutPanelIndent.Controls.Add(this._TextBoxHangingIndent, 1, 2);
            this._TableLayoutPanelIndent.Controls.Add(this._TextBoxRightIndent, 1, 1);
            this._TableLayoutPanelIndent.Controls.Add(this._LabelLeft, 0, 0);
            this._TableLayoutPanelIndent.Controls.Add(this._LabelRight, 0, 1);
            this._TableLayoutPanelIndent.Controls.Add(this._LabelFirstLine, 0, 2);
            this._TableLayoutPanelIndent.Controls.Add(this._TextBoxLeftIndent, 1, 0);
            this._TableLayoutPanelIndent.Location = new System.Drawing.Point(3, 16);
            this._TableLayoutPanelIndent.Name = "_TableLayoutPanelIndent";
            this._TableLayoutPanelIndent.RowCount = 3;
            this._TableLayoutPanelIndent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this._TableLayoutPanelIndent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this._TableLayoutPanelIndent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this._TableLayoutPanelIndent.Size = new System.Drawing.Size(147, 89);
            this._TableLayoutPanelIndent.TabIndex = 0;
            // 
            // _TextBoxHangingIndent
            // 
            this._TextBoxHangingIndent.AllowDecimals = true;
            this._TextBoxHangingIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._TextBoxHangingIndent.AppendString = "\"";
            this._TextBoxHangingIndent.Location = new System.Drawing.Point(65, 64);
            this._TextBoxHangingIndent.Name = "_TextBoxHangingIndent";
            this._TextBoxHangingIndent.Size = new System.Drawing.Size(79, 20);
            this._TextBoxHangingIndent.TabIndex = 5;
            this._TextBoxHangingIndent.Text = "0\"";
            this._TextBoxHangingIndent.TextChanged += new System.EventHandler(this._TextBoxHangingIndent_TextChanged);
            // 
            // _TextBoxRightIndent
            // 
            this._TextBoxRightIndent.AllowDecimals = true;
            this._TextBoxRightIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._TextBoxRightIndent.AppendString = "\"";
            this._TextBoxRightIndent.Location = new System.Drawing.Point(65, 34);
            this._TextBoxRightIndent.Name = "_TextBoxRightIndent";
            this._TextBoxRightIndent.Size = new System.Drawing.Size(79, 20);
            this._TextBoxRightIndent.TabIndex = 3;
            this._TextBoxRightIndent.Text = "0\"";
            this._TextBoxRightIndent.TextChanged += new System.EventHandler(this._TextBoxRightIndent_TextChanged);
            // 
            // _LabelLeft
            // 
            this._LabelLeft.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._LabelLeft.AutoSize = true;
            this._LabelLeft.Location = new System.Drawing.Point(3, 8);
            this._LabelLeft.Name = "_LabelLeft";
            this._LabelLeft.Size = new System.Drawing.Size(28, 13);
            this._LabelLeft.TabIndex = 0;
            this._LabelLeft.Text = "Left:";
            // 
            // _LabelRight
            // 
            this._LabelRight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._LabelRight.AutoSize = true;
            this._LabelRight.Location = new System.Drawing.Point(3, 37);
            this._LabelRight.Name = "_LabelRight";
            this._LabelRight.Size = new System.Drawing.Size(35, 13);
            this._LabelRight.TabIndex = 2;
            this._LabelRight.Text = "Right:";
            // 
            // _LabelFirstLine
            // 
            this._LabelFirstLine.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._LabelFirstLine.AutoSize = true;
            this._LabelFirstLine.Location = new System.Drawing.Point(3, 67);
            this._LabelFirstLine.Name = "_LabelFirstLine";
            this._LabelFirstLine.Size = new System.Drawing.Size(52, 13);
            this._LabelFirstLine.TabIndex = 4;
            this._LabelFirstLine.Text = "First Line:";
            // 
            // _TextBoxLeftIndent
            // 
            this._TextBoxLeftIndent.AllowDecimals = true;
            this._TextBoxLeftIndent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._TextBoxLeftIndent.AppendString = "\"";
            this._TextBoxLeftIndent.Location = new System.Drawing.Point(65, 4);
            this._TextBoxLeftIndent.Name = "_TextBoxLeftIndent";
            this._TextBoxLeftIndent.Size = new System.Drawing.Size(79, 20);
            this._TextBoxLeftIndent.TabIndex = 1;
            this._TextBoxLeftIndent.Text = "0\"";
            this._TextBoxLeftIndent.TextChanged += new System.EventHandler(this._TextBoxLeftIndent_TextChanged);
            // 
            // _TableLayoutPanelAlignment
            // 
            this._TableLayoutPanelAlignment.ColumnCount = 2;
            this._TableLayoutPanelAlignment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this._TableLayoutPanelAlignment.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this._TableLayoutPanelAlignment.Controls.Add(this._LabelAlignment, 0, 0);
            this._TableLayoutPanelAlignment.Controls.Add(this._ComboBoxAlignment, 1, 0);
            this._TableLayoutPanelAlignment.Dock = System.Windows.Forms.DockStyle.Fill;
            this._TableLayoutPanelAlignment.Location = new System.Drawing.Point(0, 122);
            this._TableLayoutPanelAlignment.Margin = new System.Windows.Forms.Padding(0);
            this._TableLayoutPanelAlignment.Name = "_TableLayoutPanelAlignment";
            this._TableLayoutPanelAlignment.RowCount = 1;
            this._TableLayoutPanelAlignment.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._TableLayoutPanelAlignment.Size = new System.Drawing.Size(153, 26);
            this._TableLayoutPanelAlignment.TabIndex = 1;
            // 
            // _LabelAlignment
            // 
            this._LabelAlignment.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._LabelAlignment.AutoSize = true;
            this._LabelAlignment.Location = new System.Drawing.Point(6, 6);
            this._LabelAlignment.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
            this._LabelAlignment.Name = "_LabelAlignment";
            this._LabelAlignment.Size = new System.Drawing.Size(56, 13);
            this._LabelAlignment.TabIndex = 0;
            this._LabelAlignment.Text = "Alignment:";
            // 
            // _ComboBoxAlignment
            // 
            this._ComboBoxAlignment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this._ComboBoxAlignment.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this._ComboBoxAlignment.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this._ComboBoxAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._ComboBoxAlignment.FormattingEnabled = true;
            this._ComboBoxAlignment.Location = new System.Drawing.Point(68, 3);
            this._ComboBoxAlignment.Margin = new System.Windows.Forms.Padding(3, 3, 5, 3);
            this._ComboBoxAlignment.Name = "_ComboBoxAlignment";
            this._ComboBoxAlignment.Size = new System.Drawing.Size(80, 21);
            this._ComboBoxAlignment.TabIndex = 1;
            // 
            // ParagraphDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonDock = CC.Controls.DialogFormButtonDock.Right;
            this.ButtonPadding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.ClientSize = new System.Drawing.Size(264, 166);
            this.Controls.Add(this._TableLayoutPanelMain);
            this.Name = "ParagraphDialog";
            this.Text = "Paragraph";
            this.Controls.SetChildIndex(this._TableLayoutPanelMain, 0);
            this._TableLayoutPanelMain.ResumeLayout(false);
            this._TableLayoutPanelInner.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this._TableLayoutPanelIndent.ResumeLayout(false);
            this._TableLayoutPanelIndent.PerformLayout();
            this._TableLayoutPanelAlignment.ResumeLayout(false);
            this._TableLayoutPanelAlignment.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _TableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel _TableLayoutPanelInner;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel _TableLayoutPanelAlignment;
        private System.Windows.Forms.Label _LabelAlignment;
        private System.Windows.Forms.ComboBox _ComboBoxAlignment;
        private System.Windows.Forms.TableLayoutPanel _TableLayoutPanelIndent;
        private System.Windows.Forms.Label _LabelLeft;
        private System.Windows.Forms.Label _LabelRight;
        private System.Windows.Forms.Label _LabelFirstLine;
        private NumericTextBox _TextBoxHangingIndent;
        private NumericTextBox _TextBoxRightIndent;
        private NumericTextBox _TextBoxLeftIndent;
    }
}