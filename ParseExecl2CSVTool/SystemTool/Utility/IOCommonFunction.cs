using System.IO;

namespace Utility
{
    public class IOCommonFunction
    {
        public static bool FileInUse(string path)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.OpenOrCreate);
                if (fs.CanRead || fs.CanWrite)
                {
                    fs.Close();
                    return false;
                }
                fs.Close();
                return true;
            }
            catch (IOException)
            {
                return true;
            }
        }
    }
}