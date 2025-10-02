public class MathAssignment : Assignment
{
    private string _textbookChapter;
    private string _problems;

    public MathAssignment(string studentName, string topic, string textbookChapter, string problems)
        : base(studentName, topic)
    {
        _textbookChapter = _textbookChapter;
        _problems = problems;
    }

    public string GetHomeworkList()
    {
        return $"Section {_textbookChapter} Problems {_problems}";
    }
}