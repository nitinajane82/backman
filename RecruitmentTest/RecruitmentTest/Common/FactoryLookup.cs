using RecruitmentTest.SampleGenerator.MultiLoad;
using RecruitmentTest.SampleGenerator.SingleLoad;
using RecruitmentTest.Services;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace RecruitmentTest.Common
{
    /// <summary>
    /// Unity Container Registry for ISampleLoader,ISampleDeliveryService, ILoader and IParser registration and instance creation
    /// </summary>
    public static class FactoryLookup<T>
    {
        private const string CsvFile = "Samples.csv";
        private const string PipeFile = "Samples.pip";
        private const string XmlFile = "Samples.xml";
        private static IUnityContainer _unityContainer;

        private static readonly IDictionary<string, string> FileTypeToExtensionMappings = new Dictionary<string, string>
        {
            { "csv", Properties.Settings.Default.CsvFileType },
            { "pip", Properties.Settings.Default.PipeFileType },
            { "xml", Properties.Settings.Default.XmlFileType }
        };

        /// <summary>
        /// Register and create a ISampleDeliveryService instance based on sample data load type single or multiple
        /// </summary>
        /// <param name="loader">ISampleLoader</param>
        /// <param name="loadType">LoadType</param>
        /// <returns></returns>
        public static T CreateDeliveryService(ISampleLoader loader, string loadType)
        {
            if (_unityContainer == null)
            {
                _unityContainer = new UnityContainer();

                //Register a Factory
                if(loadType == Properties.Settings.Default.SingleLoad)
                    _unityContainer.RegisterType<ISampleDeliveryService, SamplesDeliveryService>(Properties.Settings.Default.SingleLoad, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(loader));

                if (loadType == Properties.Settings.Default.MultiLoad)
                    _unityContainer.RegisterType<ISampleDeliveryService, MultiSamplesDeliveryService >(Properties.Settings.Default.MultiLoad, new ContainerControlledLifetimeManager());
            }

            return _unityContainer.Resolve<T>(loadType);
        }

        /// <summary>
        /// Register and Create a ISampleLoader instance implements ISampleLoader interface based on file type
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T CreateSampleLoader(string fileName)
        {
            if (_unityContainer == null)
            {
                _unityContainer = new UnityContainer();

                //Register a Factory
                _unityContainer.RegisterType<ISampleLoader, CsvFileSampleLoader>(Properties.Settings.Default.CsvFileType, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(CsvFile));
                _unityContainer.RegisterType<ISampleLoader, PipeFileSampleLoader>(Properties.Settings.Default.PipeFileType, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(PipeFile));
                _unityContainer.RegisterType<ISampleLoader, XmlFileSampleLoader>(Properties.Settings.Default.XmlFileType, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(XmlFile));

            }

            return _unityContainer.Resolve<T>(fileName);
        }

        /// <summary>
        /// Register and Create a IParser instance implements IParser interface based on file extensions
        /// </summary>
        /// <param name="fileInfo">FileInfo</param>
        /// <returns>IParser</returns>
        public static T CreateParser(FileInfo fileInfo)
        {
            _unityContainer = _unityContainer ?? new UnityContainer();

            var fileExtension = fileInfo.Extension.Remove(0, 1).ToLower(CultureInfo.InvariantCulture);

            if (FileTypeToExtensionMappings[fileExtension] == Properties.Settings.Default.CsvFileType)
            {
                _unityContainer.RegisterType<ILoader, FileLoader>(Properties.Settings.Default.CsvFileType, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(fileInfo.FullName));
                _unityContainer.RegisterType<IParser, CsvParser>(Properties.Settings.Default.CsvFileType, new ContainerControlledLifetimeManager(),
                   new InjectionConstructor(_unityContainer.Resolve<ILoader>(Properties.Settings.Default.CsvFileType)));
            }

            if (FileTypeToExtensionMappings[fileExtension] == Properties.Settings.Default.PipeFileType)
            {
                _unityContainer.RegisterType<ILoader, BinaryLoader>(Properties.Settings.Default.PipeFileType, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(fileInfo.FullName));
                _unityContainer.RegisterType<IParser, PipeParser>(Properties.Settings.Default.PipeFileType, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(_unityContainer.Resolve<ILoader>(Properties.Settings.Default.PipeFileType)));
            }

            if (FileTypeToExtensionMappings[fileExtension] == Properties.Settings.Default.XmlFileType)
            {
                _unityContainer.RegisterType<ILoader, XmlLoader>(Properties.Settings.Default.XmlFileType, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(fileInfo));
                _unityContainer.RegisterType<IParser, XmlParser>(Properties.Settings.Default.XmlFileType, new ContainerControlledLifetimeManager(),
                    new InjectionConstructor(_unityContainer.Resolve<ILoader>(Properties.Settings.Default.XmlFileType)));
            }

            return _unityContainer.Resolve<T>(FileTypeToExtensionMappings[fileExtension]);
        }
    }
}
