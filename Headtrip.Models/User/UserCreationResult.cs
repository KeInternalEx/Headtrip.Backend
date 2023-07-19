using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Headtrip.Models.Abstract;

namespace Headtrip.Models.User
{
    public class UserCreationResult : AServiceCallResult
    {
        public string? EmailConfirmationParameter { get; set; }
        public User? User { get; set; }
    }
}
