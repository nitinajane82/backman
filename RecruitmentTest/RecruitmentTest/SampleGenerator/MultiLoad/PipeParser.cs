using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecruitmentTest.Models;

namespace RecruitmentTest.SampleGenerator.MultiLoad
{
    /// <summary>
    /// Represents a binary file loader to load sample data from pipe file asynchronously
    /// </summary>
    public class PipeParser : IParser
    {
        private readonly ILoader _loader;
        public PipeParser(ILoader loader)
        {
            _loader = loader;
        }
        public async Task<IEnumerable<Sample>> ParseSamplesAsync()
        {
            var sampleList = new List<Sample>();
            var csvData = _loader.LoadSamplesAsync();
            foreach (var sampleData in await csvData)
            {
                var data = sampleData.Split('|');
                sampleList.Add(new Sample
                {
                    Id = data[0],
                    Value = double.Parse(data[1]),
                    Measure = data[2],
                    Refdate = DateTime.Parse(data[3])
                });

            }
            return sampleList;
        }
    }
}