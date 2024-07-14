
using Syncfusion.Licensing;
using System.Configuration;
using System.Data;
using System.Windows;

namespace CountryInfo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {



        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzM3Njc1NUAzMjM2MmUzMDJlMzBjZG9RTFB2VXp2MWhOMGl5VFhtcTR1MGhVQnFSVUVseDZIcHVNZFV5VEV3PQ==");

        }

    }

}
