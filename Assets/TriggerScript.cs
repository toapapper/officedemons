using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour
{
    private TutorialManager tutorialManager;

    public void Start()
    {
        tutorialManager = GetComponentInParent<TutorialManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        tutorialManager.HandleTriggerEnter(this.gameObject, other);
    }

    private void OnTriggerExit(Collider other)
    {
        tutorialManager.HandleTriggerExit(this.gameObject, other);
    }
}
