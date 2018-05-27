using System.Collections.Generic;

namespace SendArchives.Models
{
    public class CredentialsSendingInfo
    {
        public Dictionary<string, bool> IsSendingSuccessfulByLogin { get; } = new Dictionary<string, bool>();
    }
}
