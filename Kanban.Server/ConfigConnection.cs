namespace Kanban.Server
{
    public static class ConfigConnection
    {
        private const string PATH = "Server.config";
        
        public static string ConnectionLineToDataBase
        {
            get => ReadToFileAsync();
            set => SaveToFileAsync(value);
        }

        private static async void SaveToFileAsync(string text)
        {
            using (StreamWriter writer = new StreamWriter(PATH, false))
            {
                await writer.WriteLineAsync(text);
            }
        }

        private static string ReadToFileAsync()
        {
            using (StreamReader reader = new StreamReader(PATH))
            {
                return reader.ReadLine();
            }
        }
    }
}
