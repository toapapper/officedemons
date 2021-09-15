/*
 *      Code Initially written by Ossian 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCard : MonoBehaviour
{
    protected Transform player;//att ers�ttas med r�tt script eller s�.

    private Transform[] heartContainers;
    private int playerHealth = 0;



    public void initialize(Transform player)
    {
        this.player = player;
    }

    // Start is called before the first frame update
    void Start()
    {
        heartContainers = new Transform[3];
        heartContainers[0] = transform.Find("heart1");//m�ngden h�r beror p� hur mkt liv spelare ska ha. 6hp = 3 hj�rtan
        heartContainers[1] = transform.Find("heart2");
        heartContainers[2] = transform.Find("heart3");
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            //playerHealth = player.health;
        }
        //Endast f�r testning
        else if(playerHealth == 0)
        {
            playerHealth = (int)(Random.value * 6);
            if (playerHealth == 0)
                playerHealth = 1;
        }

        HealthUpdate();
    }

    /// <summary>
    /// Update the images of the hearts to reflect the players health.
    /// </summary>
    private void HealthUpdate()
    {
        int fullHeartAmount = playerHealth / 2;
    }
}
