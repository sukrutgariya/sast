// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;
using Codeql.Security;

Console.WriteLine("Hello, World!");

const string s = "Hello, World! this is another comments";  
Console.WriteLine(s);

// CodeQL Security Alert: SQL Injection vulnerability
// This demonstrates unsafe query construction with user input
string userId = Console.ReadLine();
string connectionString = "Server=localhost;Database=TestDB;";

using (SqlConnection connection = new SqlConnection(connectionString))
{
    // VULNERABLE: SQL injection - using string concatenation with user input
    string query = "SELECT * FROM Users WHERE UserId = " + userId;
    SqlCommand command = new SqlCommand(query, connection);
    
    try
    {
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Console.WriteLine(reader["UserId"]);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error: " + ex.Message);
    }
}

// Using DataProcessor class with security vulnerabilities
Console.WriteLine("\n--- Testing DataProcessor ---");

string userCommand = Console.ReadLine();
if (!string.IsNullOrEmpty(userCommand))
{
    // VULNERABLE: Command injection through user input
    DataProcessor.ProcessUserCommand(userCommand);
}

string userSearchInput = Console.ReadLine();
if (!string.IsNullOrEmpty(userSearchInput))
{
    // VULNERABLE: XPath injection through user input
    string result = DataProcessor.GetUserDataFromXPath(userSearchInput);
    Console.WriteLine("Result: " + result);
}