using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatusCakeWatchman.Engine.Generation.Generic
{
    public interface IAlarmCreator
    {
        void AddAlarms(AlertingGroupParameters group); //, IList<Alarm> alarms);
        Task SaveChanges(bool dryRun);
    }

    public class AlarmCreator : IAlarmCreator
    {
        public void AddAlarms(AlertingGroupParameters group)
        {
            throw new NotImplementedException();
        }

        public Task SaveChanges(bool dryRun)
        {
            throw new NotImplementedException();
        }
    }
}
