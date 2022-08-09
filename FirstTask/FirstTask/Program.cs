namespace FirstTask
{
    class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory("./../../../../");
            CommandService commandService = new CommandService();
            commandService.ProcessCommand();
        }
    }

}
