using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Stat_OffenseGroup
{
    //physical damage
    public Stat damage;
    public Stat critPower;
    public Stat critChance;
    public Stat armorReduction; //chi so tu item, buff
    //Elemental damage
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;
}
