using UnityEngine;
using System.Collections;
using TMPro;

public class InfoManager : MonoBehaviour
{
    public static InfoManager Instance;

    [SerializeField] private Canvas infoCanvas;
    [SerializeField] private TextMeshProUGUI infoText;

    private Coroutine currentShowCoroutine;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowInfo(string info, float duration)
    {
        infoText.text = info;

        if(currentShowCoroutine != null)
            StopCoroutine(currentShowCoroutine);

        currentShowCoroutine = StartCoroutine(ShowInfoEnumerator(duration));
    }

    private IEnumerator ShowInfoEnumerator(float duration)
    {
        infoCanvas.enabled = true;
        yield return new WaitForSeconds(duration);
        infoCanvas.enabled = false;
    }
}
