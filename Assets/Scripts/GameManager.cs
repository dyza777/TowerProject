using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public GameSettings GameSettings;
    [SerializeField] private GameObject LoseScreen;
    public bool isGameOver { get; private set; } = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        GameSettings = ScriptableObject.CreateInstance<GameSettings>();
    }

    public void HandleUpgrade(string upgradeId)
    {
        int currentLevel = (int)GameSettings.GetType().GetField(upgradeId + "Level").GetValue(GameSettings);
        GameSettings.GetType().GetField(upgradeId + "Level").SetValue(GameSettings, currentLevel + 1);
    }

    public void HandleGameOver()
    {
        isGameOver = true;
        LoseScreen.SetActive(true);
    }
}
