using UnityEngine;

public class EnvironmentAutoFix : MonoBehaviour
{
    [ContextMenu("Fix Environment Objects")]
    public void FixObjects()
    {
        SemanticNode[] nodes = FindObjectsByType<SemanticNode>(FindObjectsSortMode.None);
        int layer = LayerMask.NameToLayer("Environment");

        foreach (var node in nodes)
        {
            node.gameObject.layer = layer;
            if (node.gameObject.GetComponent<Collider2D>() == null)
            {
                var col = node.gameObject.AddComponent<BoxCollider2D>();
                col.isTrigger = true;
            }
        }
        Debug.Log($"Fixed {nodes.Length} environment objects.");
    }
}