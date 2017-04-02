using System;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfCalendar
{
    public enum FileType { date, location }

    public static class WidgetFileManager
    {
        //file locations
        private static string dateFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "Calendar Widget", "data.txt");
        private static string locationFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "Calendar Widget", "savedLocation.txt");

        public static bool IsFileEmpty()
        {
            if (GetDatesFromFile().Length == 0)
            {
                return true;
            }

            return false;
        }

        public static string[] GetDatesFromFile()
        {
            //read file into lines
            return (File.ReadAllLines(dateFilePath));
        }

        public static string[] GetLocationsFromFile()
        {
            //read file into lines
            return (File.ReadAllLines(locationFilePath));
        }

        public static void CreateDirectoryAndFiles()
        {
            //create a directory
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "Calendar Widget"));
            Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "Calendar Widget", "User Images"));

            if (!File.Exists(dateFilePath))
            {
                FileStream f = File.Create(dateFilePath);
                f.Close();
            }
            if (!File.Exists(locationFilePath))
            {
                FileStream f = File.Create(locationFilePath);
                f.Close();
            }
        }

        public static void ClearFileContents()
        {
            using (StreamWriter sw = new StreamWriter(dateFilePath, false))
            {
                sw.Write(string.Empty);
                sw.Close();
            }

            using (StreamWriter sw = new StreamWriter(locationFilePath, false))
            {
                sw.Write(string.Empty);
                sw.Close();
            }
        }

        public static void saveDateToFile(DateTime date)
        {
            WriteToFile(date.ToString(), FileType.date);
        }

        public static void saveLocationToFile(Point screenCoordinates)
        {
            WriteToFile(screenCoordinates.ToString(), FileType.location);
        }

        private static void WriteToFile(string text, FileType fileType)
        {
            switch (fileType)
            {
                case FileType.date:
                    using (StreamWriter sw = new StreamWriter(dateFilePath, true))
                    {
                        sw.WriteLine(text);
                        sw.Close();
                    }
                    break;

                case FileType.location:
                    using (StreamWriter sw = new StreamWriter(locationFilePath, true))
                    {
                        sw.WriteLine(text);
                        sw.Close();
                    }
                    break;
            }
        }

        public static string CopyFileToDocuments(string filename)
        {
            string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDoc‌​uments), "Calendar Widget", "User Images", Path.GetFileName(filename));
            try
            {
                File.Copy(filename, filepath, true);
            } catch (Exception)
            {
                //file is already copied (being used as current background)
            }

            return filepath;
        }

        private static bool isFileLocked(FileInfo file)
        {
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}
