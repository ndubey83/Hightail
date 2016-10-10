using System.IO;
using System.Linq;
using Autofac;
using Autofac.Core;
using log4net;
using log4net.Repository.Hierarchy;

namespace hems.HighTail.Logging {

    /// <summary>
    /// Update, this url has an alternate method of setting up the logging module: https://gist.github.com/maxfridbe/3997274 
    /// </summary>
    public class LogInjectionModule : Module {
        protected override void Load(ContainerBuilder builder) {
            log4net.Config.XmlConfigurator.Configure(new FileInfo("log4net.config"));
        }

        protected override void AttachToComponentRegistration(IComponentRegistry registry, IComponentRegistration registration) {
            registration.Preparing += OnComponentPreparing;
        }

        static void OnComponentPreparing(object sender, PreparingEventArgs e) {
            var t = e.Component.Activator.LimitType;
            e.Parameters = e.Parameters.Union(new[] {
                new ResolvedParameter((p, i) => p.ParameterType == typeof(ILog), (p, i) => LogManager.GetLogger(t))
            });
        }

        public static void SetLevel(string loggerName, string levelName) {
            ILog log = LogManager.GetLogger(loggerName);
            Logger l = (Logger)log.Logger;

            l.Level = l.Hierarchy.LevelMap[levelName];


        }
    }
}
