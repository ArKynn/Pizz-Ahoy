using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreen;

    private void Start()
    {
        loadingScreen.gameObject.SetActive(true);
        if(loadingScreen.alpha > 0f)
            StartCoroutine(FadeOutUI(loadingScreen));
    }
    public void LoadScene(int scene)
    {
        StopAllCoroutines();
        StartCoroutine(StartSceneTransition(Mathf.Min(scene, 0)));
    }

    public void QuitGame()
    {
        StopAllCoroutines();
        StartCoroutine(StartSceneTransition(-1));
    }

    private IEnumerator StartSceneTransition(int scene)
    {
        StartCoroutine(FadeInUI(loadingScreen));

        while(loadingScreen.alpha < 1f)
        {
            yield return null;
        }

        if(scene >= 0)
            SceneManager.LoadScene(scene);
        else
            Application.Quit();
    }

    public IEnumerator FadeInUI(CanvasGroup ui)
    {
        ui.blocksRaycasts = true;

        while(ui.alpha < 1f)
        {
            ui.alpha += Time.fixedDeltaTime;
            yield return null;
        }
    }

    public IEnumerator FadeOutUI(CanvasGroup ui)
    {
        ui.blocksRaycasts = false;
        
        while(ui.alpha > 0f)
        {
            ui.alpha -= Time.fixedDeltaTime;
            yield return null;
        }
    }
}
