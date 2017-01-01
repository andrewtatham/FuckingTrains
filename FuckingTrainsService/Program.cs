using System.ServiceProcess;

namespace FuckingTrainsService
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            ServiceBase.Run(new ServiceBase[]
            {
                new FuckingTrainsService()
            });
        }
    }
}