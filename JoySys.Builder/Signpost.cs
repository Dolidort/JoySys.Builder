using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JoySys.Builder
{
    static class Signpost
    {
        public static void VSTAscii()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            string asciiArt = @"
-- ┌───────────────────────────────────┐
-- │                                   │
-- │                                   │
-- │                                   │
-- │                                   │
-- │    /$$    /$$ /$$$$$$ /$$$$$$$$   │
-- │   | $$   | $$/$$__  $|__  $$__/   │
-- │   | $$   | $| $$  \__/  | $$      │
-- │   |  $$ / $$|  $$$$$$   | $$      │
-- │    \  $$ $$/ \____  $$  | $$      │
-- │     \  $$$/  /$$  \ $$  | $$      │
-- │      \  $/  |  $$$$$$/  | $$      │
-- │       \_/    \______/   |__/      │
-- │                                   │
-- │                                   │
-- │                                   │
-- │                                   │
-- └───────────────────────────────────┘  
";

            Console.WriteLine(asciiArt);
            Console.WriteLine("The Metadata was updated using the SCADA structure. \n This procedure must only be executed while the system is in development mode.");

        }



        public static class SpinnerHelper
        {
            private static bool spinnerRunning = false;

            public static void StartSpinner(string message, int spinnerCount = 10)
            {
                spinnerRunning = true;

                new Thread(() =>
                {
                    string[] spinnerSequence = new[] { "|", "/", "-", "\\" };
                    int[] indices = new int[spinnerCount];

                    while (spinnerRunning)
                    {
                        string spinners = "";
                        for (int i = 0; i < spinnerCount; i++)
                        {
                            spinners += spinnerSequence[indices[i]] + " ";
                            indices[i] = (indices[i] + 1) % spinnerSequence.Length;
                        }

                        Console.Write("\r" + message + " " + spinners.TrimEnd());

                        Thread.Sleep(100);
                    }

                    Console.Write("\r" + new string(' ', message.Length + spinnerCount * 2) + "\r");
                })
                { IsBackground = true }.Start();
            }

            public static void StopSpinner()
            {
                spinnerRunning = false;
            }
        }

    }

}
