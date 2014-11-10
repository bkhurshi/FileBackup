using System;
using System.IO;
using System.Collections.Generic;

namespace FileBackup
{
    public class FileNode
    {
        public String mFile;
        List<FileNode> mSubDirectories = new List<FileNode>();
        List<FileLeaf> mSubFiles = new List<FileLeaf>();
        
        public FileNode(String file)
        {
            mFile = file;

            try
            {
                string[] subDirectories = Directory.GetDirectories(mFile);

                foreach (string subDirectory in subDirectories)
                {
                    mSubDirectories.Add(new FileNode(subDirectory));
                }
            }
            catch (UnauthorizedAccessException e)
            {
                return;
            }

            string[] subFiles = Directory.GetFiles(mFile);

            foreach (string subFile in subFiles)
            {
                mSubFiles.Add(new FileLeaf(subFile));
            }
        }

        public List<FileNode> getSubDirectories()
        {
            return mSubDirectories;
        }

        public List<FileLeaf> getSubFiles()
        {
            return mSubFiles;
        }

        public void copy()
        {
            string path = Program.rootDestinationPath + mFile.Substring(8);

            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception e)
                {
                    return;
                }
            }

            foreach (FileLeaf file in mSubFiles)
            {
                file.copy();
            }

            foreach (FileNode subDirectory in mSubDirectories)
            {
                subDirectory.copy();
            }
        }
    }
}