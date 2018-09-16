using System;
using System.Collections.Generic;
using System.Text;

namespace SentenceStitcher
{
    class Program
    {
        static void Main(string[] args)
        {
            var successfulResult =
                "This is a test. For this to be a successful test, this test should reconstruct these sentences properly.";

            var inputs = new string[]
            {
                "This is a test.",
                "For this to be a successful test,",
                "test, this test should reconstruct",
                "a test. For this",
                "should reconstruct these test",
                "these test sentences properly."
            };

            var stitcher = new Stitcher(inputs);
            stitcher.Process();
            Console.WriteLine(stitcher.Process());
        }
    }
}
