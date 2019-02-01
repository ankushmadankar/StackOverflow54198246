using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for TraceViewerView.xaml
    /// </summary>
    public partial class TraceViewerView : UserControl
    {
        public TraceViewerView()
        {
            InitializeComponent();

           // TraceLogProvider.Instance().OnSyncTraceLogsUpdated += SyncWithDateTimeInstance;
        }

        private async void SyncWithDateTimeInstance( SyncTraceLogsEventArgs syncTracesArgs )
        {
            TraceViewerViewModel currentVm;

            if ( DataContext is TraceViewerViewModel && syncTracesArgs != null )
            {
                currentVm = DataContext as TraceViewerViewModel;

                if ( currentVm.TabGuid != syncTracesArgs.SenderTabGuid )
                {
                    var log = await Task.Run( () => TraceLogProvider.Instance().FindNearbyLog( currentVm.Logs, syncTracesArgs.SyncDateTime ) );

                    if ( log != null )
                    {
                        dgvLogs.ScrollIntoView( log );
                    }
                }
            }
        }
    }
}
