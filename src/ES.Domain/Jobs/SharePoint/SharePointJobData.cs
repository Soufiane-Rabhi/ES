using SAM.Domain.Models;
using System.Collections.Generic;

namespace SAM.Domain.Jobs.SharePoint
{
    public class SharePointJobData
    {
        public string Directory { get; }
        public IEnumerable<SharePointCommand> Permissions { get; set; }

        public SharePointJobData(string directory, IEnumerable<SharePointCommand> permissions)
        {
            Directory = directory;
            Permissions = permissions;
        }
    }
}
