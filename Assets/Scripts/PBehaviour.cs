using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBehaviour : MonoBehaviour
{
    [Header("Refs")]
    GameObject player;

    public int maxHP = 100;
    private int currHP;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        currHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PTakeDamage(int damage)
    {
        currHP = currHP - damage;

        if (currHP <= 0)
        {
            // play dead anim for player
            player.SetActive(false);
        }
    }
}
