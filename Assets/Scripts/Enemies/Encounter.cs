//Initially written by Ossian, feel free to add to it.

/*
 * Jag t�nker mig lite att denna klassen p� n�got s�tt, kanske en hitbox, kanske n�got mer avancerat,
 * k�nner av n�r ett encounter ska b�rja och skickar signalen till GameManager med en referens till sig sj�lv.
 * Denna k�nner sedan sj�lv av n�r alla dens "fiender" �r d�da och signalerar GameManagern.
 * 
 * Om ett encounter redan �r ig�ng s� kan inte ett till aktiveras. �n s� l�nge. B�r antagligen ses �ver och poleras.
 * alt. designas runt. Kanske en osynlig v�gg s� att man inte bara kan smita f�rbi fienderna f�rr�n man d�dat dem eller n�got.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Encounter : MonoBehaviour
{
    //[HideInInspector]
    public List<GameObject> enemies;

    private bool myTurn = false;
    private int currentEnemysTurn = 0;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.CompareTag("Enemy"))
                enemies.Add(child);
        }
    }

    private void Update()
    {
        if(GameManager.Instance.currentEncounter == this)
        {
            if (enemies.Count > 0)
                OssianUtils.CleanList(enemies);
            else
                GameManager.Instance.EndEncounter();
        }

        if (myTurn)//utkommenterad kod p� hur signalsystemet till fienderna skulle kunna funka
        {
            /*
             * if(enemies[currentEnemysTurn].turnDone){
             *      currentEnemysTurn++;
             *      if(currentEnemysTurn >= Enemies.Count)
             *          GameManager.enemiesTurnDone = true;
             *          myTurn = false
             *       else
             *          enemies[currentEnemysTurn].StartTurn();
             * }
             */

            //TEMPOR�RT H�R F�R TESTSYFTEN:
            //DEN V�NTAR N�N SEKUND SEN RADERAR DEN EN FIENDE OCH SIGNALERAR ATT DEN �R KLAR
            StartCoroutine("TemporaryDoneSignal");
        }
    }

    bool waited = false;
    IEnumerator TemporaryDoneSignal()//ALLTS� TEMPOR�R METOD SOM B�R RADERAS SENARE
    {
        if (!waited)
        {
            yield return new WaitForSeconds(5f);
            waited = true;
        }
        else
        {
            Destroy(enemies[0]);
            GameManager.Instance.enemiesTurnDone = true;
            waited = false;
            myTurn = false;
            StopCoroutine("TemporaryDoneSignal");
            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("collision enter " + collision.collider.CompareTag("Player"));
    }

    //Kanske tempor�r, f�r att avg�ra om spelare kommit in i encounteromr�det
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("collision enter " + other.CompareTag("Player"));
        if (other.gameObject.CompareTag("Player") && GameManager.Instance.combatState == CombatState.none)
        {
            GameManager.Instance.StartEncounter(this);
        }
    }
    

    public void EnemiesTurn()
    {
        myTurn = true;
        //enemies[0].StartTurn();
        currentEnemysTurn = 0;
        Debug.Log("Enemies turn start");
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }
}
