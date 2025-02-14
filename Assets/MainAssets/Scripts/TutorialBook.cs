using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TutorialBook : UtilityTool
{
    [SerializeField] private CanvasGroup tutorialUI;
    [SerializeField] private CanvasGroup firstSpawnTextUI;
    [SerializeField] private List<Sprite> tutorialImages;
    [SerializeField] private Image image;
    [SerializeField] private Button nextbutton;
    [SerializeField] private Button prevbutton;

    private UIManager uiManager;
    private HashSet<XRBaseInteractor> interactors = new HashSet<XRBaseInteractor>();
    private int currentPage;


    protected override void Start()
    {
        base.Start();

        uiManager = FindAnyObjectByType<UIManager>();

        grabInteractable.selectEntered.AddListener(EnableUI);
        grabInteractable.selectExited.AddListener(DisableUI);
        nextbutton.onClick.AddListener(NextPage);
        prevbutton.onClick.AddListener(PrevPage);

        tutorialUI.alpha = 0;
        tutorialUI.gameObject.SetActive(true);
        currentPage = 0;
        image.sprite = tutorialImages[0];
        prevbutton.gameObject.SetActive(false);
    }

    private void NextPage()
    {
        currentPage++;

        CheckPageButtons();

        image.sprite = tutorialImages[currentPage];
    }

    private void PrevPage()
    {
        currentPage--;

        CheckPageButtons();

        image.sprite = tutorialImages[currentPage];
    }
    
    private void EnableUI(SelectEnterEventArgs args)
    {
        interactors.Add((XRBaseInteractor)args.interactorObject);

        if(interactors.Count > 0)
        {
            uiManager.StartCoroutine(uiManager.FadeInUI(tutorialUI, 2f));

            if(firstSpawnTextUI.alpha > 0f)
                uiManager.StartCoroutine(uiManager.FadeOutUI(firstSpawnTextUI, 2f));
        }
    }

    private void DisableUI(SelectExitEventArgs args)
    {
        interactors.Remove((XRBaseInteractor)args.interactorObject);

        if(interactors.Count <= 0)
        {
            uiManager.StartCoroutine(uiManager.FadeOutUI(tutorialUI));
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

    private void CheckPageButtons()
    {
        if(currentPage == tutorialImages.Count - 1)
            nextbutton.gameObject.SetActive(false);
        else
            nextbutton.gameObject.SetActive(true);


        if(currentPage == 0)
            prevbutton.gameObject.SetActive(false);
        else
            prevbutton.gameObject.SetActive(true);
    }
}
