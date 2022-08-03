using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lore.Stats
{
    public enum StatType
    {
        MoveSpeed,
        Intelligence,
        Bargaining,
        Stealth
    }

    [Serializable]
    public class Stat
    {
        public float BaseValue; // Base value of the stat

        protected readonly List<StatModifier> statModifiers; // List of all modifiers affecting the stat
        public readonly ReadOnlyCollection<StatModifier> StatModifiers; // List for viewing stat modifiers without changing the collection

        protected bool isDirty = true;
        protected float _value;
        protected float lastBaseValue = float.MinValue;

        public Action<Stat> onValueChange;

        public float Value
        {
            get
            {
                if (isDirty || lastBaseValue != BaseValue)
                {
                    lastBaseValue = BaseValue;
                    _value = CalculateFinalValue();
                    onValueChange?.Invoke(this);
                    isDirty = false;
                }
                return _value;
            }
        }


        public Stat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public Stat(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        /// <summary>
        /// Comparison method between two stat modifiers, where the sorting key is the modifier order
        /// </summary>
        /// <param name="a">First stat modifier</param>
        /// <param name="b">Second stat modifier</param>
        /// <returns></returns>
        protected virtual int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;
            return 0; // if (a.Order == b.Order)
        }

        // Each time a new modifier is added, all modifiers are sorted by order
        public virtual void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        public virtual bool RemoveModifier(StatModifier mod)
        {
            if (statModifiers.Remove(mod))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;

            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        protected virtual float CalculateFinalValue()
        {
            float finalValue = BaseValue;
            float sumPercentAdd = 0; // This will hold the sum of our "PercentAdd" modifiers

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];

                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd) // When we encounter a "PercentAdd" modifier
                {
                    sumPercentAdd += mod.Value; // Start adding together all modifiers of this type

                    // If we're at the end of the list OR the next modifer isn't of this type
                    if (i + 1 >= statModifiers.Count || statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd; // Multiply the sum with the "finalValue", like we do for "PercentMult" modifiers
                        sumPercentAdd = 0; // Reset the sum back to 0
                    }
                }
                else if (mod.Type == StatModType.PercentMult) // Percent renamed to PercentMult
                {
                    finalValue *= 1 + mod.Value;
                }
            }

            return (float)Math.Round(finalValue, 4);
        }
    }
}