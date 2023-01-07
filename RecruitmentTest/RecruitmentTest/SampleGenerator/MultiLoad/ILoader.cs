using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecruitmentTest.SampleGenerator.MultiLoad
{
    /// <summary>
    /// Represents a Contract ILoader to load SampleData Asynchronously
    /// </summary>
    public interface ILoader
    {
        Task<IList<string>> LoadSamplesAsync();
    }
}