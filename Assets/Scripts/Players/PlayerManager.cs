using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
		[SerializeField]
    public static List<GameObject> players;

		[SerializeField]
		GameObject viciousVicky;
		[SerializeField]
		GameObject terribleTim;
		[SerializeField]
		GameObject susanTheDestroyer;
		[SerializeField]
		GameObject devin;


    //Extremt fult implementerade allihopa, men men.. Ossian som har skrivit allt detta f�rresten om ni inte pallar kolla i github efter vem som skrivit detta f�nster vars enda utsikt �r en gr� kyrkog�rd i form av kod.
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
        //Kanske kan ha n�got alertsystem som rapporterar upp till Gamemanager n�r alla spelare har l�st in sina actions
        //Om turordning av actions �r viktigt f�r ju det systemet finnas h�r i denna klassen t. ex. och till viss del implementeras h�r
    }

    public void EndTurn()
    {
        Debug.Log("End turn");
        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerController>().enabled = false;
        }

        //foreach player forcibly endTurn
        //execute actions och s�dant.

        //Beh�ver ett s�tt f�r spelet att veta n�r spelarna har gjort f�rdigt sina rundor ocks�.
    }


}
