using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonExtras : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public UnityEvent<GameObject> onSelected;
    public UnityEvent<GameObject> onDeselected;

    public void OnDeselect(BaseEventData eventData)
    {
        onDeselected.Invoke(gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelected.Invoke(gameObject);
    }

}
