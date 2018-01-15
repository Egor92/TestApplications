using System;
using System.IO;
using Vestris.ResourceLib;

namespace ResourceLibApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string assembly = "app.exe";
            if (!File.Exists(assembly))
            {
                throw new FileNotFoundException("The file " + assembly + " doesn't exist!");
            }

            const string icoFile = "ico.ico";
            if (!File.Exists(icoFile))
            {
                throw new FileNotFoundException("The file " + icoFile + " doesn't exist!");
            }

            try
            {
                IconDirectoryResource newIcon = new IconDirectoryResource(new IconFile(icoFile));
                newIcon.SaveTo(assembly);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("error: {0}", ex.Message);
            }
        }
    }
}