using System;
using System.Collections.Generic;
using System.Text;

namespace BCUnitFramework
{

    // Attribute class to flag classes
    [AttributeUsage(AttributeTargets.Class)]
    public class TestClassAttribute : Attribute
    {
    }
}
