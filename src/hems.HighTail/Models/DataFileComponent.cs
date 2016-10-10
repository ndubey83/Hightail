using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YouSendIt.Entities;

namespace hems.HighTail.Models {
    public class DataFileComponent {
        public string HighTailFolder { get; set; }
        public int? HighTailBaseFolderId { get; set; }
        public string Email { get; set; }
        public Folder Folder { get; set; }
        public FileInfoType File { get; set; }
        public bool Enabled { get; set; }
        public bool ConvertToExcel { get; set; }
        public int? ExpirationDay { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
