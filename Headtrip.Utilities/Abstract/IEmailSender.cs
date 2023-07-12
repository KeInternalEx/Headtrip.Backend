using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Utilities.Abstract
{
    public class EmailObject
    {

    }


    public interface IEmailSender
    {
        Task SendEmail(EmailObject email);


    }
}
