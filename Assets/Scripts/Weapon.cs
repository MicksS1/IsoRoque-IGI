using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string weaponName;
    public Vector3 atkPos;
    public float atkRange;
    public int atkDamage;
    public float atkRate;
    public float atkDelay;
}
