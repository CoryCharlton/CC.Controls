using System;
using System.ComponentModel;
using System.Windows.Forms;
using CC.Utilities;

namespace CC.Controls
{
    /// <summary>
    /// Respresents a <see cref="WatermarkTextBox"/> that only accepts numeric input.
    /// </summary>
    public partial class NumericTextBox : WatermarkTextBox
    {
        #region Constructor
        /// <summary>
        /// Creates a new instance of <see cref="NumericTextBox"/>
        /// </summary>
        public NumericTextBox()
        {
            // Default values...
            SelectAllOnEnter = true;
            
            InitializeComponent();
        }
        #endregion

        #region Private Fields
        private string _AppendString;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating wether to control allows not integer input.
        /// </summary>
        public bool AllowDecimals { get; set; }

        /// <summary>
        /// Gets or sets a string to append to the numeric input.
        /// </summary>
        public string AppendString
        {
            get { return _AppendString; }
            set { ChangeAppendString(value); }
        }

        /// <summary>
        /// Gets or sets a value indicating wether to select all text when the control gains focus.
        /// </summary>
        [DefaultValue(true)]
        public bool SelectAllOnEnter { get; set; }
        #endregion

        #region Protected Methods
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            if (SelectAllOnEnter)
            {
                SelectAll();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyValue != 8 && 
                e.KeyValue != 13 && 
                e.KeyValue != 27 && 
                e.KeyValue != 46 && 
                !(e.KeyValue >= 37 && e.KeyValue <= 40) && 
                !(e.KeyValue >= 48 && e.KeyValue <= 57) && 
                !(AllowDecimals && e.KeyValue == 190 && !Text.Contains(".")))
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            Logging.LogMessage(e.KeyValue + " - " + e.KeyCode + " Handled:" + e.Handled + " Suppressed:" + e.SuppressKeyPress);

            base.OnKeyDown(e);
        }

        protected override void OnTextChanged(EventArgs e)
        {
            AddAppendString();

            base.OnTextChanged(e);
        }
        #endregion

        #region Private Methods
        private void AddAppendString()
        {
            if (!string.IsNullOrEmpty(AppendString) && !TextEndsWithAppendString())
            {
                int selectionStart = SelectionStart;
                int selectionLength = SelectionLength;

                Text += _AppendString;

                SelectionStart = selectionStart;
                SelectionLength = selectionLength;
            }
        }

        private void ChangeAppendString(string value)
        {
            if (_AppendString != value)
            {
                string oldAppendString = _AppendString;
                
                _AppendString = string.Empty;

                Text = RemoveAppendString(oldAppendString);

                _AppendString = value;

                AddAppendString();
            }
        }

        private bool TextEndsWithAppendString()
        {
            return TextEndsWithAppendString(AppendString);
        }

        private bool TextEndsWithAppendString(string appendString)
        {
            return (!string.IsNullOrEmpty(appendString) && Text.Trim().EndsWith(appendString));
        }

        private string RemoveAppendString()
        {
            return RemoveAppendString(AppendString);
        }

        private string RemoveAppendString(string appendString)
        {
            string value = Text;

            if (TextEndsWithAppendString(appendString))
            {
                value = value.Trim().Remove(TextLength - appendString.Length);
            }

            return value;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the value
        /// </summary>
        /// <returns>The value as a <see cref="string"/></returns>
        public string Value()
        {
            return RemoveAppendString();
        }

        /// <summary>
        /// Gets the value as a <see cref="float"/>
        /// </summary>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value as a <see cref="float"/></returns>
        public float ValueAsFloat(float defaultValue)
        {
            float value = defaultValue;
            float tempValue;

            if (float.TryParse(Value(), out tempValue))
            {
                value = tempValue;
            }

            return value;
        }

        /// <summary>
        /// Gets the value as a <see cref="int"/>
        /// </summary>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value as a <see cref="int"/></returns>
        public float ValueAsInt(int defaultValue)
        {
            int value = defaultValue;
            int tempValue;

            if (int.TryParse(Value(), out tempValue))
            {
                value = tempValue;
            }

            return value;
        }

        /// <summary>
        /// Gets the value as a <see cref="long"/>
        /// </summary>
        /// <param name="defaultValue">The default value</param>
        /// <returns>The value as a <see cref="long"/></returns>
        public float ValueAsLong(long defaultValue)
        {
            long value = defaultValue;
            long tempValue;

            if (long.TryParse(Value(), out tempValue))
            {
                value = tempValue;
            }

            return value;
        }
        #endregion
    }
}
