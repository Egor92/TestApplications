using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace DirectoryPaths
{
    public class Path
    {
        public string CurrentPath { get; private set; }

        public Path(string path)
        {
            CurrentPath = path;
        }

        public void Cd(string newPath)
        {
            if (string.IsNullOrWhiteSpace(newPath))
                return;

            var isPathCorrect = Regex.IsMatch(newPath, @"(\.\.)?(\/[(A-Za-z)(\.\.)]+)*");
            if (!isPathCorrect)
                return;

            bool isAbsolutePath = newPath[0] == '/';
            if (isAbsolutePath)
            {
                CurrentPath = newPath;
                return;
            }

            var subPaths = newPath.Split(new[] { '/' }, StringSplitOptions.None);
            var currentSubpaths = CurrentPath.Split(new[] { '/' }, StringSplitOptions.None).ToList();

            foreach (var subPath in subPaths)
            {
                if (subPath == "..")
                {
                    if (currentSubpaths.Count == 0)
                        continue;

                    var lastItemIndex = currentSubpaths.Count - 1;
                    currentSubpaths.RemoveAt(lastItemIndex);
                }
                else
                {
                    currentSubpaths.Add(subPath);
                }
            }

            CurrentPath = currentSubpaths.Aggregate((s1, s2) => string.Format(@"{0}/{1}", s1, s2));
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            {
                Path path = new Path("/a");
                path.Cd("");
                Console.WriteLine(path.CurrentPath == "/a");
            }

            {
                Path path = new Path("/a/b");
                path.Cd("..");
                Console.WriteLine(path.CurrentPath == "/a");
            }

            {
                Path path = new Path("/a/b/c");
                path.Cd("..");
                Console.WriteLine(path.CurrentPath == "/a/b");
            }

            {
                Path path = new Path("/a/b/c");
                path.Cd("../..");
                Console.WriteLine(path.CurrentPath == "/a");
            }

            {
                Path path = new Path("/a/b/c/d");
                path.Cd("../x");
                Console.WriteLine(path.CurrentPath == "/a/b/c/x");
            }

            {
                Path path = new Path("/a/b/c/d");
                path.Cd("/x");
                Console.WriteLine(path.CurrentPath == "/x");
            }

            {
                Path path = new Path("/a/b/c/d");
                path.Cd("/x/y");
                Console.WriteLine(path.CurrentPath == "/x/y");
            }

            {
                Path path = new Path("/a/b/c/d");
                path.Cd("&");
                Console.WriteLine(path.CurrentPath == "/a/b/c/d");
            }

            {
                Path path = new Path("/a/b/c/d");
                path.Cd("//");
                Console.WriteLine(path.CurrentPath == "/a/b/c/d");
            }

            {
                Path path = new Path("/a/b/c/d");
                path.Cd("...");
                Console.WriteLine(path.CurrentPath == "/a/b/c/d");
            }

            Console.ReadLine();
        }
    }
}