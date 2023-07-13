using Headtrip.Models.Abstract;
using Headtrip.SQLUtilities.SQLAnnotationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Models.User
{
    [SqlTable("Users", "LS")]
    public class User : DatabaseObject
    {
        [SqlColumn(type ="UniqueIdentifier",primaryKey =true,unique =true,index ="userid")]
        public Guid UserId { get; set; }

        [SqlColumn(type ="nvarchar(max)",unique =true,index ="username")]
        public string? Username { get; set; }
        
        [SqlColumn(type ="nvarchar(max)",unique =true)]
        public string? PasswordHash { get; set; }

        [SqlColumn(type ="nvarchar(max)",unique =true,index ="email")]
        public string? Email { get; set; }

        [SqlColumn(type ="nvarchar(max)", nullable =true)]
        public string? PhoneNumber { get; set; }

        [SqlColumn(type ="bit",defaultValue =false)]
        public bool IsEmailConfirmed { get; set; }

        [SqlColumn(type ="bit",defaultValue =false)]
        public bool IsPhoneConfirmed { get; set; }

        [SqlColumn(type ="bit", defaultValue =false)]
        public bool Is2FAEnabled { get; set; }
    }
}
