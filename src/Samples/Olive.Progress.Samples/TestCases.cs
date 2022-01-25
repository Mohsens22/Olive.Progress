namespace Olive.Progress.Samples
{
    public static class TestCases
    {
        public static async Task RunSimpleTask(ProgressWatcher pbar)
        {
            var maxticks = 1000;
            var interval = 10;

            pbar.Register(maxticks,"Simple Test");

            for (int i = 0; i < maxticks; i++)
            {

                if (i%500==0)
                {
                    pbar.WriteLine("I'm at "+i);
                }

                if (i %200==0)
                {
                    pbar.LogError("Error happened");
                }
                pbar.Tick();
                await Task.Delay(interval);
            }
        }

        public static async Task RunTaskWithChildren(ProgressWatcher pbar)
        {
            var maxticks = 8;
            var maxticksPerChild = 200;
            var interval = 10;

            pbar.Register(maxticks, "Simple Test");

            for (int i = 0; i < maxticks; i++)
            {
                if (i==5)
                {
                    pbar.WriteLine("Write 5 from the main bar");
                }
                if (i == 7)
                {
                    pbar.LogInformation("Log 7 from the main bar");
                }
                var childBar = pbar.Spawn(maxticksPerChild,"Child "+i);
                for (int j = 0; j < maxticksPerChild; j++)
                {
                    if (j==0)
                    {
                        childBar.WriteLine("Write 0 from the child bar: "+j);
                    }
                    if (j == 6)
                    {
                        childBar.LogInformation("Log 6 from the child bar: " + j);
                    }
                    childBar.Tick();
                    await Task.Delay(interval);
                }
                pbar.Tick();
            }

            
        }
        public static async Task RunTaskWithStaticChildren(ProgressWatcher pbar)
        {
            var maxticks = 10;
            var maxticksPerChild = 100;
            var interval = 10;

            pbar.Register(maxticks, "Simple Test",false);

            for (int i = 0; i < maxticks; i++)
            {
                if (i == 5)
                {
                    pbar.WriteLine("Write 5 from the main bar");
                }
                if (i == 7)
                {
                    pbar.LogInformation("Log 7 from the main bar");
                }
                var childBar = pbar.Spawn(maxticksPerChild, "Child " + i);
                for (int j = 0; j < maxticksPerChild; j++)
                {
                    if (j == 0)
                    {
                        childBar.WriteLine("Write 0 from the child bar: " + j);
                    }
                    if (j == 6)
                    {
                        childBar.LogInformation("Log 6 from the child bar: " + j);
                    }
                    childBar.Tick();
                    await Task.Delay(interval);
                }
                pbar.Tick();
            }
        }
    }
}