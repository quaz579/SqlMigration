﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using SqlMigration.Contracts;

namespace SqlMigration
{
    public class FileIO : IFileIO
    {
        private static readonly ILog Logger = LogManager.GetLogger(typeof (FileIO));

        public bool Copy(string filePath, string locationToCopyTo)
        {
            bool success = true;
            try
            {
                //have to get the filename out of the filePath
                string fileName = GetFileNameFromFullPath(filePath);
                File.Copy(filePath, locationToCopyTo + "\\" + fileName, true);
                //set it normal, not read-only...
                File.SetAttributes(locationToCopyTo + "\\" + fileName, FileAttributes.Normal);
            }
            catch (Exception)
            {
                success = false;
            }

            return success;
        }

        public bool CreateFolder(string folderLocation)
        {
            bool success = true;
            try
            {
                //delete if it exists first
                if (Directory.Exists(folderLocation))
                    Directory.Delete(folderLocation, true);

                //create it
                Directory.CreateDirectory(folderLocation);
                //mark it worked
            }
            catch (Exception ex)
            {
                Logger.Error(string.Format("Error occured with folder stuff\r\n\r\n{0}", ex.Message));
                success = false;
            }

            return success;
        }

        public string ReadConentsOfFile(string fileLocation)
        {
            StringBuilder sb = new StringBuilder(1024);
            foreach (string line in File.ReadAllLines(fileLocation))
            {
                sb.AppendLine(line);
            }

            return sb.ToString();
        }

        public List<string> ReadDirectoryFilenames(string directoryPath)
        {
            var files = new List<string>();

            //insert files with .sql and .dat extensions
            files.InsertRange(0, Directory.GetFiles(directoryPath, "*.sql"));
            files.InsertRange(0, Directory.GetFiles(directoryPath, "*.dat"));

            return files;
        }

        public void WriteFile(string path, string fileContents)
        {
            File.WriteAllText(path, fileContents);
        }


        public static string GetFileNameFromFullPath(string filePath)
        {
            StringBuilder sb = new StringBuilder(64);
            List<char> fileName = new List<char>();
            for (int i = filePath.Length - 1; i >= 0; --i)
            {
                if (filePath[i] == Char.Parse("\\"))
                    break;
                //since its not a break, lets add the char
                fileName.Add(filePath[i]);
            }

            //reverse it
            fileName.Reverse();
            sb.Append(fileName.ToArray());

            return sb.ToString();
        }

    }
}
