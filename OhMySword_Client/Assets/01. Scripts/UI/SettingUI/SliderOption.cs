using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyUI
{
    public class SliderOption : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private TMP_InputField input;
        private float _value = 0f;
        public float Value
        {
            get { return _value; }
            private set
            {
                _value = (float)Math.Round(value, 2);
                input.text = _value.ToString();
                slider.value = _value;
            }
        }

        private void Awake()
        {
            slider.onValueChanged.AddListener(SliderAction);
            input.onEndEdit.AddListener(InputAction);
        }

        private void SliderAction(float value)
        {
            Value = value;
        }

        private void InputAction(string value)
        {
            if (float.TryParse(value, out float v))
            {
                Value = v;
            }
        }
    }

}

