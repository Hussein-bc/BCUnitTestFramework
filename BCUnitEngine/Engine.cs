using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BCUnitFramework;


// Class Library (.NET Framework)
// .NET Framework  4.7.2

namespace BCUnitEngine
{

    public class Engine
    {


        private string path;
        private List<Type> AllAtributes = null;
        private readonly string fileType = "*.dll";

        private List<string> invokedMethodNames = new List<string>();

        public Engine (string path)
        {
            this.path = path + @"\bin";
            ShowMessage(path);
            
            Go();
            ShowMessage("END OF ENGINE");
        }

        private void Go()
        {
            try {

               
                Directory.SetCurrentDirectory(path);
                
                //ShowMessage("Engine SetCurrentDirectory: " + path);
                AllAtributes = new List<Type>();

                AllAtributes = ReadAllAttribute();

                MethodsParser();



            } catch (Exception e) {
                ShowMessage("ERROR: " + e.Message);
            }


        }

        private void MethodsParser()
        {
            ShowMessage("method parser");
            foreach (Type t in AllAtributes) {

                ShowMessage("method parser before query");
                // Query to find the BeforeAllTestAttribute tag
                var BeforeAllTest = from m in t.GetMethods()
                                    where m.GetCustomAttributes(false).Any(a => a is BeforeAllTestsAttribute)
                                    select m;
                ShowMessage("1 before InvokeMethod");
                InvokeMethod(t, BeforeAllTest);


                // Query to find the TestMethodsAttriubte tags
                var allTestMethods = from m in t.GetMethods()
                                     where m.GetCustomAttributes(false).Any(a => a is TestMethodAttribute)
                                     select m;

                // Re-ordering the founded methods
                var ordered = allTestMethods.OrderBy(p => p.GetCustomAttribute<TestMethodAttribute>().Order);

                // Two lists to store the TestMethodsAttriubte tags 
                List<MethodInfo> withOrder = new List<MethodInfo>();        // has Order = 1, 2, 3, ..

                List<MethodInfo> withoutOrder = new List<MethodInfo>();     // has default Order = 0


                // adding TestMethodsAttriubte methods to the lists
                foreach (var v in ordered) {
                    int order = v.GetCustomAttribute<TestMethodAttribute>().Order;


                    if (order != 0) {
                        withOrder.Add(v);
                    } else {
                        withoutOrder.Add(v);
                    }
                }

                // Concatenate the lists with Order of 1, 2, 3......, then 0, 0, 0
                List<MethodInfo> newList = withOrder.Concat(withoutOrder).ToList();

                InvokeMethod(t, newList);


                // Checking for AfterAllTests Attribute tag
                var afterTest = from m in t.GetMethods()
                                where m.GetCustomAttributes(false).Any(a => a is AfterAllTestsAttribute)
                                select m;

                InvokeMethod(t, afterTest);

            }

        }


        private void InvokeMethod(Type type, IEnumerable<MethodInfo> testList)
        {

            ShowMessage("inside InvokeMethod ");
            if (testList != null) {
                object instance = Activator.CreateInstance(type);
                foreach (MethodInfo mInfo in testList) {
                    //Console.Write($"Engine is running on {mInfo.Name}() => ");

                    //ShowMessage($"Engine is running on {mInfo.Name}() => ");
                    invokedMethodNames.Add(mInfo.Name);
                    mInfo.Invoke(instance, new object[0]);
                    
                }
            }
        }



        private List<Type> ReadAllAttribute()
        {
            var pluginList = new List<Type>();


            ShowMessage("1 inside ReadAllAttribute");
            var files = Directory.GetFiles(Directory.GetCurrentDirectory(), fileType, SearchOption.AllDirectories);
            //foreach (var file in files)
            //    ShowMessage("file\n" + file);

            List<Assembly> tempAssemblyList = new List<Assembly>();
            foreach (var file in files) {
                

                Assembly assembly = Assembly.LoadFile(Path.Combine(Directory.GetCurrentDirectory(), file));
                ShowMessage("ass fullname\n" + assembly.FullName);
                tempAssemblyList.Add(assembly);

            }

            foreach (Assembly a in tempAssemblyList) {
                Type[] allTypes = a.GetTypes();
                foreach (Type type in allTypes) {
                    //ShowMessage("type in temp ass list\n" + type.Name);
                    if ((type.GetCustomAttribute(typeof(TestClassAttribute))) is TestClassAttribute) {
                        //ShowMessage("list has:\n" + type);
                        pluginList.Add(type);

                    }
                }
            }
            
            return pluginList;
            
        }




        public void ShowPath()
        {
            System.Windows.Forms.MessageBox.Show($"The path is {path}");
        }

        public void ShowMessage(string s)
        {
            System.Windows.Forms.MessageBox.Show($"Message:\n{s}");
        }

        public List<string> GetInvokedMethodNames()
        {
            return invokedMethodNames;
        }


        public int GetCount()
        {
            return AllAtributes.Count;
        }
    }
}
