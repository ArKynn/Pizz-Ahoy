using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private bool isMainMenu;
    [SerializeField] private CanvasGroup loadingScreen;
    [SerializeField] private CanvasGroup pauseMenu;
    [SerializeField] private CanvasGroup newDayScreen;
    [SerializeField] private TextMeshProUGUI newDayText;
    [SerializeField] private CanvasGroup endDayScreen;
    [SerializeField] private TextMeshProUGUI endDayText;
    [SerializeField] private TextMeshProUGUI endDayProfitText;
    [SerializeField] private CanvasGroup newQuotaScreen;
    [SerializeField] private TextMeshProUGUI newQuotaText;
    [SerializeField] private CanvasGroup payQuotaScreen;
    [SerializeField] private TextMeshProUGUI payQuotaText;
    [SerializeField] private CanvasGroup loseScreen;
    [SerializeField] private CanvasGroup winScreen;
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioClip newDaySound;
    [SerializeField] private AudioClip endDayProfitSound;
    [SerializeField] private AudioClip newQuotaSound;
    [SerializeField] private AudioClip paidQuotaSound;
    [SerializeField] private AudioClip loseSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private InputActionReference[] pauseInputs;

    private void Start()
    {
        
        foreach(InputActionReference input in pauseInputs)
        {
            input.action.Enable();
            input.action.performed += TogglePause;
        }
        if(isMainMenu) return;
        loadingScreen.gameObject.SetActive(true);
        if(loadingScreen.alpha < 1f) loadingScreen.alpha = 1f;
        else StartCoroutine(FadeOutUI(loadingScreen));
    }

    private void TogglePause(InputAction.CallbackContext obj)
    {
        pauseMenu.transform.position = FindAnyObjectByType<XROrigin>().transform.position;

        if(pauseMenu.alpha < 1f)
        {
            StartCoroutine(FadeInUI(pauseMenu));
            Time.timeScale = 0f;
        }
        else
        {
            StartCoroutine(FadeOutUI(pauseMenu));
            Time.timeScale = 1f;
        }
    }


    public void LoadScene(int scene)
    {
        Time.timeScale = 1f;
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
        while(payQuotaScreen.alpha > 0 || newQuotaScreen.alpha > 0)
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
        // Can't start while another screen is active
        while(payQuotaScreen.alpha > 0)
            yield return null;

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
        // Can't start while another screen is active
        while(payQuotaScreen.alpha > 0)
            yield return null;


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
    
    public IEnumerator DisplayPayQuota(int rent, int balance, float endViewTimer)
    {
        // Sets up the text for fading in
        CanvasGroup textCanvasGroup = payQuotaText.transform.parent.GetComponent<CanvasGroup>();
        textCanvasGroup.alpha = 0f;
        payQuotaText.text = $"Rent: {rent} \nBalance: {balance}";

        // Fades in the screen
        StartCoroutine(FadeInUI(payQuotaScreen));
        while(payQuotaScreen.alpha < 1)
            yield return null;
        
        yield return new WaitForSeconds(0.5f);

        // Fades in the text
        StartCoroutine(FadeInUI(textCanvasGroup, 3f));
        while(textCanvasGroup.alpha < 1)
            yield return null;
        
        int decreasingValue = 0;

        // Starts decreasing the value until either the rent or the balance reaches 0
        while(rent+decreasingValue > 0 && balance+decreasingValue > 0)
        {
            decreasingValue--;
            payQuotaText.text = $"Rent: {rent+decreasingValue} \nBalance: {balance+decreasingValue}";

            yield return new WaitForSeconds(0.1f);
        }
        

        // Gives a timeframe to see the screen results before fading back out
        yield return new WaitForSeconds(endViewTimer);
        StartCoroutine(FadeOutUI(payQuotaScreen));
    }
    
    public IEnumerator DisplayWin(float endViewTimer)
    {
        // Can't start while another screen is active
        while(payQuotaScreen.alpha > 0)
            yield return null;


        // Fades in the screen
        StartCoroutine(FadeInUI(winScreen));
        while(winScreen.alpha < 1)
            yield return null;
        
        yield return new WaitForSeconds(0.5f);


        // Gives a timeframe to see the screen results before fading back out
        yield return new WaitForSeconds(endViewTimer);
        LoadScene(0);
    }
    
    public IEnumerator DisplayLoss(float endViewTimer)
    {
        // Can't start while another screen is active
        while(payQuotaScreen.alpha > 0)
            yield return null;


        // Fades in the screen
        StartCoroutine(FadeInUI(loseScreen));
        while(loseScreen.alpha < 1)
            yield return null;
        
        yield return new WaitForSeconds(0.5f);


        // Gives a timeframe to see the screen results before fading back out
        yield return new WaitForSeconds(endViewTimer);
        LoadScene(0);
    }
}
