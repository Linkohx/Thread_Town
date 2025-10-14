using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class TransitionPanel : MonoBehaviour
{
    private static TransitionPanel instance;
    public static TransitionPanel Instance => instance;

    protected CanvasGroup canvasGroup;

    public UnityEvent transitionEvent;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        canvasGroup = GetComponent<CanvasGroup>();
        transitionEvent = new UnityEvent();
    }

    public void Show()
    {
        StartCoroutine(IShow());
    }

    protected IEnumerator IShow()
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(1f);
        transitionEvent?.Invoke();

        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;

        transitionEvent.RemoveAllListeners();

        yield break;
    }
}
