using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    public static List<GameObject> players;

    //Extremt fult implementerade allihopa, men men.. Ossian som har skrivit allt detta förresten om ni inte pallar kolla i github efter vem som skrivit detta fönster vars enda utsikt är en grå kyrkogård i form av kod.
    public void BeginCombat()
    {
        Debug.Log("Begin combat");
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerController>().enabled = false;
        }
        //disabled player movement, now automatically move them somewhere, should perhaps be contained in the encounter where to

        //And begin the first turn
        BeginTurn();
    }

    public void EndCombat()
    {
        Debug.Log("End combat");

        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerController>().enabled = true;
        }
    }

    public void BeginTurn()
    {
        Debug.Log("Begin turn");
        for(int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerController>().enabled = true;
        }

        //foreach player beginTurn
        //Kanske kan ha något alertsystem som rapporterar upp till Gamemanager när alla spelare har låst in sina actions
        //Om turordning av actions är viktigt får ju det systemet finnas här i denna klassen t. ex. och till viss del implementeras här
    }

    public void EndTurn()
    {
        Debug.Log("End turn");
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerController>().enabled = false;
        }

        //foreach player forcibly endTurn
        //execute actions och sådant.

        //Behöver ett sätt för spelet att veta när spelarna har gjort färdigt sina rundor också.
    }
}
