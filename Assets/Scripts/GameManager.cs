using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] public GameMode gameMode;
    [SerializeField] private IntEvent scoreGain;
    [SerializeField] private IntVariable playerMoney;

    public IntVariable gameModeData;

    private TextMeshProUGUI totalMoneyText;
    private TextMeshProUGUI totalTimeText;

    private TextMeshProUGUI timerText;

    private PlayableDirector showEndgameUIDir;

    private Spawner _spawner;
    public Player _player;

    private bool _gameover;
    private float _gameTimer;
    private float _endgameTime;
    private int _timedModeLength = 3;
    private int _score;

    public AudioSource source;
    public AudioClip winClip, loseClip;

    void Start()
    {
        if (instance == null) instance = this; else Destroy(this);
        
        gameMode = (GameMode)gameModeData.Value;
        
        totalMoneyText = GameObject.Find("Earnings Amount Text").GetComponent<TextMeshProUGUI>();
        totalTimeText = GameObject.Find("Time Survived Amount Text").GetComponent<TextMeshProUGUI>();
        
        timerText = GameObject.Find("Timer Text").GetComponent<TextMeshProUGUI>();

        showEndgameUIDir = GameObject.Find("Endgame UI Timeline").GetComponent<PlayableDirector>();

        _spawner = FindObjectOfType<Spawner>();
        _spawner.SetSpawnerType(gameMode);
        
        _player = FindObjectOfType<Player>();
        _gameover = false;
        scoreGain.RegisterListener(OnScoreGain);
    }

    // Update is called once per frame
    void Update()
    {
        // Game started at first frame Update is called
        /*
         * Gameplay:
         * Start spawning
         * Player has currency
         * Player has access to tower purchasing at all times
         * Mobs walk from node to node
         * Game ends when player loses all health points
         */
        if (PlayerDied() && !_gameover) {
            _gameover = true;
            _spawner.enabled = false;
            source.PlayOneShot(loseClip);
            _endgameTime = _gameTimer;
            showEndgameUI();
            return;
        }

        if (gameMode == GameMode.Timed)
        {
            _gameTimer += Time.deltaTime;
            
            float remainingTime = Mathf.Max((_timedModeLength * 60) - _gameTimer, 0);

            // Convert to minutes and seconds
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            // Update timer text in mm:ss format
            timerText.text = $"{minutes:00}:{seconds:00}";

            if (_gameTimer >= _timedModeLength * 60)
            {
                source.PlayOneShot(winClip);
                _spawner.enabled = false;
                _gameover = true;
                return;
            }
        }
        else if (gameMode == GameMode.Survival)
        {
            _gameTimer += Time.deltaTime;

            // Convert to minutes and seconds
            int minutes = Mathf.FloorToInt(_gameTimer / 60);
            int seconds = Mathf.FloorToInt(_gameTimer % 60);

            // Update timer text in mm:ss format
            timerText.text = $"{minutes:00}:{seconds:00}";
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync(0);
        }
    }

    public void playAgain()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void exitToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void showEndgameUI()
    {
        if (gameMode == GameMode.Timed) _endgameTime = Mathf.Max((_timedModeLength * 60) - _gameTimer, 0);

        int minutes = Mathf.FloorToInt(_endgameTime / 60);
        int seconds = Mathf.FloorToInt(_endgameTime % 60);

        totalMoneyText.text = $"{playerMoney.Value}";
        totalTimeText.text = $"{minutes:00}:{seconds:00}";
        showEndgameUIDir.Play();
    }

    bool PlayerDied()
    {
        return _player.Health() <= 0;
    }

    void OnScoreGain(int gain)
    {
        _score += gain;
    }

    public int getScore()
    {
        return _score;
    }

    private void OnDestroy()
    {
        scoreGain.UnregisterListener(OnScoreGain);
    }
}
