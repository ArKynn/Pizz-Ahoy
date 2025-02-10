using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SauceBottle : UtilityTool
{
    private SauceCollision sauceCollision;

    protected override void Start()
    {
        base.Start();

        sauceCollision = GetComponentInChildren<SauceCollision>();
        sauceCollision.StopParticles();
    }

    protected override void ActivatedAction(ActivateEventArgs args)
    {
        sauceCollision.PlayParticles();
    }

    protected override void DeactivatedAction(DeactivateEventArgs args)
    {
        sauceCollision.StopParticles();
    }
}
