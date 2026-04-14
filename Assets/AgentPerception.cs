using System.Collections.Generic;
using UnityEngine;

public class AgentPerception : MonoBehaviour
{
    [SerializeField] private float radius = 5f;
    [SerializeField] private LayerMask envLayer;

    public List<string> GetNearbyObservations()
    {
        List<string> results = new List<string>();
        var hits = Physics2D.OverlapCircleAll(transform.position, radius, envLayer);
        foreach (var hit in hits)
        {
            var node = hit.GetComponent<SemanticNode>();
            if (node != null) results.Add(node.GetSummary());
        }
        return results;
    }
}