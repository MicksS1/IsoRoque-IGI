using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBehaviour : MonoBehaviour
{
    [Header("Refs")]
    public GameObject enemy;
    public Transform enemyPos;
    public Transform playerPos;
    public HealthBar healthBar;
    //public Collider coll;

    [Header("Enemy Values")]
    public int maxHP = 100;
    private int currHP;
    public float speed;
    public int dirDeg;
    public float rotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        //coll = GetComponent<Collider>();

        enemyPos = GetComponent<Transform>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        //healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<HealthBar>();

        currHP = maxHP;
    }

    // Update is called once per frame
    void Update()
    {
        //enemyPos.position = Vector3.MoveTowards(enemyPos.position, playerPos.position, speed);

        //Quaternion targetRot = Quaternion.LookRotation(playerPos.position, Vector3.up);
        //enemyPos.rotation = Quaternion.Slerp(enemyPos.rotation, targetRot, rotSpeed * Time.deltaTime);

    }

    private void FixedUpdate()
    {
        // Enemy Movement
        Vector3 dirToPlayer = playerPos.position - enemyPos.position;
        dirToPlayer.y = 0f;

        if (dirToPlayer != Vector3.zero)
        {
            // kalau perlu rotation tambahan, value Quaternion.Euler tinggal di otak atik sumbu Y ny
            Vector3 isoDir = Quaternion.Euler(0, dirDeg, 0) * dirToPlayer;

            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed);

            Quaternion targetRotation = Quaternion.LookRotation(isoDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }

    public void ETakeDamage(int damage)
    {
        currHP = currHP - damage;
        healthBar.setHP(currHP);

        Debug.Log(damage);

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
