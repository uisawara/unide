using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class UnideDriver : IUnideDriver
{
    public void Open(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public List<GameObject> FindAll()
    {
        var result = new List<GameObject>();
        EnumGameObject(result);
        return result;
    }

    public GameObject FindObjectByName(string name)
    {
        return FindAll().Where(obj => obj.name == name).First();
    }

    public GameObject FindObjectByTag(string tag)
    {
        return FindAll().Where(obj => obj.tag == tag).First();
    }
    
    private void EnumGameObject(List<GameObject> results)
    {
        void SearchInChildren(GameObject parent, List<GameObject> results)
        {
            results.Add(parent);
            foreach (Transform child in parent.transform)
            {
                SearchInChildren(child.gameObject, results);
            }
        }

        GameObject[] allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in allObjects)
        {
            SearchInChildren(obj, results);
        }
    }
}
