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

    private TextMeshProUGUI totalMoneyText;
    private TextMeshProUGUI totalScoreText;

    private PlayableDirector showEndgameUIDir;

    private Spawner _spawner;
    private Player _player;

    private bool _gameover;
    private float _gameTimer;
    private int _timedModeLength = 5;
    private int _score;

    public AudioSource source;
    public AudioClip winClip, loseClip;

    void Start()
    {
        if (instance == null) instance = this; else Destroy(this);
        
        totalMoneyText = GameObject.Find("Earnings Amount Text").GetComponent<TextMeshProUGUI>();
        totalScoreText = GameObject.Find("Total Score Amount Text").GetComponent<TextMeshProUGUI>();

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
            showEndgameUI();
            return;
        }

        if (gameMode == GameMode.Timed)
        {
            _gameTimer += Time.deltaTime;
            if (_gameTimer >= _timedModeLength * 60)
            {
                Debug.Log("Game Over");
                source.PlayOneShot(winClip);
                _spawner.enabled = false;
                return;
            }
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
        totalMoneyText.text = $"{playerMoney.Value}";
        totalScoreText.text = $"{_score}";
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
