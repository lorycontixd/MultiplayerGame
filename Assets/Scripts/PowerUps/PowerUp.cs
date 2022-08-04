using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lore.Stats;

public class PowerUp : MonoBehaviour
{
    public int ID;
    public int number;
    public new string name;
    public float duration;

    [HideInInspector] public StatModifier modifier;
    public StatType statType;
    [SerializeField] float modValue;
    public StatModType modType;

    private void Start()
    {
        modifier = new StatModifier(modValue, modType, source: this);
    }

    public void OnPickUp()
    {
        Destroy(gameObject);
    }
}
