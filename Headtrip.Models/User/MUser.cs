using Headtrip.Objects.Abstract.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Objects.User
{
    public sealed class MUser : ADatabaseObject, IDatabaseObjectDelayedDeletable, IDatabaseObjectTimeStamped 
    {
        public Guid UserId { get; set; }

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
