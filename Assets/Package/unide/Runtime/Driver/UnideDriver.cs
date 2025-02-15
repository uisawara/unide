using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cysharp.Threading.Tasks;
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
        return FindAll().Where(obj => obj.name == name).FirstOrDefault();
    }

    public GameObject FindObjectByTag(string tag)
    {
        return FindAll().Where(obj => obj.tag == tag).FirstOrDefault();
    }

    public GameObject FindObjectByComponent<TComponent>() where TComponent : Component
    {
        return FindAll().Where(obj => obj.gameObject.GetComponent<TComponent>()).FirstOrDefault();
    }

    public GameObject FindChildByNameDepth(GameObject element, string name)
    {
        foreach (Transform transform in element.transform)
        {
            if (transform.gameObject.name == name)
            {
                return transform.gameObject;
            }
            var result = FindChildByNameDepth(transform.gameObject, name);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public GameObject FindChildByTagDepth(GameObject element, string tag)
    {
        foreach (Transform transform in element.transform)
        {
            if (transform.gameObject.tag == tag)
            {
                return transform.gameObject;
            }
            var result = FindChildByTagDepth(transform.gameObject, tag);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public GameObject FindChildByComponentDepth<TComponent>(GameObject element) where TComponent : Component
    {
        return element.GetComponentInChildren<TComponent>()
            .gameObject;
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

    public async UniTask CaptureScreenshot(string filePath)
    {
        var fileParentPath = Directory.GetParent(filePath).FullName;
        if (!Directory.Exists(fileParentPath)) Directory.CreateDirectory(fileParentPath);

        ScreenCapture.CaptureScreenshot(filePath);
        await UniTask.Delay(100);
    }
}
