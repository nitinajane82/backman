using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTest.SampleGenerator.SingleLoad
{
    /// <summary>
    /// Represents a csv file loader class to load and parse csv sample data
    /// </summary>
    internal class CsvFileSampleLoader : SampleLoader
    {
        public CsvFileSampleLoader(string fileName) : base(fileName)
        {
            LoaderType = Properties.Settings.Default.CsvFileType;
        }

        protected override async Task<IList<string>> GetSampleDataAsync()
        {
            byte[] result;
            using (var sourceStream = File.Open(FilePath, FileMode.Open))
            {
                result = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
            }

            var sampleDataString = Encoding.ASCII.GetString(result);
            var samples = sampleDataString.Trim().Split('\n');
            var sampleRows = new List<string>(samples);

            return sampleRows;
        }

        protected override Task<string[]> ParseSampleDataAsync(string sampleData)
        {
            var data = sampleData.Split(',');
            return Task.FromResult(data);
        }
    }
}