using UnityEngine;
using System.Threading.Tasks;

public class AgentBrain : MonoBehaviour
{
    [SerializeField] private OllamaConnector ollama;
    [SerializeField] private AgentMovement movement;
    [SerializeField] private float thinkInterval = 7f;

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
        if (ollama == null) return;

        string prompt = "You are an Human-like Agent in a World. Decide your next short-term goal. " +
                        "Options: [MOVE, STAY]. Respond with ONLY the word.";

        string response = await ollama.AskLlama(prompt);
        
        if (!string.IsNullOrEmpty(response))
        {
            string decision = response.Trim().ToUpper();
            Debug.Log($"[AI Decision]: {decision}");
            ProcessDecision(decision);
        }
    }

    private void ProcessDecision(string decision)
    {
        if (decision.Contains("MOVE"))
        {
            float range = 4f;
            Vector2 randomTarget = new Vector2(
                transform.position.x + Random.Range(-range, range),
                transform.position.y + Random.Range(-range, range)
            );
            movement.SetDestination(randomTarget);
        }
    }
}