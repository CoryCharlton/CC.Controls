using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CC.Utilities;
using CC.Utilities.Interop;

namespace CC.Controls
{
    public partial class RichTextBoxEx : UserControl
    {
        // TODO: Missing some comments, focusing on functionality first
        // TODO: Do something with HScroll and VScroll events?

        // TODO: Add optional Save/Open buttons with a bool property to display or not
        // TODO: Add a "Find" button?
        // TODO: Modify the Selection... properties to use the native calls rather than the RichTextBox calls
        // TODO: Make sure "everything" is using the native calls

        // TODO: BUG: Horizontal scroll bar appears when it shouldn't after resize. ** Mostly fixed, see note below about wrap to window

        #region Constructor
        public RichTextBoxEx()
        {
            InitializeComponent();

            _ImageMarker.Source = _RichTextBox;

            _MarginBar.ImageMarkerControl = _ImageMarker;
            _MarginBar.ImageMarkerDashStyle = DashStyle.Dash;
            _MarginBar.ImageMarkerLineColor = DefaultForeColor;

            _RichTextBox.DragDrop +=_RichTextBox_DragDrop;
            _RichTextBox.DragEnter += _RichTextBox_DragEnter;
            _RichTextBox.DragLeave += _RichTextBox_DragLeave;
            _RichTextBox.DragOver += _RichTextBox_DragOver;
            _RichTextBox.GiveFeedback += _RichTextBox_GiveFeedback;
            _RichTextBox.MouseWheel += _RichTextBox_MouseWheel;
        }
        #endregion

        #region Private Fields
        private bool _AutoWordSelection = true;
        private int _FirstCharOnNextPage;
        private int _PrintableWidth;
        private CHARFORMAT2 _SelectedCharformat;
        private PARAFORMAT2 _SelectedParaformat;
        private WordWrap _WordWrap = WordWrap.WrapToPrintDocument;
        #endregion

        #region Public Events
        public event EventHandler AcceptsTabChanged;
        public event EventHandler HideSelectionChanged;
        public event EventHandler ModifiedChanged;
        public event EventHandler MultilineChanged;
        public event EventHandler Protected;
        public event EventHandler ReadOnlyChanged;
        public event EventHandler SelectionChanged;
        public new event EventHandler TextChanged; // Had to override this so I could fire it. base.OnTextChanged() did't work.
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets a value indicating whether pressing the TAB key in a multiline text box control types a TAB character in the control instead of moving the focus to the next control in the tab order.
        /// </summary>
        public bool AcceptsTab
        {
            get { return _RichTextBox.AcceptsTab; }
            set { _RichTextBox.AcceptsTab = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control will enable drag-and-drop operations.
        /// </summary>
        public new bool AllowDrop
        {
            get { return _RichTextBox.AllowDrop; }
            set { _RichTextBox.AllowDrop = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether automatic word selection is enabled.
        /// </summary>
        public bool AutoWordSelection
        {
            // Tried to fix this but it seems to be broken at the Native API level
            get { return _AutoWordSelection; }
            set
            {
                _AutoWordSelection = value;
                ChangeOptions(ECO.AUTOWORDSELECTION, _AutoWordSelection);
            }
        }

        /// <summary>
        /// Gets or sets the indentation used in the <see cref="RichTextBoxEx"/> control when the bullet style is applied to the text.
        /// </summary>
        // TODO: Look into this more, what is it in Native API?
        public int BulletIndent
        {
            get { return _RichTextBox.BulletIndent; }
            set { _RichTextBox.BulletIndent = value; }
        }

        /// <summary>
        /// Gets a value indicating whether there are actions that have occurred within the <see cref="RichTextBoxEx"/> that can be reapplied.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanRedo
        {
            get { return _RichTextBox.CanRedo; }
        }

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation in a text box control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanUndo
        {
            get { return _RichTextBox.CanUndo; }
        }

        /// <summary>
        /// Gets or sets the custom colors displayed in the color dialog box.
        /// </summary>
        public int[] CustomColors { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the RichTextBox will automatically format a Uniform Resource Locator (URL) when it is typed into the control.
        /// </summary>
        // TODO: Find this Native
        public bool DetectUrls
        {
            get { return _RichTextBox.DetectUrls; }
            set { _RichTextBox.DetectUrls = value; }
        }

        /// <summary>
        /// Gets or sets a value that enables drag-and-drop operations on text, pictures, and other data.
        /// </summary>
        public bool EnableAutoDragDrop
        {
            get { return _RichTextBox.EnableAutoDragDrop; }
            set { _RichTextBox.EnableAutoDragDrop = value; }
        }

        /// <summary>
        /// Gets or sets the lines of text in a text box control.
        /// </summary>
        public string[] Lines
        {
            get { return _RichTextBox.Lines; }
            set { _RichTextBox.Lines = value; }
        }

        /// <summary>
        /// Gets or sets the maximum number of characters the user can type or paste into the rich text box control.
        /// </summary>
        public int MaxLength
        {
            get { return _RichTextBox.MaxLength; }
            set { _RichTextBox.MaxLength = value; }
        }

        public MenuStrip MenuStrip
        {
            get { return _MenuStrip; }
        }

        /// <summary>
        /// Gets or sets a value that indicates that the text box control has been modified by the user since the control was created or its contents were last set.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool Modified
        {
            get { return _RichTextBox.Modified; }
            set { _RichTextBox.Modified = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this is a multiline <see cref="RichTextBoxEx"/> control. 
        /// </summary>
        public bool MultiLine
        {
            get { return _RichTextBox.Multiline; }
            set { _RichTextBox.Multiline = value; }
        }

        /// <summary>
        /// Gets the <see cref="PrintDocument"/>
        /// </summary>
        public PrintDocument PrintDocument
        {
            get { return _PrintDocument; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether text in the text box is read-only.
        /// </summary>
        public bool ReadOnly
        {
            get { return _RichTextBox.ReadOnly; }
            set { _RichTextBox.ReadOnly = value; }
        }

        /// <summary>
        /// Gets the name of the action that can be reapplied to the control when the Redo method is called.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string RedoActionName
        {
            get { return _RichTextBox.RedoActionName; }
        }

        /// <summary>
        /// Gets or sets the text of the <see cref="RichTextBoxEx"/> control, including all rich text format (RTF) codes.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Rtf
        {
            get { return _RichTextBox.Rtf; }
            set
            {
                try
                {
                    _RichTextBox.Rtf = value;
                }
                catch (ArgumentException)
                {
                    // NOTE: Is this the right thing to do? I could see it resulting in RTF control codes in the text for mis-formatted RTF which isn't what the user wants either
                    _RichTextBox.Text = value; // Assume the argument exception is due to plain text
                }

                ChangeWordWrap(_WordWrap);

                // A hack to get the ToolStrip and MarginBar updated with the correct values
                SelectionStart = 0;
                SelectionLength = 1;
                SelectionLength = 0;
            }
        }

        /// <summary>
        /// Gets or sets the type of scroll bars to display in the <see cref="RichTextBoxEx"/> control.
        /// </summary>
        public RichTextBoxScrollBars ScrollBars
        {
            get { return _RichTextBox.ScrollBars; }
            set { _RichTextBox.ScrollBars = value; }
        }

        /// <summary>
        /// Gets or sets the currently selected rich text format (RTF) formatted text in the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedRtf
        {
            get { return _RichTextBox.SelectedRtf; }
            set { _RichTextBox.SelectedRtf = value; }
        }

        /// <summary>
        /// Gets or sets the selected text within the <see cref="RichTextBoxEx"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectedText
        {
            get { return _RichTextBox.SelectedText; }
            set { _RichTextBox.SelectedText = value; }
        }

        /// <summary>
        /// Gets or sets the alignment to apply to the current selection or insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HorizontalAlignment? SelectionAlignment
        {
            get { return IsParaFormatSameAlignment(_SelectedParaformat) ? InteropConvert.PFA_ToHorizontalAlignment(_SelectedParaformat.wAlignment) : new HorizontalAlignment?(); }
            set
            {
                if (value != null)
                {
                    ChangeAlignment(value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the color of text when the text is selected in a <see cref="RichTextBoxEx"/> control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color? SelectionBackColor
        {
            get { return IsCharFormatSameBackColor(_SelectedCharformat) ? ConvertEx.ColorRefToColor(_SelectedCharformat.crBackColor) : new Color?(); }
            set
            {
                if (value != null)
                {
                    ChangeBackColor(value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the font bold of the current text selection or insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool? SelectionBold
        {
            get { return IsCharFormatSameBold(_SelectedCharformat) ? (_SelectedCharformat.dwEffects & CFE.BOLD) == CFE.BOLD : new bool?(); }
            set
            {
                if (value != null)
                {
                    ChangeFontStyle(FontStyle.Bold, value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the bullet style is applied to the current selection or insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool? SelectionBullet
        {
            get { return IsParaFormatSameBullets(_SelectedParaformat) ? (_SelectedParaformat.wNumbering == (ushort)PFN.BULLET) : new bool?(); }
            set
            {
                if (value != null)
                {
                    ChangeBullet(value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets whether text in the control appears on the baseline, as a superscript, or as a subscript below the baseline.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        // TODO: Convert to Native
        public int SelectionCharOffset
        {
            get { return _RichTextBox.SelectionCharOffset; }
            set { _RichTextBox.SelectionCharOffset = value; }
        }

        /// <summary>
        /// Gets or sets the text color of the current text selection or insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Color? SelectionColor
        {
            get { return IsCharFormatSameColor(_SelectedCharformat) ? ConvertEx.ColorRefToColor(_SelectedCharformat.crTextColor) : new Color?(); }
            set
            {
                if (value != null)
                {
                    ChangeColor(value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the font family of the current text selection or insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SelectionFontFamily
        {
            get { return IsCharFormatSameName(_SelectedCharformat) ? _SelectedCharformat.szFaceName : string.Empty; }
            set { ChangeFontFamily(value); }
        }

        /// <summary>
        /// Gets or sets the font size of the current text selection or insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float? SelectionFontSize
        {
            get { return IsCharFormatSameSize(_SelectedCharformat) ? ConvertEx.TwipsToPoints(_SelectedCharformat.yHeight) : new float?(); }
            set
            {
                if (value != null)
                {
                    ChangeFontSize(value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the distance between the left edge of the first line of text in the selected paragraph in inches.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float SelectionHangingIndent
        {
            get { return _MarginBar.HangingIndent; }
            set { _MarginBar.HangingIndent = value; }
        }

        /// <summary>
        /// Gets or sets the distance between the left edge of the selected paragraph in inches.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float SelectionIndent
        {
            get { return _MarginBar.LeftMargin; }
            set { _MarginBar.LeftMargin = value; }
        }

        /// <summary>
        /// Gets or sets the font italic of the current text selection or insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool? SelectionItalic
        {
            get { return IsCharFormatSameItalic(_SelectedCharformat) ? (_SelectedCharformat.dwEffects & CFE.ITALIC) == CFE.ITALIC : new bool?(); }
            set
            {
                if (value != null)
                {
                    ChangeFontStyle(FontStyle.Italic, value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the number of characters selected in control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionLength
        {
            get { return _RichTextBox.SelectionLength; }
            set { _RichTextBox.SelectionLength = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current text selection is protected.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        // TODO: Convert to Native
        public bool SelectionProtected
        {
            get { return _RichTextBox.SelectionProtected; }
            set { _RichTextBox.SelectionProtected = value; }
        }

        /// <summary>
        /// The distance (in pixels) between the right edge of the RichTextBox control and the right edge of the text that is selected or added at the current insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public float SelectionRightIndent
        {
            get { return _MarginBar.RightMargin; }
            set { _MarginBar.RightMargin = value; }
        }

        /// <summary>
        /// Gets or sets the starting point of text selected in the <see cref="RichTextBoxEx"/>.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectionStart
        {
            get { return _RichTextBox.SelectionStart; }
            set { _RichTextBox.SelectionStart = value; }
        }

        /// <summary>
        /// Gets or sets the absolute tab stop positions in a <see cref="RichTextBoxEx"/> control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int[] SelectionTabs
        {
            get { return _RichTextBox.SelectionTabs; }
            set { _RichTextBox.SelectionTabs = value; }
        }

        /// <summary>
        /// Gets the selection type within the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public RichTextBoxSelectionTypes SelectionType
        {
            get { return _RichTextBox.SelectionType; }
        }

        /// <summary>
        /// Gets or sets the font underline of the current text selection or insertion point.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool? SelectionUnderline
        {
            get { return IsCharFormatSameUnderline(_SelectedCharformat) ? (_SelectedCharformat.dwEffects & CFE.UNDERLINE) == CFE.UNDERLINE: new bool?(); }
            set
            {
                if (value != null)
                {
                    ChangeFontStyle(FontStyle.Underline, value.Value);
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the defined shortcuts are enabled.
        /// </summary>
        public bool ShortcutsEnabled
        {
            get { return _RichTextBox.ShortcutsEnabled; }
            set { _RichTextBox.ShortcutsEnabled = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a ruler is displayed in the <see cref="RichTextBoxEx"/>.
        /// </summary>
        public bool ShowRuler
        {
            get { return _MarginBar.Visible; }
            set { _MarginBar.Visible = value; SetupRichTextBoxSize(); SetupMarginBar(true); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a selection margin is displayed in the <see cref="RichTextBoxEx"/>.
        /// </summary>
        public bool ShowSelectionMargin
        {
            // No need to switch to native. Same behavior both ways.
            get { return _RichTextBox.ShowSelectionMargin; }
            set { _RichTextBox.ShowSelectionMargin = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="ToolStrip"/> will be displayed or not
        /// </summary>
        public bool ShowToolStrip
        {
            get { return _ToolStrip.Visible; }
            set { _ToolStrip.Visible = value; SetupRichTextBoxSize(); }
        }

        public bool SnapToRuler
        {
            get { return _MarginBar.SnapToRuler; }
            set { _MarginBar.SnapToRuler = value; }
        }

        /// <summary>
        /// Gets or sets the current text in the <see cref="RichTextBoxEx"/>
        /// </summary>
        public new string Text
        {
            get { return _RichTextBox.Text; }
            set { _RichTextBox.Text = value; }
        }

        /// <summary>
        /// Gets the length of text in the control.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TextLength
        {
            get { return _RichTextBox.TextLength; }
        }

        /// <summary>
        /// Gets or sets the default font in the <see cref="RichTextBoxEx"/>
        /// </summary>
        public Font TextFont
        {
            get { return _RichTextBox.Font; }
            set { _RichTextBox.Font = value; }
        }

        /// <summary>
        /// Gets the name of the action that can be undone in the control when the Undo method is called.
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string UndoActionName
        {
            get { return _RichTextBox.UndoActionName; }
        }

        /// <summary>
        /// Indicates whether a <see cref="RichTextBoxEx"/> automatically wraps words to the beginning of the next line when necessary.
        /// </summary>
        public WordWrap WordWrap
        {
            get { return _WordWrap; }
            set
            {
                _WordWrap = value;
                ChangeWordWrap(_WordWrap);
            }
        }

        /// <summary>
        /// Gets or sets the current zoom level of the <see cref="RichTextBoxEx"/>.
        /// </summary>
        public float ZoomFactor
        {
            get { return _RichTextBox.ZoomFactor; }
            set { _RichTextBox.ZoomFactor = value; }
        }
        #endregion

        #region Event Handlers
        private void _ContextMenuStripMain_Opening(object sender, CancelEventArgs e)
        {
            SetupEditMenu();
        }

        private void _CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void _CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void _EditToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            SetupEditMenu();
        }

        private void _MarginBar_LeftMarginsChanged(object sender, EventArgs e)
        {
            PARAFORMAT2 paraformat = new PARAFORMAT2
                                         {
                                             dwMask = (PFM.STARTINDENT | PFM.OFFSET), 
                                             dxStartIndent = (int) Math.Round(ConvertEx.InchesToTwips(_MarginBar.HangingIndent))
                                         };

            int offSetTwips = 0;

            if (_MarginBar.HangingIndent > _MarginBar.LeftMargin)
            {
                offSetTwips = (int)Math.Round(ConvertEx.InchesToTwips(_MarginBar.HangingIndent - _MarginBar.LeftMargin) * -1);
            }
            else if (_MarginBar.LeftMargin > _MarginBar.HangingIndent)
            {
                offSetTwips = (int)Math.Round(ConvertEx.InchesToTwips(_MarginBar.LeftMargin - _MarginBar.HangingIndent));
            }

            paraformat.dxOffset = offSetTwips;

            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETPARAFORMAT, (int)SCF.DEFAULT, paraformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in _MarginBar_LeftMarginsChanged()");
            }
        }

        private void _MarginBar_RightMarginChanged(object sender, EventArgs e)
        {
            PARAFORMAT2 paraformat = new PARAFORMAT2
                                         {
                                             dwMask = PFM.RIGHTINDENT, 
                                             dxRightIndent = (int) Math.Round(ConvertEx.InchesToTwips(_MarginBar.RightMargin))
                                         };

            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETPARAFORMAT, (int)SCF.DEFAULT, paraformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in _MarginBar_RightMarginChanged()");
            }
        }

        private void _NoWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WordWrap = WordWrap.NoWrap;
        }

        private void _ParagraphToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ParagraphDialog paragraphDialog = new ParagraphDialog())
            {
                paragraphDialog.Alignment = SelectionAlignment;
                paragraphDialog.HangingIndent = SelectionHangingIndent;
                paragraphDialog.LeftIndent = SelectionIndent;
                paragraphDialog.RightIndent = SelectionRightIndent;

                if (DialogResult.OK == paragraphDialog.ShowDialog())
                {
                    SelectionAlignment = paragraphDialog.Alignment;
                    SelectionHangingIndent = paragraphDialog.HangingIndent;
                    SelectionIndent = paragraphDialog.LeftIndent;
                    SelectionRightIndent = paragraphDialog.RightIndent;
                }
            }   
        }

        private void _PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void _PrintDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            // TODO: Reset page count 
            _FirstCharOnNextPage = 0;
        }

        private void _PrintDocument_EndPrint(object sender, PrintEventArgs e)
        {
            User32.SendMessage(_RichTextBox.Handle, (uint)EM.FORMATRANGE, 0, new IntPtr(0));
        }

        private void _PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {

            // TODO: Use e.PageSettings.PrinterSettings.PrintRange and associated settings to set the measure only flag

            _FirstCharOnNextPage = FormatRange(false, e, _FirstCharOnNextPage, TextLength);
            e.HasMorePages = _FirstCharOnNextPage < TextLength;
        }

        private void _RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
        }

        #region RichTextBox Events
        private void _RichTextBox_AcceptsTabChanged(object sender, EventArgs e)
        {
            OnAcceptsTabChanged(e);
        }

        private void _RichTextBox_Click(object sender, EventArgs e)
        {
            base.OnClick(e);
        }

        private void _RichTextBox_DoubleClick(object sender, EventArgs e)
        {
            base.OnDoubleClick(e);
        }

        // ReSharper disable InconsistentNaming
        private void _RichTextBox_DragDrop(object sender, DragEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            base.OnDragDrop(e);
        }

        // ReSharper disable InconsistentNaming
        private void _RichTextBox_DragEnter(object sender, DragEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            base.OnDragEnter(e);
        }

        // ReSharper disable InconsistentNaming
        private void _RichTextBox_DragLeave(object sender, EventArgs e)
        // ReSharper restore InconsistentNaming
        {
            base.OnDragLeave(e);
        }

        // ReSharper disable InconsistentNaming
        private void _RichTextBox_DragOver(object sender, DragEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            base.OnDragOver(e);
        }

        // ReSharper disable InconsistentNaming
        private void _RichTextBox_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            base.OnGiveFeedback(e);
        }

        private void _RichTextBox_HideSelectionChanged(object sender, EventArgs e)
        {
            OnHideSelectionChanged(e);
        }

        private void _RichTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
        }

        private void _RichTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        private void _RichTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            base.OnKeyUp(e);
        }

        private void _RichTextBox_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void _RichTextBox_ModifiedChanged(object sender, EventArgs e)
        {
            OnModifiedChanged(e);
        }

        private void _RichTextBox_MouseCaptureChanged(object sender, EventArgs e)
        {
            base.OnMouseCaptureChanged(e);
        }

        private void _RichTextBox_MouseClick(object sender, MouseEventArgs e)
        {
            base.OnMouseClick(e);
        }

        private void _RichTextBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }

        private void _RichTextBox_MouseDown(object sender, MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        private void _RichTextBox_MouseEnter(object sender, EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        private void _RichTextBox_MouseHover(object sender, EventArgs e)
        {
            base.OnMouseHover(e);
        }

        private void _RichTextBox_MouseLeave(object sender, EventArgs e)
        {
            base.OnMouseLeave(e);
        }

        private void _RichTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        private void _RichTextBox_MouseUp(object sender, MouseEventArgs e)
        {
            base.OnMouseUp(e);
        }

        // ReSharper disable InconsistentNaming
        private void _RichTextBox_MouseWheel(object sender, MouseEventArgs e)
        // ReSharper restore InconsistentNaming
        {
            base.OnMouseWheel(e);
        }

        private void _RichTextBox_MultilineChanged(object sender, EventArgs e)
        {
            OnMultilineChanged(e);
        }

        private void _RichTextBox_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            base.OnPreviewKeyDown(e);
        }

        private void _RichTextBox_Protected(object sender, EventArgs e)
        {
            OnProtected(e);
        }

        private void _RichTextBox_ReadOnlyChanged(object sender, EventArgs e)
        {
            OnReadOnlyChanged(e);
        }

        private void _RichTextBox_Resize(object sender, EventArgs e)
        {
            _ImageMarker.Location = _RichTextBox.Location;
            _ImageMarker.Size = _RichTextBox.Size;

            SCROLLINFO scrollinfo = new SCROLLINFO {fMask = SIF.ALL};
            User32.GetScrollInfo(_RichTextBox.Handle, (int)SB.HORZ, scrollinfo);

            //Logging.LogMessage("Resize - ScrollInfo: Max: " + scrollinfo.nMax + " Min: " + scrollinfo.nMin + " Page: " + scrollinfo.nPage + " Pos: " + scrollinfo.nPos + " TrackPos: " + scrollinfo.nTrackPos + " Size: " + Size + " ClientRectangle: " + _RichTextBox.DisplayRectangle);

            //RECT rect = new RECT();

            //User32.SendMessage(_RichTextBox.Handle, EM.GETRECT, 0, ref rect);

            switch (WordWrap)
            {
                case WordWrap.WrapToControl:
                    {
                        // NOTE: This logic is broken. Need a way to accurately determine if text runs off the screen.
                        if (scrollinfo.nMax > _RichTextBox.ClientSize.Width)
                        {
                            User32.ShowScrollBar(_RichTextBox.Handle, (uint)SB.HORZ, false);
                        }
                        break;
                    }
                case WordWrap.WrapToPrintDocument:
                    {
                        if (scrollinfo.nMax > _PrintableWidth)
                        {
                            User32.ShowScrollBar(_RichTextBox.Handle, (uint)SB.HORZ, false);
                        }
                        break;
                    }
            }
        }

        private void _RichTextBox_SelectionChanged(object sender, EventArgs e)
        {
            OnSelectionChanged(e);
            SetupMarginBar(false);
            SetupToolStrip();
        }

        private void _RichTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TextChanged != null)
            {
                TextChanged(this, e);
            }
        }

        #endregion

        private void _SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectAll();
        }

        #region ToolStrip Events
        private void _ToolStripButtonAlignCenter_Click(object sender, EventArgs e)
        {
            if (SelectionAlignment != HorizontalAlignment.Center)
            {
                SelectionAlignment = HorizontalAlignment.Center;
                SetupToolStrip();
            }
        }

        private void _ToolStripButtonAlignLeft_Click(object sender, EventArgs e)
        {
            if (SelectionAlignment != HorizontalAlignment.Left)
            {
                SelectionAlignment = HorizontalAlignment.Left;
                SetupToolStrip();
            }
        }

        private void _ToolStripButtonAlignRight_Click(object sender, EventArgs e)
        {
            if (SelectionAlignment != HorizontalAlignment.Right)
            {
                SelectionAlignment = HorizontalAlignment.Right;
                SetupToolStrip();
            }
        }

        private void _ToolStripButtonBold_Click(object sender, EventArgs e)
        {
            _ToolStripButtonBold.Checked = !_ToolStripButtonBold.Checked;
            SelectionBold = _ToolStripButtonBold.Checked;
            SetupToolStrip();
        }

        private void _ToolStripButtonBullets_Click(object sender, EventArgs e)
        {
            _ToolStripButtonBullets.Checked = !_ToolStripButtonBullets.Checked;
            SelectionBullet = _ToolStripButtonBullets.Checked;
            SetupToolStrip();
        }

        private void _ToolStripButtonCopy_Click(object sender, EventArgs e)
        {
            Copy();
        }

        private void _ToolStripButtonCut_Click(object sender, EventArgs e)
        {
            Cut();
        }

        private void _ToolStripButtonItalic_Click(object sender, EventArgs e)
        {
            _ToolStripButtonItalic.Checked = !_ToolStripButtonItalic.Checked;
            SelectionItalic = _ToolStripButtonItalic.Checked;
            SetupToolStrip();
        }

        private void _ToolStripButtonPaste_Click(object sender, EventArgs e)
        {
            Paste();
        }

        private void _ToolStripButtonRedo_Click(object sender, EventArgs e)
        {
            Redo();
        }

        private void _ToolStripButtonUnderline_Click(object sender, EventArgs e)
        {
            _ToolStripButtonUnderline.Checked = !_ToolStripButtonUnderline.Checked;
            SelectionUnderline = _ToolStripButtonUnderline.Checked;
            SetupToolStrip();
        }

        private void _ToolStripButtonUndo_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void _ToolStripComboBoxFontColor_BackColorChanged(object sender, EventArgs e)
        {
            if (SelectionColor != _ToolStripComboBoxFontColor.BackColor)
            {
                SelectionColor = _ToolStripComboBoxFontColor.BackColor;
            }
        }

        private void _ToolStripComboBoxFontColor_DropDown(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                colorDialog.AllowFullOpen = true;
                colorDialog.AnyColor = true;
                colorDialog.Color = _ToolStripComboBoxFontColor.BackColor;

                if (CustomColors != null && CustomColors.Length > 0)
                {
                    colorDialog.CustomColors = CustomColors;    
                }

                if (DialogResult.OK == colorDialog.ShowDialog())
                {
                    CustomColors = colorDialog.CustomColors;

                    _ToolStripComboBoxFontColor.BackColor = colorDialog.Color;
                }
            }

            _ToolStripComboBoxFontColor.DroppedDown = false;
            _RichTextBox.Focus();
        }

        private void _ToolStripComboBoxFontSize_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue < 48 || e.KeyValue > 57)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void _ToolStripComboBoxFontName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_ToolStripComboBoxFontName.SelectedIndex > -1)
            {
                UpdateSelectedFormats();

                if (!string.IsNullOrEmpty(SelectionFontFamily) || SelectionFontFamily != _ToolStripComboBoxFontName.Items[_ToolStripComboBoxFontName.SelectedIndex].ToString())
                {
                    ChangeFontFamily(_ToolStripComboBoxFontName.Items[_ToolStripComboBoxFontName.SelectedIndex].ToString());
                }
            }
        }

        private void _ToolStripComboBoxFontSize_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_ToolStripComboBoxFontSize.Text))
            {
                UpdateSelectedFormats();

                if (!SelectionFontSize.HasValue || ((int)SelectionFontSize.Value).ToString() != _ToolStripComboBoxFontSize.Text)
                {
                    float newFontSize;

                    if (float.TryParse(_ToolStripComboBoxFontSize.Text, out newFontSize))
                    {
                        ChangeFontSize(newFontSize);
                    }
                }
            }
        }
        #endregion

        private void _UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void _ViewToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            SetupViewMenu();
        }

        private void _WrapToPrinterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WordWrap = WordWrap.WrapToPrintDocument;
        }

        private void _WrapToWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WordWrap = WordWrap.WrapToControl;
        }
        #endregion

        #region Private Methods
        private void ChangeAlignment(HorizontalAlignment alignment)
        {
            PARAFORMAT2 paraformat = new PARAFORMAT2
                                         {
                                             dwMask = PFM.ALIGNMENT, 
                                             wAlignment = InteropConvert.HorizontalAlignmentTo_PFA(alignment)
                                         };

            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETPARAFORMAT, (int)SCF.SELECTION, paraformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in ChangeAlignment()");
            }
        }

        private void ChangeBackColor(Color color)
        {
            CHARFORMAT2 charformat = new CHARFORMAT2
                                         {
                                             crBackColor = color.ToColorRef(), 
                                             dwMask = CFM.BACKCOLOR
                                         };

            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETCHARFORMAT, (int)SCF.SELECTION, charformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in ChangeBackColor()");
            }
        }

        private void ChangeBullet(bool addBullet)
        {
            PARAFORMAT2 paraformat = new PARAFORMAT2
                                         {
                                             dwMask = PFM.NUMBERING, 
                                             wNumbering = (ushort) (addBullet ? PFN.BULLET : 0)
                                         };

            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETPARAFORMAT, (int)SCF.SELECTION, paraformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in ChangeBullet()");
            }
        }

        private void ChangeColor(Color color)
        {
            CHARFORMAT2 charformat = new CHARFORMAT2
                                         {
                                             crTextColor = color.ToColorRef(), 
                                             dwMask = CFM.COLOR
                                         };

            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETCHARFORMAT, (int)SCF.SELECTION, charformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in ChangeColor()");
            }
        }

        private void ChangeFontFamily(string fontFamily)
        {
            CHARFORMAT2 charformat = new CHARFORMAT2
                                         {
                                             dwMask = CFM.FACE, 
                                             szFaceName = fontFamily
                                         };

            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETCHARFORMAT, (int)SCF.SELECTION, charformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in ChangeFontFamily()");
            }
        }

        private void ChangeFontSize(float fontSize)
        {
            CHARFORMAT2 charformat = new CHARFORMAT2
                                         {
                                             dwMask = CFM.SIZE, 
                                             yHeight = (int) ConvertEx.PointsToTwips(fontSize)
                                         };

            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETCHARFORMAT, (int)SCF.SELECTION, charformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in ChangeFontSize()");
            }
        }

        private void ChangeFontStyle(FontStyle fontStyle, bool addStyle)
        {
            CFM dwMask = InteropConvert.FontStyleTo_CFM(fontStyle);
            CFE dwEffects = addStyle ? (CFE)dwMask : 0;

            if (dwMask > 0)
            {
                CHARFORMAT2 charformat = new CHARFORMAT2 {dwEffects = dwEffects, dwMask = dwMask};

                bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETCHARFORMAT, (int)SCF.SELECTION, charformat) == 0;

                if (flag)
                {
                    Logging.LogMessage("Error: User32.SendMessage() returned an error in ChangeFontStyle()");
                }
            }
        }

        private void ChangeOptions(ECO options, bool enable)
        {
            User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETOPTIONS, (int)(enable ? ECOOP.OR : ECOOP.XOR), (int)options);
        }

        private void ChangeWordWrap(WordWrap wordWrap)
        {
            CycleScrollbars(_RichTextBox.ScrollBars);
            
            switch (wordWrap)
            {
                case WordWrap.NoWrap:
                    {
                        User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETTARGETDEVICE, 0, 1);
                        break;                        
                    }
                case WordWrap.WrapToPrintDocument:
                    {
                        using (Graphics g = PrintDocument.PrinterSettings.CreateMeasurementGraphics(PrintDocument.DefaultPageSettings))
                        {
                            _PrintableWidth = (PrintDocument.DefaultPageSettings.Bounds.Width - PrintDocument.DefaultPageSettings.Margins.Left - PrintDocument.DefaultPageSettings.Margins.Right);
                            int lParam = ConvertEx.HundredthInchToTwips(_PrintableWidth);
                            IntPtr wParam = g.GetHdc();
                            User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETTARGETDEVICE, wParam, lParam);
                            g.ReleaseHdc();
                        }
                        break;                                                
                    }
                case WordWrap.WrapToControl:
                    {
                        User32.SendMessage(_RichTextBox.Handle, (uint)EM.SETTARGETDEVICE, 0, 0);
                        break;
                    }
            }
        }

        private void CycleScrollbars(RichTextBoxScrollBars currentValue)
        {
            // NOTE: This hack is to resolve the issue where the horizontal scroll bar was showing up when it shouldn't.

            _RichTextBox.ScrollBars = RichTextBoxScrollBars.None;
            _RichTextBox.ScrollBars = currentValue;
        }

        private int FormatRange(bool measureOnly, PrintPageEventArgs printPageEventArgs, int startIndex, int endIndex)
        {
            CHARRANGE charrange = new CHARRANGE(startIndex, endIndex);

            RECT rc = new RECT
                          {
                              bottom = ConvertEx.HundredthInchToTwips(printPageEventArgs.MarginBounds.Bottom), 
                              left = ConvertEx.HundredthInchToTwips(printPageEventArgs.MarginBounds.Left), 
                              right = ConvertEx.HundredthInchToTwips(printPageEventArgs.MarginBounds.Right),
                              top = ConvertEx.HundredthInchToTwips(printPageEventArgs.MarginBounds.Top), 
                          };

            RECT rcPage = new RECT
                              {
                                  bottom = ConvertEx.HundredthInchToTwips(printPageEventArgs.PageBounds.Bottom), 
                                  left = ConvertEx.HundredthInchToTwips(printPageEventArgs.PageBounds.Left), 
                                  right = ConvertEx.HundredthInchToTwips(printPageEventArgs.PageBounds.Right),
                                  top = ConvertEx.HundredthInchToTwips(printPageEventArgs.PageBounds.Top),
                              };

            IntPtr hdc = printPageEventArgs.Graphics.GetHdc();

            FORMATRANGE formatrange = new FORMATRANGE
                                          {
                                              chrg = charrange, 
                                              hdc = hdc, 
                                              hdcTarget = hdc, 
                                              rc = rc, 
                                              rcPage = rcPage
                                          };

            int wParam = (measureOnly ? 0 : 1); // Non-Zero wParam means render, Zero means measure
            int returnValue = User32.SendMessage(_RichTextBox.Handle, (uint)EM.FORMATRANGE, wParam, formatrange);
            printPageEventArgs.Graphics.ReleaseHdc(hdc);

            return returnValue;
        }

        private CHARFORMAT2 GetCharformat()
        {
            CHARFORMAT2 charformat = new CHARFORMAT2();
            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.GETCHARFORMAT, (int)SCF.SELECTION, charformat) == 0;
            
            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in GetCharformat()");
            }

            return charformat;
        }

        private PARAFORMAT2 GetParaformat()
        {
            PARAFORMAT2 paraformat = new PARAFORMAT2
                                         {
                                             dwMask = (PFM.STARTINDENT | PFM.OFFSET)
                                         };

            //bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.GETPARAFORMAT, (int)SCF.DEFAULT, paraformat) == 0;
            bool flag = User32.SendMessage(_RichTextBox.Handle, (uint)EM.GETPARAFORMAT, (int)SCF.SELECTION, paraformat) == 0;

            if (flag)
            {
                Logging.LogMessage("Error: User32.SendMessage() returned an error in GetParaformat()");
            }
            
            return paraformat;
        }

        private static bool IsCharFormatSameBackColor(CHARFORMAT2 charformat)
        {
            return (charformat.dwMask & CFM.BACKCOLOR) == CFM.BACKCOLOR;
        }

        private static bool IsCharFormatSameBold(CHARFORMAT2 charformat)
        {
            return (charformat.dwMask & CFM.BOLD) == CFM.BOLD;
        }

        private static bool IsCharFormatSameColor(CHARFORMAT2 charformat)
        {
            return (charformat.dwMask & CFM.COLOR) == CFM.COLOR;
        }

        private static bool IsCharFormatSameItalic(CHARFORMAT2 charformat)
        {
            return (charformat.dwMask & CFM.ITALIC) == CFM.ITALIC;
        }

        private static bool IsCharFormatSameName(CHARFORMAT2 charformat)
        {
            return (charformat.dwMask & CFM.FACE) == CFM.FACE;
        }

        private static bool IsCharFormatSameSize(CHARFORMAT2 charformat)
        {
            return (charformat.dwMask & CFM.SIZE) == CFM.SIZE;            
        }

        private static bool IsCharFormatSameUnderline(CHARFORMAT2 charformat)
        {
            return (charformat.dwMask & CFM.UNDERLINE) == CFM.UNDERLINE;
        }

        private static bool IsParaFormatSameAlignment(PARAFORMAT2 paraformat)
        {
            return (paraformat.dwMask & PFM.ALIGNMENT) == PFM.ALIGNMENT;
        }

        private static bool IsParaFormatSameBullets(PARAFORMAT2 paraformat)
        {
            return (paraformat.dwMask & PFM.NUMBERING) == PFM.NUMBERING;
        }

        private void PopulateFontNames()
        {
            _ToolStripComboBoxFontName.Items.Clear();

            InstalledFontCollection fonts = new InstalledFontCollection();

            foreach (FontFamily fontFamily in fonts.Families)
            {
                _ToolStripComboBoxFontName.Items.Add(fontFamily.Name);  
            }
        }

        private void SetupEditMenu()
        {
            _CopyToolStripMenuItem.Enabled = (SelectionLength > 0);
            _CopyToolStripMenuItem2.Enabled = (SelectionLength > 0);
            _CutToolStripMenuItem.Enabled = (SelectionLength > 0);
            _CutToolStripMenuItem2.Enabled = (SelectionLength > 0);
            _PasteToolStripMenuItem.Enabled = CanPaste();
            _PasteToolStripMenuItem2.Enabled = CanPaste();
            _RedoToolStripMenuItem.Enabled = CanRedo;
            _RedoToolStripMenuItem2.Enabled = CanRedo;
            _UndoToolStripMenuItem.Enabled = CanUndo;
            _UndoToolStripMenuItem2.Enabled = CanUndo;
        }

        private void SetupRichTextBoxSize()
        {
            if (!_MarginBar.Visible && !_ToolStrip.Visible)
            {
                _RichTextBox.Height = Height;
                _RichTextBox.Top = 0;
            }
            else if (_MarginBar.Visible && _ToolStrip.Visible)
            {
                _RichTextBox.Height = Height - _MarginBar.Height - _ToolStrip.Height;
                _RichTextBox.Top = (_MarginBar.Bottom > _ToolStrip.Bottom ? _MarginBar.Bottom : _ToolStrip.Bottom);
            }
            else if (_MarginBar.Visible && !_ToolStrip.Visible)
            {
                _RichTextBox.Height = Height - _MarginBar.Height;
                _RichTextBox.Top = _MarginBar.Bottom;                
            }
            else if (!_MarginBar.Visible && _ToolStrip.Visible)
            {
                _RichTextBox.Height = Height - _ToolStrip.Height;
                _RichTextBox.Top = _ToolStrip.Bottom;
            }
        }

        private void SetupMarginBar(bool updateRuler)
        {
            if (updateRuler)
            {
                int pageWidthPixels = _PrintDocument.DefaultPageSettings.Bounds.Width;
                int printableWidthPixels = (int) Math.Round((double)pageWidthPixels - _PrintDocument.DefaultPageSettings.Margins.Left - _PrintDocument.DefaultPageSettings.Margins.Right);

                _MarginBar.RulerLength = ConvertEx.PixelsToInches(pageWidthPixels, 100);
                _MarginBar.RightOffset = ConvertEx.PixelsToInches((pageWidthPixels > printableWidthPixels ? pageWidthPixels - printableWidthPixels : 0), 100);
            }

            _SelectedParaformat = GetParaformat();

            if ((_SelectedParaformat.dwMask & PFM.STARTINDENT) == PFM.STARTINDENT &&
                (_SelectedParaformat.dwMask & PFM.OFFSET) == PFM.OFFSET)
            {
                _MarginBar.HangingIndent = ConvertEx.TwipsToInches(_SelectedParaformat.dxStartIndent);
                float selectionIndent = ConvertEx.TwipsToInches(_SelectedParaformat.dxStartIndent + _SelectedParaformat.dxOffset);
                _MarginBar.LeftMargin = selectionIndent;
            }

            if ((_SelectedParaformat.dwMask & PFM.RIGHTINDENT) == PFM.RIGHTINDENT)
            {
                _MarginBar.RightMargin = ConvertEx.TwipsToInches(_SelectedParaformat.dxRightIndent);
            }
        }

        private void SetupToolStrip()
        {
#if DEBUG
            DateTime enterTime = Logging.EnterMethod("SetupToolStrip");
#endif
            UpdateSelectedFormats();

            // Action buttons
            _ToolStripButtonCopy.Enabled = (SelectionLength > 1);
            _ToolStripButtonCut.Enabled = (SelectionLength > 1);
            _ToolStripButtonPaste.Enabled = CanPaste();
            _ToolStripButtonRedo.Enabled = CanRedo;
            _ToolStripButtonUndo.Enabled = CanUndo;

            // Alignment
            SetupToolStripButton(_ToolStripButtonAlignCenter, (SelectionAlignment.HasValue ? (SelectionAlignment.Value == HorizontalAlignment.Center) : false), SelectionAlignment.HasValue, Properties.Resources.AlignCenter);
            SetupToolStripButton(_ToolStripButtonAlignLeft, (SelectionAlignment.HasValue ? (SelectionAlignment.Value == HorizontalAlignment.Left) : false), SelectionAlignment.HasValue, Properties.Resources.AlignLeft);
            SetupToolStripButton(_ToolStripButtonAlignRight, (SelectionAlignment.HasValue ? (SelectionAlignment.Value == HorizontalAlignment.Right) : false), SelectionAlignment.HasValue, Properties.Resources.AlignRight);

            // Bullets
            SetupToolStripButton(_ToolStripButtonBullets, (SelectionBullet.HasValue ? SelectionBullet.Value : false), SelectionBullet.HasValue, Properties.Resources.ListBullets);

            // Font Color
            if (SelectionColor.HasValue && _ToolStripComboBoxFontColor.BackColor.ToColorRef() != SelectionColor.Value.ToColorRef())
            {
                _ToolStripComboBoxFontColor.BackColor = SelectionColor.Value;
            }

            // Font Name
            if (!string.IsNullOrEmpty(SelectionFontFamily))
            {
                if (!_ToolStripComboBoxFontName.Items.Contains(SelectionFontFamily))
                {
                    _ToolStripComboBoxFontName.Items.Add(SelectionFontFamily);
                }

                int fontIndex = _ToolStripComboBoxFontName.Items.IndexOf(SelectionFontFamily);

                if (_ToolStripComboBoxFontName.SelectedIndex != fontIndex || string.IsNullOrEmpty(_ToolStripComboBoxFontName.SelectedText))
                {
                    _ToolStripComboBoxFontName.SelectedIndex = fontIndex;
                }
            }
            else
            {
                _ToolStripComboBoxFontName.SelectedIndex = -1;
                _ToolStripComboBoxFontName.Text = string.Empty;
            }
            
            // Font Size
            if (SelectionFontSize.HasValue)
            {
                string fontSize = (Math.Round(SelectionFontSize.Value)).ToString();

                if (_ToolStripComboBoxFontSize.Text != fontSize)
                {
                    _ToolStripComboBoxFontSize.Text = fontSize;
                }
            }
            else
            {
                _ToolStripComboBoxFontSize.Text = string.Empty;
            }

            // Font Style
            SetupToolStripButton(_ToolStripButtonBold, (SelectionBold.HasValue ? SelectionBold.Value : false), SelectionBold.HasValue, Properties.Resources.Bold);
            SetupToolStripButton(_ToolStripButtonItalic, (SelectionItalic.HasValue ? SelectionItalic.Value : false), SelectionItalic.HasValue, Properties.Resources.Italic);
            SetupToolStripButton(_ToolStripButtonUnderline, (SelectionUnderline.HasValue ? SelectionUnderline.Value : false), SelectionUnderline.HasValue, Properties.Resources.Underline);

#if DEBUG
            Logging.ExitMethod("SetupToolStrip", enterTime);
#endif
        }

        private static void SetupToolStripButton(ToolStripButton toolStripButton, bool isChecked, bool isEnabled, Image enabledImage)
        {
            toolStripButton.Checked = isChecked;
            toolStripButton.Image = isEnabled ? enabledImage : enabledImage.GetDisabledImage();
        }

        private void SetupViewMenu()
        {
            _NoWrapToolStripMenuItem.Checked = (WordWrap == WordWrap.NoWrap);
            _WrapToPrinterToolStripMenuItem.Checked = (WordWrap == WordWrap.WrapToPrintDocument);
            _WrapToWindowToolStripMenuItem.Checked = (WordWrap == WordWrap.WrapToControl);
        }

        private void UpdateSelectedFormats()
        {
            _SelectedCharformat = GetCharformat();
            _SelectedParaformat = GetParaformat();
        }
        #endregion

        #region Protected Methods
        protected void OnAcceptsTabChanged(EventArgs e)
        {
            if (AcceptsTabChanged != null)
            {
                AcceptsTabChanged(this, e);
            }
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            _RichTextBox.Focus();
        }

        protected void OnHideSelectionChanged(EventArgs e)
        {
            if (HideSelectionChanged != null)
            {
                HideSelectionChanged(this, e);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PopulateFontNames();
            SetupToolStrip();

            _RichTextBox.Focus();
            _RichTextBox.SelectionStart = 0;
            _RichTextBox.SelectionLength = 0;
        }

        protected void OnModifiedChanged(EventArgs e)
        {
            if (ModifiedChanged != null)
            {
                ModifiedChanged(this, e);
            }
        }

        protected void OnMultilineChanged(EventArgs e)
        {
            if (MultilineChanged != null)
            {
                MultilineChanged(this, e);
            }
        }

        protected void OnProtected(EventArgs e)
        {
            if (Protected != null)
            {
                Protected(this, e);
            }
        }

        protected void OnReadOnlyChanged(EventArgs e)
        {
            if (ReadOnlyChanged != null)
            {
                ReadOnlyChanged(this, e);
            }
        }

        protected void OnSelectionChanged(EventArgs e)
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, e);
            }
        }
        #endregion

        #region Public Methods
        public void AppendText(string text)
        {
            _RichTextBox.AppendText(text);
        }

        public bool CanPaste()
        {
            bool canPaste = false;

            IDataObject dataObject = null;

            try
            {
                dataObject = Clipboard.GetDataObject();
            }
            catch (ExternalException exception)
            {
                Logging.LogException(exception);
            }

            if (dataObject != null)
            {
                // Assuming that if the native format can't be pasted then it will be converted in the Paste() method. May need to switch to GetFormats(bool) to get the native format only
                string[] formats = dataObject.GetFormats();

                foreach (string format in formats)
                {
                    DataFormats.Format clipFormat = DataFormats.GetFormat(format);

                    if (clipFormat != null && CanPaste(clipFormat))
                    {
                        canPaste = true;
                        break;
                    }
                }
            }
         
            return canPaste;
        }

        public bool CanPaste(DataFormats.Format clipFormat)
        {
            return _RichTextBox.CanPaste(clipFormat);
        }

        public void Clear()
        {
            _RichTextBox.Clear();
        }

        public void ClearUndo()
        {
            _RichTextBox.ClearUndo();
        }

        public void Copy()
        {
            _RichTextBox.Copy();
        }

        public void Cut()
        {
            _RichTextBox.Cut();
        }

        public void DeselectAll()
        {
            _RichTextBox.DeselectAll();
        }

        public int Find(char[] characterSet)
        {
            return _RichTextBox.Find(characterSet);
        }

        public int Find(char[] characterSet, int start)
        {
            return _RichTextBox.Find(characterSet, start);
        }

        public int Find(char[] characterSet, int start, int end)
        {
            return _RichTextBox.Find(characterSet, start, end);
        }

        public int Find(string text)
        {
            return _RichTextBox.Find(text);
        }

        public int Find(string text, int start, int end, RichTextBoxFinds options)
        {
            return _RichTextBox.Find(text, start, end, options);
        }

        public int Find(string text, int start, RichTextBoxFinds options)
        {
            return _RichTextBox.Find(text, start, options);
        }

        public int Find(string text, RichTextBoxFinds options)
        {
            return _RichTextBox.Find(text, options);
        }

        public char GetCharFromPosition(Point point)
        {
            return _RichTextBox.GetCharFromPosition(point);
        }

        public int GetCharIndexFromPosition(Point point)
        {
            return _RichTextBox.GetCharIndexFromPosition(point);
        }

        public int GetFirstCharIndexFromLine(int lineNumber)
        {
            return _RichTextBox.GetFirstCharIndexFromLine(lineNumber);
        }

        public int GetFirstCharIndexOfCurrentLine()
        {
            return _RichTextBox.GetFirstCharIndexOfCurrentLine();
        }

        public int GetLineFromCharIndex(int index)
        {
            return _RichTextBox.GetLineFromCharIndex(index);
        }

        public Point GetPositionFromCharIndex(int index)
        {
            return _RichTextBox.GetPositionFromCharIndex(index);
        }

        public void LoadFile(Stream data, RichTextBoxStreamType fileType)
        {
            _RichTextBox.LoadFile(data, fileType);
        }

        public void LoadFile(string path)
        {
            _RichTextBox.LoadFile(path);
        }

        public void LoadFile(string path, RichTextBoxStreamType fileType)
        {
            _RichTextBox.LoadFile(path, fileType);
        }

        public void Paste()
        {
            _RichTextBox.Paste();
        }

        public void Redo()
        {
            _RichTextBox.Redo();
        }

        public void SaveFile(Stream data, RichTextBoxStreamType fileType)
        {
            _RichTextBox.SaveFile(data, fileType);
        }

        public void SaveFile(string path)
        {
            _RichTextBox.SaveFile(path);
        }

        public void SaveFile(string path, RichTextBoxStreamType fileType)
        {
            _RichTextBox.SaveFile(path, fileType);
        }

        public void ScrollToCaret()
        {
            _RichTextBox.ScrollToCaret();
        }

        public void Select(int start, int length)
        {
            _RichTextBox.Select(start, length);
        }

        public void SelectAll()
        {
            _RichTextBox.SelectAll();
        }

        public void Undo()
        {
            _RichTextBox.Undo();
        }

        public void UpdatePrintDocument(PrintDocument printDocument)
        {
            UseWaitCursor = true;
            Application.DoEvents();

            _PrintDocument.DefaultPageSettings = printDocument.DefaultPageSettings;
            _PrintDocument.PrinterSettings = printDocument.PrinterSettings;

            SetupMarginBar(true);

            if (_WordWrap == WordWrap.WrapToPrintDocument)
            {
                ChangeWordWrap(_WordWrap);
                Refresh();
            }

            UseWaitCursor = false;
        }
        #endregion       
    }
}
