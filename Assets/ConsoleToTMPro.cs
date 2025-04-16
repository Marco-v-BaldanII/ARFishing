using UnityEngine;
using TMPro;

public class ConsoleToTMP : MonoBehaviour
{
    public TMP_Text outputText;
    private string logText = "";

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logText = "";
        logText += logString + "\n";
        if (outputText != null)
        {
            outputText.text = logText;
        }
    }
}