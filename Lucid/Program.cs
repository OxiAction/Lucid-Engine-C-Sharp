namespace Lucid
{
    class Program
    {
        /// <summary>
        ///  Entry
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            _ = new Demo();
        }
    }
}