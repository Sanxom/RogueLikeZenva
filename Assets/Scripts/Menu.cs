using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Menu : MonoBehaviour
{
    [SerializeField] private TMP_InputField seedInput;

    private void Start()
    {
        seedInput.text = PlayerPrefs.GetInt("Seed").ToString();
    }

    /// <summary>
    /// Saves the seed to the PlayerPrefs.
    /// </summary>
    public void OnUpdateSeed()
    {
        PlayerPrefs.SetInt("Seed", int.Parse(seedInput.text));
    }

    /// <summary>
    /// Loads the Game scene when the Play button is pressed.
    /// </summary>
    public void OnPlayButton()
    {
        if(seedInput.text != "")
        {
            SceneManager.LoadScene("Game");
        }
    }

    /// <summary>
    /// Generates a random seed number and plays the game with that seed.
    /// </summary>
    public void OnRandomButton()
    {
        Random.InitState((int)System.DateTime.Now.Ticks);
        seedInput.text = Random.Range(0, 10000000).ToString();
        OnUpdateSeed();
        SceneManager.LoadScene("Game");
    }
}
