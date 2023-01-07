using RecruitmentTest.Common;
using RecruitmentTest.Models;
using RecruitmentTest.SampleGenerator.SingleLoad;
using RecruitmentTest.Services;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
namespace RecruitmentTest.ViewModels
{
    /*
     * Propose solution:
    There can be two use cases:
    UseCase 1: The user may want to load only a single data from a provided data source on the load samples button click.
    UseCase 2: The user may want to load multiple data from provided different sources at a time on the load samples button click.

    A Microsoft Ioc Unity Container was used for instance creation and management.

    Both the approaches can be easily configured via the Settings editor provided value for load type Single or Multiple.
    If the User opts to load Single data source he may choose a specific data source he may be interested either XML, CSV or Pip configured in the settings editor.
    For demonstration purposes for both approaches, the factory configured in the constructor of SampleViewModel will create either instance of SingleDeliveryService
    or MultipleSampleDeliveryService instance as well as interested file type DataSource in case of single load use case.

    For Use Case Case 1: Single Data source load
    Create an abstraction for LoadAsync data samples
    A base abstract class define the common behaviour LoadAsync method for LoadSamples asynchronously and two abstract methods 
    "GetSampleDataAsync", "ParseSampleDataAsync" define for getting and parse SampleData from different sources based on concrete subclasses. 
    This can be easily extended by adding a new concrete implementation based on a newly added data source that must override the above behaviour.
 
    Currently, there are three concrete implementations CsvFileSampleLoader, PipeFileSampleLoader and XmlFileSampleLoader based on different 
    data source file types and can be extended if a new data source file is needed in future.

    For Use Case Case 2: Multiload Data source
    Two Separate Contracts interface define for load and parse the data called "ILoader" and "IParser" can be implemented via different concrete implementation
    for load and parse based on DataSource type ie: for binary data source BinaryLoader and PipeParser can be implemented for load and parse sample data.
 
    Look for the DataSource folder for each sample data file type loads the appropriate Parser from FactoryLookup via CreateParser method. Unity Container manage the dependency of ILoader instance
    and finally, ParseSampleAsync method parse the data provided by the appropriate loader.

    This solution can be easily extendable via implementing the contracts IParser and ILoader for the newly added data source and Register the loader and parser in Unity Container.
     */
    /// <summary>
    /// SamplesViewModel
    /// </summary>
    public class SamplesViewModel : INotifyPropertyChanged
    {
        private readonly ISampleDeliveryService _devliveryService;
        
        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Sample> Samples { get; private set; }

        public ICommand ReadSamples { get; private set; }

        /// <summary>
        /// Constructor:SampleViewModel 
        /// For demonstration single and multi data source load and DataSource Type for single load,
        /// currently Factory lookup configured in constructor
        /// </summary>
        public SamplesViewModel()
        {
            ReadSamples = new DelegateCommand(Execute);
            var sampleLoader = FactoryLookup<ISampleLoader>.CreateSampleLoader(Properties.Settings.Default.CsvFileType);
            _devliveryService = FactoryLookup<ISampleDeliveryService>.CreateDeliveryService(sampleLoader, Properties.Settings.Default.MultiLoad);
        }

        private async void Execute(object obj)
        {
            Samples = await _devliveryService.ProvideSamples();
            RaisePropertyChange("Samples");
        }

        private void RaisePropertyChange(string propertyName)
        {
            var handlers = PropertyChanged;
            if (handlers != null)
            {
                handlers(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
