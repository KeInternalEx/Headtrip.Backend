using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.Account
{
    public sealed class MAccount : ADatabaseObject, IDatabaseObjectTimeStamped, IDatabaseObjectDelayedDeletable
    {
        public Guid AccountId { get; set; }

        public Guid? CurrentCharacterId { get; set; }
        public Guid? CurrentChannelId { get; set; }
        public Guid? CurrentPartyId { get; set; }

        public DateTime LastLoginTime { get; set; }
        public long TotalPlayTimeMs { get; set; }

        public bool IsLocked { get; set; }
        public bool IsSuspended { get; set; }


        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsPhoneConfirmed { get; set; }
        public bool Is2FAEnabled { get; set; }



        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }

        public bool IsPendingDeletion { get; set; }




    }
}
