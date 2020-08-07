using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TestUIManager : MonoBehaviour
{
    [SerializeField] private Canvas logInCanvas;
    [SerializeField] private Canvas registerCanvas;
    [SerializeField] private Canvas verifyEmailCanvas;
    [SerializeField] private Canvas loadingCanvas;
    [SerializeField] private Canvas userCanvas;

    [SerializeField] private TMP_InputField logInEmailInputField;
    [SerializeField] private TMP_InputField logInPasswordInputField;
    [SerializeField] private TMP_InputField registerEmailInputField;
    [SerializeField] private TMP_InputField registerPasswordInputField;

    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TMP_InputField newCoinsInput;

    public void GoToLogIn()
    {
        userCanvas.enabled = false;
        loadingCanvas.enabled = false;
        registerCanvas.enabled = false;
        verifyEmailCanvas.enabled = false;
        logInCanvas.enabled = true;
    }

    public void GoToRegistration()
    {
        userCanvas.enabled = false;
        loadingCanvas.enabled = false;
        logInCanvas.enabled = false;
        verifyEmailCanvas.enabled = false;
        registerCanvas.enabled = true;
    }

    public void GoToVerifyEmail()
    {
        userCanvas.enabled = false;
        loadingCanvas.enabled = false;
        logInCanvas.enabled = false;
        registerCanvas.enabled = false;
        verifyEmailCanvas.enabled = true;
    }

    public void GoToLoading()
    {
        userCanvas.enabled = false;
        logInCanvas.enabled = false;
        registerCanvas.enabled = false;
        verifyEmailCanvas.enabled = false;
        loadingCanvas.enabled = true;
    }

    public void GoToUser()
    {
        DatabaseHandler.GetUser(AuthHandler.userId,
            user =>
            {
                coinsText.text = $"Coins: {user.coins}";
                loadingCanvas.enabled = false;
                userCanvas.enabled = true;
            },
            AuthHandler.idToken);
    }

    public void LogInButton()
    {
        GoToLoading();

        AuthHandler.SignIn(logInEmailInputField.text, logInPasswordInputField.text,
            () =>
            {
                GoToUser();
            },
            info =>
            {
                InfoManager.Instance.ShowInfo(info, 3.0f);
                GoToLogIn();
            });
    }

    public void RegisterButton()
    {
        GoToLoading();

        AuthHandler.SignUp(registerEmailInputField.text, registerPasswordInputField.text, new User(),
            () =>
            {
                GoToVerifyEmail();
            },
            info =>
            {
                InfoManager.Instance.ShowInfo(info, 3.0f);
                GoToRegistration();
            });
    }

    public void PostNewCoinsButton()
    {

        User newUserData = new User();

        if (int.TryParse(newCoinsInput.text, out newUserData.coins))
        {
            userCanvas.enabled = false;
            loadingCanvas.enabled = true;

            DatabaseHandler.PostUser(newUserData, AuthHandler.userId,
                () =>
                {
                    coinsText.text = $"Coins: {newCoinsInput.text}";
                    loadingCanvas.enabled = false;
                    userCanvas.enabled = true;
                },
                AuthHandler.idToken);
        }
        else
            InfoManager.Instance.ShowInfo("Coin input invalid.", 2.0f);
    }
}
