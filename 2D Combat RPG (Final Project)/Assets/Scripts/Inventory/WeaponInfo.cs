using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This scriptable object holds a prefab for a specific weapon
// and info about that weapon, which we will reference often
// in other scripts.
[CreateAssetMenu(menuName = "New Weapon")]
public class WeaponInfo : ScriptableObject
{
    public GameObject weaponPrefab;
    public float weaponCooldown;
    public int weaponDamage;
    public float weaponRange;
}
