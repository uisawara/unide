using UnityEngine;

public class ChildSwitcher : MonoBehaviour
{
    [SerializeField] private int _childIndex;

    private void Awake()
    {
        foreach (Transform child in transform.GetComponentsInChildren<Transform>())
        {
            child.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }
        
        ChangeActiveChild(_childIndex);
    }

    public void ChangeActiveChild(int index)
    {
        _childIndex = index;

        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i)
                .gameObject.SetActive(i == index);
        }
    }
}