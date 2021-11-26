using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField] private GameObject start;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject exit;
    private GameObject previousButton;
    bool hasPlayed = false;

    public void onClick()
    {
        AkSoundEngine.PostEvent("Play_Office_SFX_Chug1", gameObject);
    }

    public void PlayerSelectSound()
    {
        AkSoundEngine.PostEvent("Play_Office_SFX_BramBram2", gameObject);
    }

    private void Update()
    {
        if(start == EventSystem.current.currentSelectedGameObject)
        {
            if(previousButton != start)
            {
                hasPlayed = false;
            }
            if (!hasPlayed)
            {
                AkSoundEngine.PostEvent("Play_Office_SFX_MutedString1", gameObject);
                hasPlayed = true;
                previousButton = start;
            }
        }
        else if (options == EventSystem.current.currentSelectedGameObject)
        {
            if (previousButton != options)
            {
                hasPlayed = false;
            }
            if (!hasPlayed)
            {
                previousButton = options;
                AkSoundEngine.PostEvent("Play_Office_SFX_MutedString1", gameObject);
                hasPlayed = true;
            }
        }
        else if (exit == EventSystem.current.currentSelectedGameObject)
        {
            if (previousButton != exit)
            {
                hasPlayed = false;
            }
            if (!hasPlayed)
            {
                previousButton = exit;
                AkSoundEngine.PostEvent("Play_Office_SFX_MutedString1", gameObject);
                hasPlayed = true;
            }
        }
    }
}

