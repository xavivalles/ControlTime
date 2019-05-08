using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControlTime
{
    class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;
            // Display title as the C# console calculator app
            Console.WriteLine("Console to change the interval to rules\r");
            Console.WriteLine("----------------------------------------\n");
            DatabaseConnection databaseConnection = new DatabaseConnection();
            var events = databaseConnection.GetTriggerNames();
            while (!endApp)
            {
                // Declare variables and set to empty
                string eventNumSelected = "";

                if (events.Count() > 0)
                {
                    foreach (var eventName in events)
                    {
                        Console.WriteLine(string.Format(eventName.Key + ": " + eventName.Value));
                    }
                }

                // Ask the user to type the first number
                Console.Write("\nChoose the event to change the interval: ");
                eventNumSelected = Console.ReadLine();

                var endLoop = false;
                int isNum = 0;

                while (!endLoop)
                {
                    bool eventSelected = int.TryParse(eventNumSelected, out isNum);
                    if (!eventSelected)
                    {
                        Console.Write("\nThis is not valid input. Please enter an integer value: ");
                        eventNumSelected = Console.ReadLine();
                    }
                    else
                    {
                        foreach (var eventOne in events)
                        {

                            if (eventOne.Key == int.Parse(eventNumSelected))
                            {
                                Console.Write(eventOne.Value);
                                endLoop = true;
                            }
                        }
                        if (endLoop == false)
                        {
                            Console.Write("\nThis number not is in list. Please enter an integer value: ");
                            eventNumSelected = Console.ReadLine();
                        }
                    }
                }

                Console.WriteLine("\n------------------------\n");

                // Wait for the user to respond before closing
                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") endApp = true;

                Console.WriteLine("\n"); // Friendly linespacing
            }
            return;
        }
    }
}
