using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    private TutorialManager tutorialManager;
    public bool Triggered = false;

    public void Start()
    {
        tutorialManager = GetComponentInParent<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!Triggered)
        {
            Triggered = true;
            tutorialManager.HandleTrigger(this.gameObject, other);
        }
    }
}
