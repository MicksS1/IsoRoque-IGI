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
    public GameObject weaponObject;

    [Header("Weapon")]
    public string weaponName;
    public Weapon weaponStats;

    [Header("Atk Values")]
    public float atkRange;
    public int atkDamage;
    public float atkRate;
    public float atkDelay;

    public LayerMask enemyLayers;

    private float nextAtkTime = 0f;
    private float orMoveSpeed;
    private float orRotSpeed;
    //private int count;
    //private int weaponType;

    void Start()
    {
        PlayerPrefs.SetString("weaponName", weaponName);

        anim = GetComponent<Animator>();
        atkPos = GameObject.FindGameObjectWithTag("AttackPosition").GetComponent<Transform>();
        //EBehave = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EBehaviour>();
        pm = GetComponent<PMove>();
        mousePos = GameObject.FindGameObjectWithTag("MouseTarget").GetComponent<Transform>();
        playerPos = GetComponent<Transform>();

        orMoveSpeed = pm.moveSpeed;
        orRotSpeed = pm.rotSpeed;

        weaponStats = Resources.Load<Weapon>(weaponName);
        applyStats();
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextAtkTime)
            if (Input.GetMouseButtonDown(0))
            {
                attack(weaponName);
                nextAtkTime = Time.time + 1f / atkRate;
            }

        weaponName = PlayerPrefs.GetString("weaponName");

        //mousePos = new Vector3(Input.mousePosition.x, 0f, Input.mousePosition.z);
    }

    // func attack bisa ditambahin parameter string weapon biar nanti tinggal anim.SetTrigger(weapon)
    void attack(string weapon)
    {
        // play atk anim
        anim.SetTrigger("atk" + weapon);
        Invoke("attackHit", atkDelay);

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

    void attackHit()
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

        //count = 0;
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

    public void applyStats()
    {
        atkPos.localPosition = weaponStats.atkPos;
        atkRange = weaponStats.atkRange;
        atkDamage = weaponStats.atkDamage;
        atkRate = weaponStats.atkRate;
        atkDelay = weaponStats.atkDelay;
    }
}
