using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace RecruitmentTest.SampleGenerator.MultiLoad
{
    /// <summary>
    /// Represents a binary loader to load sample data from binary file asynchronously
    /// </summary>
    public class BinaryLoader : ILoader
    {
        private readonly string _loaderType;
        private readonly string _fileName;

        public BinaryLoader(string fileName)
        {
            _loaderType = Properties.Settings.Default.PipeFileType;
            _fileName = fileName;
        }

        public async Task<IList<string>> LoadSamplesAsync()
        {
            var sampleRows = new List<string>();
            using (var stream = new StreamReader(File.OpenRead(_fileName)))
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
    }
}