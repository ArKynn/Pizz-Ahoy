using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreen;
    
    public void LoadScene(int scene)
    {
        StopAllCoroutines();
        StartCoroutine(StartLoadingScene(scene));
    }

    private IEnumerator StartLoadingScene(int scene)
    {
        StartCoroutine(FadeInUI(loadingScreen));

        while(loadingScreen.alpha < 1f)
        {
            yield return null;
        }

        SceneManager.LoadScene(scene);
    }

    private IEnumerator FadeInUI(CanvasGroup ui)
    {
        ui.blocksRaycasts = true;

        while(ui.alpha < 1f)
        {
            ui.alpha += Time.fixedDeltaTime;
            yield return null;
        }
    }

    private IEnumerator FadeOutUI(CanvasGroup ui)
    {
        ui.blocksRaycasts = false;
        
        while(ui.alpha > 0f)
        {
            ui.alpha -= Time.fixedDeltaTime;
            yield return null;
        }
    }
}
