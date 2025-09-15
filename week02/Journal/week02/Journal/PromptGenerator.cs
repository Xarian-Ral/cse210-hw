using System;
using System.Collections.Generic;

public class PromptGenerator
{
    private List<string> _prompts = new List<string>
    {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?",
        "What was a situation or event that happened today that could have allowed me to exercise the Spirit?"
    };

    private Random _rng = new Random();

    public string GetRandomPrompt()
    {
        int index = _rng.Next(_prompts.Count);
        return _prompts[index];
    }
}
