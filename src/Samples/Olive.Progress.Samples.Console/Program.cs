// See https://aka.ms/new-console-template for more information

using Olive.Progress;
using Olive.Progress.Samples;
using Olive.Progress.Samples.Console;

Console.WriteLine("Hello, World!");

var watcher = new ProgressWatcher();
watcher.HandleForConsole();

await TestCases.RunSimpleTask(watcher);


var cgildWatcher = new ProgressWatcher();
cgildWatcher.HandleForConsole();

await TestCases.RunTaskWithChildren(cgildWatcher);

var staticChildren = new ProgressWatcher();
staticChildren.HandleForConsole();

await TestCases.RunTaskWithStaticChildren(staticChildren);