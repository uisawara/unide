using System.Collections.Generic;
using UnityEngine;

public interface IUnideDriver
{
    void Open(string sceneName);
    List<GameObject> FindAll();
    GameObject FindObjectByName(string name);
    GameObject FindObjectByTag(string tag);
}