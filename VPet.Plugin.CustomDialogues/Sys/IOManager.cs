using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LinePutScript;
using LinePutScript.Converter;
using VPet.Plugin.CustomDialogues.Utilities;

namespace VPet.Plugin.CustomDialogues.Sys
{
    public class IOManager
    {

        public IOManager() { }

        private static string setDefaultPath(string path)
        {
            if (path == null) return FolderPath.Get();
            return path;
        }

        public static List<T> LoadLPS<T>(string path = null, string name = null, bool encrypted = false, bool erase = false) where T : new()
        {
            path = setDefaultPath(path);
            EnsureDirectoryExists(path);
            List<T> lines = new List<T>();

            foreach (FileInfo fi in new DirectoryInfo(path).EnumerateFiles("*.lps"))
            {
                if (name != null && fi.Name != name + ".lps") continue;

                try
                {
                    string fileContent = File.ReadAllText(fi.FullName, Encoding.UTF8);

                    foreach (ILine li in new LpsDocument(fileContent))
                    {
                        if (li != null) lines.Add(LPSConvert.DeserializeObject<T>(li));
                    }
                }
                catch (IOException)
                {

                }
            }

            if (erase && name == null) EraseOldestFile(path, DateTime.Now.Date.ToString("yyyy-MM-dd"));
            else if (erase) EraseOldestFile(path, name);

            return lines;
        }

        private static void EraseOldestFile(string path, string name)
        {
            try
            {
                var files = new DirectoryInfo(path).EnumerateFiles("*.lps").ToList();
                int maxFiles = path.Contains("short_term") ? 1 : 14;

                if (files.Count <= maxFiles) return;

                FileInfo oldestFile = files.OrderBy(f => f.CreationTime).FirstOrDefault();
                if (oldestFile == null) return;

                RemoveMatchingLines(files, oldestFile);
                oldestFile.Delete();
            }
            catch (IOException)
            {

            }
        }

        private static void RemoveMatchingLines(List<FileInfo> files, FileInfo oldestFile)
        {
            try
            {
                string[] oldestFileLines = File.ReadAllLines(oldestFile.FullName, Encoding.UTF8);
                if (oldestFileLines.Length == 0) return;

                foreach (var file in files.Where(f => f != oldestFile))
                {
                    var currentLines = File.ReadAllLines(file.FullName, Encoding.UTF8).ToList();
                    currentLines.RemoveAll(line => oldestFileLines.Contains(line));
                    File.WriteAllLines(file.FullName, currentLines, Encoding.UTF8);
                }
            }
            catch (IOException)
            {

            }
        }


        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static void SaveLPS(object obj, string path = null, string name = null, bool encrypt = false, bool append = false)
        {
            try
            {
                path = setDefaultPath(path);
                EnsureDirectoryExists(path);
                name ??= DateTime.Now.Date.ToString("yyyy-MM-dd");
                string filePath = Path.Combine(path, name + ".lps");

                if (obj == null) throw new ArgumentNullException(nameof(obj), "Cannot save a null object.");

                List<string> lines = new List<string>();

                if (obj is IEnumerable<object> list)
                {
                    foreach (var item in list)
                    {
                        string line = FormatLPS(item, name, encrypt);
                        lines.Add(line);
                    }
                }
                else
                {
                    lines.Add(FormatLPS(obj, name, encrypt));
                }

                if (append || encrypt) File.AppendAllLines(filePath, lines, Encoding.UTF8);
                else File.WriteAllLines(filePath, lines, Encoding.UTF8);
            }
            catch (IOException)
            {

            }
        }


        private static string FormatLPS(object obj, string name, bool encrypt)
        {
            string line = $"{name}:|{LPSConvert.SerializeObject(obj).ToString().Replace("\r", "").Replace("\n", "").Trim()}";
            return line;
        }
    }
}
