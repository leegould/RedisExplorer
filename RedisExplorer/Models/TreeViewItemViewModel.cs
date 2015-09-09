using System.Collections.ObjectModel;

using Caliburn.Micro;

namespace RedisExplorer.Models
{
    /// <summary>
    /// Tree View Item view Model
    /// </summary>
    /// <see cref="http://www.codeproject.com/Articles/26288/Simplifying-the-WPF-TreeView-by-Using-the-ViewMode"/>
    public class TreeViewItemViewModel : PropertyChangedBase
    {
        #region Members

        static readonly TreeViewItemViewModel DummyChild = new TreeViewItemViewModel();

        readonly ObservableCollection<TreeViewItemViewModel> children;
        readonly TreeViewItemViewModel parent;

        bool isExpanded;
        bool isSelected;

        #endregion // Members

        #region Properties

        public TreeViewItemViewModel Parent
        {
            get { return parent; }
        }

        public ObservableCollection<TreeViewItemViewModel> Children
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

        protected TreeViewItemViewModel(TreeViewItemViewModel parent, bool lazyLoadChildren)
        {
            this.parent = parent;

            children = new ObservableCollection<TreeViewItemViewModel>();

            if (lazyLoadChildren)
            {
                children.Add(DummyChild);
            }
        }

        // This is used to create the DummyChild instance.
        private TreeViewItemViewModel()
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
