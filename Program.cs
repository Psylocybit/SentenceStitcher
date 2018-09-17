using System;
using System.Collections.Generic;

namespace SentenceStitcher
{
    class Program
    {
        static void Main(string[] args)
        {
            //var successfulResult =
            //    "This is a test. For this to be a successful test, this test should reconstruct these sentences properly.";

            var inputs = new List<string>
            {
                "This is a", "is a test.",
                "For this to be a successful test,", "be a successful test, this test",
                "this test should reconstruct",
                "a test. For this",
                "should reconstruct these test",
                "these test sentences properly."
            };

            var stitcher = new Stitcher(inputs);
            var result = stitcher.Process();

            Console.WriteLine("Given the following independent sentence(s)/fragment(s):");
            inputs.ForEach(s => Console.WriteLine("\t{0}", s));
            Console.WriteLine("\nStitcher was able to produce the following sentence(s):");
            Console.WriteLine(result);
        }
    }
}
