![Banner](docs/images/banner.png)

# Reviewabot
A friend that can help you review PRs when there are no one else to help you.

## Limitations
This is a very simple bot that uses the OpenAI API to generate a review message for a PR. 
It is not very smart and will not be able to provide a lot of help. 
It is more of a proof of concept than a real tool.

The reason is that the tool does not have the entire context of the repository like GitHub copilot would have.

So there might be better solutions out there. And I am sure that GitHub is working on a CodeReview Bot that will be able to do this better.

## How to use this too
### 1. Create a user account for the reviewer/bot
Go to Github and create a new user account that will be used to review the PRs.
### 2. Create a Personal Access Token
Under settings for the new user account, create a new Personal Access Token that has access to source and read/write to Pull Requests.
### 3. Create an OpenAI API key
Go to OpenAI and create an API key.
### 4. Create a GitHub Action in your repository
Create a new GitHub Action in your repository that runs the Reviewabot.

Action example
```yaml
# INSTRUCTIONS
# This action will run whenever a reviewer is assigned and the reviewer name matches the value of the repository variable.
# The action will download Reviewabot and run, and as a result your assigned reviewer will generate a review based on GPT.
#
# Create the following GitHub action secrets in your repository settings:
#   OPENAPI_KEY -  Contains your OpenAI API Key
#   REVIEWER_GITHUB_PAT - Contains your reviewers GitHub PAT woth permissions to read content and write Pull Requests.
#
# Create the following GitHub action variable in your repository settings:
#   REQUESTED_REVIEWER_NAME - Your reviewers GitHub username 

name: Review PR

on:
  pull_request_target:
    types: [review_requested]
    
env:
  REQUESTED_REVIEWER_NAME: ${{ github.event.requested_reviewer.login}}
  REVIEWER_NAME: ${{ vars.REVIEWER_NAME }}

jobs:
  review:
    if: ${{ github.event.requested_reviewer.login == vars.REVIEWER_NAME }}
    runs-on: ubuntu-latest
    steps:
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: 8.0.x

    - name: Download Reviewabot Latest Release
      uses: robinraju/release-downloader@v1
      with:
        repository: 'simon-k/reviewabot'
        latest: true
        fileName: 'Reviewabot.Console-linux-x64'
        out-file-path: 'Reviewabot'

    - name: Make Reviewabot executable
      run: chmod +x $GITHUB_WORKSPACE/Reviewabot/Reviewabot.Console-linux-x64

    - name: Review PR
      run: |
        $GITHUB_WORKSPACE/Reviewabot/Reviewabot.Console-linux-x64 \
          --owner=${{github.repository_owner}} \
          --repo=${{github.event.repository.name}} \
          --prnumber=${{ github.event.number }} \
          --openapikey=${{secrets.OPENAPI_KEY}} \
          --githubpat=${{secrets.REVIEWER_GITHUB_PAT}}
```

### 5. Create a PR and assign your reviewer to it
Create a PR and assign the reviewer to it. The reviewer will then generate a review message for the PR.
The reviewer will always approve your PR.

### Debugging the pipeline
_The pipeline is skipped_
This is most likeley because the repository action variable `REVIEWER_NAME` has the wrong value. 
It should be the name of the user that reviews the code. It is the user that you created in step 1.

## How to run this locally
Download the source and run the following command in the root of the project:

```bash
dotnet run --owner={repo_owner} --repo={repository} --prnumber={pr-number} --openapikey={apenai-api-key} --githubpat={github-pat}
```

Here is an example where we review PR #4 in the GitHub repository `https://github.com/simon-k/hello-api/`.
The GitHub user that does the review is represented by it's PAT `YYY`. We use the OpenAI API key `XXX` to generate the review with GPT4o. 

```bash
dotnet run --owner=simon-k --repo=hello-api --prnumber=4 --openapikey=XXX --githubpat=YYY
```

### Command line arguments
| Argument | Description                                                            | Required |
| --- |------------------------------------------------------------------------| --- |
| owner | The owner of the repository                                            | Yes |
| repo | The name of the repository                                             | Yes |
| prnumber | The PR number                                                 | Yes |
| openapikey | The key for the OpenAI API                                             | Yes |
| githubpat | The Personal Access Token for the GitHub Account that makes the review | Yes |

## How to contribute
Contact me. I would like your input. Or just create a PR with your improvements.

## How to release
Create a new release in GitHub. The GitHub Release Action generate the binaries and attach them to the release.

## TODO
* Use a better tool for parsing command line arguments
* Right now the HttpClient ignores certificates. Make that configurable.
* Make instructions configurable form a markdown file
* Use Spectre Console to make things nice to look at
* Create some tests
* Create a GitHub Action that runs the bot

## License
See the [LICENSE](LICENSE) file for license rights and limitations (MIT).

If you use this for anything cool, please let me know. I would love to hear about it 🫶.

![Logo](docs/images/reviewabot.png)
