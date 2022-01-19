using System;
using System.Collections.Generic;
using System.Text;


// This will run before TestMethod Attribute
namespace BCUnitFramework
{
    [AttributeUsage(AttributeTargets.Method)]

    public class AfterAllTestsAttribute : Attribute
    {
    }
}
