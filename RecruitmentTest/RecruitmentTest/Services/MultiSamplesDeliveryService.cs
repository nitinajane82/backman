using RecruitmentTest.Common;
using RecruitmentTest.Models;
using RecruitmentTest.SampleGenerator.MultiLoad;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RecruitmentTest.Services
{
    /// <summary>
    /// MultiSamplesDeliveryService, Returns Multi DataSource samples
    /// Look for the DataSource folder for each sample data file type. Loads the appropriate loader via FactoryLookup via CreateLoader and Creates a Parser
    /// via CreateParser method. Finally ParseSampleAsync method, parse the data provided by the appropriate loader.
    /// </summary>
    public class MultiSamplesDeliveryService : ISampleDeliveryService
    {
        private readonly string _filePath;
        private const string DataSourceFolderName = "DataSources";
        public MultiSamplesDeliveryService()
        {
            _filePath = string.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, DataSourceFolderName);
        }

        public async Task<IEnumerable<Sample>> ProvideSamples()
        {
            var samples = new List<Sample>();

            FileInfo[] fileInfos = new DirectoryInfo(_filePath).GetFiles();
            foreach (var fileInfo in fileInfos)
            {
                //ILoader loader = FactoryLookup<ILoader>.CreateLoader(fileInfo);
                //IParser parser = FactoryLookup<IParser>.CreateParser(loader, fileInfo);
                IParser parser = FactoryLookup<IParser>.CreateParser(fileInfo);

                var parsedSamples = await parser.ParseSamplesAsync();

                samples.AddRange(parsedSamples);
            }

            return samples;
        }
    }
}