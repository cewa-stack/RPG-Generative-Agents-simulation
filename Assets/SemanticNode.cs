using UnityEngine;

public class SemanticNode : MonoBehaviour
{
    public string objectName;
    public string description;
    public bool isInteractable;
    public string currentState = "idle";
    public string ownerName = "None";

    public string GetObservation()
    {
        return $"{objectName} is {currentState}. Owner: {ownerName}. Description: {description}";
    }
}