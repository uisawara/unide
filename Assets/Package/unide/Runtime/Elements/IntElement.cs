using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace unide
{
    public sealed class IntElement
    {
        public GameObject Target { get; }

        private Slider _slider;
        private Dropdown _dropdown;
        private TMP_Dropdown _tmpDropdown;
    
        private enum Types
        {
            Slider,
            Dropdown,
            DropdownTmp
        }

        private Types _type;

        public IntElement(GameObject target)
        {
            Target = target;
            _slider = Target.GetComponent<Slider>();
            _dropdown = Target.GetComponent<Dropdown>();
            _tmpDropdown = Target.GetComponent<TMP_Dropdown>();
            object[] objs = { _slider, _dropdown, _tmpDropdown };
            var count = objs.Where(o => o != null)
                .Count();
            if (count != 1)
            {
                throw new ArgumentException($"GameObject have too many text components: count={count}");
            }

            _type = (Types)objs.Select((o, i) => new { o, i })
                .First(x => x.o != null)
                .i;
        }

        public float GetValue()
        {
            switch (_type)
            {
                case Types.Slider:
                    return _slider.value;
                case Types.Dropdown:
                    return _dropdown.value;
                case Types.DropdownTmp:
                    return _tmpDropdown.value;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetValue(float value)
        {
            switch (_type)
            {
                case Types.Slider:
                    _slider.value = value;
                    break;
                case Types.Dropdown:
                    _dropdown.value = (int)value;
                    break;
                case Types.DropdownTmp:
                    _tmpDropdown.value = (int)value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
