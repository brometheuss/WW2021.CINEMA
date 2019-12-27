# Prerequisites

To follow this workshop, it's assumed that you have at least a basic knowledge of C# language and version control with Git.

In order to prepare for the workshop, there are several tools that need to be installed. Follow the links below to prepare your machine.

## Required Tools

1. Visual Studio 2019 Community - https://visualstudio.microsoft.com/vs/
2. Visual Studio Code - https://code.visualstudio.com/
3. Microsoft SQL Server 2019 Developer - https://www.microsoft.com/en-us/sql-server/sql-server-downloads
4. Postman - https://www.getpostman.com/downloads/

optional - Azure Data Studio instead of SQL Server Management Strudio https://docs.microsoft.com/en-us/sql/azure-data-studio/download?view=sql-server-ver15

## Git Basics

### What is Git?

Git is a free and open source version control system. 
Unlike older centralized version control systems such as SVN and CVS, Git is distributed: every developer has the full history of their code repository locally. 
This makes the initial clone of the repository slower, but subsequent operations such as commit, blame, diff, merge, and log dramatically faster. 
Git also has excellent support for branching, merging, and rewriting repository history, which has lead to many innovative and powerful workflows and tools. 
Pull requests are one such popular tool that allow teams to collaborate on Git branches and efficiently review each others code. 
Git is the most widely used version control system in the world today and is considered the modern standard for software development.

### What are Git Branches?

Branches are used to develop features isolated from each other. 
The master branch is the "default" branch when you create a repository. 
Use other branches for development and merge them back to the master branch upon completion. 

### How git works?

Here is a basic overview of how Git works:

*   Create a "repository" (project) with a git hosting tool (like Bitbucket)
*   Copy (or clone) the repository to your local machine
*   Add a file to your local repo and "commit" (save) the changes
*   Create a "branch" (version), make a change, commit the change
*   Make a change to your file with a git hosting tool and commit
*   "Pull" the changes to your local machine
*   "Push" your changes to your feature branch
*   Open a "pull request" (propose changes to the master branch)
*   "Merge" your branch to the master branch

### Usefull link
**Using Git with Visual Studio** - https://www.youtube.com/watch?v=jUiuIAZt6Dw
