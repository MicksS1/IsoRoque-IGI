using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBehaviour : MonoBehaviour
{
    [Header("Refs")]
    GameObject enemy;
    Collider coll;

    public int maxHP = 100;
    private int currHP;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        coll = GetComponent<Collider>();

        currHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ETakeDamage(int damage)
    {
        currHP = currHP - damage;

        // play hurt anim for enemy

        if (currHP <= 0)
        {
            // play dead anim for enemy

            Debug.Log("enemy dead!");

            //coll.enabled = false;
            this.enabled = false;

            enemy.SetActive(false);
        }
    }
}
