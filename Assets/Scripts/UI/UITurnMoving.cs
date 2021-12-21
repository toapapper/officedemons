using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITurnMoving : MonoBehaviour
{
    [SerializeField] private float speed = 5;
    private Vector3 startPos;
    private float timer = 0;
    [SerializeField] float stopTime = 0.5f;
    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.position;
    }

    private void OnEnable()
    {
        timer = 0;
        transform.position = startPos;
    }





    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= stopTime)
        {
            transform.position = new Vector3(transform.position.x + Time.deltaTime * speed, transform.position.y, transform.position.z);
        }
    }
}
