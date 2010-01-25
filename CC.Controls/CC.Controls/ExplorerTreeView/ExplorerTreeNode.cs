using System.IO;
using System.Windows.Forms;

namespace CC.Controls
{
    /// <summary>
    /// Represents a <see cref="TreeNode"/> in an <see cref="ExplorerTreeView"/>
    /// </summary>
    public class ExplorerTreeNode: TreeNode
    {
        #region Constructor
        /// <summary>
        /// Creates a new <see cref="ExplorerTreeNode"/>
        /// </summary>
        public ExplorerTreeNode() : this(string.Empty) { }

        /// <summary>
        /// Creates a new <see cref="ExplorerTreeNode"/> using the supplied path
        /// </summary>
        /// <param name="path">The file path represented by this <see cref="ExplorerTreeNode"/></param>
        public ExplorerTreeNode(string path)
        {
            SetProperties(path);
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets or sets the file path represented by this <see cref="ExplorerTreeNode"/>
        /// </summary>
        public string FullName { get; private set; }

        /// <summary>
        /// Gets or sets the <see cref="ExplorerTreeNodeType"/> represented by this <see cref="ExplorerTreeNode"/>
        /// </summary>
        public ExplorerTreeNodeType NodeType { get; private set; }
        #endregion

        #region Private Methods
        private void SetProperties(string path)
        {
            if (Directory.Exists(path))
            {
                var directoryInfo = new DirectoryInfo(path);

                FullName = directoryInfo.FullName;
                Name = directoryInfo.Name;
                NodeType = ExplorerTreeNodeType.Directory;
            }
            else if (File.Exists(path))
            {
                var fileInfo = new FileInfo(path);

                FullName = fileInfo.FullName;
                Name = fileInfo.Name;
                NodeType = ExplorerTreeNodeType.File;                
            }
            else
            {
                FullName = string.Empty;
                Name = path;
                NodeType = ExplorerTreeNodeType.Standard;
            }

            Text = Name;
            ToolTipText = !string.IsNullOrEmpty(FullName) ? FullName : Name;
        }
        #endregion
    }
}
