using System;
using System.Collections.Generic;
using System.Text;

namespace BCUnitFramework
{
    public class AssertionFailedError : Exception
    {
        public AssertionFailedError() { }
        public AssertionFailedError(string message) : base(message) {   }
           
        
    }
    
    
}
