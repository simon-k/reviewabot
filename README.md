![Banner](docs/images/banner-with-text.png)

# Reviewabot
A friend that can help you review PRs when there are no one else to help you.

## Limitations
This is a very simple bot that uses the OpenAI GPT40 model to generate a review message for a PR. 

Reviewabot only knows about the PR diff. It does have the entire repository as a context like GitHub Copilot might have. 

There might be better solutions out there. And I am sure that GitHub is working on a CodeReview Bot that will be able to do this better.

## How to use this tool
### 1. Create a user account for the reviewer/bot and add it as a collaborator
Go to Github and create a new user account that will be used to review the PRs. Add the new user as a collaborator to your repository.

### 2. Create a Personal Access Token
Under settings for the new user account, create a new Personal Access Token that has access to source and read/write to Pull Requests.

### 3. Create an OpenAI API key OR an Azure OpenAI API key
Either:

- Go to OpenAI and create an API key.
- Go to the Azure Portal and create an OpenAI resource and deploy the gpt-4o model. Get the API key and the endpoint.

### 4. Create a GitHub Action in your repository
Create a new GitHub Action in your repository that runs the Reviewabot. Use the action from the Marketplace.

https://github.com/marketplace/actions/reviewabot

### 5. Create a PR and assign your reviewer to it
Make sure the GitHub action is triggered in a workflow. See the description in [Reviewabot Marketplace](https://github.com/marketplace/actions/reviewabot).

Reviewabot will always approve your PR.

## How to run this locally
Download the source and run the following command in the root of the project:

```bash
dotnet run --owner={repo_owner} --repo={repository} --prnumber={pr-number} --githubpat={github-pat} --openaikey={apenai-api-key}
```

Here is an example where we review PR #4 in the GitHub repository `https://github.com/simon-k/hello-api/`.
The GitHub user that does the review is represented by it's PAT `YYY`. We use the OpenAI API key `XXX` to generate the review with GPT4o. 

```bash
dotnet run --owner=simon-k --repo=hello-api --prnumber=4 --githubpat=thisisadummypat --openaikey=thisisadummykey
```

And the same example with Azure OpenAI:

```bash
dotnet run --Owner=simon-k --Repo=hello-api --PrNumber=4 --GitHubPat=YYY --OpenAiKey=XXX --AzureOpenAiKey=thisisadummykey --AzureOpenAiEndpoint=https://yourresourcename.openai.azure.com
```

### Command line arguments
| Argument            | Description                                                            | Required |
|---------------------|------------------------------------------------------------------------|----------|
| Owner               | The owner of the repository                                            | Yes      |
| Repo                | The name of the repository                                             | Yes      |
| PrNumber            | The PR number                                                          | Yes      |
| GitHubPat           | The Personal Access Token for the GitHub Account that makes the review | Yes      |
| OpenAiKey           | The key for the OpenAI API                                             | No       |
| AzureOpenAiKey      | The key for the Azure OpenAI API                                       | No       |
| AzureOpenAiEndpoint | The endpoint for the Azure OpenAI API                                  | No       |

You must provide either an OpenAI key or an Azure OpenAI key together with an Azure OpenAI endpoint.

## How to contribute
Contact me. I would like your input. Or just create a PR with your improvements.

## How to release
Create a new release in GitHub. The GitHub Release Action generate the binaries and attach them to the release.

## TODO
* Make instructions configurable from a markdown file
* Use Spectre Console to make things nice to look at
* Use a better tool for parsing command line arguments
* Create some tests
* Add OpenSSF Scorecard

## License
See the [LICENSE](LICENSE) file for license rights and limitations (MIT).

If you use this for anything cool, please let me know. I would love to hear about it 🫶.
