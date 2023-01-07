using System.Collections.Generic;
using System.Threading.Tasks;
using RecruitmentTest.Models;

namespace RecruitmentTest.SampleGenerator.SingleLoad
{
    /// <summary>
    /// Represents a Contract to load samples asynchronously
    /// </summary>
    public interface ISampleLoader
    {
        Task<IEnumerable<Sample>> LoadAsync();
    }
}