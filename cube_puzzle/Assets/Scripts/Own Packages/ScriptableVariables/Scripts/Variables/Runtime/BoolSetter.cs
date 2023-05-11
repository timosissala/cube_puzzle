using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableVariables
{
    public class BoolSetter : MonoBehaviour
    {
        [SerializeField]
        private BoolVariable boolVariable = null;

        public void ToggleVariable()
        {
            boolVariable.Value = !boolVariable.Value;
        }
    }
}
