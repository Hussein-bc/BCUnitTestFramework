using Community.VisualStudio.Toolkit;
using Microsoft.VisualStudio.Shell;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Task = System.Threading.Tasks.Task;
using BCUnitEngine;



namespace Extension
{
    [Command(PackageIds.MyCommandRunProject)]
    internal sealed class MyCommandRunProject : BaseCommand<MyCommandRunProject>
    {
        public string currentProjectPath;
        protected async override Task ExecuteAsync(OleMenuCmdEventArgs e)
        {



            try
            {
                await VS.MessageBox.ShowAsync("Current Project");

                // This will get the active project ( highlighted with a mouse click)
                Project activeProject = await VS.Solutions.GetActiveProjectAsync();
                string DllNameToEngine = activeProject.Name + ".dll";

                DirectoryInfo info = Directory.GetParent(activeProject.FullPath);
                Directory.SetCurrentDirectory(info.ToString());

                var files = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.dll", SearchOption.AllDirectories);
                foreach (var item in files)
                {

                    if (item.Contains(DllNameToEngine))
                    {

                        currentProjectPath = item.ToString();

                    }
                }
                // await VS.MessageBox.ShowAsync("currentProjectPath   " + currentProjectPath);
                OutputWindowPane pane = null;



                if (activeProject != null)
                {

                    pane = await VS.Windows.CreateOutputWindowPaneAsync(activeProject.Name.ToString());


                    Engine engine = new Engine(currentProjectPath);



                    var list = engine.GetInvokedMethodNames();
                    foreach (var item in list)
                    {
                        await pane.WriteLineAsync(item);
                    }



                }
                else
                {


                    await VS.MessageBox.ShowErrorAsync("select a project");
                    pane = await VS.Windows.CreateOutputWindowPaneAsync("No project is selected");
                }


            }
            catch (Exception ex)
            {
                await VS.MessageBox.ShowAsync(ex.Message.ToString());
            }
        }
    }


}
