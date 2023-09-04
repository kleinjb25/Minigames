using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCommands : MonoBehaviour
{
    public Text commandText;
    public float blinkSpeed = 0.5f;
    public float scrollSpeed = 10f;
    private bool isTyping = false;
    private List<GameObject> listeners = new List<GameObject>();

    private void Start()
    {
        AddListener(gameObject);
        StartCoroutine(BlinkCursor());
    }
    public void AddListener(GameObject listener)
    {
        if (!listeners.Contains(listener))
        {
            listeners.Add(listener);
        }   
    }
    public void Command(string input)
    {
        string[] parts = input.Split(new char[] { '.', '(', ')' }, 3 );
        GameObject go = listeners.Where(x => x.name == parts[0]).SingleOrDefault();
        if (go != null)
        {
            go.SendMessage(parts[1], parts[2]);
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
    
}
