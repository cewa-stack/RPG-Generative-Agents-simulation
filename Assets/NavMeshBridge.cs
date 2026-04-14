using UnityEngine;
using Unity.AI.Navigation;
using System.Collections.Generic;

public class NavMeshBridge : MonoBehaviour
{
    [SerializeField] private GameObject[] groundObjects;
    [SerializeField] private GameObject[] obstacleLayers;

    [ContextMenu("Setup Navigation")]
    public void SetupNavigation()
    {
        ClearOldProxies();

        foreach (var ground in groundObjects)
        {
            if (ground == null) continue;
            GameObject proxy = CreateProxy(ground, "Ground_Proxy", 0);
            BoxCollider col = proxy.AddComponent<BoxCollider>();
            col.size = new Vector3(500, 500, 0.1f);
            proxy.transform.position = new Vector3(ground.transform.position.x, ground.transform.position.y, 1f);
        }

        foreach (var layer in obstacleLayers)
        {
            if (layer == null) continue;
            Collider2D[] childrenColliders = layer.GetComponentsInChildren<Collider2D>();
            
            foreach (var col2D in childrenColliders)
            {
                if (col2D.name.Contains("Proxy")) continue;

                GameObject proxy = CreateProxy(col2D.gameObject, "Obstacle_Proxy", 1);
                BoxCollider col3D = proxy.AddComponent<BoxCollider>();
                col3D.size = new Vector3(col2D.bounds.size.x, col2D.bounds.size.y, 50f);
                proxy.transform.position = new Vector3(col2D.bounds.center.x, col2D.bounds.center.y, 0f);
            }
        }
        Debug.Log("Old Proxies cleared and new ones generated. Click BAKE now.");
    }

    private GameObject CreateProxy(GameObject parent, string name, int areaIndex)
    {
        GameObject proxy = new GameObject(name);
        proxy.transform.SetParent(parent.transform);
        proxy.isStatic = true;

        NavMeshModifier mod = proxy.AddComponent<NavMeshModifier>();
        mod.overrideArea = true;
        mod.area = areaIndex;

        return proxy;
    }

    private void ClearOldProxies()
    {
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None);
        List<GameObject> toDestroy = new List<GameObject>();

        foreach (var obj in allObjects)
        {
            if (obj.name.Contains("Proxy"))
            {
                toDestroy.Add(obj);
            }
        }

        foreach (var obj in toDestroy)
        {
            DestroyImmediate(obj);
        }
    }
}