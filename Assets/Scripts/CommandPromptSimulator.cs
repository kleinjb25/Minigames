using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CommandPromptSimulator : MonoBehaviour
{
    public Text commandText;
    public float typingSpeed = 0.05f;
    public float blinkSpeed = 0.5f;
    public float scrollSpeed = 10f;
    private int currentCommandIndex = 0;
    private bool isTyping = false;
    private string[] commands = {
        "dir",
        "echo Hello, World!",
        "ping google.com",
        "exit"
    };

    private void Start()
    {
        StartCoroutine(TypeCommand());
        StartCoroutine(BlinkCursor());
    }

    private IEnumerator TypeCommand()
    {
        isTyping = true;
        commandText.text += $"\nC:\\Windows\\System32> ";

        foreach (char letter in commands[currentCommandIndex])
        {
            commandText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        yield return new WaitForSeconds(blinkSpeed);
        StartCoroutine(PrintCommandResult());
    }

    private IEnumerator PrintCommandResult()
    {
        string commandResult = ExecuteCommand(commands[currentCommandIndex]);
        string[] resultLines = commandResult.Split('\n');

        for (int i = 0; i < resultLines.Length; i++)
        {
            commandText.text += $"\n{resultLines[i]}";
            yield return new WaitForSeconds(typingSpeed);
            StartCoroutine(ScrollText());
        }
        currentCommandIndex++;
        StartCoroutine(TypeCommand());
    }

    private IEnumerator ScrollText()
    {
        while (commandText.rectTransform.rect.height > commandText.rectTransform.parent.GetComponent<RectTransform>().rect.height)
        {
            commandText.rectTransform.anchoredPosition -= new Vector2(0f, scrollSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator BlinkCursor()
    {
        while (true)
        {
            
            commandText.text = isTyping ? commandText.text + "|" : commandText.text.Replace("|", "");
            yield return new WaitForSeconds(blinkSpeed);
            commandText.text = isTyping ? commandText.text.Replace("|", "") : commandText.text + "|";
            yield return new WaitForSeconds(blinkSpeed);
            
        }
    }

    private string ExecuteCommand(string command)
    {
        switch (command)
        {
            case "dir":
                return "Directory of C:\\\n\n07/13/2023  09:00 AM    <DIR>          Program Files\n07/13/2023  09:00 AM    <DIR>          Users\n07/13/2023  09:00 AM    <DIR>          Windows";

            case "echo Hello, World!":
                return "Hello, World!";

            case "ping google.com":
                return "Pinging google.com [172.217.168.174] with 32 bytes of data:\nReply from 172.217.168.174: bytes=32 time=13ms TTL=117\nReply from 172.217.168.174: bytes=32 time=14ms TTL=117\nReply from 172.217.168.174: bytes=32 time=12ms TTL=117\nReply from 172.217.168.174: bytes=32 time=15ms TTL=117";

            default:
                return " \'" + command + "\' is not recognized as an internal or external command, operable program, or batch file.";
        }
    }
}
