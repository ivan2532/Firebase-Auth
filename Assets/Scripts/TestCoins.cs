using UnityEngine;
using TMPro;

public class TestCoins : MonoBehaviour
{
    public TextMeshProUGUI coinsText;
    public TMP_InputField newCoinsInput;

    public void PostNewCoinsButton()
    {
        User newUserData = new User();

        if (int.TryParse(newCoinsInput.text, out newUserData.coins))
            DatabaseHandler.PostUser(newUserData, AuthHandler.userId, () => { coinsText.text = newCoinsInput.text; }, AuthHandler.idToken);
        else
            InfoManager.Instance.ShowInfo("Coin input invalid.", 2.0f);
    }
}
