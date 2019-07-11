using Cockpit.Core.Persistence;
using Cockpit.Core.Persistence.Paths;
using Cockpit.Core.Services;
using Ninject;

namespace Cockpit.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var kernel = ServiceBootstrapper.Create();
            kernel.Get<ConsoleHost>().Start(args);
        }
    }
}
