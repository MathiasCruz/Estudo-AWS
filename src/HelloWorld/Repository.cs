namespace HelloWorld
{
    public class Repository : IRepository
    {
        public void GetTableInformation()
        {
            var connection = new Connection();
            connection.GetInformation("RestricaoCVM");
        }
    }
}
