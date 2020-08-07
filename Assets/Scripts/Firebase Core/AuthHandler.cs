using System;
using FullSerializer;
using Proyecto26;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserInfo
{
    public string localId;
    public bool emailVerified;
}

[Serializable]
public class UserData
{
    public UserInfo[] users;
}

public static class AuthHandler
{
    private const string apiKey = "AIzaSyDCnkgIiT_hk7skD8fiIbUaFuDbtZfCgOc";

    private static fsSerializer serializer = new fsSerializer();

    public delegate void SignInSuccess();
    public delegate void SignInFail(string info);
    public delegate void RegistrationSuccess();
    public delegate void RegistrationFail(string info);
    public delegate void EmailVerificationSuccess();
    public delegate void EmailVerificationFail();

    public static string idToken;
    public static string userId;

    public static void SignUp(string email, string password, User user, RegistrationSuccess callback, RegistrationFail fallback)
    {
        var payLoad = $"{{\"email\":\"{email}\",\"password\":\"{password}\",\"returnSecureToken\":true}}";
        RestClient.Post($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/signupNewUser?key={apiKey}",
            payLoad).Then(
            response =>
            {
                var responseJson = response.Text;

                var data = fsJsonParser.Parse(responseJson);
                object deserialized = null;
                serializer.TryDeserialize(data, typeof(Dictionary<string, string>), ref deserialized);

                var authResponse = deserialized as Dictionary<string, string>;

                DatabaseHandler.PostUser(user, authResponse["localId"], () => { }, authResponse["idToken"]);

                SendEmailVerification(authResponse["idToken"]);

                callback();
            }).Catch(exception => { fallback("Failed to create account."); });
    }

    private static void SendEmailVerification(string newIdToken)
    {
        var payLoad = $"{{\"requestType\":\"VERIFY_EMAIL\",\"idToken\":\"{newIdToken}\"}}";
        RestClient.Post(
            $"https://www.googleapis.com/identitytoolkit/v3/relyingparty/getOobConfirmationCode?key={apiKey}", payLoad);
    }

    public static void SignIn(string email, string password, SignInSuccess callback, SignInFail fallback)
    {
        var payLoad = $"{{\"email\":\"{email}\",\"password\":\"{password}\",\"returnSecureToken\":true}}";
        RestClient.Post($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key={apiKey}",
            payLoad).Then(
            response =>
            {
                var responseJson = response.Text;

                var data = fsJsonParser.Parse(responseJson);
                object deserialized = null;
                serializer.TryDeserialize(data, typeof(Dictionary<string, string>), ref deserialized);

                var authResponse = deserialized as Dictionary<string, string>;

                CheckEmailVerification(authResponse["idToken"], () =>
                {
                    callback();
                    //DatabaseHandler.GetUser(userId, user => { Debug.Log($"{user.coins}"); }, idToken);
                }, () => { fallback("E-mail not verified"); });
            }).Catch(exception => { fallback("E-mail or password is incorrect."); });
    }

    private static void CheckEmailVerification(string newIdToken, EmailVerificationSuccess callback,
        EmailVerificationFail fallback)
    {
        var payLoad = $"{{\"idToken\":\"{newIdToken}\"}}";
        RestClient.Post($"https://www.googleapis.com/identitytoolkit/v3/relyingparty/getAccountInfo?key={apiKey}",
            payLoad).Then(
            response =>
            {
                var responseJson = response.Text;

                var data = fsJsonParser.Parse(responseJson);
                object deserialized = null;
                serializer.TryDeserialize(data, typeof(UserData), ref deserialized);

                var authResponse = deserialized as UserData;

                if (authResponse.users[0].emailVerified)
                {
                    userId = authResponse.users[0].localId;
                    idToken = newIdToken;

                    callback();
                }
                else
                    fallback();
            });
    }
}
