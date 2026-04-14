using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AgentBrain : MonoBehaviour
{
    [SerializeField] private OllamaConnector ollama;
    [SerializeField] private AgentMovement movement;
    [SerializeField] private MemoryStream memory;
    [SerializeField] private AgentPerception perception;
    [SerializeField] private SpeechBubble speechBubble;
    [SerializeField] private float thinkInterval = 10f;

    private float _timer;

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= thinkInterval)
        {
            _ = ProcessCognition();
            _timer = 0;
        }
    }

    private async Task ProcessCognition()
    {
        if (ollama == null || perception == null) return;

        var observations = perception.GetNearbyObservations();
        string obsText = observations.Count > 0 ? string.Join(", ", observations) : "peaceful village paths";
        foreach (var o in observations) await memory.AddMemory(o);

        string history = memory.GetRecentContext(2);
        
        // Bardzo surowy prompt dla modelu
        string prompt = $"### Instruction ###\n" +
                        $"You are an AI NPC. Format your response EXACTLY like this: thought_text | action_word\n" +
                        $"Allowed actions: MOVE, STAY. Max 10 words total.\n" +
                        $"Context: {history}\n" +
                        $"Surroundings: {obsText}\n" +
                        $"Response: ";

        string response = await ollama.AskLlama(prompt);
        
        if (!string.IsNullOrEmpty(response))
        {
            Debug.Log("[AI]: " + response);
            ParseAndExecute(response);
        }
    }

    private void ParseAndExecute(string response)
    {
        string thought = "Thinking...";
        string action = "STAY";

        if (response.Contains("|"))
        {
            string[] parts = response.Split('|');
            thought = parts[0].Trim();
            action = parts[1].Trim().ToUpper();
        }
        else
        {
            thought = response; // Fallback jeśli Llama zapomni o kresce
            action = response.ToUpper().Contains("MOVE") ? "MOVE" : "STAY";
        }
        
        speechBubble.Show(thought);

        if (action.Contains("MOVE"))
        {
            SetSafeDestination();
        }
    }

    private void SetSafeDestination()
    {
        for (int i = 0; i < 5; i++) // Próbujemy 5 razy znaleźć wolne miejsce
        {
            Vector2 randomDir = Random.insideUnitCircle * 7f;
            Vector3 targetPos = transform.position + new Vector3(randomDir.x, randomDir.y, 0);
            
            // Sprawdzamy czy punkt jest na NavMesh (niebieskiej siatce)
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPos, out hit, 2.0f, NavMesh.AllAreas))
            {
                Debug.Log("[NAV]: Found valid spot at " + hit.position);
                movement.SetDestination(hit.position);
                return;
            }
        }
        Debug.LogWarning("[NAV]: Could not find safe spot, staying put.");
    }
}