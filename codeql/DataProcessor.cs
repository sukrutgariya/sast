using System.Diagnostics;

namespace Codeql.Security
{
    public class DataProcessor
    {
        // VULNERABLE: Command Injection vulnerability
        // User input is directly concatenated into system command
        public static void ProcessUserCommand(string userInput)
        {
            try
            {
                // VULNERABLE: Command injection - user input directly used in system command
                string command = "cmd.exe /c echo " + userInput;
                
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c " + userInput,  // VULNERABLE: Direct concatenation with user input
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                
                using (Process process = Process.Start(psi))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    Console.WriteLine("Command Output: " + output);
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Process Error: " + ex.Message);
            }
        }

        // VULNERABLE: XPath Injection vulnerability
        public static string GetUserDataFromXPath(string username)
        {
            try
            {
                // VULNERABLE: XPath injection - unsanitized user input in XPath query
                string xpathQuery = "//User[Username='" + username + "']/Email";
                
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml("<Users><User><Username>admin</Username><Email>admin@example.com</Email></User></Users>");
                
                var nodes = doc.SelectNodes(xpathQuery);  // VULNERABLE: XPath injection
                
                if (nodes.Count > 0)
                {
                    return nodes[0].InnerText;
                }
                
                return "User not found";
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
