using Headtrip.Objects.Instance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.Repositories.Abstract
{
    public interface IZoneRepository
    {
        public Task<IEnumerable<Zone>> GetAllZones();


    }
}
