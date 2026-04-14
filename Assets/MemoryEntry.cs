using System;

[Serializable]
public class MemoryEntry
{
    public string description;
    public DateTime createdAt;
    public int importance;

    public MemoryEntry(string description, int importance)
    {
        this.description = description;
        this.importance = importance;
        this.createdAt = DateTime.Now;
    }
}