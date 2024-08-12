using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCombat : MonoBehaviour
{
    [Header("Refs")]
    public Animator anim;
    public Transform atkPos;
    public EBehaviour EBehave;
    public PMove pm;
    public Transform mousePos;
    public Transform playerPos;

    [Header("Atk Values")]
    public float atkRange;
    public int atkDamage;
    public float atkRate;
    public float atkDelay;

    public LayerMask enemyLayers;

    [Header("Mouse Position")]
    //[SerializeField]private Vector3 mousePos;

    private float nextAtkTime = 0f;
    private float orMoveSpeed;
    private float orRotSpeed;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        atkPos = GameObject.FindGameObjectWithTag("AttackPosition").GetComponent<Transform>();
        //EBehave = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EBehaviour>();
        pm = GetComponent<PMove>();
        mousePos = GameObject.FindGameObjectWithTag("MouseTarget").GetComponent<Transform>();
        playerPos = GetComponent<Transform>();

        orMoveSpeed = pm.moveSpeed;
        orRotSpeed = pm.rotSpeed;
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

        //mousePos = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.z);
    }

    void attack()
    {
        // play atk anim
        anim.SetTrigger("atk");
        Invoke("attackTiming", atkDelay);

        // slow down character when attacking
        pm.moveSpeed = pm.moveSpeed * 0.05f;
        pm.rotSpeed = pm.rotSpeed * 0.05f;
        pm.isDashing = true;
        Invoke("resetSpeed", atkDelay);

        // point character towards cursor when left click is pressed
        Vector3 dirToMouse = mousePos.position - playerPos.position;
        dirToMouse.y = 0f;

        Vector3 isoDir = Quaternion.Euler(0, 0, 0) * dirToMouse;
        Quaternion targetRotation = Quaternion.LookRotation(isoDir, Vector3.up);
        transform.rotation = targetRotation;
    }

    void attackTiming()
    {
        Collider[] hitEnemies;
        hitEnemies = Physics.OverlapSphere(atkPos.position, atkRange, enemyLayers);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log(enemy.name + " hit ");
            EBehave = enemy.GetComponent<EBehaviour>();
            EBehave.ETakeDamage(atkDamage);
            //Debug.Log(++count);
        }

        count = 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (atkPos == null)
            return;

        Gizmos.DrawWireSphere(atkPos.position, atkRange);
    }

    public void resetSpeed()
    {
        pm.moveSpeed = orMoveSpeed;
        pm.rotSpeed = orRotSpeed;
        pm.isDashing = false;
    }
}
