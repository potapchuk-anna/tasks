namespace FirstTask.Models
{
    internal class Service
    {
        public string Name { get; set; }
        public List<Payer> Payers { get; set; } = new List<Payer>();
        public decimal Total { get; set; }
    }
}