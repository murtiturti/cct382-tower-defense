using UnityEngine;
using UnityEngine.Serialization;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private int health;
    [FormerlySerializedAs("money")] [SerializeField] private int startMoney;

    [SerializeField] private IntEvent takeDamage;
    [SerializeField] private IntEvent winMoney;

    [SerializeField] private IntVariable playerMoney;

    [SerializeField] private GameObject specialAttack;
    [SerializeField] private float specialCooldown;

    private TextMeshProUGUI specialStatusText;
    private TextMeshProUGUI specialCooldownText;

    private float _specialTimer;
    private Camera mainCam;

    private void Start()
    {
        takeDamage.RegisterListener(OnDamageTaken);
        winMoney.RegisterListener(OnMoneyGain);
        playerMoney.Value = startMoney;
        mainCam = Camera.main;

        specialCooldownText = GameObject.Find("Special Cooldown Text").GetComponent<TextMeshProUGUI>();
        specialStatusText = GameObject.Find("Special Status Text").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        _specialTimer += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha1) && _specialTimer >= specialCooldown)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 1f;
            Vector3 worldPos = mainCam.ScreenToWorldPoint(mousePos);
            Instantiate(specialAttack, worldPos, Quaternion.identity);
            _specialTimer = 0f;
        }

        if (_specialTimer < specialCooldown)
        {
            float remainingTime = Mathf.Max(specialCooldown - _specialTimer, 0);

            // Convert to minutes and seconds
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);

            specialCooldownText.text = $"{minutes:00}:{seconds:00}";
            specialStatusText.text = "On Cooldown";
        }
        else
        {
            specialCooldownText.text = "00:00";
            specialStatusText.text = "Ready";
        }
    }

    public int Health()
    {
        return health;
    }

    private void OnMoneyGain(int moneyGain)
    {
        playerMoney.Value += moneyGain;
    }

    private void OnDamageTaken(int damage)
    {
        health -= damage;
    }

    private void OnDestroy()
    {
        takeDamage.UnregisterListener(OnDamageTaken);
    }
}
