using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CC.Utilities.Interop;

namespace CC.Controls
{
    public partial class ExplorerTreeView : TreeView
    {
        //TODO: Could override TreeView more to provide a more strongly typed class. Good enough for now...
        //TODO: Needs comments...

        public ExplorerTreeView()
        {
            InitializeComponent();
        }

        #region Private Fields
        private bool _AutoCheck;
        private string _AutoCheckExcludedDirectories;
        private string _AutoCheckExcludedFiles;
        private readonly List<string> _AutoCheckExcludedDirectoriesList = new List<string>();
        private readonly List<string> _AutoCheckExcludedFilesList = new List<string>();
        private string _RootPath;
        #endregion

        #region Public Properties
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoCheck
        {
            get { return _AutoCheck; }
            set
            {
                _AutoCheck = value;

                if (value)
                {
                    AutoPopulate = true;
                    CheckBoxes = true;
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AutoCheckExcludedDirectories
        {
            get { return _AutoCheckExcludedDirectories; }
            set
            {
                _AutoCheckExcludedDirectories = value;
                _AutoCheckExcludedDirectoriesList.Clear();

                if (!string.IsNullOrEmpty(_AutoCheckExcludedDirectories))
                {
                    string[] excludeValues = _AutoCheckExcludedDirectories.Split(';');

                    foreach (string excludeValue in excludeValues)
                    {
                        _AutoCheckExcludedDirectoriesList.Add(excludeValue);
                    }
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string AutoCheckExcludedFiles
        {
            get { return _AutoCheckExcludedFiles; }
            set
            {
                _AutoCheckExcludedFiles = value;
                _AutoCheckExcludedFilesList.Clear();

                if (!string.IsNullOrEmpty(_AutoCheckExcludedFiles))
                {
                    string[] excludeValues = _AutoCheckExcludedFiles.Split(';');

                    foreach (string excludeValue in excludeValues)
                    {
                        _AutoCheckExcludedFilesList.Add(excludeValue);
                    }
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool AutoPopulate { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new TreeNodeCollection Nodes
        {
            get { return base.Nodes; }
        }

        public string RootPath
        {
            get { return _RootPath; }
            set { UpdateRootPath(value); }
        }
        #endregion

        #region Event Handlers
        private void ExplorerTreeView_AfterCheck(object sender, TreeViewEventArgs e)
        {
            ExplorerTreeNode explorerTreeNode = e.Node as ExplorerTreeNode;

            if (explorerTreeNode != null)
            {
                if (!explorerTreeNode.Checked)
                {
                    UncheckNodes(explorerTreeNode);
                }
                else if (AutoCheck)
                {
                    CheckNodes(explorerTreeNode);
                }
            }
        }

        private void ExplorerTreeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            ExplorerTreeNode node = e.Node as ExplorerTreeNode;

            if (node != null)
            {
                PopulateNode(ref node);
            }
        }
        #endregion

        #region Private Methods
        private bool AutoCheckDirectory(string shortPath)
        {
            bool returnValue = true;

            foreach (string excludedDirectory in _AutoCheckExcludedDirectoriesList)
            {
                if (!string.IsNullOrEmpty(excludedDirectory))
                {
                    if (excludedDirectory.Contains("*"))
                    {
                        returnValue = !shortPath.ToLower().Contains(excludedDirectory.Replace("*", string.Empty).ToLower());
                    }
                    else
                    {
                        returnValue = !(excludedDirectory.ToLower() == shortPath.ToLower());
                    }

                    if (!returnValue)
                    {
                        break;
                    }
                }
            }

            return returnValue;
        }

        private bool AutoCheckFile(string shortPath)
        {
            bool returnValue = true;

            foreach (string excludedFile in _AutoCheckExcludedFilesList)
            {
                if (!string.IsNullOrEmpty(excludedFile))
                {
                    if (excludedFile.Contains("*"))
                    {
                        if (excludedFile.StartsWith("*") && excludedFile.EndsWith("*"))
                        {
                            returnValue = !shortPath.ToLower().Contains(excludedFile.Replace("*", string.Empty).ToLower());
                        }
                        else if (excludedFile.StartsWith("*"))
                        {
                            returnValue = !shortPath.ToLower().EndsWith(excludedFile.Replace("*", string.Empty).ToLower());
                        }
                        else if (excludedFile.EndsWith("*"))
                        {
                            returnValue = !shortPath.ToLower().StartsWith(excludedFile.Replace("*", string.Empty).ToLower());
                        }
                    }
                    else
                    {
                        returnValue = !shortPath.ToLower().Contains(excludedFile.ToLower());
                    }

                    if (!returnValue)
                    {
                        break;
                    }
                }
            }

            return returnValue;
        }

        private bool AutoCheckExplorerTreeNode(ExplorerTreeNode explorerTreeNode)
        {
            bool returnValue;

            switch (explorerTreeNode.NodeType)
            {
                case ExplorerTreeNodeType.Directory:
                    {
                        returnValue = AutoCheckDirectory(explorerTreeNode.Name);
                        break;
                    }
                case ExplorerTreeNodeType.File:
                    {
                        returnValue = AutoCheckFile(explorerTreeNode.Name);
                        break;
                    }
                default:
                    {
                        returnValue = false;
                        break;
                    }
            }

            return returnValue;
        }

        private void CheckNodes(TreeNode parentNode)
        {
            foreach (ExplorerTreeNode node in parentNode.Nodes)
            {
                node.Checked = AutoCheckExplorerTreeNode(node);
            }
        }

        private ExplorerTreeNode CreateNode(string path)
        {
            ExplorerTreeNode treeNode = new ExplorerTreeNode(path)
                                            {
                                                ImageIndex = GetImageIndex(path),
                                                SelectedImageIndex = GetImageIndex(path),
                                            };

            if (treeNode.NodeType == ExplorerTreeNodeType.Directory)
            {
                treeNode.Nodes.Add(new ExplorerTreeNode());
            }

            return treeNode;
        }

        private static void GetCheckedFiles(TreeNode treeNode, ref List<string> checkedFiles)
        {
            foreach (ExplorerTreeNode explorerTreeNode in treeNode.Nodes)
            {
                if (explorerTreeNode.Checked && explorerTreeNode.NodeType == ExplorerTreeNodeType.File)
                {
                    checkedFiles.Add(explorerTreeNode.FullName);
                }

                GetCheckedFiles(explorerTreeNode, ref checkedFiles);
            }    
        }

        private List<ExplorerTreeNode> GetDirectoryNodes(string folder)
        {
            List<ExplorerTreeNode> returnValue = new List<ExplorerTreeNode>();

            foreach (string directory in Directory.GetDirectories(folder, "*", SearchOption.TopDirectoryOnly))
            {
                ExplorerTreeNode directoryNode = CreateNode(directory);

                if (directoryNode != null)
                {
                    if (AutoPopulate)
                    {
                        PopulateNode(ref directoryNode);    
                    }

                    returnValue.Add(directoryNode);
                }
            }

            return returnValue;
        }

        private List<ExplorerTreeNode> GetFileNodes(string folder)
        {
            List<ExplorerTreeNode> returnValue = new List<ExplorerTreeNode>();

            foreach (string file in Directory.GetFiles(folder, "*", SearchOption.TopDirectoryOnly))
            {
                ExplorerTreeNode fileNode = CreateNode(file);

                if (fileNode != null)
                {
                    returnValue.Add(fileNode);
                }
            }

            return returnValue;
        }

        private int GetImageIndex(string path)
        {
            int imageIndex = -1;
            string imageKey = string.Empty;

            if (Directory.Exists(path))
            {
                imageKey = "Folder";    
            }
            else if (File.Exists(path))
            {
                FileInfo fileInfo = new FileInfo(path);
                imageKey = fileInfo.Name.ToLower(); //TODO: This should be on an exclude list excluding extensions like *.ico, *.exe, etc.
            }

            if (!string.IsNullOrEmpty(imageKey))
            {
                if (!_ImageListSmall.Images.ContainsKey(imageKey))
                {
                    SHFILEINFO shfileinfo = new SHFILEINFO();
                    IntPtr hResult = Shell32.SHGetFileInfo(path, 0, ref shfileinfo, (uint)Marshal.SizeOf(shfileinfo), (uint)(SHGFI.ICON | SHGFI.SMALLICON));
                    //IntPtr hResult = NativeImports.SHGetFileInfo(path, 0, ref shfileinfo, (uint)Marshal.SizeOf(shfileinfo), NativeImports.SHGFI_ICON | NativeImports.SHGFI_LARGEICON);

                    _ImageListSmall.Images.Add(imageKey, Icon.FromHandle(shfileinfo.hIcon));
                }

                imageIndex = _ImageListSmall.Images.IndexOfKey(imageKey);
            }

            return imageIndex;
        }

        private void PopulateNode(ref ExplorerTreeNode node)
        {
            if (node.Nodes.Count <= 1)
            {
                node.Nodes.Clear();

                List<ExplorerTreeNode> directoryNodes = GetDirectoryNodes(node.FullName);
                List<ExplorerTreeNode> fileNodes = GetFileNodes(node.FullName);

                if (directoryNodes.Count > 0)
                {
                    node.Nodes.AddRange(directoryNodes.ToArray());
                }

                if (fileNodes.Count > 0)
                {
                    node.Nodes.AddRange(fileNodes.ToArray());
                }
            }

            Application.DoEvents(); // TODO: Not sure if this is necessary
        }

        private static void UncheckNodes(TreeNode parentNode)
        {
            foreach (TreeNode node in parentNode.Nodes)
            {
                node.Checked = false;
            }
        }

        private void UpdateRootPath(string rootPath)
        {
            if (Directory.Exists(rootPath))
            {
                _RootPath = rootPath;

                Nodes.Clear();


                ExplorerTreeNode rootNode = new ExplorerTreeNode(rootPath)
                                        {
                                            ImageIndex = GetImageIndex(rootPath),
                                            SelectedImageIndex = GetImageIndex(rootPath),
                                            Text = rootPath,
                                            ToolTipText = rootPath,
                                        };

                PopulateNode(ref rootNode);
                Nodes.Add(rootNode);
                rootNode.Checked = AutoCheck;
                rootNode.Expand();
            }
        }
        #endregion

        #region Public Methods
        public List<string> GetCheckedFiles()
        {
            List<string> returnValue = new List<string>();
            
            foreach (ExplorerTreeNode explorerTreeNode in Nodes)
            {
                if (explorerTreeNode.Checked && explorerTreeNode.NodeType == ExplorerTreeNodeType.File)
                {
                    returnValue.Add(explorerTreeNode.FullName);
                }

                GetCheckedFiles(explorerTreeNode, ref returnValue);
            }

            return returnValue;
        }
        #endregion
    }
}
