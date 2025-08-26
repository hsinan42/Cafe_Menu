using System;
using System.IO;
using System.Security.Cryptography;
using System.Web;

public static class FileHelper
{
    public static string ComputeFileHash(string filePath)
    {
        if (!File.Exists(filePath)) return null;

        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filePath))
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }

    public static string ComputeFileHash(HttpPostedFileBase file)
    {
        if (file == null) return null;

        using (var md5 = MD5.Create())
        {
            using (var stream = file.InputStream)
            {
                byte[] hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}
