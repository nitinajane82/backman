using System.Collections.Generic;
using System.Threading.Tasks;
using RecruitmentTest.Models;

namespace RecruitmentTest.SampleGenerator.MultiLoad
{
    /// <summary>
    /// Represents a Contract IParser to parse SampleData Asynchronously
    /// </summary>
    public interface IParser
    {
        Task<IEnumerable<Sample>> ParseSamplesAsync();
    }
}