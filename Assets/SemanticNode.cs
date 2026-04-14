using UnityEngine;

public class SemanticNode : MonoBehaviour
{
    public string objectName;
    public string description;
    public string ownerName = "Public";
    public string currentState = "idle";

    public string GetSummary()
    {
        return $"[{objectName}: {description}, Owner: {ownerName}, State: {currentState}]";
    }
}