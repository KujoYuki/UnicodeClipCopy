
namespace Unicode
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
			Application.ThreadException += Application_ThreadException;
		}

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
			MessageBox.Show("ó·äOÇ™î≠ê∂ÇµÇ‹ÇµÇΩÅB\n" + e.Exception.Message);
		}
    }
}