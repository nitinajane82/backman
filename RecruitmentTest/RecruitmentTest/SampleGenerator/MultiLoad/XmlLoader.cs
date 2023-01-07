using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace RecruitmentTest.SampleGenerator.MultiLoad
{
    /// <summary>
    /// Represents a xml loader to load sample data from xml file asynchronously
    /// </summary>
    public class XmlLoader : ILoader
    {
        private readonly string _loaderType;
        private const string XmlElementSample = "sample";
        private const string XmlAttributeId = "Id";
        private const string XmlAttributeValue = "Value";
        private const string XmlAttributeMeasure = "Measure";
        private const string XmlAttributeRefdate = "Refdate";
        private const string Delim = "#";
        private const string DataSourceFolderName = "DataSources";
        private readonly string _resourceName;

        public XmlLoader(FileInfo fileInfo)
        {
            _loaderType = Properties.Settings.Default.XmlFileType;
            _resourceName = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".", DataSourceFolderName, ".", fileInfo.Name);
        }

        public Task<IList<string>> LoadSamplesAsync()
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
    }
}