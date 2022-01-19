
using System;
using System.Collections.Generic;
using System.Text;


// The fail will not throw an exception it will print only
namespace BCUnitFramework
{


    



    
    public class Assertions
    {


        private static ITestOutput testOutput = new TestOutput();

        
        public static void SetTestOutput(ITestOutput test)
        {
            testOutput = test;
        }

        public static void AssertArrayEquals(int[] expected, int[] actual)
        {
            if (expected.Length != actual.Length) 
                testOutput.Print($"Arrays Not Equal!");

            for (int i = 0; i < expected.Length; i++) {
                if (expected[i] != actual[i])
                    testOutput.Print($"At index {i} Expected != Actual");
              
            }
            testOutput.Print("PASS\n");
            


        }

        public static void AssertEquals(int expected, int acutal)
        {
            if (expected != acutal)
                testOutput.Print($"Expected: {expected}\n Actual: {acutal}");

           
            testOutput.Print("PASS");
        }


        public static void AssertNotEquals(int expected, int acutal)
        {
            if (expected == acutal)
                testOutput.Print($"Expected: {expected}\n Actual: {acutal}");

            testOutput.Print("PASS");
        }


        public static void AssertNotEquals(double expected, double acutal)
        {
            if (expected == acutal)
                testOutput.Print($"Expected: {expected}\n Actual: {acutal}");

            testOutput.Print("PASS\n");
        }


        //static public void Fail(String message)
        //{
        //    throw new AssertionFailedError(message);
        //}
    }
}
