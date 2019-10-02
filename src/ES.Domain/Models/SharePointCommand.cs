using System;
using System.Text.RegularExpressions;

namespace ES.Domain.Models
{

    public enum Status
    {
        Default = 1,
        Add = 2,
        Delete = 3,
    }

    public class SharePointCommand
    {
        public Guid PermissionId { get; set; }
        public string TechnicalId { get; set; }
        public string Name { get; set; }
        public Status Status { get; set; }

        public Guid SiteId { get; set; }
        public string SiteUrl { get; set; }

        public Guid UserId { get; set; }
        public string UserUpn { get; set; }
        public string UserEmail { get; set; }

        public string FormattedUserUpn => ExtractUpn(UserUpn);
        
        private readonly Regex emailMatcher = new Regex(@"(?<Email>.*)#EXT#.*", RegexOptions.IgnoreCase);
        private string ExtractUpn(string upn)
        {
            if (string.IsNullOrEmpty(upn)) return string.Empty;

            var match = emailMatcher.Match(upn);
            if (match.Success)
            {
                return match.Groups["Email"].Value.Replace('_', '@');
            }
            return upn;
        }
    }
}
