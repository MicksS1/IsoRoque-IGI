using System.Collections;
using System.Collections.Generic;
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

    [Header("Atk Values")]
    public float atkRangeE;
    public int atkDamageE;
    public float atkRateE;
    public float atkRadE;
    public float atkDelayE;

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

        orSpeed = eb.speed;
        orRotSpeed = eb.rotSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        difference = playerPos.position - pos.position;
        distance = difference.magnitude;

        if (Time.time >= nextAtkTime)
            if (distance < atkRadE)
            {
                attack();
                nextAtkTime = Time.time + 1f / atkRateE;
            }
    }

    public void attack()
    {
        anim.SetTrigger("atkE");
        Invoke("attackHit", atkDelayE);

        eb.speed = eb.speed * 0.05f;
        eb.rotSpeed = eb.rotSpeed * 0.05f;
        Invoke("resetSpeed", atkDelayE);
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
    }

    private void OnDrawGizmosSelected()
    {
        if (atkPos == null)
            return;

        Gizmos.DrawWireSphere(atkPos.position, atkRangeE);
    }
}
