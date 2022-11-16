using UnityEngine;
using Zork.Common;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
    }

    private Game _game;
}
