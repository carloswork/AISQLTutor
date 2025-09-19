# 🎓 TutorBot – SQL Server AI Assistant

TutorBot is a **C# console application** that uses the **OpenAI API** to act as a SQL Server tutor.  
It can explain concepts, provide T-SQL examples, quiz you, and save/reload study sessions.  

---

## ✨ Features
- 🧠 **Explain concepts**: `explain clustered index`  
- 💻 **Show examples**: `example join`  
- 🎯 **Quiz mode**: `quiz indexes`  
- 📝 **Help menu**: `help`  
- 💾 **Save & reload sessions**: Automatically saves when you exit, and you can reload later.  
- 📊 **Token usage & cost tracking** per request.  

---

## 🚀 Getting Started

### 1. Clone the repo
```bash
git clone https://github.com/your-username/TutorBot.git
cd TutorBot
```

### 2. Install dependencies
The project uses the **official OpenAI .NET SDK**:  
```powershell
dotnet add package OpenAI
```

### 3. Set your API key
Add your OpenAI API key to an environment variable (⚠️ do not hardcode it).

**Windows (PowerShell):**
```powershell
setx OPENAI_API_KEY "your_api_key_here"
```

**macOS/Linux (bash/zsh):**
```bash
export OPENAI_API_KEY="your_api_key_here"
```

Restart your IDE/terminal after setting the variable.

### 4. Run the app
```powershell
dotnet run
```

---

## 💡 Usage Examples

```
🎓 Welcome to SQL Tutor (type 'help' for commands, 'exit' to quit)

You: help
📝 Available commands:
  explain <topic>   → Explain a SQL Server concept
  example <topic>   → Show a practical T-SQL example
  quiz <topic>      → Ask me a quiz question
  help              → Show this help message
  exit              → Quit the tutor

You: explain clustered index
Assistant: A clustered index determines the physical order of rows...

You: example join
Assistant: Sure! Here’s a T-SQL example:
SELECT e.Name, d.DepartmentName
FROM Employees e
JOIN Departments d ON e.DeptId = d.Id;
```

---

## 📂 Saved Sessions
- When you type `exit`, the conversation is saved automatically to a file:  
  ```
  SqlTutor_20250918_163055.txt
  ```
- You can reload it later:
  ```
  load SqlTutor_20250918_163055.txt
  ```

---

## ⚠️ Security Notes
- Keep your API key **safe** – never commit it to GitHub.  
- Costs are usually very small (`gpt-4o-mini` is cheap), but token usage per request is displayed for transparency.  

---

## 📜 License
This project is licensed under the [MIT License](LICENSE).

---

## 🛡️ .gitignore (recommended)

If you don’t already have one, create a `.gitignore` file in the project root with the following:

```
# Build artifacts
bin/
obj/

# User-specific files
.vs/
*.user
*.suo

# Logs
*.log

# Secrets
.env
```

This prevents API keys, local configs, and build files from being uploaded to GitHub.
