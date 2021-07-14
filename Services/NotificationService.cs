using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tambola.Services
{
    public class NotificationService
    {
        public event Func<Task> Notify;
        public async Task Update()
        {
            if(Notify != null)
            {
                await Notify.Invoke();
            }
        }
    }
}
