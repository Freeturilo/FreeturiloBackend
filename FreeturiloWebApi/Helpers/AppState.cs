using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Helpers
{
    public enum AppStateEnum
    {
        Started = 0,
        Stopped = 1,
        Demo = 2
    }
    public static class AppState
    {
        public static AppStateEnum State { get; private set; } = AppStateEnum.Started;

        public static void Start()
        {
            State = AppStateEnum.Started;
        }
        public static void Stop()
        {
            State = AppStateEnum.Stopped;
        }
        public static void Demo()
        {
            State = AppStateEnum.Demo;
        }
    }
}
