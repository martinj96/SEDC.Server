using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public class FileHandler
    {
        private string _contentPath;

        public FileHandler(string contentPath)
        {
            _contentPath = contentPath;
        }

        internal bool DoesFileExists(string directory)
        {
            return File.Exists(_contentPath + directory);
        }

        internal byte[] ReadFile(string path)
        {
            //return File.ReadAllBytes(path);
            if (ServerCache.Contains(_contentPath + path))
            {
                Console.WriteLine("cache hit");
                return ServerCache.Get(_contentPath + path);
            }
            else
            {
                byte[] content = File.ReadAllBytes(_contentPath + path);
                ServerCache.Insert(_contentPath + path, content);
                return content;
            }

        }
    }
}
