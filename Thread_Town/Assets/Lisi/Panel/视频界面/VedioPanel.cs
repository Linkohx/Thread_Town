using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

[RequireComponent(typeof(CanvasGroup))]
public class VedioPanel : MonoBehaviour
{
    public VideoPlayer vedioPlayer;

    public void PlayVedio()
    {
        gameObject.SetActive(true);
        StartCoroutine(IPlayVedio());
    }

    protected IEnumerator IPlayVedio()
    {
        vedioPlayer.Play();

        yield return null;

        yield return new WaitForSeconds(6f);

        vedioPlayer.Stop();

        SceneManager.LoadScene(0);

        yield break;
    }
}
