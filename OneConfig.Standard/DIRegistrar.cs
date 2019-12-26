using OneConfig.Services.ConfigurationReaderFactories;
using OneConfig.Services.Interfaces;
using StructureMap;
using System.Linq;

namespace OneConfig
{
    class DIRegistrar
    {
        private static Container container;

        private static Container GetContainer()
        {
            return container ?? (container = new Container(c =>
            {
                c.Scan(s =>
                {
                    s.TheCallingAssembly();
                    s.AddAllTypesOf<IConfigurationReaderFactory>();
                    s.AddAllTypesOf<IConfigurationReader>();
                });


            }));
        }

        public static T[] GetInstances<T>()
        {
            return GetContainer().GetAllInstances<T>().ToArray();
        }
    }
}
