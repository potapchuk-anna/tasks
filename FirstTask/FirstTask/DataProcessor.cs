using FirstTask.Models;
using FirstTask.Reader;
using FirstTask.Writer;
namespace FirstTask
{
    internal class DataProcessor
    {
        public static void Process(FileReader fileReader)
        {  
            try
            {
                List<Model> models = fileReader.ReadData();
                List<TransformedData> datas = DataTransformer.DataTransformer.Transform(models);
                string output = PathGetter.GetOutputPath();
                FileWriter.WriteOutputFile(datas, output);
            }
            catch
            {
                fileReader.Close();
            }
            finally
            {                
                fileReader.DeleteFile();
            }
                        
        }
    }
}
