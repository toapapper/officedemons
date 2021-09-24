//Initially written by Ossian, feel free to add to it.

/*
 * Jag tänker mig lite att denna klassen på något sätt, kanske en hitbox, kanske något mer avancerat,
 * känner av när ett encounter ska börja och skickar signalen till GameManager med en referens till sig själv.
 * Denna känner sedan själv av när alla dens "fiender" är döda och signalerar GameManagern.
 * 
 * Om ett encounter redan är igång så kan inte ett till aktiveras. Än så länge. Bör antagligen ses över och poleras.
 * alt. designas runt. Kanske en osynlig vägg så att man inte bara kan smita förbi fienderna förrän man dödat dem eller något.
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

        if (myTurn)//utkommenterad kod på hur signalsystemet till fienderna skulle kunna funka
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

            //TEMPORÄRT HÄR FÖR TESTSYFTEN:
            //DEN VÄNTAR NÅN SEKUND SEN RADERAR DEN EN FIENDE OCH SIGNALERAR ATT DEN ÄR KLAR
            StartCoroutine("TemporaryDoneSignal");
        }
    }

    bool waited = false;
    IEnumerator TemporaryDoneSignal()//ALLTSÅ TEMPORÄR METOD SOM BÖR RADERAS SENARE
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

    //Kanske temporär, för att avgöra om spelare kommit in i encounterområdet
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
