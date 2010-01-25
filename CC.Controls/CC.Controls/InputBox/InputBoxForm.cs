using System;
using System.Drawing;
using System.Windows.Forms;

namespace CC.Controls
{
    /// <summary>
    /// The internal form used by the <see cref="InputBox"/> class.
    /// </summary>
    internal partial class InputBoxForm : DialogForm
    {
        #region Constructor
        /// <summary>
        /// Creates a new instance of the <see cref="InputBoxForm"/>
        /// </summary>
        public InputBoxForm()
        {
            InitializeComponent();

            _TextBoxInput.Focus();
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the prompt
        /// </summary>
        public string Prompt
        {
            get { return _LabelPrompt.Text; }
            set
            {
                _LabelPrompt.Text = value;

                using (Graphics graphics = CreateGraphics())
                {
                    SizeF size = graphics.MeasureString(value, _LabelPrompt.Font, _LabelPrompt.Width);

                    if (size.Height > _LabelPrompt.Height)
                    {
                        Height += (int)size.Height - _LabelPrompt.Height;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title
        {
            get { return Text; }
            set { Text = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating wether to use a password character or not
        /// </summary>
        public bool UseSystemPasswordChar
        {
            get { return _TextBoxInput.UseSystemPasswordChar; }
            set { _TextBoxInput.UseSystemPasswordChar = value; }
        }

        /// <summary>
        /// Gets or sets the value
        /// </summary>
        public string Value
        {
            get { return _TextBoxInput.Text; }
            set
            {
                if (!value.Equals(_TextBoxInput.Text))
                {
                    _TextBoxInput.Text = value;
                    _TextBoxInput.SelectionStart = 0;
                    _TextBoxInput.SelectionLength = _TextBoxInput.Text.Length;
                }
            }
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Fires the <see cref="Form.Shown"/> event.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/></param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            BringToFront();
            Focus();
            _TextBoxInput.Focus();
        }
        #endregion
    }
}
