using UnityEngine;
using TMPro;
using System.Collections;

public class SpeechBubble : MonoBehaviour
{
    [SerializeField] private GameObject bubbleContainer;
    [SerializeField] private TextMeshProUGUI speechText;
    [SerializeField] private float displayDuration = 5f;

    private Coroutine hideCoroutine;

    private void Awake()
    {
        bubbleContainer.SetActive(false);
    }

    public void Show(string text)
    {
        if (hideCoroutine != null)
        {
            StopCoroutine(hideCoroutine);
        }

        bubbleContainer.SetActive(true);
        speechText.text = text;
        hideCoroutine = StartCoroutine(HideRoutine());
    }

    private IEnumerator HideRoutine()
    {
        yield return new WaitForSeconds(displayDuration);
        bubbleContainer.SetActive(false);
    }
}