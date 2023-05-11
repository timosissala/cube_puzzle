using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableVariables.UI
{
    public class IntTextController : TextController<int>
    {
        [SerializeField]
        private IntVariable intVariable;

        private void Awake()
        {
            base.variable = intVariable;
        }
    }
}
