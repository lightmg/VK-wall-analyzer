using System;
using System.Diagnostics;
using System.IO;

namespace WallStats.Helpers
{
    public static class FileSystemHelpers
    {
        public static FileInfo GetFile(string fileName) =>
            new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName));

        public static void OpenFile(this FileInfo fileInfo) =>
            Process.Start(new ProcessStartInfo(fileInfo.FullName) {UseShellExecute = true});
    }
}