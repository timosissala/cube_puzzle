using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableVariables
{
    public class BoolListener : VariableListener<bool>
    {
        [SerializeField]
        private BoolVariable boolVariable = null;
        private void OnEnable()
        {
            ScriptableVariable = boolVariable;
        }
    }
}
