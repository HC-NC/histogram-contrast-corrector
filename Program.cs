using MaxRev.Gdal.Core;
using System.Globalization;

namespace Histogram_Contrast_Corrector
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            GdalBase.ConfigureAll();

            if (args.Length == 1 && args[0] == "lang-en")
                CultureInfo.CurrentUICulture = new CultureInfo("en-EN");
            else if (args.Length == 1 && args[0] == "lang-ru")
                CultureInfo.CurrentUICulture = new CultureInfo("ru-RU");

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new WorkSpace());
        }
    }
}