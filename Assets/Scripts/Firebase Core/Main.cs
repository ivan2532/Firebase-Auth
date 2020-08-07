using UnityEngine;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void OnAppStart()
    {
        //AuthHandler.SignUp("ivan.bozovic02@gmail.com", "BestPasswordEvah", new User(17));
        //AuthHandler.SignIn("ivan.bozovic02@gmail.com", "BestPasswordEvah");
    }
}
