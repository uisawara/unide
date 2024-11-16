using System.Collections.Generic;
using UnityEngine;

public interface IUnideDriver
{
    void Open(string sceneName);
    List<GameObject> FindAll();
    GameObject FindObjectByName(string name);
    GameObject FindObjectByTag(string tag);
    GameObject FindObjectByComponent<TComponent>() where TComponent : Component;
    GameObject FindChildByNameDepth(GameObject element, string name);
    GameObject FindChildByTagDepth(GameObject element, string tag);
    GameObject FindChildByComponentDepth<TComponent>(GameObject element) where TComponent : Component;
}
