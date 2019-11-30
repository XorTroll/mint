using System;
using System.Reflection;
using System.Drawing;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using CommandLine;

namespace mintc
{
    namespace cmdline
    {
        public static class Utils
        {
            public static void ThrowError(string what)
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("Error: ");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(what);
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(-1);
            }
        }

        [Verb("assemble", HelpText = "Assemble a Mint-Bridge.NET Visual Studio project into an executable NRO file.")]
        public class AssembleOptions
        {
            [Option("icon", HelpText = "Icon path (must be a 256x256 JPEG)")]
            public string Icon { get; set; }

            [Option("name", HelpText = "Name string")]
            public string Name { get; set; }

            [Option("author", HelpText = "Author string")]
            public string Author { get; set; }

            [Option("version", HelpText = "Version string")]
            public string Version { get; set; }

            [Option("mode", HelpText = "Build mode (Debug/Release)", Required = true)]
            public string Mode { get; set; }

            [Value(0, HelpText = "Input *.csproj file", Required = true)]
            public string ProjectFile { get; set; }
        }

        [Verb("create", HelpText = "Create a new Mint-Bridge.NET Visual Studio project.")]
        public class CreateOptions
        {
            [Option("path", HelpText = "Path in which to create the project.")]
            public string Path { get; set; }

            [Value(0, HelpText = "Project's name", Required = true)]
            public string Name { get; set; }
        }
    }

    public class Program
    {
        public static int RunAssemble(cmdline.AssembleOptions options)
        {
            if(!File.Exists(options.ProjectFile)) cmdline.Utils.ThrowError("Invalid *.csproj file.");
            if(options.Mode != "Debug" && options.Mode != "Release") cmdline.Utils.ThrowError("Invalid mode (must be Debug or Release)");

            var name = string.IsNullOrEmpty(options.Name) ? "Mint project" : options.Name;
            var author = string.IsNullOrEmpty(options.Author) ? "Mint project" : options.Author;
            var version = string.IsNullOrEmpty(options.Version) ? "0.0.0.0" : options.Version;

            var icon = (!string.IsNullOrEmpty(options.Icon) && File.Exists(options.Icon)) ? new Bitmap(options.Icon) : null;

            var projdir = Path.GetDirectoryName(options.ProjectFile);
            var projname = Path.GetFileName(projdir);
            var outdir = Path.Combine(projdir, "bin", options.Mode);
            var outdll = Path.Combine(outdir, projname + ".dll");
            if(!File.Exists(outdll)) cmdline.Utils.ThrowError("Unable to find the output release DLL. Make sure this is run after a clean and recompile.");

            try
            {
                var dll = Assembly.LoadFile(outdll);
                var info = dll.GetName();
                name = info.Name;
                version = info.Version.ToString();
            }
            catch(Exception ex)
            {
                cmdline.Utils.ThrowError("An error happened: " + ex.Message);
            }

            var mintoutdir = Path.Combine(outdir, "mintc");
            var metajson = Bridge.CreateMetaJSONFromOutput(projname, mintoutdir);
            var tmpdir = Path.Combine(outdir, ".mint");
            Utils.RecreateDirectory(tmpdir);
            var tmpromfsdir = Path.Combine(tmpdir, ".mint");
            Directory.CreateDirectory(tmpromfsdir);
            var romfssrcdir = Path.Combine(tmpromfsdir, "src");
            Directory.CreateDirectory(romfssrcdir);
            FileSystem.CopyDirectory(mintoutdir, romfssrcdir);
            File.WriteAllText(Path.Combine(tmpromfsdir, "meta.json"), metajson.ToString());

            var tmpnro = Path.Combine(outdir, projname + "-temp.nro");
            var outnro = Path.Combine(outdir, projname + ".nro");

            File.WriteAllBytes(tmpnro, Properties.Resources.target_nro);

            var nro = new NRO.NROStruct
            {
                Name = name,
                Author = author,
                Version = version,
                Icon = icon
            };
            nro.ApplySave(tmpnro, tmpdir, outnro);

            try
            {
                File.Delete(tmpnro);
                Directory.Delete(tmpdir, true);
            }
            catch
            {
            }

            Console.WriteLine("Done -> NRO generated at: " + outnro);

            return 0;
        }

        public static int RunCreate(cmdline.CreateOptions options)
        {
            var path = (string.IsNullOrEmpty(options.Path)) ? Environment.CurrentDirectory : options.Path;
            var ok = Bridge.CreateBaseVisualStudioProject(path, options.Name);
            if(!ok) cmdline.Utils.ThrowError("Unable to create the project.");

            Console.WriteLine($"The project was successfully created at '{Path.Combine(path, options.Name)}'.");

            return 0;
        }

        public static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<cmdline.AssembleOptions, cmdline.CreateOptions>(args).MapResult(
                    (cmdline.AssembleOptions options) => RunAssemble(options),
                    (cmdline.CreateOptions options) => RunCreate(options),
                    err => -1
                );
        }
    }
}
