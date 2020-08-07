using Proyecto26;
using UnityEngine;
using FullSerializer;
using System.Collections.Generic;

public static class DatabaseManager
{
    private const string projectId = "freelance-auth";
    private static readonly string databaseURL = $"https://{projectId}.firebaseio.com/";

    private static fsSerializer serializer = new fsSerializer();

    public delegate void PostUserCallback();
    public delegate void GetUserCallback(User user);
    public delegate void GetUsersCallback(Dictionary<string, User> users);

    public static void PostUser(User user, string userId, PostUserCallback callback)
    {
        RestClient.Put<User>($"{databaseURL}users/{userId}.json", user).Then(response => { callback(); });
    }

    public static void GetUser(string userId, GetUserCallback callback)
    {
        RestClient.Get<User>($"{databaseURL}users/{userId}.json").Then(user => { callback(user); });
    }

    public static void GetUsers(GetUsersCallback callback)
    {
        RestClient.Get($"{databaseURL}users.json").Then(response =>
        {
            var responseJson = response.Text;
            var data = fsJsonParser.Parse(responseJson);
            object deserialized = null;
            serializer.TryDeserialize(data, typeof(Dictionary<string, User>), ref deserialized);

            var users = deserialized as Dictionary<string, User>;
            callback(users);
        });
    }
}
