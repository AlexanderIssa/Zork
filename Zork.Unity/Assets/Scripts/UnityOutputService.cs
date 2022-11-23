using Zork.Common;
using UnityEngine;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine.UI;
using System.Collections.Generic;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField]
    private TextMeshProUGUI TextLinePrefab;

    [SerializeField]
    private Image NewLinePrefab;

    [SerializeField]
    private Transform ContentTransform;

    [SerializeField]
    [Range(0,100)]
    private int MaxEntries;

    public void Write(object obj) => ParseAndWriteLine(obj.ToString());

    public void Write(string message) => ParseAndWriteLine(message);

    public void WriteLine(object obj) => ParseAndWriteLine(obj.ToString());

    public void WriteLine(string message) => ParseAndWriteLine(message);

    private void ParseAndWriteLine(string message)
    {
        var textLine = Instantiate(TextLinePrefab, ContentTransform);

        char separator = '\n';
        string[] messageSplit = message.Split(separator);

        if (messageSplit.Length == 0)
        {
            return;
        }
        else if (messageSplit.Length == 1)
        {
            textLine.text = message;
            _entries.Enqueue(textLine.gameObject);
        }
        else
        {
            textLine.text = message;
            var newLine = Instantiate(NewLinePrefab, ContentTransform);
            _entries.Enqueue(textLine.gameObject);
            _entries.Enqueue(newLine.gameObject);
        }
        
        if(_entries.Count >= MaxEntries)
        {
            Destroy(_entries.Dequeue());
        }
    }

    private Queue<GameObject> _entries = new Queue<GameObject>();
}
