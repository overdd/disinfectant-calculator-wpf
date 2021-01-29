using System.IO;

namespace WpfApplication1
{
    public static class FileLogger
    {
        public static void WriteToFile(string log)
        {
            using (FileStream fstream = new FileStream($"{Config.LOG_FILE_PATH}", FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(log + "\n");
                fstream.Write(array, 0, array.Length);
            }
        } 
    }
}