using System;
using System.Collections.ObjectModel;

namespace WpfApp1
{
    public class MainWindowViewModel : PropertyObservable
    {
        public MainWindowViewModel()
        {
            tabs = new ObservableCollection<ITabItem>();

            //Default tab
            Tabs.Add( new TraceViewerViewModel(5) );
            Tabs.Add( new TraceViewerViewModel(10) );
            Tabs.Add( new TraceViewerViewModel(15) );

            AddTabCommand = new RelayCommand<object>( obj => Tabs.Add( new TraceViewerViewModel(20) ) );
        }

        public RelayCommand<object> AddTabCommand { get; }

        public ObservableCollection<ITabItem> Tabs
        {
            get
            {
                return tabs;
            }
        }
        
        private readonly ObservableCollection<ITabItem> tabs;
    }

    public interface ITabItem
    {
        /// <summary>Gets or sets the display name of the tab.</summary>
        /// <value>The name of the tab.</value>
        string TabName { get; set; }

        /// <summary>Gets the tab unique identifier.</summary>
        /// <value>The tab unique identifier.</value>
        Guid TabGuid { get; }
    }
}
