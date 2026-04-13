using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AgentPerception : MonoBehaviour
{
    [SerializeField] private float viewRadius = 5f;
    [SerializeField] private LayerMask observationLayer;

    public List<string> GetNearbyObservations()
    {
        List<string> observations = new List<string>();
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, viewRadius, observationLayer);

        foreach (var hit in hitColliders)
        {
            SemanticNode node = hit.GetComponent<SemanticNode>();
            if (node != null)
            {
                observations.Add(node.GetObservation());
            }
        }

        return observations;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}