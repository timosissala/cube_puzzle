using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScriptableVariables
{
    public abstract class ScriptableVariable<T> : ScriptableObject
    {
        [SerializeField]
        private T value;
        public T Value { get { return value; } set { this.value = value; OnValueChanged?.Invoke(this.value); } }

        public event Action<T> OnValueChanged;

        public void SetValue(T value)
        {
            this.value = value;
            OnValueChanged?.Invoke(this.value);
        }

        public T GetValue()
        {
            return value;
        }
    }
}