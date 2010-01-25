using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace CC.Controls
{
    /// <summary>
    /// Respresents a standard dialog that can be used as a base for custom dialog forms.
    /// </summary>
    public partial class DialogForm : Form
    {
        #region Contructor
        /// <summary>
        /// Creates a new <see cref="DialogForm"/>
        /// </summary>
        public DialogForm()
        {
            InitializeComponent();

            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowIcon = false;
            base.ShowInTaskbar = false;
        }
        #endregion

        #region Private Fields
        private DialogFormButtonDock _ButtonDock = DialogFormButtonDock.Bottom;
        private Padding _ButtonPadding = new Padding(0);
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating how to dock the buttons.
        /// </summary>
        public DialogFormButtonDock ButtonDock
        {
            get { return _ButtonDock; }
            set { _ButtonDock = value; SetupButtons(); }
        }

        /// <summary>
        /// Gets or sets additional padding around the buttons.
        /// </summary>
        public Padding ButtonPadding
        {
            get { return _ButtonPadding; }
            set { _ButtonPadding = value; SetupButtons(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Maximize button is displayed in the caption bar of the form.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool MaximizeBox
        {
            get { return base.MaximizeBox; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Minimize button is displayed in the caption bar of the form.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool MinimizeBox
        {
            get { return base.MinimizeBox; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an icon is displayed in the caption bar of the form.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool ShowIcon
        {
            get { return base.ShowIcon; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form is displayed in the Windows taskbar.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool ShowInTaskbar
        {
            get { return base.ShowInTaskbar; }
        }
        #endregion

        #region Private Methods
        private void SetupButtons()
        {
            _TableLayoutPanelButtons.Controls.Clear();
            _TableLayoutPanelButtons.ColumnCount = 0;
            _TableLayoutPanelButtons.ColumnStyles.Clear();
            _TableLayoutPanelButtons.RowCount = 0;
            _TableLayoutPanelButtons.RowStyles.Clear();

            switch (ButtonDock)
            {
                case DialogFormButtonDock.Bottom:
                    {
                        _TableLayoutPanelButtons.ColumnCount = 6;
                        _TableLayoutPanelButtons.Dock = DockStyle.Bottom;
                        _TableLayoutPanelButtons.Height = 42;
                        _TableLayoutPanelButtons.RowCount = 3;

                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9 + ButtonPadding.Left));
                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75));
                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9));
                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 75));
                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9 + ButtonPadding.Right));

                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 9 + ButtonPadding.Top));
                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 9 + ButtonPadding.Bottom));

                        _TableLayoutPanelButtons.Controls.Add(_ButtonOk, 2, 1);
                        _TableLayoutPanelButtons.Controls.Add(_ButtonCancel, 4, 1);
                        break;
                    }
                case DialogFormButtonDock.Right:
                    {
                        _TableLayoutPanelButtons.ColumnCount = 3;
                        _TableLayoutPanelButtons.Dock = DockStyle.Right;
                        _TableLayoutPanelButtons.RowCount = 6;
                        _TableLayoutPanelButtons.Width = 93;

                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9 + ButtonPadding.Left));
                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                        _TableLayoutPanelButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 9 + ButtonPadding.Right));

                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 9 + ButtonPadding.Top));
                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 9));
                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 24));
                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                        _TableLayoutPanelButtons.RowStyles.Add(new RowStyle(SizeType.Absolute, 9 + ButtonPadding.Bottom));

                        _TableLayoutPanelButtons.Controls.Add(_ButtonOk, 1, 1);
                        _TableLayoutPanelButtons.Controls.Add(_ButtonCancel, 1, 3);
                        break;                        
                    }
            }
        }
        #endregion

        #region Event Handlers
        private void _ButtonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            base.Close();
        }

        private void _ButtonOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            base.Close();
        }
        #endregion

        #region Public Methods
        [Obsolete("A form that inherits from DialogForm can only use the ShowDialog() method!")]
        public new void Close()
        {
            throw new NotImplementedException("A form that inherits from DialogForm can only use the ShowDialog() method!");
        }

        [Obsolete("A form that inherits from DialogForm can only use the ShowDialog() method!")]
        public new void Hide()
        {
            throw new NotImplementedException("A form that inherits from DialogForm can only use the ShowDialog() method!");
        }

        [Obsolete("A form that inherits from DialogForm can only use the ShowDialog() method!")]
        public new void Show()
        {
            throw new NotImplementedException("A form that inherits from DialogForm can only use the ShowDialog() method!");
        }

        [Obsolete("A form that inherits from DialogForm can only use the ShowDialog() method!")]
        public new void Show(IWin32Window window)
        {
            throw new NotImplementedException("A form that inherits from DialogForm can only use the ShowDialog() method!");
        }
        #endregion
    }
}
