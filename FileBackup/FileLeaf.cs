using System;
using System.IO;

namespace FileBackup
{
    public class FileLeaf
    {
        FileInfo mLeaf;
        bool mBackedup = false;

        public FileLeaf(string file)
        {
            mLeaf = new FileInfo(file);
            Program.fileCount++;

            if (mLeaf.LastWriteTimeUtc <= Program.lastBackup)
            {
                mBackedup = true;
                Program.backedupfileCount++;
            }
        }

        public string getFileName()
        {
            return mLeaf.Name;
        }


        public void copy()
        {
            string path = Program.rootDestinationPath + mLeaf.FullName.Substring(8);

            if (!mBackedup)
            {
                try
                {
                    File.Copy(mLeaf.FullName, path, true);
                }
                catch (Exception e)
                {
                    mBackedup = true;
                    Program.backedupfileCount++;
                    return;
                }
            }

            mBackedup = true;
            Program.backedupfileCount++;
        }
    }
}
