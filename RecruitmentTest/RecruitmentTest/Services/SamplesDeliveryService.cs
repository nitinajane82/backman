using RecruitmentTest.Models;
using RecruitmentTest.SampleGenerator.SingleLoad;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentTest.Services
{
    /// <summary>
    /// SamplesDeliveryService, Returns samples from single data source
    /// </summary>
    public class SamplesDeliveryService : ISampleDeliveryService
    {
        private readonly ISampleLoader _sampleLoader;

        public SamplesDeliveryService(ISampleLoader sampleLoader)
        {
            _sampleLoader = sampleLoader;
        }

        public async Task<IEnumerable<Sample>> ProvideSamples()
        {
            var samples = await _sampleLoader.LoadAsync();
            return samples;
        }
    }
}
