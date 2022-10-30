# Olive.Progress

Olive.Progress will help you to track a long running process' progress. This abstract progress watcher which can be used in various scenarios (Console, desktop, mobile, web).

## How does it work

Olive.Progress provides an abstract progress in which you can track a progress of a task with it. Then, this progress bar can be binded to a UI to show the task's progress information.

Olive.Progress is platform agnostic and you can use it in any UI library. Also you can use this to render progress bar on Console applications. In addition to that you can send a progress report from server to a web client.

## How to use

 1. Reference Olive.Progress to your project
 2. Register a task
 3. Bind the progress bar to your UI

### Code Sample

#### To run the Console app
Open the solution > Samples > Olive.Progress.Sample.Console
Right click, set at startup project and run it

#### To run the Web app
Open the solution > Samples > Blazor > Olive.Progress.Sample.Api
Right click on the project > Debug > Run without debugging

Then Samples > Blazor > Olive.Progress.Sample.Blazor.Client
Right click, set at startup project and run it