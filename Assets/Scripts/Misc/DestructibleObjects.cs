using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObjects : MonoBehaviour
{
    [SerializeField]private GameObject destroyedPrefab;
    [SerializeField] private GameObject particleEffect;

    int objectHealth = 50;
    public void Explode()
    {
        AkSoundEngine.PostEvent("Play_Explosion", gameObject);
        Instantiate(particleEffect, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z), transform.rotation);
        Instantiate(destroyedPrefab, transform.position, transform.rotation);
        CameraShake.Shake(0.25f, 0.25f);
        Destroy(gameObject);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
       
    //        Explode();
        
    //}
}
