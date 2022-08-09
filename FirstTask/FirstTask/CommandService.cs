using FirstTask.HelpFileManager;
using FirstTask.Reader;
using System.Configuration;

namespace FirstTask
{
    internal class CommandService
    {
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        public void ProcessCommand()
        {
            Console.WriteLine("Welcome.\n" +
                "You should write command start in order to start program.\n" +
                "If you want to see list of commands you should write 'help'.\n");
            string command = "";
            while (command != "exit")
            {
                Console.Write("Enter command: ");
                command = Console.ReadLine();
                switch (command)
                {
                    case "start":
                        Start();
                        break;
                    case "reset":
                        if (!cancellationToken.IsCancellationRequested)
                            Reset();
                        else
                            Console.WriteLine("You should start before reset.");
                        break;
                    case "stop":
                        Stop();
                        break;
                    case "help":
                        Help();
                        break;
                    case "exit":
                        break;
                    default:
                        Console.WriteLine("Such command does not exist.");
                        break;
                }
            }

        }

        private void Help()
        {
            Console.WriteLine("**********************");
            Console.WriteLine("start - starts processing files.");
            Console.WriteLine("reset - interrupts processing files and then starts again.");
            Console.WriteLine("stop - stops processing files.");
            Console.WriteLine("help - shows list of commands.");
            Console.WriteLine("exit - exits the program");
            Console.WriteLine("**********************");
        }

        private void Start()
        {
            DateTime date = DateTime.Now;
            MetaLogFileManager metaLogFileManager = new MetaLogFileManager();
            cancellationToken = new CancellationTokenSource();
            if (ConfigurationSettings.AppSettings["input"] is not null
            && ConfigurationSettings.AppSettings["output"] is not null)
                Task.Run(() =>
                {
                    try
                    {
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            List<Task> tasks = new List<Task>();
                            List<string> filePathes = PathGetter.GetPathes();
                            foreach (string filePath in filePathes)
                            {
                                if (!IsFileLocked(filePath))
                                    tasks.Add(Task.Run(() =>
                                    {
                                        FileReader fileReader = filePath.EndsWith(".csv") ? new CsvReader(filePath) : new TxtReader(filePath);
                                        DataProcessor.Process(fileReader);
                                        metaLogFileManager.ChangeMetaLogData(fileReader);

                                    }, cancellationToken.Token));
                            }
                            if (MetaLogFileManager.HasDateChanged(date))
                            {
                                WriteMetaLog(metaLogFileManager);                                
                                TempFileManager.DeleteTemp(PathGetter.GetOutputDirectory(DateTime.Now.Date.AddDays(-1).ToShortDateString()) + "/.temp");
                                date = DateTime.Now;
                            }
                            Task.WaitAll(tasks.ToArray());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                });
            else
                Console.WriteLine("You should write input and output derictory in config first and run program again.");
        }
        private void Reset()
        {
            Stop();
            Start();
        }
        private void Stop()
        {
            cancellationToken.Cancel();
        }
        protected bool IsFileLocked(string path)
        {
            try
            {
                using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }
            return false;
        }
        private void WriteMetaLog(MetaLogFileManager metaLogFile)
        {
            if (!metaLogFile.MetaLogExist())
                metaLogFile.CreateMetaLog();
            metaLogFile.WriteMetaLog();
        }     
    }
}
