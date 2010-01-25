using System.ComponentModel;
using System.Windows.Forms;

namespace CC.Controls
{
    // TODO: Needs comments ...
    public partial class ParagraphDialog : DialogForm
    {
        #region Constructor
        public ParagraphDialog()
        {
            InitializeComponent();

            _ComboBoxAlignment.Items.Add(string.Empty);
            _ComboBoxAlignment.Items.Add(HorizontalAlignment.Center);
            _ComboBoxAlignment.Items.Add(HorizontalAlignment.Left);
            _ComboBoxAlignment.Items.Add(HorizontalAlignment.Right);
        }
        #endregion

        #region Private Fields
        private float _HangingIndent;
        private float _LeftIndent;
        private float _RightIndent;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the alignment.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HorizontalAlignment? Alignment
        {
            get { return _ComboBoxAlignment.SelectedItem as HorizontalAlignment?; }
            set
            {
                if (value != null)
                {
                    _ComboBoxAlignment.SelectedIndex = _ComboBoxAlignment.Items.IndexOf(value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the hanging indent.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float HangingIndent
        {
            get { return _HangingIndent; }
            set { _HangingIndent = value; SetText(_TextBoxHangingIndent, _HangingIndent); }
        }

        /// <summary>
        /// Gets or sets the left indent.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float LeftIndent
        {
            get { return _LeftIndent; }
            set { _LeftIndent = value; SetText(_TextBoxLeftIndent, _LeftIndent); }
        }

        /// <summary>
        /// Gets or sets the right indent.
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float RightIndent
        {
            get { return _RightIndent; }
            set { _RightIndent = value; SetText(_TextBoxRightIndent, _RightIndent); }
        }
        #endregion

        #region Event Handlers
        private void _TextBoxHangingIndent_TextChanged(object sender, System.EventArgs e)
        {
            _HangingIndent = _TextBoxHangingIndent.ValueAsFloat(0);
        }

        private void _TextBoxLeftIndent_TextChanged(object sender, System.EventArgs e)
        {
            _LeftIndent = _TextBoxLeftIndent.ValueAsFloat(0);
        }

        private void _TextBoxRightIndent_TextChanged(object sender, System.EventArgs e)
        {
            _RightIndent = _TextBoxRightIndent.ValueAsFloat(0);
        }
        #endregion

        #region Protected Methods
        #endregion

        #region Private Methods
        private static void SetText(Control textBox, float value)
        {
            textBox.Text = value.ToString();
        }
        #endregion
    }
}
