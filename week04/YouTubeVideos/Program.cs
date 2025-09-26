using System;
using System.Collections.Generic;

class Comment
{
    public string CommenterName { get; set; }
    public string Text { get; set; }

    public Comment(string commenterName, string text)
    {
        CommenterName = commenterName;
        Text = text;
    }
}

class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int LengthSeconds { get; set; }
    private List<Comment> comments = new List<Comment>();

    public Video(string title, string author, int lengthSeconds)
    {
        Title = title;
        Author = author;
        LengthSeconds = lengthSeconds;
    }

    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }

    public int GetCommentCount()
    {
        return comments.Count;
    }

    public void DisplayVideoInfo()
    {
        Console.WriteLine($"Title: {Title}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Length: {LengthSeconds} seconds");
        Console.WriteLine($"Number of Comments: {GetCommentCount()}");
        Console.WriteLine("Comments:");
        foreach (Comment c in comments)
        {
            Console.WriteLine($"   {c.CommenterName}: {c.Text}");
        }
        Console.WriteLine(new string('-', 40));
    }
}

class Program
{
    static void Main()
    {
        Video v1 = new Video("Learning C#", "Alice Johnson", 600);
        v1.AddComment(new Comment("Tom", "This was super helpful, thanks!"));
        v1.AddComment(new Comment("Maria", "Clear explanation, I get it now."));
        v1.AddComment(new Comment("Hris345$", "can u do one about linq"));

        Video v2 = new Video("Fishing in Alaska", "Bob Smith", 1200);
        v2.AddComment(new Comment("Ella", "Wow, that scenery is amazing!"));
        v2.AddComment(new Comment("kofee", "boringgg"));
        v2.AddComment(new Comment("Grace", "Nice rod, what brand is it?"));

        Video v3 = new Video("Cooking Pasta", "Chef Mario", 900);
        v3.AddComment(new Comment("pyth452", "L cooking u cracked the pasta in half"));
        v3.AddComment(new Comment("Sophia", "Perfect for dinner tonight."));
        v3.AddComment(new Comment("James R", "Can you make a gluten-free version?"));

        List<Video> videos = new List<Video> { v1, v2, v3 };

        foreach (Video v in videos)
        {
            v.DisplayVideoInfo();
        }
    }
}
