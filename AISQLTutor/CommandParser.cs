namespace AISQLTutor
{
    /// <summary>
    /// Parses user input commands for the SQL tutor application.
    /// </summary>
    public static class CommandParser
    {
        /// <summary>
        /// Parses the input string and returns the corresponding command and topic.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static (TutorCommand command, string topic) Parse(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (TutorCommand.None, string.Empty);

            var parts = input.Trim().Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0) 
                return (TutorCommand.None, string.Empty);

            return parts[0].ToLower() switch
            {
                "explain" => (TutorCommand.Explain, parts.Length > 1 ? parts[1] : string.Empty),
                "example" => (TutorCommand.Example, parts.Length > 1 ? parts[1] : string.Empty),
                "quiz" => (TutorCommand.Quiz, parts.Length > 1 ? parts[1] : string.Empty),
                "help" => (TutorCommand.Help, string.Empty),
                _ => (TutorCommand.None, input)
            };
        }
    }
}
