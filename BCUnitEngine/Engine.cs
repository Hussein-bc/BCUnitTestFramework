using BCUnitFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;


// Class Library (.NET Framework)
// .NET Framework  4.7.2

namespace BCUnitEngine
{
    public class Engine
    {

        private string path;
        private List<Type> AllAtributes = null;
        private List<string> invokedMethodNames = new List<string>();

        public Engine(string path)
        {
            this.path = path;

            Go();
            ShowMessage("END OF ENGINE");
        }

        private void Go()
        {
            try
            {

                AllAtributes = new List<Type>();

                AllAtributes = ReadAllAttribute();

                MethodsParser();

            }
            catch (Exception e)
            {
                ShowMessage("ERROR: " + e.Message);
            }


        }

        private void MethodsParser()
        {
            foreach (Type t in AllAtributes)
            {


                // Query to find the BeforeAllTestAttribute tag
                var BeforeAllTest = from m in t.GetMethods()
                                    where m.GetCustomAttributes(false).Any(a => a is BeforeAllTestsAttribute)
                                    select m;

                InvokeMethod(t, BeforeAllTest);


                //    // Query to find the TestMethodsAttriubte tags
                var allTestMethods = from m in t.GetMethods()
                                     where m.GetCustomAttributes(false).Any(a => a is TestMethodAttribute)
                                     select m;

                // Re-ordering the founded methods
                var ordered = allTestMethods.OrderBy(p => p.GetCustomAttribute<
                    TestMethodAttribute>().Order);

                // Two lists to store the TestMethodsAttriubte tags 
                List<MethodInfo> withOrder = new List<MethodInfo>();        // has Order = 1, 2, 3, ..

                List<MethodInfo> withoutOrder = new List<MethodInfo>();     // has default Order = 0


                // adding TestMethodsAttriubte methods to the lists
                foreach (var v in ordered)
                {
                    int order = v.GetCustomAttribute<TestMethodAttribute>().Order;

                    if (order != 0)
                    {
                        withOrder.Add(v);
                    }
                    else
                    {
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

            if (testList != null)
            {
                object instance = Activator.CreateInstance(type);
                foreach (MethodInfo mInfo in testList)
                {
                    //Console.Write($"Engine is running on {mInfo.Name}() => ");
                    invokedMethodNames.Add(mInfo.ToString());

                    ShowMessage($"Engine is running on {mInfo.Name}() => ");
                    mInfo.Invoke(instance, new object[0]);

                }
            }
        }

        private List<Type> ReadAllAttribute()
        {
            var pluginList = new List<Type>();

            Assembly assembly = Assembly.LoadFrom(path);

            Type[] allTypes = assembly.GetTypes();
            foreach (Type type in allTypes)
            {

                if (((type.GetCustomAttribute(typeof(TestClassAttribute))) is TestClassAttribute))
                {
                    pluginList.Add(type);
                }
            }


            return pluginList;

        }


        public void ShowMessage(string s)
        {
            System.Windows.Forms.MessageBox.Show($"Message:\n{s}");
        }
        public List<string> GetInvokedMethodNames()
        {
            return invokedMethodNames;
        }

    }
}
