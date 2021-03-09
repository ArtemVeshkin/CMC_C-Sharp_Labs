using System;

namespace ClassLibrary
{
    public class DataChangedEventArgs : EventArgs
    {
        public ChangeInfo ChangeInfo { get; set; }

        public string Info { get; set; }

        public DataChangedEventArgs(ChangeInfo ChangeInfo, string Info)
        {
            this.ChangeInfo = ChangeInfo;
            this.Info = Info;
        }

        public override string ToString() => $"Type of event: {ChangeInfo}\nInfo: \"{Info}\"";
    }
}
