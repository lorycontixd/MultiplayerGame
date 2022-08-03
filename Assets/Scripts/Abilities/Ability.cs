using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lore.Stats;

/// <summary>
/// Abstract class for all abilities
/// </summary>
public abstract class Ability : ScriptableObject
{
    public int ID;
    public new string name;
    public string description;

    public Stat baseCooldown;
    public Stat damage;

    public abstract void Initialize();
    public abstract void Fire();
}
