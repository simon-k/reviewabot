namespace Reviewabot.Console.GitHub.Models;

public class PullRequestFile
{
    /*{
           "sha": "2498e9e766c7267de5b906a14ecbe1d8c17ace81",
           "filename": "Program.cs",
           "status": "modified",
           "additions": 15,
           "deletions": 1,
           "changes": 16,
           "blob_url": "https://github.com/simon-k/hello-api/blob/dc6cf86afa3a5989cbfa8e224d0d81c9d9347c1e/Program.cs",
           "raw_url": "https://github.com/simon-k/hello-api/raw/dc6cf86afa3a5989cbfa8e224d0d81c9d9347c1e/Program.cs",
           "contents_url": "https://api.github.com/repos/simon-k/hello-api/contents/Program.cs?ref=dc6cf86afa3a5989cbfa8e224d0d81c9d9347c1e",
           "patch": "@@ -46,6 +46,9 @@\n     .WithName(\"HealthCheck\")\n     .WithOpenApi();\n \n+app.MapGet(\"/hello\", () => Results.Ok(SomeService.Execute()))\n+    .WithName(\"Hello\")\n+    .WithOpenApi();\n \n app.MapGet(\"/test\", async () =>\n     {\n@@ -87,7 +90,18 @@\n \n app.Run();\n \n+\n+\n+void DumbFunction()\n+{\n+    //TODO: This is a dumb and unused function. Remove it.\n+    var i = 10 + 13;\n+    var j = i + 2;\n+\n+    Console.WriteLine($\"i = {i}\");\n+}\n+\n record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)\n {\n     public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);\n-}\n+}\n\\ No newline at end of file"
       },*/
    public required string Sha { get; set; }
    public required string Filename { get; set; }
    public required string Status { get; set; }
    public required string Patch { get; set; }
}