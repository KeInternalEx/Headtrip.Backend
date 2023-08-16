using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headtrip.UnrealService.Threading
{
    public sealed class TsObject<TObject>
    {
        private object _LockObject = new object();
        private TObject? _Value;

        public TObject? Value
        {
            get
            {
                lock (_LockObject)
                {
                    return _Value;
                }
            }
            set
            {
                lock (_LockObject)
                {
                    _Value = Value;
                }
            }
        }

        public TsObject(TObject? initialValue)
            => _Value = initialValue;

        public void Update(Action<TObject?> action)
        {
            lock (_LockObject)
            {
                action(_Value);
            }
        }
    }
}
