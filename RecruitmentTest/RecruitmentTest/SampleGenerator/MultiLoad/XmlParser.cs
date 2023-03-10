using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RecruitmentTest.Models;

namespace RecruitmentTest.SampleGenerator.MultiLoad
{
    /// <summary>
    /// Represents a xml parser to parse sample data from xml file asynchronously
    /// </summary>
    public class XmlParser : IParser
    {
        private readonly ILoader _loader;
        public XmlParser(ILoader loader)
        {
            _loader = loader;
        }
        public async Task<IEnumerable<Sample>> ParseSamplesAsync()
        {
            var sampleList = new List<Sample>();
            var csvData = _loader.LoadSamplesAsync();
            foreach (var sampleData in await csvData)
            {
                var data = sampleData.Split('#');
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