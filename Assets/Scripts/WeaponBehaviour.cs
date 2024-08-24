using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    public PCombat pc;
    public GameObject weaponPar;

    // Start is called before the first frame update
    void Start()
    {
        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PCombat>();
        weaponPar = this.gameObject;

        pc.weaponObject = GameObject.FindGameObjectWithTag(pc.weaponName);
        
        foreach (Transform weapons in weaponPar.transform)
            if (weapons.tag != pc.weaponName)
                weapons.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeWeapon(string name)
    {
        foreach (Transform weapons in weaponPar.transform)
            weapons.gameObject.SetActive(true);

        pc.weaponObject = GameObject.FindGameObjectWithTag(name);

        foreach (Transform weapons in weaponPar.transform)
            if (weapons.tag != name)
                weapons.gameObject.SetActive(false);

        PlayerPrefs.SetString("weaponName", name);
        pc.weaponName = name;
        pc.weaponStats = Resources.Load<Weapon>(name);
        pc.applyStats();
    }
}
