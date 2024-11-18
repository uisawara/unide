using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class TopPage : MonoBehaviour
{
    [SerializeField] private Button _pageAButton;
    [SerializeField] private Button _pageBButton;
    [SerializeField] private ChildSwitcher _childSwitcher;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _sliderValue;

    private void OnValidate()
    {
        if (_childSwitcher == null)
        {
            _childSwitcher = GetComponentInParent<ChildSwitcher>();
        }
    }

    private void Awake()
    {
        _slider.onValueChanged.AddListener(Slider_OnValueChanged);
    }

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(Slider_OnValueChanged);
    }

    public void OpenPageA() => _childSwitcher.ChangeActiveChild(1);
    public void OpenPageB() => _childSwitcher.ChangeActiveChild(2);

    private void Slider_OnValueChanged(float value)
    {
        _sliderValue.text = value.ToString();
    }
}
