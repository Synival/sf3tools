using DFRLib;

namespace DFRTool
{
    internal class Program
    {
        static int Main(string[] args)
        {
            // Require two arguments.
            if (args.Length < 2)
            {
                Console.WriteLine("usage: dfrtool <originalFile> <alteredFile>");
                return 1;
            }

            // Create the diff. Log and errors.
            ByteDiff diff;
            try
            {
                diff = new ByteDiff(args[0], args[1]);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return 1;
            }

            // We did it! Write the DFR file.
            Console.Write(diff.ToDFR());
            return 0;
        }
    }
}
