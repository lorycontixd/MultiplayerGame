using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lore.Stats
{
    /// <summary>
    /// Type of stat modifier.
    /// It has a default order to facilitate custom orders with values in between.
    /// </summary>
    public enum StatModType
    {
        Flat = 100,
        PercentAdd = 200,
        PercentMult = 300,
    }
    
    [Serializable]
    public class StatModifier
    {
        public readonly float Value;
        public readonly StatModType Type;
        public readonly int Order;
        public readonly object Source; // Origin of the modifier (item, spell, buff, etc)

        public StatModifier(float value, StatModType type, int order, object source)
        {
            Value = value;
            Type = type;
            Order = order;
            Source = source;
        }

        // Requires Value and Type. Calls the "Main" constructor and sets Order and Source to their default values: (int)type and null, respectively.
        public StatModifier(float value, StatModType type) : this(value, type, (int)type, null) { }

        // Requires Value, Type and Order. Sets Source to its default value: null
        public StatModifier(float value, StatModType type, int order) : this(value, type, order, null) { }

        // Requires Value, Type and Source. Sets Order to its default value: (int)Type
        public StatModifier(float value, StatModType type, object source) : this(value, type, (int)type, source) { }
    }
}
