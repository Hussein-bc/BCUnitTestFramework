using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCUnitFramework
{

    // Call back design pattern
    class TestOutput : ITestOutput
    {
        public void Print(string str)
        {
            System.Windows.Forms.MessageBox.Show($"ITestOutput:\n{str}");
        }
    }
}
