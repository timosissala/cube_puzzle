using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableVariables
{
    public class VariableListener<T> : MonoBehaviour
    {
        [SerializeField]
        private ScriptableVariable<T> scriptableVariable;
        public ScriptableVariable<T> ScriptableVariable { get { return scriptableVariable; } set { scriptableVariable = value; Initialise(); } }

        public UnityEvent OnValueChanged;

        protected void Initialise()
        {
            scriptableVariable.OnValueChanged -= OnValueChangedInvoke;
            scriptableVariable.OnValueChanged += OnValueChangedInvoke;
        }

        private void OnValueChangedInvoke(T value)
        {
            OnValueChanged?.Invoke();
        }

        public void PrintValue()
        {
            Debug.Log(scriptableVariable.Value);
        }

        private void OnDestroy()
        {
            scriptableVariable.OnValueChanged -= OnValueChangedInvoke;
        }
    }
}
