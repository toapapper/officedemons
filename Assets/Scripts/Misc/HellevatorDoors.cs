using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HellevatorDoors : MonoBehaviour
{
    [SerializeField]
    GameObject right;
    [SerializeField]
    GameObject left;

    bool isClosed = false;
    Vector3 rightStartPos;
    Vector3 leftStartPos;

    // Start is called before the first frame update
    void Start()
    {
        rightStartPos = right.transform.position;
        leftStartPos = left.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float step = 1 * Time.deltaTime;
        if (isClosed)
        {
            left.transform.position = Vector3.MoveTowards(left.transform.position, new Vector3(1.5025f, left.transform.position.y, left.transform.position.z), step);
            right.transform.position = Vector3.MoveTowards(right.transform.position, new Vector3(-1.5025f, right.transform.position.y, right.transform.position.z), step);
        }
        else
        {
            left.transform.position = Vector3.MoveTowards(left.transform.position, leftStartPos, step);
            right.transform.position = Vector3.MoveTowards(right.transform.position, rightStartPos, step);
        }
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            isClosed = !isClosed;
        }
    }
}
