using UnityEngine;
using UnityEngine.UI;

public sealed class TopPage : MonoBehaviour
{
    [SerializeField] private Button _pageAButton;
    [SerializeField] private Button _pageBButton;
    [SerializeField] private ChildSwitcher _childSwitcher;

    private void OnValidate()
    {
        if (_childSwitcher == null)
        {
            _childSwitcher = GetComponentInParent<ChildSwitcher>();
        }
    }

    public void OpenPageA() => _childSwitcher.ChangeActiveChild(1);
    public void OpenPageB() => _childSwitcher.ChangeActiveChild(2);
}
