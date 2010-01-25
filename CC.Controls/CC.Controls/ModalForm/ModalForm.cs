using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace CC.Controls
{
    // TODO: Needs comments ...
    public partial class ModalForm : Form
    {
        #region Constructor
        public ModalForm()
        {
            base.FormBorderStyle = FormBorderStyle.None;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowInTaskbar = false;
            base.SizeGripStyle = SizeGripStyle.Hide;
            base.StartPosition = FormStartPosition.CenterScreen;
            base.TopMost = true;
            base.WindowState = FormWindowState.Maximized;

            InitializeComponent();
        }
        #endregion

        #region Private Fields
        private readonly List<Form> _ChildrenForms = new List<Form>();
        #endregion

        #region Public Properties
        public Form Content { get; set; }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DialogResult DialogResult
        {
            get
            {
                return (Content != null) ? Content.DialogResult : DialogResult.Cancel;
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool IsMdiContainer
        {
            get { return base.IsMdiContainer; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool MaximizeBox
        {
            get { return base.MaximizeBox; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool MinimizeBox
        {
            get { return base.MinimizeBox; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool ShowInTaskbar
        {
            get { return base.ShowInTaskbar; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new SizeGripStyle SizeGripStyle
        {
            get { return base.SizeGripStyle; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormStartPosition StartPosition
        {
            get { return base.StartPosition; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool TopMost
        {
            get { return base.TopMost; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormWindowState WindowState
        {
            get { return base.WindowState; }
        }
        #endregion

        #region Private Methods
        private void CloseChildrenForms()
        {
            if (_ChildrenForms.Count > 0)
            {
                foreach (Form childForm in _ChildrenForms)
                {
                    childForm.Close();
                }
            }
        }

        /// <summary>
        /// A method to create a child <see cref="Form"/> with the same properties of the <see cref="ModalForm"/> that occupies the entire <see cref="Screen"/>
        /// </summary>
        /// <returns></returns>
        private Form CreateChildForm(Screen screen)
        {
            //TODO: Probably want to copy all properties eventually. For now I'll just work on the ones I know I need for my current implementation

            return new Form
                       {
                           BackColor = BackColor,
                           FormBorderStyle = FormBorderStyle,
                           MaximizeBox = MaximizeBox,
                           MinimizeBox = MinimizeBox,
                           Opacity = Opacity,
                           ShowInTaskbar = ShowInTaskbar,
                           SizeGripStyle = SizeGripStyle,
                           StartPosition = FormStartPosition.Manual,
                           TopMost = true,
                           WindowState = WindowState,

                           Location = screen.WorkingArea.Location,
                           Size = screen.WorkingArea.Size
                       };
        }

        private void CreateChildrenForms()
        {
            if (Screen.AllScreens.Length > 1)
            {
                foreach (Screen screen in Screen.AllScreens)
                {
                    if (screen.Primary == false)
                    {
                        _ChildrenForms.Add(CreateChildForm(screen));
                    }
                }
            }
        }

        private void ShowChildrenForms()
        {
            if (_ChildrenForms.Count > 0)
            {
                foreach (Form childForm in _ChildrenForms)
                {
                    childForm.Show();
                }
            }
        }

        private void ShowContentForm()
        {
            if (Content != null)
            {
                Content.ShowDialog(this);
            }

            Close();
        }
        #endregion

        #region Protected Methods
        protected override void OnClosing(CancelEventArgs e)
        {
            CloseChildrenForms();

            base.OnClosing(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            CreateChildrenForms();
            ShowChildrenForms();
            ShowContentForm();
        }
        #endregion

        #region Public Methods
        [Obsolete("Use ShowDialog() instead.")]
        public new void Show()
        {
            throw new InvalidOperationException("You cannot Show() a ModalForm. Use ShowDialog() instead.");
        }

        [Obsolete("Use ShowDialog(IWin32Window owner) instead.")]
        public new void Show(IWin32Window owner)
        {
            throw new InvalidOperationException("You cannot Show(IWin32Window owner) a ModalForm. Use ShowDialog(IWin32Window owner) instead.");
        }
        
        public new DialogResult ShowDialog()
        {
            if (Content == null)
            {
                throw new InvalidOperationException("Cannot show a ModalForm that does not have it's Content property set.");    
            }

            base.ShowDialog();
            return DialogResult;
        }

        public new DialogResult ShowDialog(IWin32Window owner)
        {
            if (Content == null)
            {
                throw new InvalidOperationException("Cannot show a ModalForm that does not have it's Content property set.");
            }

            base.ShowDialog(owner);
            return DialogResult;
        }
        #endregion
    }
}
