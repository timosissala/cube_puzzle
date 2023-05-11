using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScriptableVariables.UI
{
    public class FloatTextController : TextController<float>
    {
        [SerializeField]
        private FloatVariable floatVariable;

        [SerializeField, Range(0, 5)]
        private int decimalCount = 2;

        [SerializeField]
        private bool inverseText = false;

        [SerializeField, ConditionalField("Inverse Text")]
        private float maxValue;

        private void Awake()
        {
            variable = floatVariable;

            maxValue = Mathf.Round(maxValue * (10 ^ decimalCount)) / (10 ^ decimalCount);
        }

        public new void UpdateTextFromVariable(bool append)
        {
            string floatText;

            float value = Mathf.Round(floatVariable.Value * (10 ^ decimalCount)) / (10 ^ decimalCount);
            if (inverseText)
            {
                floatText = (Mathf.Round((maxValue - value) * (10 ^ decimalCount)) / (10 ^ decimalCount)).ToString();
            }
            else
            {
                floatText = value.ToString();
            }

            UpdateText(floatText, append);
        }
    }
}