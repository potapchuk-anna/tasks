namespace FirstTask
{
    class Program
    {
        public static void Main(string[] args)
        {
            Directory.SetCurrentDirectory("./../../../../");
            DataGetter dataGetter = new DataGetter(new TxtReader());
            dataGetter.Process();
        }
    }

}
