using System.Collections.Generic;

namespace SendArchives.Models
{
    public class CredentialsSendingInfo
    {
        public Dictionary<string, string> IsSendingSuccessfulByLogin { get; } = new Dictionary<string, string>();
    }
}
