using OpenAI.Chat;

namespace AISQLTutor
{    
    public static class PromptBuilder
    {
        /// <summary>
        /// Build the system message that defines the tutor’s role and behavior.
        /// </summary>
        public static SystemChatMessage BuildSystemMessage()
        {
            return new SystemChatMessage(
                "You are a SQL Server expert tutor. " +
                "Explain concepts clearly, step by step, using simple language. " +
                "Whenever possible, include practical examples in T-SQL. " +
                "Keep answers concise but complete, like teaching a student."
            );
        }
    }
}
