using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Manages the connection and data operations for the Redis database.
/// </summary>
public static class RedisManager
{
    // The connection to Redis should be created once and reused throughout the application.
    // Lazy<T> ensures the connection is only established when it's first needed.
    private static readonly Lazy<ConnectionMultiplexer> LazyConnection;

    static RedisManager()
    {
        // This is the default endpoint for a local Redis server.
        string connectionString = "localhost:6379";
        LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(connectionString));
    }

    /// <summary>
    /// Gets the active Redis connection multiplexer.
    /// </summary>
    private static ConnectionMultiplexer Connection => LazyConnection.Value;

    /// <summary>
    /// Gets the Redis database instance.
    /// </summary>
    private static IDatabase RedisDatabase => Connection.GetDatabase();

    /// <summary>
    /// Saves a user's data to a Redis Hash. This acts like saving a row to a table.
    /// </summary>
    /// <param name="userId">The unique ID for the user.</param>
    /// <param name="userName">The user's name.</param>
    /// <param name="email">The user's email address.</param>
    public static void SaveUser(string userId, string userName, string email)
    {
        // We use a convention for keys, like "user:[id]", to keep data organized.
        var key = $"user:{userId}";

        var userProperties = new HashEntry[]
        {
            new HashEntry("name", userName),
            new HashEntry("email", email)
        };

        // HashSet will create or update the hash at the specified key.
        RedisDatabase.HashSet(key, userProperties);
        Console.WriteLine($"SUCCESS: Saved user '{userName}' to Redis with key '{key}'.");
    }

    /// <summary>
    /// Retrieves a user's data from a Redis Hash.
    /// </summary>
    /// <param name="userId">The ID of the user to retrieve.</param>
    /// <returns>A dictionary containing the user's properties, or null if not found.</returns>
    public static Dictionary<string, string>? GetUser(string userId)
    {
        var key = $"user:{userId}";

        // HashGetAll retrieves all the fields and values from the specified hash.
        var userProperties = RedisDatabase.HashGetAll(key);

        if (userProperties.Length == 0)
        {
            return null; // User not found
        }

        // Convert the HashEntry array to a more usable Dictionary.
        return userProperties.ToStringDictionary();
    }
}
