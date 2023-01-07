using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RecruitmentTest.SampleGenerator.SingleLoad
{
    /// <summary>
    /// Represents a class PipeFileSampleLoader for binary file loader to load and parse binary sample data
    /// </summary>
    internal class PipeFileSampleLoader : SampleLoader
    {
        public PipeFileSampleLoader(string fileName) : base(fileName)
        {
            LoaderType = Properties.Settings.Default.PipeFileType;
        }

        protected override async Task<IList<string>> GetSampleDataAsync()
        {
            var sampleRows = new List<string>();
            using (var stream = new StreamReader(File.OpenRead(FilePath)))
            {
                string line;
                while ((line = await stream.ReadLineAsync()) != null)
                {
                    var trimmedLine = line.Trim();
                    sampleRows.Add(trimmedLine);
                }
            }
            return sampleRows;
        }

        protected override Task<string[]> ParseSampleDataAsync(string sampleData)
        {
            var data = sampleData.Split('|');
            return Task.FromResult(data);
        }
    }
}