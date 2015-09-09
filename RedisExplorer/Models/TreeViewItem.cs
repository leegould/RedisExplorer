using System.Collections.ObjectModel;

using Caliburn.Micro;

using RedisExplorer.Interface;

namespace RedisExplorer.Models
{
    /// <summary>
    /// Tree View Item view Model
    /// </summary>
    /// <see cref="http://www.codeproject.com/Articles/26288/Simplifying-the-WPF-TreeView-by-Using-the-ViewMode"/>
    public class TreeViewItem : PropertyChangedBase, ITreeViewItemViewModel
    {
        #region Members

        static readonly TreeViewItem DummyChild = new TreeViewItem();

        readonly ObservableCollection<TreeViewItem> children;
        readonly TreeViewItem parent;

        bool isExpanded;
        bool isSelected;

        #endregion // Members

        #region Properties

        public string Display { get; set; }

        public TreeViewItem Parent
        {
            get { return parent; }
        }

        public ObservableCollection<TreeViewItem> Children
        {
            get { return children; }
        }

        public bool HasDummyChild
        {
            get { return Children.Count == 1 && Children[0] == DummyChild; }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (value.Equals(isExpanded)) return;
                
                isExpanded = value;
                    
                if (isExpanded && parent != null)
                    parent.IsExpanded = true;

                if (HasDummyChild)
                {
                    Children.Remove(DummyChild);
                    LoadChildren();
                }
                NotifyOfPropertyChange(() => IsExpanded);
            }
        }

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value.Equals(isSelected)) return;

                isSelected = value;
                NotifyOfPropertyChange(() => IsSelected);
            }
        }

        #endregion // Properties

        #region Constructors

        protected TreeViewItem(TreeViewItem parent, bool lazyLoadChildren)
        {
            this.parent = parent;

            children = new ObservableCollection<TreeViewItem>();

            if (lazyLoadChildren)
            {
                children.Add(DummyChild);
            }
        }

        // This is used to create the DummyChild instance.
        private TreeViewItem()
        {
        }

        #endregion // Constructors

        /// <summary>
        /// Invoked when the child items need to be loaded on demand.
        /// Subclasses can override this to populate the Children collection.
        /// </summary>
        protected virtual void LoadChildren()
        {
        }
    }
}
