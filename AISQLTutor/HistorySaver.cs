using OpenAI.Chat;

namespace AISQLTutor
{
    /// <summary>
    /// Handles saving and loading chat history to and from text files.
    /// </summary>
    public static class HistorySaver
    {
        /// <summary>
        /// Save chat history to a text file.
        /// </summary>
        /// <param name="history"></param>
        /// <param name="filePath"></param>
        public static void SaveToFile(List<ChatMessage> history, string filePath)
        {
            using var writer = new StreamWriter(filePath);

            foreach (var message in history)
            {
                switch(message)
                {
                    case SystemChatMessage sysMsg:
                        writer.WriteLine("[System] " + sysMsg.Content[0].Text);
                        break;
                    case UserChatMessage userMsg:
                        writer.WriteLine("[User] " + userMsg.Content[0].Text);
                        break;
                    case AssistantChatMessage assistantMsg:
                        writer.WriteLine("[Assistant] " + assistantMsg.Content[0].Text);
                        break;
                }
            }
        }

        /// <summary>
        /// Load chat history from a text file.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<ChatMessage> LoadFromFile(string filePath)
        {
            var history = new List<ChatMessage>();
            string? role = null;
            var buffer = new List<string>();

            foreach (var line in File.ReadLines(filePath))
            {
                if (line.StartsWith("[System]") || line.StartsWith("[User]") || line.StartsWith("[Assistant]"))
                {
                    // Save previous message if exists
                    if (role != null && buffer.Count > 0)
                    {
                        var content = string.Join(Environment.NewLine, buffer);
                        history.Add(CreateMessage(role, content));
                        buffer.Clear();
                    }
                    role = line.Substring(0, line.IndexOf(']') + 1);
                    buffer.Add(line[(line.IndexOf(']') + 1)..].Trim());
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(line) || buffer.Count > 0)
                        buffer.Add(line);
                }
            }

            // Save last buffered message
            if (role != null && buffer.Count > 0)
            {
                var content = string.Join(Environment.NewLine, buffer);
                history.Add(CreateMessage(role, content));
            }

            return history;
        }

        private static ChatMessage CreateMessage(string role, string content)
        {
            return role switch
            {
                "[System]" => new SystemChatMessage(content),
                "[User]" => new UserChatMessage(content),
                "[Assistant]" => new AssistantChatMessage(content),
                _ => throw new InvalidOperationException($"Unknown role: {role}")
            };
        }
    }
}
