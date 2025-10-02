using System;

class Program
{
    static void Main(string[] args)
    {
        Assignment a1 = new Assignment("Mika Hunnigan", "Addition");
        Console.WriteLine(a1.GetSummary());

        MathAssignment a2 = new MathAssignment("Lani Simons", "Multiplication", "7 x 45", "3^2 x 9");
        Console.WriteLine(a2.GetSummary());
        Console.WriteLine(a2.GetHomeworkList());

        WritingAssignment a3 = new WritingAssignment("Nicholas Williams", "The Rise and Fall of Rome");
        Console.WriteLine(a3.GetSummary());
        Console.WriteLine(a3.GetWritingInformation());
    }
}  

