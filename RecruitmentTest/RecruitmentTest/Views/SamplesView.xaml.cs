using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using RecruitmentTest.Models;

namespace RecruitmentTest.Views
{
	/// <summary>
	/// Interaction logic for SamplesView.xaml
	/// </summary>
	public partial class SamplesView : UserControl
	{

		public static readonly DependencyProperty SamplesProperty =
			DependencyProperty.Register("Samples", typeof (IEnumerable<Sample>), typeof (SamplesView), new PropertyMetadata(default(IEnumerable<Sample>)));

		public IEnumerable<Sample> Samples
		{
			get { return (IEnumerable<Sample>) GetValue(SamplesProperty); }
			set { SetValue(SamplesProperty, value); }
		}

		public SamplesView()
		{
			InitializeComponent();
		}
	}
}
