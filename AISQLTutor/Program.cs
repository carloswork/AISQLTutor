namespace AISQLTutor
{
    internal class Program
    {
        static async Task Main()
        {
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            TutorBot tutor;

            Console.WriteLine("Welcome to SQL Tutor");
            Console.WriteLine("Type 'new' for a fresh session or 'load <filename>' to continue a saved one.");
            Console.Write("> ");

            var startCmd = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(startCmd) && startCmd.StartsWith("load"))
            {
                var fileName = startCmd["load".Length..].Trim();
                if (File.Exists(fileName))
                {
                    var history = HistorySaver.LoadFromFile(fileName);
                    tutor = new TutorBot(apiKey, history);
                    Console.WriteLine($"Loaded conversation from {fileName}");
                }
                else
                {
                    tutor = new TutorBot(apiKey);
                    Console.WriteLine($"File {fileName} not found. Starting a new session.");
                }
            }
            else
            {
                tutor = new TutorBot(apiKey);
                Console.WriteLine("New session started.");
            }

            Console.WriteLine("Type 'help' for commands, 'exit' to quit.\n");

            while (true)
            {
                Console.Write("You: ");
                var question = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(question) || question.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;

                await tutor.AskQuestionAsync(question);
            }

            // Save conversation when quitting
            var saveFileName = $"SqlTutor_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
            HistorySaver.SaveToFile(tutor.GetHistory(), saveFileName);
            Console.WriteLine($"Conversation saved to {saveFileName}");
        }
    }
}
