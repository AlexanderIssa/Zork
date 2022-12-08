using Newtonsoft.Json;
using UnityEngine;
using Zork.Common;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI LocationText;

    [SerializeField]
    private TextMeshProUGUI ScoreText;

    [SerializeField]
    private TextMeshProUGUI MovesText;

    [SerializeField]
    private UnityInputService InputService;

    [SerializeField]
    private UnityOutputService OutputService;

    [SerializeField]
    private Sprite[] HealthSprites;

    [SerializeField]
    private Image HealthImage;

    private void Awake()
    {
        TextAsset gameJson = Resources.Load<TextAsset>("GameJson");
        _game = JsonConvert.DeserializeObject<Game>(gameJson.text);
        _game.Player.LocationChanged += Player_LocationChanged;
        _game.Player.MovesChanged += Player_MovesChanged;
        _game.Player.ScoreChanged += Player_ScoreChanged;
        _game.Player.HealthChanged += Player_HealthChanged;
        _game.Run(InputService, OutputService);
    }

    private void Player_LocationChanged(object sender, Room location)
    {
        LocationText.text = location.Name;
    }

    private void Player_MovesChanged(object sender, int moves)
    {
        MovesText.text = $"Moves: {moves}";
    }

    private void Player_ScoreChanged(object sender, int score)
    {
        ScoreText.text = $"Score: {score}";
    }

    private void Player_HealthChanged(object sender, float health)
    {
        switch (health)
        {
            case 5f:
                HealthImage.sprite = HealthSprites[0];
                break;
            case 4.5f:
                HealthImage.sprite = HealthSprites[1];
                break;
            case 4f:
                HealthImage.sprite = HealthSprites[2];
                break;
            case 3.5f:
                HealthImage.sprite = HealthSprites[3];
                break;
            case 3f:
                HealthImage.sprite = HealthSprites[4];
                break;
            case 2.5f:
                HealthImage.sprite = HealthSprites[5];
                break;
            case 2f:
                HealthImage.sprite = HealthSprites[6];
                break;
            case 1.5f:
                HealthImage.sprite = HealthSprites[7];
                break;
            case 1f:
                HealthImage.sprite = HealthSprites[8];
                break;
            case 0.5f:
                HealthImage.sprite = HealthSprites[9];
                break;
            case 0f:
                HealthImage.sprite = HealthSprites[10];
                break;
            default:
                HealthImage.sprite = HealthSprites[10];
                break;
        }
    }

    private void Start()
    {
        InputService.SetFocus();
        LocationText.text = _game.Player.CurrentRoom.Name;
        MovesText.text = $"Moves: {_game.Player.Moves}";
        ScoreText.text = $"Score: {_game.Player.Score}";
        HealthImage.sprite = HealthSprites[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InputService.ProcessInput();
            InputService.SetFocus();
        }

        if (_game.IsRunning == false)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }

    private Game _game;
}
