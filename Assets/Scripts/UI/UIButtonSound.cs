using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIButtonSound : MonoBehaviour
{
    [SerializeField]private Button start;
    [SerializeField]private Button options;
    [SerializeField]private Button exit;
    public void onClick()
    {
        AkSoundEngine.PostEvent("Play_Office_SFX_Chug1", gameObject);
    }

    private void Update()
    {
        if(start == EventSystem.current.currentSelectedGameObject)
        {
            AkSoundEngine.PostEvent("Play_Office_SFX_MutedString1", gameObject);
        }
        else if (options== EventSystem.current.currentSelectedGameObject)
        {
            AkSoundEngine.PostEvent("Play_Office_SFX_MutedString1", gameObject);
        }
        else if (exit == EventSystem.current.currentSelectedGameObject)
        {
            AkSoundEngine.PostEvent("Play_Office_SFX_MutedString1", gameObject);
        }
    }
    
}

