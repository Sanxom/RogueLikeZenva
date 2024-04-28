using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject keyIcon;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] RawImage map;

    public static UIManager instance;

    public RawImage Map { get => map; set => map = value; }

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Handles updating visuals for the health.
    /// </summary>
    /// <param name="health"></param>
    public void UpdateHealth(int health)
    {
        for (int x = 0; x < hearts.Length; ++x)
        {
            hearts[x].SetActive(x < health);
        }
    }

    /// <summary>
    /// Handles updating visuals for the coins.
    /// </summary>
    /// <param name="coins"></param>
    public void UpdateCoinText(int coins)
    {
        coinText.text = coins.ToString();
    }

    /// <summary>
    /// Handles toggling whether or not the Player has the Key.
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleKeyIcon(bool toggle)
    {
        keyIcon.SetActive(toggle);
    }

    /// <summary>
    /// Handles updating visuals for the current Level.
    /// </summary>
    /// <param name="level"></param>
    public void UpdateLevelText(int level)
    {
        levelText.text = "Level " + level;
    }
}
