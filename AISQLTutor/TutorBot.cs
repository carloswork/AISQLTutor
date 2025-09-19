using OpenAI;
using OpenAI.Chat;

namespace AISQLTutor
{
    /// <summary>
    /// Manages the chat interaction with the AI tutor.
    /// </summary>
    public class TutorBot
    {
        private readonly ChatClient _chatClient;
        private readonly List<ChatMessage> _history;

        // Pricing constants (gpt-4o-mini)
        private const decimal InputCostPer1M = 0.15m;
        private const decimal OutputCostPer1M = 0.60m;

        /// <summary>
        /// Get the current chat history.
        /// </summary>
        /// <returns></returns>
        public List<ChatMessage> GetHistory() => _history;

        /// <summary>
        /// Initialize the TutorBot with the given OpenAI API key.
        /// </summary>
        /// <param name="apiKey"></param>
        public TutorBot(string apiKey, List<ChatMessage>? existingHistory = null)
        {
            var client = new OpenAIClient(apiKey);
            _chatClient = client.GetChatClient("gpt-4o-mini");

            _history = existingHistory ?? 
                new List<ChatMessage>()
                {
                    PromptBuilder.BuildSystemMessage()
                };
        }

        /// <summary>
        /// Process a user question and get a response from the AI tutor.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task AskQuestionAsync(string input)
        {
            var (command, topic) = CommandParser.Parse(input);

            if (command == TutorCommand.Help)
            {
                ShowHelp();
                return;
            }

            string formattedInput = GetFormattedInput(command, topic, input);

            _history.Add(new UserChatMessage(formattedInput));

            var response = await _chatClient.CompleteChatAsync(_history);
            var reply = response.Value.Content[0].Text;

            Console.WriteLine($"Assistant: {reply}");
            Console.WriteLine();

            _history.Add(new AssistantChatMessage(reply));

            ShowUsage(response.Value.Usage);
        }

        /// <summary>
        /// Format the user input based on the command type.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="topic"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetFormattedInput(TutorCommand command, string topic, string input)
        {
            return command switch
            {
                TutorCommand.Explain => $"Explain the following SQL Server concept: {topic}",
                TutorCommand.Example => $"Provide a practical T-SQL example for: {topic}",
                TutorCommand.Quiz => $"Ask me a quiz question about: {topic}. Do not give the answer immediately.",
                _ => input
            };
        }

        /// <summary>
        /// Display token usage and estimated cost.
        /// </summary>
        /// <param name="usage"></param>
        private void ShowUsage(ChatTokenUsage usage)
        {
            var inputTokens = usage.InputTokenCount;
            var outputTokens = usage.OutputTokenCount;
            var totalTokens = usage.TotalTokenCount;

            decimal inputCost = (inputTokens / 1_000_000m) * InputCostPer1M;
            decimal outputCost = (outputTokens / 1_000_000m) * OutputCostPer1M;
            decimal totalCost = inputCost + outputCost;

            Console.WriteLine($"Tokens: {inputTokens} in, {outputTokens} out (total {totalTokens})");
            Console.WriteLine($"Cost: ${totalCost:F6}");
            Console.WriteLine();
        }

        /// <summary>
        /// Display available commands.
        /// </summary>
        private void ShowHelp()
        {
            Console.WriteLine("Available commands:");
            Console.WriteLine("  explain <topic>  - Explain a SQL Server concept");
            Console.WriteLine("  example <topic>  - Provide a T-SQL example");
            Console.WriteLine("  quiz <topic>     - Ask a quiz question on a topic");
            Console.WriteLine("  help             - Show this help message");
            Console.WriteLine("  exit             - Quit the tutor");
            Console.WriteLine();
        }
    }
}
