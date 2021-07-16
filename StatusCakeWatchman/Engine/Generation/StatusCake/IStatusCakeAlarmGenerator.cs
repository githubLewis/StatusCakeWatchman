using System.Threading.Tasks;
using StatusCakeWatchman.Configuration;

namespace StatusCakeWatchman.Engine.Generation.StatusCake
{
    public interface IAlarmGenerator
    {
        Task GenerateAlarmsFor(StatusCakeWatchmanConfiguration config, RunMode mode);
    }
}