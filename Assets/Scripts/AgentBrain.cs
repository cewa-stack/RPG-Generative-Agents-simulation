using UnityEngine;
using System.Threading.Tasks;

public class AgentBrain : MonoBehaviour
{
    [SerializeField] private OllamaConnector ollama;
    [SerializeField] private AgentMovement movement;
    [SerializeField] private float thinkInterval = 5f;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= thinkInterval)
        {
            Think();
            timer = 0;
        }
    }

    private async void Think()
    {
        string prompt = "You are a village resident. Decided what to do next based on your current position. " +
                        "Options: [STAY, MOVE_RANDOM]. Respond with ONLY the word.";
        
        string decision = await ollama.AskLlama(prompt);
        ProcessDecision(decision.Trim().ToUpper());
    }

    private void ProcessDecision(string decision)
    {
        if (decision.Contains("MOVE_RANDOM"))
        {
            Vector2 randomPos = new Vector2(
                transform.position.x + UnityEngine.Random.Range(-3f, 3f),
                transform.position.y + UnityEngine.Random.Range(-3f, 3f)
            );
            movement.SetDestination(randomPos);
            Debug.Log("[BRAIN]: Moving to new location.");
        }
        else
        {
            Debug.Log("[BRAIN]: Staying in place.");
        }
    }
}