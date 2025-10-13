using System;
using System.Collections.Generic;
using System.IO;


namespace EternalQuest
{
    public abstract class Goal
    {
        protected string _name;
        protected string _description;
        protected int _points;

        public Goal(string name, string description, int points)
        {
            _name = name;
            _description = description;
            _points = points;
        }

        public string GetName() => _name;
        
        public abstract int RecordEvent();
        public abstract bool IsComplete();
        public abstract string GetDetailsString();
        public abstract string GetStringRepresentation();
    }

    public class SimpleGoal : Goal
    {
        private bool _isComplete;

        public SimpleGoal(string name, string description, int points) 
            : base(name, description, points)
        {
            _isComplete = false;
        }

        public SimpleGoal(string name, string description, int points, bool isComplete) 
            : base(name, description, points)
        {
            _isComplete = isComplete;
        }

        public override int RecordEvent()
        {
            if (!_isComplete)
            {
                _isComplete = true;
                return _points;
            }
            return 0;
        }

        public override bool IsComplete() => _isComplete;

        public override string GetDetailsString()
        {
            string checkbox = _isComplete ? "[X]" : "[ ]";
            return $"{checkbox} {_name} ({_description})";
        }

        public override string GetStringRepresentation()
        {
            return $"SimpleGoal:{_name}|{_description}|{_points}|{_isComplete}";
        }
    }

    public class EternalGoal : Goal
    {
        private int _timesCompleted;

        public EternalGoal(string name, string description, int points) 
            : base(name, description, points)
        {
            _timesCompleted = 0;
        }

        public EternalGoal(string name, string description, int points, int timesCompleted) 
            : base(name, description, points)
        {
            _timesCompleted = timesCompleted;
        }

        public override int RecordEvent()
        {
            _timesCompleted++;
            return _points;
        }

        public override bool IsComplete() => false;

        public override string GetDetailsString()
        {
            return $"[ ] {_name} ({_description}) - Completed {_timesCompleted} times";
        }

        public override string GetStringRepresentation()
        {
            return $"EternalGoal:{_name}|{_description}|{_points}|{_timesCompleted}";
        }
    }

    public class ChecklistGoal : Goal
    {
        private int _amountCompleted;
        private int _target;
        private int _bonus;

        public ChecklistGoal(string name, string description, int points, int bonus, int target) 
            : base(name, description, points)
        {
            _amountCompleted = 0;
            _target = target;
            _bonus = bonus;
        }

        public ChecklistGoal(string name, string description, int points, int bonus, int amountCompleted, int target) 
            : base(name, description, points)
        {
            _amountCompleted = amountCompleted;
            _target = target;
            _bonus = bonus;
        }

        public override int RecordEvent()
        {
            _amountCompleted++;
            
            if (_amountCompleted >= _target)
            {
                return _points + _bonus; 
            }
            return _points;
        }

        public override bool IsComplete() => _amountCompleted >= _target;

        public override string GetDetailsString()
        {
            string checkbox = IsComplete() ? "[X]" : "[ ]";
            string progress = $"Completed {_amountCompleted}/{_target}";
            return $"{checkbox} {_name} ({_description}) -- {progress}";
        }

        public override string GetStringRepresentation()
        {
            return $"ChecklistGoal:{_name}|{_description}|{_points}|{_bonus}|{_amountCompleted}|{_target}";
        }
    }

    public class NegativeGoal : Goal
    {
        private int _timesFailed;

        public NegativeGoal(string name, string description, int pointsLost) 
            : base(name, description, pointsLost)
        {
            _timesFailed = 0;
        }

        public NegativeGoal(string name, string description, int pointsLost, int timesFailed) 
            : base(name, description, pointsLost)
        {
            _timesFailed = timesFailed;
        }

        public override int RecordEvent()
        {
            _timesFailed++;
            return -_points; 
        }

        public override bool IsComplete() => false;

        public override string GetDetailsString()
        {
            return $"[!] {_name} ({_description}) - Failed {_timesFailed} times";
        }

        public override string GetStringRepresentation()
        {
            return $"NegativeGoal:{_name}|{_description}|{_points}|{_timesFailed}";
        }
    }

    class Program
    {
        static List<Goal> _goals = new List<Goal>();
        static int _totalScore = 0;
        static List<string> _achievements = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Eternal Quest!");
            Console.WriteLine("========================================\n");

            bool running = true;
            while (running)
            {
                DisplayScoreAndLevel();
                Console.WriteLine("\nMenu Options:");
                Console.WriteLine("  1. Create New Goal");
                Console.WriteLine("  2. List Goals");
                Console.WriteLine("  3. Save Goals");
                Console.WriteLine("  4. Load Goals");
                Console.WriteLine("  5. Record Event");
                Console.WriteLine("  6. View Achievements");
                Console.WriteLine("  7. Quit");
                Console.Write("Select a choice from the menu: ");
                
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        CreateGoal();
                        break;
                    case "2":
                        ListGoals();
                        break;
                    case "3":
                        SaveGoals();
                        break;
                    case "4":
                        LoadGoals();
                        break;
                    case "5":
                        RecordEvent();
                        break;
                    case "6":
                        ViewAchievements();
                        break;
                    case "7":
                        running = false;
                        Console.WriteLine("Thank you for using Eternal Quest! Keep striving!");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static void DisplayScoreAndLevel()
        {
            int level = (_totalScore / 1000) + 1;
            if (level > 10) level = 10;
            
            string[] levelTitles = {
                "Beginner", "Apprentice", "Journeyman", "Skilled", "Expert",
                "Master", "Grandmaster", "Champion", "Hero", "Legend"
            };

            Console.WriteLine($"\nYou have {_totalScore} points.");
            Console.WriteLine($"Level {level}: {levelTitles[level - 1]}");
            
            // Show progress to next level
            if (level < 10)
            {
                int pointsToNextLevel = ((level) * 1000) - _totalScore;
                Console.WriteLine($"({pointsToNextLevel} points until Level {level + 1})");
            }
            else
            {
                Console.WriteLine("(MAX LEVEL ACHIEVED!)");
            }
        }

        static void CreateGoal()
        {
            Console.WriteLine("The types of Goals are:");
            Console.WriteLine("  1. Simple Goal");
            Console.WriteLine("  2. Eternal Goal");
            Console.WriteLine("  3. Checklist Goal");
            Console.WriteLine("  4. Negative Goal (lose points for bad habits)");
            Console.Write("Which type of goal would you like to create? ");
            
            string type = Console.ReadLine();
            
            Console.Write("What is the name of your goal? ");
            string name = Console.ReadLine();
            
            Console.Write("What is a short description of it? ");
            string description = Console.ReadLine();
            
            Console.Write("What is the amount of points associated with this goal? ");
            int points = int.Parse(Console.ReadLine());

            switch (type)
            {
                case "1":
                    _goals.Add(new SimpleGoal(name, description, points));
                    break;
                case "2":
                    _goals.Add(new EternalGoal(name, description, points));
                    break;
                case "3":
                    Console.Write("How many times does this goal need to be accomplished for a bonus? ");
                    int target = int.Parse(Console.ReadLine());
                    Console.Write("What is the bonus for accomplishing it that many times? ");
                    int bonus = int.Parse(Console.ReadLine());
                    _goals.Add(new ChecklistGoal(name, description, points, bonus, target));
                    break;
                case "4":
                    _goals.Add(new NegativeGoal(name, description, points));
                    Console.WriteLine("Remember: You'll LOSE points each time you record this goal!");
                    break;
            }
            
            Console.WriteLine("Goal created successfully!");
        }

        static void ListGoals()
        {
            Console.WriteLine("The goals are:");
            for (int i = 0; i < _goals.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_goals[i].GetDetailsString()}");
            }
        }

        static void SaveGoals()
        {
            Console.Write("What is the filename for the goal file? ");
            string filename = Console.ReadLine();

            using (StreamWriter outputFile = new StreamWriter(filename))
            {
                outputFile.WriteLine(_totalScore);
                
                foreach (Goal goal in _goals)
                {
                    outputFile.WriteLine(goal.GetStringRepresentation());
                }

                outputFile.WriteLine("ACHIEVEMENTS:");
                foreach (string achievement in _achievements)
                {
                    outputFile.WriteLine(achievement);
                }
            }

            Console.WriteLine("Goals saved successfully!");
        }

        static void LoadGoals()
        {
            Console.Write("What is the filename for the goal file? ");
            string filename = Console.ReadLine();

            string[] lines = System.IO.File.ReadAllLines(filename);

            _goals.Clear();
            _achievements.Clear();
            _totalScore = int.Parse(lines[0]);

            bool readingAchievements = false;

            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];

                if (line == "ACHIEVEMENTS:")
                {
                    readingAchievements = true;
                    continue;
                }

                if (readingAchievements)
                {
                    _achievements.Add(line);
                }
                else
                {
                    string[] parts = line.Split(":");
                    string goalType = parts[0];
                    string[] details = parts[1].Split("|");

                    switch (goalType)
                    {
                        case "SimpleGoal":
                            _goals.Add(new SimpleGoal(details[0], details[1], 
                                int.Parse(details[2]), bool.Parse(details[3])));
                            break;
                        case "EternalGoal":
                            _goals.Add(new EternalGoal(details[0], details[1], 
                                int.Parse(details[2]), int.Parse(details[3])));
                            break;
                        case "ChecklistGoal":
                            _goals.Add(new ChecklistGoal(details[0], details[1], 
                                int.Parse(details[2]), int.Parse(details[3]), 
                                int.Parse(details[4]), int.Parse(details[5])));
                            break;
                        case "NegativeGoal":
                            _goals.Add(new NegativeGoal(details[0], details[1], 
                                int.Parse(details[2]), int.Parse(details[3])));
                            break;
                    }
                }
            }

            Console.WriteLine("Goals loaded successfully!");
        }

        static void RecordEvent()
        {
            ListGoals();
            Console.Write("Which goal did you accomplish? ");
            int choice = int.Parse(Console.ReadLine()) - 1;

            if (choice >= 0 && choice < _goals.Count)
            {
                int previousScore = _totalScore;
                int pointsEarned = _goals[choice].RecordEvent();
                _totalScore += pointsEarned;

                if (pointsEarned > 0)
                {
                    Console.WriteLine($"\n🎉 Congratulations! You have earned {pointsEarned} points!");
                    Console.WriteLine($"You now have {_totalScore} points.");
                    
                    int previousLevel = (previousScore / 1000) + 1;
                    int currentLevel = (_totalScore / 1000) + 1;
                    
                    if (currentLevel > previousLevel && currentLevel <= 10)
                    {
                        string[] levelTitles = {
                            "Beginner", "Apprentice", "Journeyman", "Skilled", "Expert",
                            "Master", "Grandmaster", "Champion", "Hero", "Legend"
                        };
                        Console.WriteLine($"\n⭐ LEVEL UP! You are now Level {currentLevel}: {levelTitles[currentLevel - 1]}! ⭐");
                    }

                    CheckAchievements();
                }
                else if (pointsEarned < 0)
                {
                    Console.WriteLine($"\n❌ Ouch. You lost {Math.Abs(pointsEarned)} points.");
                    Console.WriteLine($"You now have {_totalScore} points. Keep trying!");
                }
                else
                {
                    Console.WriteLine("\nThis goal is already complete!");
                }
            }
            else
            {
                Console.WriteLine("Invalid goal number.");
            }
        }

        static void CheckAchievements()
        {
            int simpleCompleted = 0;
            int eternalTotal = 0;
            int checklistCompleted = 0;

            foreach (Goal goal in _goals)
            {
                if (goal is SimpleGoal && goal.IsComplete())
                    simpleCompleted++;
                else if (goal is EternalGoal eternal)
                    eternalTotal++;
                else if (goal is ChecklistGoal && goal.IsComplete())
                    checklistCompleted++;
            }

            if (eternalTotal >= 50 && !_achievements.Contains("Scripture Warrior"))
            {
                _achievements.Add("Scripture Warrior");
                Console.WriteLine("\n🏆 NEW ACHIEVEMENT UNLOCKED: Scripture Warrior!");
                Console.WriteLine("   (Completed 50 eternal goals)");
            }

            if (simpleCompleted >= 10 && !_achievements.Contains("Goal Crusher"))
            {
                _achievements.Add("Goal Crusher");
                Console.WriteLine("\n🏆 NEW ACHIEVEMENT UNLOCKED: Goal Crusher!");
                Console.WriteLine("   (Completed 10 simple goals)");
            }

            if (checklistCompleted >= 5 && !_achievements.Contains("Completionist"))
            {
                _achievements.Add("Completionist");
                Console.WriteLine("\n🏆 NEW ACHIEVEMENT UNLOCKED: Completionist!");
                Console.WriteLine("   (Completed 5 checklist goals)");
            }
        }

        static void ViewAchievements()
        {
            Console.WriteLine("🏆 YOUR ACHIEVEMENTS 🏆");
            Console.WriteLine("========================");
            
            if (_achievements.Count == 0)
            {
                Console.WriteLine("No achievements yet. Keep working on your goals!");
            }
            else
            {
                foreach (string achievement in _achievements)
                {
                    Console.WriteLine($"✓ {achievement}");
                }
            }
            
            Console.WriteLine("\nAvailable Achievements:");
            Console.WriteLine("  - Scripture Warrior (Complete 50 eternal goals)");
            Console.WriteLine("  - Goal Crusher (Complete 10 simple goals)");
            Console.WriteLine("  - Completionist (Complete 5 checklist goals)");
        }
    }
}