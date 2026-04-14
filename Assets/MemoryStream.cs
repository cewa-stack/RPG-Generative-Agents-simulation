using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class MemoryStream : MonoBehaviour
{
    [SerializeField] private OllamaConnector ollama;
    private readonly List<MemoryEntry> _memories = new List<MemoryEntry>();

    public async Task AddMemory(string description)
    {
        int importance = await RateImportance(description);
        _memories.Add(new MemoryEntry(description, importance));
    }

    private async Task<int> RateImportance(string description)
    {
        string prompt = $"Rate poignancy (1-10) of this memory: {description}. Respond ONLY with integer.";
        string response = await ollama.AskLlama(prompt);
        if (int.TryParse(response?.Trim(), out int score)) return Mathf.Clamp(score, 1, 10);
        return 3;
    }

    public string GetRecentContext(int count)
    {
        if (_memories.Count == 0) return "No past memories.";
        
        var recent = _memories
            .OrderByDescending(m => m.createdAt)
            .Take(count)
            .Select(m => m.description);
            
        return string.Join(". ", recent);
    }

    public List<MemoryEntry> GetAllMemories() => _memories;
}