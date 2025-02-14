using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup loadingScreen;
    [SerializeField] private CanvasGroup newDayScreen;
    [SerializeField] private TextMeshProUGUI newDayText;
    [SerializeField] private CanvasGroup endDayScreen;
    [SerializeField] private TextMeshProUGUI endDayText;
    [SerializeField] private TextMeshProUGUI endDayProfitText;
    [SerializeField] private CanvasGroup newQuotaScreen;
    [SerializeField] private TextMeshProUGUI newQuotaText;
    [SerializeField] private CanvasGroup paidQuotaScreen;
    [SerializeField] private CanvasGroup loseScreen;
    [SerializeField] private CanvasGroup winScreen;
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioClip newDaySound;
    [SerializeField] private AudioClip endDayProfitSound;
    [SerializeField] private AudioClip newQuotaSound;
    [SerializeField] private AudioClip paidQuotaSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;

    private void Start()
    {
        loadingScreen.gameObject.SetActive(true);
        if(loadingScreen.alpha < 1f) loadingScreen.alpha = 1f;
        else StartCoroutine(FadeOutUI(loadingScreen));
    }
    public void LoadScene(int scene)
    {
        StopAllCoroutines();
        StartCoroutine(StartSceneTransition(Mathf.Max(scene, 0)));
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

    public IEnumerator FadeInUI(CanvasGroup ui, float speed = 1)
    {
        ui.blocksRaycasts = true;

        while(ui.alpha < 1f)
        {
            ui.alpha += Time.fixedDeltaTime * speed;
            yield return null;
        }
    }

    public IEnumerator FadeOutUI(CanvasGroup ui, float speed = 1)
    {
        ui.blocksRaycasts = false;
        
        while(ui.alpha > 0f)
        {
            ui.alpha -= Time.fixedDeltaTime * speed;
            yield return null;
        }
    }

    public IEnumerator DisplayNewDay(int dayNum, float endViewTimer)
    {
        // Can't start while another screen is active
        while(newQuotaScreen.alpha > 0)
            yield return null;


        // Sets up the text for fading in with a lower number, then increasing later
        CanvasGroup textCanvasGroup = newDayText.transform.parent.GetComponent<CanvasGroup>();
        textCanvasGroup.alpha = 0f;
        newDayText.text = $"Day {dayNum - 1}";

        // Fades in the screen
        StartCoroutine(FadeInUI(newDayScreen));
        while(newDayScreen.alpha < 1)
            yield return null;

        // Fades in the text
        StartCoroutine(FadeInUI(textCanvasGroup, 3f));
        while(textCanvasGroup.alpha < 1)
            yield return null;
        
        yield return new WaitForSeconds(0.5f);

        // Increases the scale by 50% and changes the text, then scales back
        Vector3 originalScale = newDayText.transform.localScale;
        while(newDayText.transform.localScale.x < originalScale.x * 1.5f)
        {
            newDayText.transform.localScale += originalScale * Time.fixedDeltaTime * 3f;
            yield return new WaitForSeconds(0.05f);
        }

        newDayText.text = $"Day {dayNum}";
        SFXAudioSource.PlayOneShot(newDaySound);
        
        while(newDayText.transform.localScale.x > originalScale.x)
        {
            newDayText.transform.localScale -= originalScale * Time.fixedDeltaTime * 3f;
            yield return new WaitForSeconds(0.05f);
        }

        // Gives a timeframe to see the screen results before fading back out
        yield return new WaitForSeconds(endViewTimer);
        loadingScreen.alpha = 0f;
        StartCoroutine(FadeOutUI(newDayScreen));
    }

    public IEnumerator DisplayEndOfDay(int dayNum, float profit, float endViewTimer)
    {
        // Sets up the value of the next quota to start at 0 and rise until it reaches the right amount
        int incrementingValue = 0;
        endDayText.text = $"Day {dayNum}";
        endDayProfitText.text = $"Today's Profit: {incrementingValue} Gold";

        // Fades in the screen
        StartCoroutine(FadeInUI(endDayScreen));
        while(endDayScreen.alpha < 1)
            yield return null;
        
        yield return new WaitForSeconds(0.5f);

        // Starts increasing the value until it reaches the actual value
        while(incrementingValue < profit)
        {
            incrementingValue++;
            endDayProfitText.text = $"Today's Profit: {incrementingValue} Gold";

            if(incrementingValue == profit)
            {
                SFXAudioSource.PlayOneShot(endDayProfitSound);
            }

            yield return new WaitForSeconds(0.1f);
        }

        // Gives a timeframe to see the screen results before fading back out
        yield return new WaitForSeconds(endViewTimer);
        loadingScreen.alpha = 0f;
        StartCoroutine(FadeOutUI(endDayScreen));
        
    }

    public IEnumerator DisplayNewQuota(int newQuota, float endViewTimer)
    {
        // Sets up the value of the next quota to start at 0 and rise until it reaches the right amount
        int incrementingValue = 0;
        newQuotaText.text = $"{incrementingValue} Gold";

        // Fades in the screen
        StartCoroutine(FadeInUI(newQuotaScreen));
        while(newQuotaScreen.alpha < 1)
            yield return null;
        
        yield return new WaitForSeconds(0.5f);

        // Starts increasing the value until it reaches the actual value
        while(incrementingValue < newQuota)
        {
            incrementingValue++;
            newQuotaText.text = $"{incrementingValue} Gold";

            if(incrementingValue == newQuota)
            {
                SFXAudioSource.PlayOneShot(newQuotaSound);
            }

            yield return new WaitForSeconds(0.1f);
        }

        loadingScreen.alpha = 1f;
        // Gives a timeframe to see the screen results before fading back out
        yield return new WaitForSeconds(endViewTimer);
        StartCoroutine(FadeOutUI(newQuotaScreen));
    }
}
