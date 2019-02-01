using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    public class TraceViewerViewModel : PropertyObservable, ITabItem
    {
        public TraceViewerViewModel( ) : this( 0 )
        {

        }

        public TraceViewerViewModel( int offset )
        {
            TabName = "Test Tab";
            TabGuid = Guid.NewGuid();

            SyncLogsCommand = new RelayCommand<TraceLog>( log => SyncLogs( log ) );

            GenerateTestLogs( 500, offset );

            TraceLogProvider.Instance().OnSyncTraceLogsUpdated += SyncWithDateTimeInstance;
        }

        /// <summary>
        /// Sync logs for datetime with other open tabs.
        /// </summary>
        public RelayCommand<TraceLog> SyncLogsCommand { get; }

        public IEnumerable<TraceLog> Logs
        {
            get { return traceLogs; }
            private set
            {
                traceLogs = value;

                OnPropertyChanged();
            }
        }

        public TraceLog Log
        {
            get { return synclog; }
            private set
            {
                synclog = value;

                OnPropertyChanged();
            }
        }
        
        public string TabName { get; set; }

        public Guid TabGuid { get; }

        private void GenerateTestLogs( int logCount, int offset )
        {
            List<TraceLog> logs = new List<TraceLog>( logCount );

            for( int i = 0; i < logCount; i++ )
            {
                logs.Add( new TraceLog ( DateTime.Now.AddMinutes( i + offset ), $"Test log number {i}") );
            }

            Logs = logs;
        }

        private void SyncLogs( TraceLog log )
        {
            if ( log != null )
            {
                TraceLogProvider.Instance().RaiseSyncTraceLogsUpdated( TabGuid, log.LogDateTime );
            }
        }

        private async void SyncWithDateTimeInstance( SyncTraceLogsEventArgs syncTracesArgs )
        {
            if ( syncTracesArgs != null )
            {
                if ( TabGuid != syncTracesArgs.SenderTabGuid )
                {
                    var log = await Task.Run( () => TraceLogProvider.Instance().FindNearbyLog( Logs, syncTracesArgs.SyncDateTime ) );

                    if ( log != null )
                    {
                        Log = null;
                        Log = log;
                    }
                }
            }
        }

        private TraceLog synclog;
        private IEnumerable<TraceLog> traceLogs;
    }

    public class TraceLog
    {
        public TraceLog( DateTime logDateTime, string traceString )
        {
            LogDateTime = logDateTime;
            TraceString = traceString;
        }

        public DateTime LogDateTime { get; private set; }

        public string TraceString { get; private set; }

    }
}
