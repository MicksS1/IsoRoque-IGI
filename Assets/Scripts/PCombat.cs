using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCombat : MonoBehaviour
{
    [Header("Refs")]
    public Animator anim;
    public Transform atkPos;
    public EBehaviour EBehave;

    [Header("Atk Values")]
    public float atkRange;
    public int atkDamage;
    public float atkRate;
    public float atkDelay;

    public LayerMask enemyLayers;

    private float nextAtkTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        atkPos = GameObject.FindGameObjectWithTag("AttackPosition").GetComponent<Transform>();
        EBehave = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAtkTime)
            if (Input.GetMouseButtonDown(0))
            {
                attack();
                nextAtkTime = Time.time + 1f / atkRate;
            }
    }

    void attack()
    {
        // play atk anim
        anim.SetTrigger("atk");

        Invoke("attackTiming", atkDelay);
    }

    void attackTiming()
    {
        Collider[] hitEnemies;
        hitEnemies = Physics.OverlapSphere(atkPos.position, atkRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " hit");
            EBehave.ETakeDamage(atkDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (atkPos == null)
            return;

        Gizmos.DrawWireSphere(atkPos.position, atkRange);
    }
}
