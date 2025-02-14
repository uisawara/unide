using UnityEngine;
using UnityEngine.UI;

namespace unide
{
    public sealed class FloatElement
    {
        public GameObject Target { get; }

        private Slider _slider;
    
        public FloatElement(GameObject target)
        {
            Target = target;
            _slider = Target.GetComponent<Slider>();
        }

        public float GetValue()
        {
            return _slider.value;
        }

        public void SetValue(float value)
        {
            _slider.value = value;
        }
    }
}