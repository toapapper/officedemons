using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    FieldOfView fov;
    Actions actions;

    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        actions = GetComponent<Actions>();

    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> targetList = fov.visibleTargets;

        if (targetList.Count > 0)
        {
            actions.Attack(); 
        }
    }
}
