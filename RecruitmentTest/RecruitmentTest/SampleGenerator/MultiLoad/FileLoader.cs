using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RecruitmentTest.SampleGenerator.MultiLoad
{
    /// <summary>
    /// Represents a File loader to load sample data from text or csv file asynchronously
    /// </summary>
    public class FileLoader : ILoader
    {
        private readonly string _fileName;
        private readonly string _loaderType;

        public FileLoader(string fileName)
        {
            _loaderType = Properties.Settings.Default.CsvFileType;
            _fileName = fileName;
        }

        public async Task<IList<string>> LoadSamplesAsync()
        {
            byte[] result;
            using (var sourceStream = File.Open(_fileName, FileMode.Open))
            {
                result = new byte[sourceStream.Length];
                await sourceStream.ReadAsync(result, 0, (int)sourceStream.Length);
            }

            var sampleDataString = Encoding.ASCII.GetString(result);
            var samples = sampleDataString.Trim().Split('\n');
            var sampleRows = new List<string>(samples);

            return sampleRows;
        }
    }
}