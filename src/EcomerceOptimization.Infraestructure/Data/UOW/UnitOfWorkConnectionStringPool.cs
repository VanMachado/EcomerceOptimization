namespace EcomerceOptimization.Infraestructure.Data.UOW
{
    public static class UnitOfWorkConnectionStringPool
    {
        private static readonly Dictionary<string, UnitOfWorkConnectionStringPoolParameters> dbConnectionString = new Dictionary<string, UnitOfWorkConnectionStringPoolParameters>();

        public static void SetConnectionString(string name, string connectionString, int commandTimeout)
        {
            if (!dbConnectionString.ContainsKey(name)) 
            {
                dbConnectionString.Add(name, new UnitOfWorkConnectionStringPoolParameters()
                {
                    ConnectionString = connectionString,
                    CommnadTimeout = commandTimeout
                });
            }
        }

        public static string GetConnectionString(string name)
        {
            var param = dbConnectionString[name];
            return param.ConnectionString;
        }

        public static int GetConnectionTimeout(string name)
        {
            var param = dbConnectionString[name];
            return param.CommnadTimeout;
        }

        internal struct UnitOfWorkConnectionStringPoolParameters
        {
            public string ConnectionString { get; set; }
            public int CommnadTimeout { get; set; }
        }
    }
}
