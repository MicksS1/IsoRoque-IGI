using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ECombat : MonoBehaviour
{
    [Header("Refs")]
    public Animator anim;
    public Transform playerPos;
    public Transform pos;
    public Transform atkPos;
    public EBehaviour eb;
    public PBehaviour pb;

    [Header("Position Values")]
    [SerializeField] private Vector3 difference;
    [SerializeField] private float distance;
    private float activateDistance = 20f;

    [Header("Atk Values")]
    public float atkRangeE;
    public int atkDamageE;
    public float atkRateE;
    public float atkRadE;
    public float atkDelayE;
    public float speedResetDelayE;

    [Header("Validations")]
    public bool isBoss;
    public bool isAttacking;

    public LayerMask playerLayer;

    private float nextAtkTime = 0f;
    private float orSpeed;
    private float orRotSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        pos = GetComponent<Transform>();
        eb = GetComponent<EBehaviour>();
        pb = GameObject.FindGameObjectWithTag("Player").GetComponent<PBehaviour>();

        if (!eb.isMalware)
            anim = GetComponent<Animator>();

        orSpeed = eb.speed;
        orRotSpeed = eb.rotSpeed;
        isAttacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        difference = playerPos.position - pos.position;
        distance = difference.magnitude;

        if (distance < activateDistance)
            eb.enabled = true;
        else
            eb.enabled = false;

        if (Time.time >= nextAtkTime && !isBoss && !isAttacking)
        {
            if (distance < atkRadE)
            {
                attack();
                nextAtkTime = Time.time + 1f / atkRateE;
            }
        }
        else if (Time.time >= nextAtkTime && isBoss && !isAttacking)
        {
            if (distance < atkRadE && !eb.isAdware)
            {
                attackBoss('1'); // short range
                nextAtkTime = Time.time + 1f / atkRateE;

            }
            else if (distance > atkRadE && !eb.isAdware)
            {
                attackBoss('2'); // long range
                nextAtkTime = Time.time + 1f / atkRateE;
            }
            else if (distance < atkRadE && eb.isAdware)
            {
                attackBoss('1'); // short range
                nextAtkTime = Time.time + 1f / atkRateE;
            }
        }
    }

    public void attack()
    {
        isAttacking = true;

        anim.SetTrigger("atkE");
        Invoke("attackHit", atkDelayE);

        eb.speed = eb.speed * 0.05f;
        eb.rotSpeed = eb.rotSpeed * 0.05f;
        Invoke("resetSpeed", atkDelayE + speedResetDelayE);
    }

    public void attackBoss(char type)
    {
        isAttacking = true;

        if (type == '1')
            changeToShortAtk();
        else if (type == '2')
            changeToLongAtk();

        anim.SetTrigger("atkE" + type);
        Invoke("attackHit", atkDelayE);

        eb.speed = eb.speed * 0.05f;
        eb.rotSpeed = eb.rotSpeed * 0.05f;
        Invoke("resetSpeed", atkDelayE + speedResetDelayE);
    }

    public void attackHit()
    {
        Collider[] hitPlayer;
        hitPlayer = Physics.OverlapSphere(atkPos.position, atkRangeE, playerLayer);

        foreach (Collider player in hitPlayer)
        {
            Debug.Log(player.name + " hit ");
            pb = player.GetComponent<PBehaviour>();
            pb.PTakeDamage(atkDamageE);
            //Debug.Log(++count);
        }
    }

    public void resetSpeed()
    {
        eb.speed = orSpeed;
        eb.rotSpeed = orRotSpeed;
        isAttacking = false;
    }

    public void changeToLongAtk()
    {
        // ganti stat utk long range atk (AOE), kecuali atkrade
        // klo long range, atk rate hrs kecil
    }

    public void changeToShortAtk()
    {
        // ganti stat utk short range atk, kecuali atkrade
    }

    private void OnDrawGizmosSelected()
    {
        if (atkPos == null)
            return;

        Gizmos.DrawWireSphere(atkPos.position, atkRangeE);
    }
}
