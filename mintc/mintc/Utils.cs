using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

namespace mintc
{
    public static class Utils
    {
        public static string Cwd
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            }
        }

        public static string Exec(string program, string args, string cwd = null)
        {
            var cmd = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "cmd",
                    Arguments = "/c " + "\"" + program + "\" " + args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true
                }
            };
            if(cwd != null) cmd.StartInfo.WorkingDirectory = cwd;
            cmd.Start();
            cmd.WaitForExit();
            return cmd.StandardOutput.ReadToEnd();
        }

        public static bool IsDirectoryAndContains(string path, string sub_file)
        {
            if(Directory.Exists(path))
            {
                if(File.Exists(Path.Combine(path, sub_file))) return true;
            }
            return false;
        }

        public static void RecreateDirectory(string path)
        {
            try
            {
                if(Directory.Exists(path)) Directory.Delete(path, true);
                Directory.CreateDirectory(path);
            }
            catch
            {
            }
        }

        public static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }

        public static byte[] GenerateNACPFrom(string name, string author, string version)
        {
            var nacp = new List<byte>();
            int size;
            for(var i = 0; i < 16; i++)
            {
                var bname = new List<byte>(Encoding.ASCII.GetBytes(name));
                size = bname.Count;
                while(size < 0x200)
                {
                    bname.Add(0);
                    size = bname.Count;
                }
                var bauthor = new List<byte>(Encoding.ASCII.GetBytes(author));
                size = bauthor.Count;
                while(size < 0x100)
                {
                    bauthor.Add(0);
                    size = bauthor.Count;
                }
                nacp.AddRange(bname);
                nacp.AddRange(bauthor);
            }

            for(int i = 0; i < 0x60; i++) nacp.Add(0);

            var bver = new List<byte>(Encoding.ASCII.GetBytes(version));
            size = bver.Count;
            while(size < 0x10)
            {
                bver.Add(0);
                size = bver.Count;
            }

            nacp.AddRange(bver);
            for (int i = 0; i < 0xF90; i++) nacp.Add(0);

            return nacp.ToArray();
        }
    }
}
