using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RecruitmentTest.SampleGenerator.SingleLoad
{
    /// <summary>
    /// Represents a xml file loader class to load and parse xml sample data
    /// </summary>
    internal class XmlFileSampleLoader : SampleLoader
    {
        private const string XmlElementSample = "sample";
        private const string XmlAttributeId = "Id";
        private const string XmlAttributeValue = "Value";
        private const string XmlAttributeMeasure = "Measure";
        private const string XmlAttributeRefdate = "Refdate";
        private const string Delim = "#";

        private readonly string _resourceName;

        public XmlFileSampleLoader(string fileName) : base(fileName)
        {
            LoaderType = Properties.Settings.Default.XmlFileType;
            _resourceName = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".", DataSourceFolderName, ".", fileName);
        }

        protected override Task<IList<string>> GetSampleDataAsync()
        {
            IList<string> sampleRows = new List<string>();
            StringBuilder builder = new StringBuilder();
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(_resourceName))
            using (var reader = XmlReader.Create(stream))
            {
                reader.ReadToFollowing(XmlElementSample);
                do
                {
                    builder.Clear();
                    if (reader.MoveToAttribute(XmlAttributeId) || reader.MoveToAttribute(XmlAttributeId.ToLower()))
                        builder.Append(reader.Value).Append(Delim);
                    if (reader.MoveToAttribute(XmlAttributeValue) || reader.MoveToAttribute(XmlAttributeValue.ToLower()))
                        builder.Append(reader.Value).Append(Delim);
                    if (reader.MoveToAttribute(XmlAttributeMeasure) || reader.MoveToAttribute(XmlAttributeMeasure.ToLower()))
                        builder.Append(reader.Value).Append(Delim);
                    if (reader.MoveToAttribute(XmlAttributeRefdate) || reader.MoveToAttribute(XmlAttributeRefdate.ToLower()))
                        builder.Append(reader.Value);

                    sampleRows.Add(builder.ToString());
                } while (reader.ReadToFollowing(XmlElementSample));
            }

            return Task.FromResult(sampleRows);
        }

        protected override Task<string[]> ParseSampleDataAsync(string sampleData)
        {
            var data = sampleData.Split('#');
            return Task.FromResult(data);
        }
    }
}