using System.Configuration;

namespace FirstTask
{
    internal class CommandService
    {
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();
        public void ProcessCommand()
        {
            while (true)
            {
                string command = Console.ReadLine();
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
                    default:
                        throw new Exception("There is not such command.");
                }
            }

        }
        private void Start()
        {
            DateTime date = DateTime.Now;           
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
                                if(!IsFileLocked(filePath))
                                tasks.Add(Task.Run(() =>
                                {
                                    FileReader fileReader = filePath.EndsWith(".csv") ? new CsvReader(filePath) : new TxtReader(filePath);
                                    try
                                    {
                                        DataProcessor.Process(fileReader);                                     
                                    }
                                    catch
                                    {
                                        fileReader.Close();
                                    }
                                }, cancellationToken.Token));                              
                            }
                            if (MetaLogFileManager.HasDateChanged(date))
                            {
                                WriteMetaLog();
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
        }
        private void Reset()
        {
            Stop();
            Start();
        }
        private void Stop()
        {
            cancellationToken.Cancel();
            FileReader.MetaLogDataClear();
        }
        protected bool IsFileLocked(string path)
        {
            try
            {
                using (FileStream stream = File.Open(path,FileMode.Open, FileAccess.Read, FileShare.None))
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
        private void WriteMetaLog()
        {
            if (!MetaLogFileManager.MetaLogExist())
                MetaLogFileManager.CreateMetaLog();
            MetaLogFileManager.WriteMetaLog();
            FileReader.MetaLogDataClear();
        }
    }
}
