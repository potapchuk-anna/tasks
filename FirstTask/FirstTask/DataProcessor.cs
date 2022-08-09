namespace FirstTask
{
    internal class DataProcessor
    {
        public static void Process(FileReader fileReader)
        {         
            List<Model> models = fileReader.ReadData();
            List<TransformedData> datas = DataTransformer.Transform(models);
            string output = PathGetter.GetOutputPath();
            FileWriter.WriteOutputFile(datas, output);
            fileReader.Close();
            fileReader.DeleteFile();            
        }
    }
}
