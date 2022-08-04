using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lore.Stats;
using System;

public enum AbilityState
{
    READY,
    ACTIVE,
    COOLDOWN
}

/// <summary>
/// Abstract class for all abilities
/// </summary>
public abstract class Ability : ScriptableObject
{
    public int ID;
    public new string name;
    public string description;
    public float abilityDuration = 0f;

    public Stat Cooldown;
    public Stat Damage;

    
    public bool canUse;
    public Action<Ability> onAbilityUse;
    public AbilityState state;

    protected Player player;
    [NonSerialized] public float cooldownTimer;
    [NonSerialized] public float activeTimer;



    public abstract void Initialize();

    public abstract void Fire(GameObject parent);
}
