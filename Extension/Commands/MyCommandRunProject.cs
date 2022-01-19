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
        protected  async override Task ExecuteAsync(OleMenuCmdEventArgs e)
        {

            // questions to ask:
            // why catch doesn't catch the exception? if the user doesn't select a project and the solution is highlighted
            try {
                await VS.MessageBox.ShowAsync("Current Project");
                
                // This will get the active project ( highlighted with a mouse click)
                Project activeProject = await VS.Solutions.GetActiveProjectAsync();


                OutputWindowPane pane = null;



                if (activeProject != null) {

                    pane = await VS.Windows.CreateOutputWindowPaneAsync(activeProject.Name.ToString());


                    // this will return the parent directory of a selected project 
                    DirectoryInfo info = Directory.GetParent(activeProject.FullPath);
                    string currentProjectPath = info.ToString();

                    //await VS.MessageBox.ShowAsync("currentProjectPath: " + currentProjectPath);


                    Engine engine = new Engine(currentProjectPath);
                    //engine.ShowPath();


                    var list = engine.GetInvokedMethodNames();
                    foreach (var item in list) {
                        await pane.WriteLineAsync(item);
                    }
                    


                } else {

                    
                    await VS.MessageBox.ShowErrorAsync("select a project");
                    pane = await VS.Windows.CreateOutputWindowPaneAsync("No project is selected");
                    await pane.WriteLineAsync("stupid");
                }


            } catch (Exception ex) {
                await VS.MessageBox.ShowAsync(ex.Message.ToString());
            }
        }
    }


}
