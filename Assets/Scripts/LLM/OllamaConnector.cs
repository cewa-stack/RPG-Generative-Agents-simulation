using System;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class OllamaRequest
{
    public string model;
    public string prompt;
    public bool stream;

    public OllamaRequest(string targetModel, string targetPrompt)
    {
        model = targetModel;
        prompt = targetPrompt;
        stream = false;
    }
}

[Serializable]
public class OllamaResponse
{
    public string response;
}

public class OllamaConnector : MonoBehaviour
{
    [SerializeField] private string ollamaUrl = "http://localhost:11434/api/generate";
    [SerializeField] private string modelName = "llama3.1:8b-instruct-q8_0";

    public async Task<string> AskLlama(string prompt)
    {
        OllamaRequest requestData = new OllamaRequest(modelName, prompt);
        string jsonData = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request = new UnityWebRequest(ollamaUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                return null;
            }

            OllamaResponse responseData = JsonUtility.FromJson<OllamaResponse>(request.downloadHandler.text);
            return responseData.response;
        }
    }
}