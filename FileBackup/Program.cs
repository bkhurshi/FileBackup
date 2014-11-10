using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace FileBackup
{
    class Program
    {
        public const string rootSourcePath = "C:\\Users";
        private const string lastBackupFileName = ".lastBackup";
        private static DateTime defaultBackupDate = new DateTime(1000, 1, 1);
        public static string rootDestinationPath;
        public static DateTime lastBackup;
        private static FileNode rootSourceDirectory;

        public static int fileCount = 0;
        public static volatile int backedupfileCount = 0;
        private static FileInfo lastBackupFile;
        /**
         * To use this program:
         * 1. Copy into Hard Disk Drive under Backup_Program folder
         * 2. Log into your computer as Admin
         * 
         * Features:
         * 1. Only copy files after a certain date
         * 2. Progress Bar
         */
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("**Welcome to Beenish's Awsomely Awsome Back-up Program**");
                Console.WriteLine();
                Console.WriteLine("To use this program, you must be logged on as administrator");
                Console.WriteLine("To Continue, press the Enter Key");
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.WriteLine("\nHave a nice day!");
                    Thread.Sleep(500);
                    return;
                };

                rootDestinationPath = "..\\" + Environment.MachineName;
                DirectoryInfo mainBackupDirectory = Directory.CreateDirectory(rootDestinationPath);

                // Get the file structure
                Console.WriteLine("Getting initial information.");
                rootSourceDirectory = new FileNode(rootSourcePath);

                Console.WriteLine("Total files to back-up: " + fileCount);

                // Get the time of last back-up
                String lastBackupFilePath = Path.Combine(mainBackupDirectory.FullName, lastBackupFileName);
                lastBackupFile = new FileInfo(lastBackupFilePath);
                if (!lastBackupFile.Exists)
                {
                    lastBackupFile.Create();
                    lastBackup = defaultBackupDate;
                }
                else
                {
                    lastBackup = lastBackupFile.LastWriteTimeUtc;
                }

                Console.WriteLine("Last Back-up: " + lastBackup.ToLocalTime().ToString());

                // Copy the files
                Console.WriteLine("Copying...");
                Thread copyThread = new Thread(new ThreadStart(copyFiles));
                copyThread.Start();

                // progress bar
                Thread progressThread = new Thread(new ThreadStart(progress));
                progressThread.Start();

                copyThread.Join();
                progressThread.Join();
                Console.WriteLine("Copying Complete");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void copyFiles()
        {
            try
            {
                rootSourceDirectory.copy();

                if (lastBackup > defaultBackupDate)
                {
                    lastBackupFile.LastWriteTimeUtc = DateTime.Now;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void progress()
        {
            while (fileCount > backedupfileCount)
            {
                Console.WriteLine("Files Remaining: " + (fileCount - backedupfileCount));
                Thread.Sleep(1000);
            }
        }
    }
}
