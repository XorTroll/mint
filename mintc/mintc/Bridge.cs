using System;
using Microsoft.VisualBasic.FileIO;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace mintc
{
    public class Bridge
    {
        public static bool IsInstalled()
        {
            return !string.IsNullOrEmpty(Utils.Exec("bridge", "-v"));
        }

        public const string MintLibraryName = "libmint";

        public static JObject CreateMetaJSONFromOutput(string projname, string path)
        {
            dynamic json = new JObject();
            var table = new JArray();
            var entry = new List<string>();
            var jsfiles = Directory.GetFiles(path).Where((file) => file.EndsWith(".min.js") && !file.EndsWith(".meta.min.js"));
            var bridgejs = jsfiles.Where((js) => Path.GetFileName(js).StartsWith("bridge."));
            foreach(var js in bridgejs)
            {
                jsfiles = jsfiles.Where((jsf) => jsf != js);
                table.Add(Path.GetFileName(js));
            }
            var libjs = jsfiles.Where((js) => Path.GetFileName(js).StartsWith(MintLibraryName + "."));
            foreach (var js in libjs)
            {
                jsfiles = jsfiles.Where((jsf) => jsf != js);
                table.Add(Path.GetFileName(js));
            }
            var entryjs = jsfiles.Where((js) => Path.GetFileName(js).StartsWith(projname + "."));
            foreach (var js in entryjs)
            {
                jsfiles = jsfiles.Where((jsf) => jsf != js);
                entry.Add(Path.GetFileName(js));
            }
            foreach(var leftjs in jsfiles)
            {
                table.Add(Path.GetFileName(leftjs));
            }
            foreach(var js in entry)
            {
                table.Add(Path.GetFileName(js));
            }
            json.script_table = table;
            return json;
        }

        public static bool CreateBaseVisualStudioProject(string base_path, string name)
        {
            if(!IsInstalled()) return false;
            if(!Directory.Exists(base_path)) Directory.CreateDirectory(base_path);
            var projdir = Path.Combine(base_path, name);
            Utils.RecreateDirectory(projdir);

            var projtempbridgenew = Path.Combine(projdir, ".mintc-bridge-new");
            Directory.CreateDirectory(projtempbridgenew);

            Utils.Exec("bridge", "new", projtempbridgenew);
            string bridgenewpackagesdir = Path.Combine(projtempbridgenew, "packages");
            if(!Directory.Exists(bridgenewpackagesdir)) return false;
            var packagessubdir = Path.GetFileName(Directory.GetDirectories(bridgenewpackagesdir).Where((dir) => Path.GetFileName(dir).StartsWith("Bridge.Min.")).FirstOrDefault());
            if(string.IsNullOrEmpty(packagessubdir)) return false;
            string bridgever = packagessubdir.Substring("Bridge.Min.".Length);

            string slndata = Encoding.UTF8.GetString(Properties.Resources._mintc_dummy_project_sln).Replace("[mintc-dummy-project]", name);
            File.WriteAllText(Path.Combine(projdir, name + ".sln"), slndata);

            var csprojdir = Path.Combine(projdir, name);
            Directory.CreateDirectory(csprojdir);
            var csproj = Path.Combine(csprojdir, name + ".csproj");

            string projdata = Properties.Resources._mintc_dummy_project_csproj;
            projdata = projdata.Replace("[mintc-dummy-project]", name);
            projdata = projdata.Replace("[mintc-post-build-cmdline]", "@echo off\necho Assembling NRO with mintc...\nmintc assemble --mode=$(Configuration) --path=$(ProjectDir) $(TargetPath)");
            projdata = projdata.Replace("[mintc-bridge-min-ver]", bridgever);
            projdata = projdata.Replace("[mintc-bridge-core-ver]", bridgever);
            
            File.WriteAllText(csproj, projdata);

            FileSystem.CopyDirectory(bridgenewpackagesdir, Path.Combine(projdir, "packages"));
            var libmintdir = Path.Combine(projdir, MintLibraryName);
            Directory.CreateDirectory(libmintdir);
            File.WriteAllBytes(Path.Combine(libmintdir, MintLibraryName + ".dll"), Properties.Resources.libmint_dll);

            File.Copy(Path.Combine(projtempbridgenew, "packages.config"), Path.Combine(csprojdir, "packages.config"));

            File.WriteAllText(Path.Combine(csprojdir, "Main.cs"), Properties.Resources.Main_cs.Replace("[mintc-dummy-project]", name));
            var csprojprop = Path.Combine(csprojdir, "Properties");
            Directory.CreateDirectory(csprojprop);
            var asminfo = Properties.Resources.AssemblyInfo_cs;
            asminfo = asminfo.Replace("[mintc-dummy-project]", name);

            File.WriteAllText(Path.Combine(csprojprop, "AssemblyInfo.cs"), asminfo);
            File.WriteAllBytes(Path.Combine(csprojdir, "bridge.json"), Properties.Resources.bridge_json);

            Directory.Delete(projtempbridgenew, true);

            return true;
        }
    }
}
