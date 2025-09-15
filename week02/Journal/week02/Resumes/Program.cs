using System;


class Program
{
    static void Main(string[] args)
    {
        Job job1 = new Job();
        job1._title = "Software Engineer";
        job1._company = "The IT Goons";
        job1._startyear = 2017;
        job1._endyear = 2022;

        Job job2 = new Job();
        job2._title = "Manager";
        job2._company = "Weenie";
        job2._startyear = 2022;
        job2._endyear = 2023;

        Resume myResume = new Resume();
        myResume._name = "David Bazcuki";

        myResume._jobs.Add(job1);
        myResume._jobs.Add(job2);

        myResume.Display();
    }
}

    
   
