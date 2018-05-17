using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace FolderNames
{
    internal class Program
    {
        public static IEnumerable<string> FolderNames(string xml, char startingLetter)
        {
            var folderNames = new List<string>();

            using (var reader = new StringReader(xml))
            {
                XDocument xdoc = XDocument.Load(reader);
                XElement element = xdoc.Element("folder");
                CollectFolderNames(element, folderNames);
            }

            return folderNames.Where(x => x != null)
                              .Where(x => x.Length > 0)
                              .Where(x => x[0] == (startingLetter));
        }

        private static void CollectFolderNames(XElement element, ICollection<string> folderNames)
        {
            folderNames.Add(element.Attribute("name").Value);
            foreach (XElement childElement in element.Elements("folder"))
            {
                CollectFolderNames(childElement, folderNames);
            }
        }

        public static void Main(string[] args)
        {
            string xml =
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
                    "<folder name=\"c\">" +
                    "<folder name=\"program files\">" +
                    "<folder name=\"uninstall information\" />" +
                    "</folder>" +
                    "<folder name=\"users\" />" +
                    "</folder>";

            foreach (string name in FolderNames(xml, 'u'))
            {
                Console.WriteLine(name);
            }

            Console.ReadLine();
        }
    }
}