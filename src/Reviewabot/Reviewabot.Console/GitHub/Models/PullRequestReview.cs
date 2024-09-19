namespace Reviewabot.Console.GitHub.Models;

public class PullRequestReview
{
    public string Body { get; set; }
    public string Event { get; set; }
    //TODO: Support comments
    /*{
        "comments": [
          {
            "path": "Program.cs",
            "position": 9,
            "body": "Consider consolidating excessive line breaks in this file. TEST 1"
          }
        ]
    }*/
}