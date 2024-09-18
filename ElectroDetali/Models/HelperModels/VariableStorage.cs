namespace ElectroDetali.Models.HelperModels
{
    public static class VariablesStorage
    {
        private static IConfiguration? Configuration { get; set; } = null;

        static VariablesStorage()
        {
            if (Configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Environment.CurrentDirectory)
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                Configuration = builder.Build();
            }
        }

        /// <summary>
        /// Попытка получить переменную из среды, если не найдено, тогда из конфига
        /// </summary>
        public static string? GetVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name) ?? Configuration?.GetSection("AppConfig")[name] ?? throw new Exception($"Can't find config value for {name}");
        }
    }
}
