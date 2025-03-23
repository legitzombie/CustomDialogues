using System.IO;
using System.Reflection;

namespace VPet.Plugin.CustomDialogues.Utilities
{
    public class FolderPath
    {
        public static string Get(string folder = null, string sub = null, string sub2 = null, string sub3 = null)
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string[] subs = { folder, sub, sub2, sub3 };

            foreach (string s in subs)
            {
                if (!string.IsNullOrWhiteSpace(s))
                {
                    path = Path.Combine(path, s);
                }
            }

            return path;
        }
    }
}
