// See https://aka.ms/new-console-template for more information
using ExternalDriveDupeDeleter;
using Microsoft.Data.Sqlite;
using StackExchange.Redis;
using System;
using System.IO;


//Console.WriteLine("Hello, World!");

public class Program
{
    public static void Main(string[] args)
    {
        // --- IMPORTANT ---
        // Change this to the actual name of your external hard drive.
        const string myExternalDriveName = "BLUE 2TB TOSHIBA EXT";

        Console.WriteLine($"Searching for external drive named '{myExternalDriveName}'...");

        // 1. Get the drive letter by its name using the helper class.
        string foundDriveLetter = DriveFinder.GetDriveLetterByName(myExternalDriveName);

        if (!string.IsNullOrEmpty(foundDriveLetter))
        {
            Console.WriteLine($"SUCCESS: Found drive letter: {foundDriveLetter}");

            // 2. Now, do the reverse: get the name from the letter we just found.
            string foundDriveName = DriveFinder.GetDriveNameByLetter(foundDriveLetter);
            Console.WriteLine($"SUCCESS: Verified drive name: {foundDriveName}");
        }
        else
        {
            Console.WriteLine($"FAILURE: Could not find an external drive named '{myExternalDriveName}'.");
            Console.WriteLine("Please ensure the drive is connected and has the correct name.");
        }

        // --- NEW: Redis Logic ---
        Console.WriteLine("--- Testing Redis Database ---");

        // 1. Save a new user to Redis.
        string testUserId = "101";
        RedisManager.SaveUser(testUserId, "Alice", "alice@example.com");

        // 2. Retrieve that user's data from Redis.
        Console.WriteLine($"\nAttempting to retrieve user with ID '{testUserId}'...");
        Dictionary<string, string>? retrievedUser = RedisManager.GetUser(testUserId);

        if (retrievedUser != null)
        {
            Console.WriteLine("SUCCESS: Found user! Details:");
            foreach (var property in retrievedUser)
            {
                Console.WriteLine($"  - {property.Key}: {property.Value}");
            }
        }
        else
        {
            Console.WriteLine($"FAILURE: Could not find user with ID '{testUserId}'.");
        }

        // Keep the window from closing - derp.
        Console.WriteLine("\nPress Enter to exit...");
        Console.ReadLine();
    }
}