namespace CC.Controls
{
    partial class ExplorerTreeView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._ImageListSmall = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // _ImageListSmall
            // 
            this._ImageListSmall.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this._ImageListSmall.ImageSize = new System.Drawing.Size(16, 16);
            this._ImageListSmall.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // ExplorerTreeView
            // 
            this.CheckBoxes = true;
            this.ImageIndex = 0;
            this.ImageList = this._ImageListSmall;
            this.LineColor = System.Drawing.Color.Black;
            this.SelectedImageIndex = 0;
            this.ShowNodeToolTips = true;
            this.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.ExplorerTreeView_AfterCheck);
            this.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.ExplorerTreeView_BeforeExpand);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList _ImageListSmall;
    }
}
