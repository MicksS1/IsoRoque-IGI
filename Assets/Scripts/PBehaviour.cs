using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PBehaviour : MonoBehaviour
{
    [Header("Refs")]
    public HealthBar healthBar;
    public GameObject player;
    public Material dissolveMaterial;

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
        healthBar.setHP(currHP);

        Debug.Log(damage);

        if (currHP <= 0)
        {
            foreach (SkinnedMeshRenderer smr in player.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                smr.material = dissolveMaterial;
                StartCoroutine(dissolve(smr.material));
            }

            Invoke(nameof(disablePlayer), 1f);
        }
    }

    IEnumerator dissolve(Material mat)
    {
        float elapsed = 0f;

        while (elapsed < 1f)
        {
            Debug.Log("jalan");
            elapsed += Time.deltaTime;
            float dissolveAmount = Mathf.Clamp01(elapsed / 1f);
            mat.SetFloat("_Dissolve", dissolveAmount);
            yield return null;
        }

        mat.SetFloat("_Dissolve", 1f);
    }

    void disablePlayer()
    {
        player.SetActive(false);
    }
}
