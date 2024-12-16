using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private IntVariable playerMoney;

    private TextMeshProUGUI moneyText;
    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI healthText;

    void Start()
    {
        moneyText = GameObject.Find("Money Amount Text").GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score Amount Text").GetComponent<TextMeshProUGUI>();
        healthText = GameObject.Find("Health Amount Text").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        moneyText.text = $"${playerMoney.Value}";
        scoreText.text = $"{GameManager.instance.getScore()}";
        healthText.text = $"{GameManager.instance._player.Health()}";
    }
}
