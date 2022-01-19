using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using System;
using System.IO;
using Task = System.Threading.Tasks.Task;

namespace Extension
{
    [Command(PackageIds.MyCommandRunSolution)]
    internal sealed class MyCommandRunSolution : BaseCommand<MyCommandRunSolution>
    {
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            
            try {
                await VS.MessageBox.ShowAsync("Solution");

                // This will get projects in the solution
                var solution = await VS.Solutions.GetAllProjectsAsync();

                // this will return the parent directory 
                DirectoryInfo info;


                OutputWindowPane pane = await VS.Windows.CreateOutputWindowPaneAsync("From Solution");

                foreach (var project in solution) { 

                    info = Directory.GetParent(project.FullPath);
                    string currentProjectPath = info.ToString();
                    await VS.MessageBox.ShowAsync(currentProjectPath);
                    await pane.WriteLineAsync("NICEEEEE");
                }

            } catch(System.Exception ex) {
                await VS.MessageBox.ShowWarningAsync(ex.Message);
            }


           
        }
    }
}
