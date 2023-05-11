using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ScriptableVariables.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextController<T> : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI tmpText;

        [SerializeField]
        protected ScriptableVariable<T> variable;

        private void Start()
        {
            tmpText = GetComponent<TextMeshProUGUI>();
        }

        public void UpdateTextFromVariable(bool append)
        {
            tmpText.text = append ? tmpText.text + "\n" + variable.Value : "\n" + variable.Value;
        }

        public void UpdateText(string text, bool append = false)
        {
            tmpText.text = append ? tmpText.text + text : text;
        }
    }
}