using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace unide
{
    public sealed class StringElement
    {
        public GameObject Target { get; }

        private TextMesh _textMesh;
        private TextMeshPro _textPro;
        private TextMeshProUGUI _textUGUI;
        private InputField _inputField;
        private TMP_InputField _tmpInputField;
    
        private enum Types
        {
            TextMesh,
            TextMeshPro,
            TextMeshProUGUI,
            InputField,
            TMP_InputField,
        }

        private Types _type;
    
        public StringElement(GameObject target)
        {
            Target = target;
            _textMesh = Target.GetComponent<TextMesh>();
            if (_textMesh == null)
            {
                // Target.GetComponent<TextMesh>() を呼び出した結果が null だった場合、nullではあるがExceptionメッセージを含むオブジェクトが返ってくる。
                // これがTextMeshPro,InputField等と挙動が違っているのを解消するため、明示的にnull代入をやり直すことで対策にしている。
                _textMesh = null;
            }
            _textPro = Target.GetComponent<TextMeshPro>();
            _textUGUI = Target.GetComponent<TextMeshProUGUI>();
            _inputField = Target.GetComponent<InputField>();
            _tmpInputField = Target.GetComponent<TMP_InputField>();
            object[] objs = { _textMesh, _textPro, _textUGUI, _inputField, _tmpInputField };
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

        public string GetText()
        {
            switch (_type)
            {
                case Types.TextMesh:
                    return _textMesh.text;
                case Types.TextMeshPro:
                    return _textPro.text;
                case Types.TextMeshProUGUI:
                    return _textUGUI.text;
                case Types.InputField:
                    return _inputField.text;
                case Types.TMP_InputField:
                    return _tmpInputField.text;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void SetText(string text)
        {
            switch (_type)
            {
                case Types.TextMesh:
                    _textMesh.text = text;
                    break;
                case Types.TextMeshPro:
                    _textPro.text = text;
                    break;
                case Types.TextMeshProUGUI:
                    _textUGUI.text = text;
                    break;
                case Types.InputField:
                    _inputField.text = text;
                    break;
                case Types.TMP_InputField:
                    _tmpInputField.text = text;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}