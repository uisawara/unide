using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public sealed class SubPageA : MonoBehaviour
{
    [SerializeField] private Button _backButton;
    [SerializeField] private ChildSwitcher _childSwitcher;

    private void OnValidate()
    {
        if (_childSwitcher == null)
        {
            _childSwitcher = GetComponentInParent<ChildSwitcher>();
        }
    }
    
    [UsedImplicitly]
    public void BackToTopPage() => _childSwitcher.ChangeActiveChild(0);
}