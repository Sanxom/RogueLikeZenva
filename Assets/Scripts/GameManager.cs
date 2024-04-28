using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int baseSeed;
    private int prevRoomPlayerHealth;
    private int prevRoomPlayerCoins;

    private Player player;

    public static GameManager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        level = 1;
        baseSeed = PlayerPrefs.GetInt("Seed");
        Random.InitState(baseSeed);
        Generation.instance.Generate();
        UIManager.instance.UpdateLevelText(level);

        player = FindObjectOfType<Player>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    /// <summary>
    /// Called when the player has a key and goes through the ExitDoor.
    /// </summary>
    public void GoToNextLevel()
    {
        prevRoomPlayerHealth = player.CurrentHealth;
        prevRoomPlayerCoins = player.Coins;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Is called when changing scenes.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "Game")
        {
            Destroy(gameObject);
            return;
        }

        player = FindObjectOfType<Player>();
        level++;
        baseSeed++;

        Generation.instance.Generate();

        player.CurrentHealth = prevRoomPlayerHealth;
        player.Coins = prevRoomPlayerCoins;

        UIManager.instance.UpdateHealth(prevRoomPlayerHealth);
        UIManager.instance.UpdateCoinText(prevRoomPlayerCoins);
        UIManager.instance.UpdateLevelText(level);
    }
}
