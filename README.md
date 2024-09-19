# Reviewabot
A friend that can help you review PRs when there are no one else to help you.

## Limitations
This is a very simple bot that uses the OpenAI API to generate a review message for a PR. 
It is not very smart and will not be able to provide a lot of help. 
It is more of a proof of concept than a real tool.

The reason is that the tool does not have the entire context of the repository like GitHub copilot would have.

So there might be better solutions out there. And I am sure that GitHub is working on a CodeReview Borg that will be able to do this better.

## How to run this
Download the source and run the following command in the root of the project:

```bash
dotnet run --owner=simon-k --repo=hello-api --prnumber=4 --openapikey=redacted --githubpat=redacted
```

Or download the released executable and run it with the same arguments.

### Command line arguments
| Argument | Description | Required |
| --- | --- | --- |
| owner | The owner of the repository | Yes |
| repo | The name of the repository | Yes |
| prnumber | The number of the PR | Yes |
| openapikey | The API key for the OpenAI API | Yes |
| githubpat | The Personal Access Token for the GitHub API | Yes |

## TODO
* Use a better tool for parsing command line arguments
* Right now the HttpClient ignores certificates. Make that configurable.
* Make instructions configurable form a markdown file
* Use Spectre Console to make things nice to look at
* Create a GitHub task template

