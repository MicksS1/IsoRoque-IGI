using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EBehaviour : MonoBehaviour
{
    [Header("Refs")]
    public GameObject enemy;
    public Transform enemyPos;
    public Transform playerPos;
    public HealthBar healthBar;
    public Animator anim;
    public Material dissolveMaterial;
    //public Collider coll;

    [Header("Enemy Values")]
    public int maxHP;
    private int currHP;
    public float speed;
    public int dirDeg;
    public float rotSpeed;

    private float blinkIntensity = 1f;
    private float blinkDuration = 0.15f;
    private float blinkTimer;

    [Header("Validations")]
    public bool isHit;
    public bool isMalware;
    public bool isAdware;
    public bool isPopUp;

    private Color hitColor = new Color(1.0f, 0f, 0f, 0.1f);

    // Start is called before the first frame update
    void Start()
    {
        //enemy = GameObject.FindGameObjectWithTag("Enemy");
        //coll = GetComponent<Collider>();

        enemyPos = GetComponent<Transform>();
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        if (!isMalware)
            anim = GetComponent<Animator>();

        currHP = maxHP;
        healthBar.setMaxHP(maxHP);
    }

    // Update is called once per frame
    void Update()
    {
        //enemyPos.position = Vector3.MoveTowards(enemyPos.position, playerPos.position, speed);

        //Quaternion targetRot = Quaternion.LookRotation(playerPos.position, Vector3.up);
        //enemyPos.rotation = Quaternion.Slerp(enemyPos.rotation, targetRot, rotSpeed * Time.deltaTime);

        blinkTimer = blinkTimer - Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intensity = (lerp * blinkIntensity) + 1.0f;

        if (isHit)
        {
            foreach (SkinnedMeshRenderer smr in enemyPos.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                Color orColor = smr.material.color;
                smr.material.color = hitColor * intensity;
                StartCoroutine(hitReset(smr, blinkDuration, orColor));
            }

            isHit = false;
        }
    }

    private void FixedUpdate()
    {
        // Enemy Movement
        Vector3 dirToPlayer = playerPos.position - enemyPos.position;
        dirToPlayer.y = 0f;

        if (dirToPlayer != Vector3.zero && !isPopUp)
        {
            // kalau perlu rotation tambahan, value Quaternion.Euler tinggal di otak atik sumbu Y ny
            Vector3 isoDir = Quaternion.Euler(0, dirDeg, 0) * dirToPlayer;

            if (!isMalware)
                anim.SetTrigger("moveE");
            transform.position = Vector3.MoveTowards(transform.position, playerPos.position, speed);

            Quaternion targetRotation = Quaternion.LookRotation(isoDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
    }

    public void ETakeDamage(int damage)
    {
        isHit = true;
        currHP = currHP - damage;
        healthBar.setHP(currHP);

        Debug.Log(damage);

        // play hurt anim for enemy
        blinkTimer = blinkDuration;

        if (currHP <= 0)
        {
            // play dead anim for enemy
            foreach (SkinnedMeshRenderer smr in enemyPos.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                smr.material = dissolveMaterial;
                StartCoroutine(dissolve(smr.material));
            }

            Debug.Log("enemy dead!");
            Invoke(nameof(disableEnemy), 1f);
        }
    }

    IEnumerator hitReset(SkinnedMeshRenderer smr, float dur, Color color)
    {
        yield return new WaitForSeconds(dur);
        smr.material.color = color;
    }

    IEnumerator dissolve(Material mat)
    {
        float i = 0f;

        while (i < 1f)
        {
            mat.SetFloat("_Dissolve", i);
            i = i + 0.01f;            
            yield return new WaitForSeconds(0.01f);
        }

        mat.SetFloat("_Dissolve", 1f);
    }

    void disableEnemy()
    {
        enemy.SetActive(false);
    }
}
