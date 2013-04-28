using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Owin.Hosting;

namespace Gate.Adapters.AspNet.TestConsoleHost {
    public class Program {
        public static void Main(string[] args) {
            try {
                MainThatCanThrow();
            }
            catch (Exception ex) {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(ex);
                Console.ReadKey();
                Console.ResetColor();
            }
        }

        private static void MainThatCanThrow() {
            var url = "http://localhost:60880";

            Console.WriteLine("Starting server at {0}.", url);
            using (WebApplication.Start<Startup>(url)) {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  Server started.");
                Console.ResetColor();

                Process.Start(new ProcessStartInfo {
                    FileName = url,
                    UseShellExecute = true
                });

                Console.WriteLine("  Press enter to stop.");
                Console.ReadLine();
            }
        }
    }
}
