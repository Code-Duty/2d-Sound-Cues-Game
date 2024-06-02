using System;

using UnityEngine;
using UnityEngine.SceneManagement;
public class View : MonoBehaviour
{
    Camera cam;
    public void Initialize(Model model)
    {
        model.GameLoaded += OnGameLoaded;
        model.CharacterPositionChanged += OnCharacterPositionChanged;
        model.EffectsApplied += OnEffectsApplied;
        model.GameEnded += OnGameEnded;
        cam = Camera.main;
    }

    public void ShowMainMenu()
    {
        //SceneManager.LoadScene("MainMenuScene");
    }

    void UpdateCameraPos(Vector2 pos)
    {
        cam.transform.position = new Vector3(pos.x, pos.y, -50);
    }

    void OnGameLoaded()
    {
        SceneManager.LoadScene("TestScene");
    }

    void LoadGraphics()
    {
        // Load and initialize game graphics here
    }

    void OnCharacterPositionChanged(Vector2 newPos)
    {
        UpdateCameraPos(newPos);
    }

    void OnEffectsApplied()
    {
        // Play effects like sounds, animations, etc.
        // Inverter  a sprite de Main 
    }

    void OnGameEnded(int gameResult)
    {
        DisplayEndScreen(gameResult);
    }

    void DisplayEndScreen(int gameResult)
    {
        if (gameResult == 1)
        {
            SceneManager.LoadScene("GameOverScene");
        }
        else
        {
            SceneManager.LoadScene("MissionSuccessfulScene");
        }
    }
}
