using System;
using System.Collections.Generic;
using System.Linq;

namespace WpfApp1
{
    public class TraceLogProvider
    {
        /// <summary>
        /// Singleton instance for TraceLogProvider
        /// </summary>
        public static TraceLogProvider Instance()
        {
            return provider;
        }

        /// <summary>
        /// Event to sync logs for instance of datetime.
        /// </summary>
        public event DelegateSyncTraceLogs OnSyncTraceLogsUpdated;

        /// <summary>
        /// Find near by log for datetime in list of logs.
        /// </summary>
        /// <param name="logs"></param>
        /// <param name="nearByTime"></param>
        /// <returns></returns>
        public TraceLog FindNearbyLog( IEnumerable<TraceLog> logs, DateTime nearByTime )
        {
            TraceLog nearByLog = null;

            if ( logs != null && nearByTime != default( DateTime ) )
            {
                nearByLog = logs.OrderBy( t => Math.Abs( ( t.LogDateTime - nearByTime ).Ticks ) ).FirstOrDefault();
            }

            return nearByLog;
        }

        public void RaiseSyncTraceLogsUpdated( Guid senderGuid, DateTime dateTime )
        {
            OnSyncTraceLogsUpdated?.Invoke( new SyncTraceLogsEventArgs( senderGuid, dateTime ) );
        }

        private static TraceLogProvider provider = new TraceLogProvider();
    }

    public delegate void DelegateSyncTraceLogs( SyncTraceLogsEventArgs syncTracesArgs );

    public class SyncTraceLogsEventArgs : EventArgs
    {
        /// <summary>
        /// Event data when user whats to sync logs between multiple tabs.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="syncDateTime"></param>
        public SyncTraceLogsEventArgs( Guid senderTabGuid, DateTime syncDateTime )
        {
            SenderTabGuid = senderTabGuid;
            SyncDateTime = syncDateTime;
        }

        public Guid SenderTabGuid { get; }

        public DateTime SyncDateTime { get; }
    }
}
