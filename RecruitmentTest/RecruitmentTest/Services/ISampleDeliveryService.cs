using RecruitmentTest.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentTest.Services
{
    public interface ISampleDeliveryService
    {
        Task<IEnumerable<Sample>> ProvideSamples();
    }
}
