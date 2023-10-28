using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System.Linq;
using System.Diagnostics;

namespace BuildAntlr
{
    public class BuildAntlr : Task
    {
        public string Namespace { get; set; }

        public string OutputDir { get; set; }

        [Required]
        public string JavaPath { get; set; }

        [Required]
        public string AntlrJar { get; set; }

        [Required]
        public bool Visitors { get; set; }

        [Required]
        public bool Listeners { get; set; }

        [Required]
        public ITaskItem[] Files { get; set; }

        public override bool Execute()
        {
            string pathSeparator = System.IO.Path.PathSeparator.ToString();
            string classpath = System.Environment.GetEnvironmentVariable("CLASSPATH");
            string cp = $"{classpath}{pathSeparator}{AntlrJar}";

            var filenames = Files.Select(item => item.GetMetadata("FullPath")).ToArray();
            string files = string.Join(" ", filenames);
            
            var visitor = Visitors ? "-visitor" : "-no-visitor";
            var listener = Listeners ? "-listener" : "-no-listener";

            var output = string.IsNullOrEmpty(OutputDir) ? "" : $"-o {OutputDir}";
            var @namespace = string.IsNullOrEmpty(Namespace) ? "" : $"-package {Namespace}";

            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = JavaPath,
                    Arguments = $"-Xmx500M -cp {cp} org.antlr.v4.Tool {visitor} {listener} {output} {@namespace} -Dlanguage=CSharp {files}",
                    UseShellExecute = false,
                    RedirectStandardOutput = false,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            proc.WaitForExit();
            return true;
        }
    }
}
