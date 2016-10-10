using System;
using System.IO;

namespace YouSendIt.Entities
{
    public class DownloadedFileInfo
    {
        public double Progress
        {
            get;
            set;
        }

        public long BytesReceived
        {
            get;
            set;
        }

        public Exception Error
        {
            get;
            set;
        }

        public FileInfo File
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }
    }
}
