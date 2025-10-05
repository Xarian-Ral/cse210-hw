using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MindfulnessProgram
{
    class Activity
    {
        private string _name;
        private string _description;
        private int _duration; 
        private static readonly string LogFilePath = "activity_log.txt";

        protected Random rng = new Random();

        public Activity(string name, string description)
        {
            _name = name;
            _description = description;
            _duration = 0;
        }

        public void DisplayStartingMessage()
        {
            Console.Clear();
            Console.WriteLine($"--- {_name} ---");
            Console.WriteLine();
            Console.WriteLine(_description);
            Console.WriteLine();
            SetDurationFromUser();
            Console.WriteLine();
            Console.WriteLine("Get ready...");
            ShowSpinner(3);
        }

        public void DisplayEndingMessage()
        {
            Console.WriteLine();
            Console.WriteLine("Pat yourself on the back!");
            Console.WriteLine($"You have completed the {_name} for {_duration} seconds.");
            ShowSpinner(3);
            LogActivity();
        }

        protected int Duration => _duration;

        private void SetDurationFromUser()
        {
            while (true)
            {
                Console.Write("Enter duration in seconds: ");
                string input = Console.ReadLine() ?? "";
                if (int.TryParse(input.Trim(), out int seconds) && seconds > 0)
                {
                    _duration = seconds;
                    break;
                }
                Console.WriteLine("Please enter a valid positive integer for seconds.");
            }
        }

        protected void ShowSpinner(int seconds)
        {
            char[] sequence = new char[] { '|', '/', '-', '\\' };
            int seqIndex = 0;
            DateTime end = DateTime.Now.AddSeconds(seconds);
            while (DateTime.Now < end)
            {
                Console.Write(sequence[seqIndex]);
                Thread.Sleep(250);
                Console.Write("\b \b");
                seqIndex = (seqIndex + 1) % sequence.Length;
            }
        }

        protected void ShowCountDown(int seconds)
        {
            for (int i = seconds; i >= 1; i--)
            {
                Console.Write(i);
                Thread.Sleep(1000);
                Console.Write("\b \b");
            }
        }

        private void LogActivity()
        {
            try
            {
                string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}\t{_name}\t{_duration}s";
                File.AppendAllText(LogFilePath, entry + Environment.NewLine);
            }
            catch
            {
                
            }
        }

        public virtual void Run()
        {

        }
    }

    class BreathingActivity : Activity
    {
        public BreathingActivity() : base("Breathing Activity",
            "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.")
        {
        }

        public override void Run()
        {
            DisplayStartingMessage();

            int inhale = 4;
            int exhale = 6;

            DateTime endTime = DateTime.Now.AddSeconds(Duration);
            bool inhaleTurn = true;

            while (DateTime.Now < endTime)
            {
                if (inhaleTurn)
                {
                    Console.WriteLine();
                    Console.Write("Breathe in... ");
                    int remaining = (int)(endTime - DateTime.Now).TotalSeconds;
                    int countdown = Math.Min(inhale, Math.Max(1, remaining));
                    ShowCountDown(countdown);
                }
                else
                {
                    Console.WriteLine();
                    Console.Write("Breathe out... ");
                    int remaining = (int)(endTime - DateTime.Now).TotalSeconds;
                    int countdown = Math.Min(exhale, Math.Max(1, remaining));
                    ShowCountDown(countdown);
                }
                inhaleTurn = !inhaleTurn;
            }

            Console.WriteLine();
            DisplayEndingMessage();
        }
    }

    class ReflectingActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "Think of a time when you stood up for someone else.",
            "Think of a time when you did something really difficult.",
            "Think of a time when you helped someone in need.",
            "Think of a time when you did something truly selfless."
        };

        private List<string> _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "Have you ever done anything like this before?",
            "How did you get started?",
            "How did you feel when it was complete?",
            "What made this time different than other times when you were not as successful?",
            "What is your favorite thing about this experience?",
            "What could you learn from this experience that applies to other situations?",
            "What did you learn about yourself through this experience?",
            "How can you keep this experience in mind in the future?",
            "What can YOU do differently?"
        };

        public ReflectingActivity() : base("Reflecting Activity",
            "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.")
        {
        }

        private string GetRandomPrompt()
        {
            return _prompts[rng.Next(_prompts.Count)];
        }

        private string GetRandomQuestion()
        {
            return _questions[rng.Next(_questions.Count)];
        }

        public override void Run()
        {
            DisplayStartingMessage();

            Console.WriteLine();
            Console.WriteLine(GetRandomPrompt());
            Console.WriteLine();
            Console.WriteLine("When you are ready, ponder the following questions:");
            Console.WriteLine();

            DateTime endTime = DateTime.Now.AddSeconds(Duration);

            ShowSpinner(3);

            while (DateTime.Now < endTime)
            {
                string q = GetRandomQuestion();
                Console.WriteLine("> " + q);
                int secondsRemaining = (int)(endTime - DateTime.Now).TotalSeconds;
                int spinnerTime = Math.Min(5, Math.Max(1, secondsRemaining));
                ShowSpinner(spinnerTime);
                Console.WriteLine();
            }

            DisplayEndingMessage();
        }
    }

    class ListingActivity : Activity
    {
        private List<string> _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "Who are people that you have helped this week?",
            "When have you felt the Holy Ghost this month?",
            "Who are some of your personal heroes?"
        };

        public ListingActivity() : base("Listing Activity",
            "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.")
        {
        }

        private string GetRandomPrompt()
        {
            return _prompts[rng.Next(_prompts.Count)];
        }

        public override void Run()
        {
            DisplayStartingMessage();

            Console.WriteLine();
            string prompt = GetRandomPrompt();
            Console.WriteLine(prompt);
            Console.WriteLine();
            Console.WriteLine("You will have a few seconds to think about this prompt.");
            ShowCountDown(5);

            Console.WriteLine();
            Console.WriteLine("Start listing items. Press Enter after each one. (Timer is ticking!)");
            Console.WriteLine();

            List<string> items = new List<string>();
            DateTime endTime = DateTime.Now.AddSeconds(Duration);

         
            while (DateTime.Now < endTime)
            {
                int remainingSeconds = (int)(endTime - DateTime.Now).TotalSeconds;
                Console.Write($"({remainingSeconds}s left) Item: ");
                string? line = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(line))
                {
                    items.Add(line.Trim());
                }

                if (DateTime.Now >= endTime)
                {
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Great job! You listed {items.Count} items:");
            foreach (var it in items)
            {
                Console.WriteLine(" - " + it);
            }

            DisplayEndingMessage();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Mindfulness Program";
            MainMenu();
        }

        static void MainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Mindfulness Program");
                Console.WriteLine("-------------------");
                Console.WriteLine("1. Breathing Activity");
                Console.WriteLine("2. Reflecting Activity");
                Console.WriteLine("3. Listing Activity");
                Console.WriteLine("4. Quit");
                Console.Write("Select an option (1-4): ");
                string? choice = Console.ReadLine();

                Activity? activity = null;

                switch (choice?.Trim())
                {
                    case "1":
                        activity = new BreathingActivity();
                        break;
                    case "2":
                        activity = new ReflectingActivity();
                        break;
                    case "3":
                        activity = new ListingActivity();
                        break;
                    case "4":
                        Console.WriteLine("Goodbye! Take care.");
                        Thread.Sleep(800);
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Press Enter to continue...");
                        Console.ReadLine();
                        continue;
                }

                activity.Run();
                Console.WriteLine();
                Console.WriteLine("Press Enter to return to the main menu...");
                Console.ReadLine();
            }
        }
    }
}
