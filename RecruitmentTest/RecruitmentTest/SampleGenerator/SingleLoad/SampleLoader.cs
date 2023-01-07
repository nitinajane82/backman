using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using RecruitmentTest.Models;

namespace RecruitmentTest.SampleGenerator.SingleLoad
{
    /// <summary>
    /// A Template method pattern used to perform a single data load scenario can be easily
    /// extendable by creating a new subclass and overriding the behaviour for loading and parsing data
    /// Represents an Abstract base class SampleLoader for common behaviour LoadAsync
    /// and lets subclasses to define specific behaviour for load and parse.
    /// </summary>
    public abstract class SampleLoader : ISampleLoader
    {
        protected string LoaderType = string.Empty;
        private readonly string _fileName;
        protected const string DataSourceFolderName = "DataSources";

        protected SampleLoader(string fileName)
        {
            _fileName = fileName;
        }

        /// <summary>
        /// Get the Sample data asynchronously row by row from a different source based on the data source type
        /// An unparsed single data sample represent by each row of string 
        /// </summary>
        /// <returns>string[] </returns>
        protected abstract Task<IList<string>> GetSampleDataAsync();

        /// <summary>
        /// Parse the Sample data asynchronously from a different source based on the data source type
        /// </summary>
        /// <param name="sampleData"></param>
        /// <returns></returns>
        protected abstract Task<string[]> ParseSampleDataAsync(string sampleData);

        public async Task<IEnumerable<Sample>> LoadAsync()
        {
            var samples = new List<Sample>();
            foreach (var sampleData in await GetSampleDataAsync())
            {
                var data = await ParseSampleDataAsync(sampleData);

                samples.Add(new Sample
                {
                    Id = data[0],
                    Value = double.Parse(data[1]),
                    Measure = data[2],
                    Refdate = DateTime.Parse(data[3])
                });
            }

            return samples;
        }

        protected string FilePath => string.Concat(Directory.GetCurrentDirectory(), Path.DirectorySeparatorChar, DataSourceFolderName, Path.DirectorySeparatorChar, _fileName);
    }
}
