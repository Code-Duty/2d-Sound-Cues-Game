using System.Diagnostics;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class View : MonoBehaviour
{
    public void Initialize(Model model)
    {
        model.GameLoaded += OnGameLoaded;
        model.CharacterPositionChanged += OnCharacterPositionChanged;
        model.EffectsApplied += OnEffectsApplied;
        model.GameEnded += OnGameEnded;
    }

    public void ShowMainMenu()
    {
        UnityEngine.Debug.Log("Menu Shown");
        
    }

    void Update()
    {

    }

    void OnGameLoaded()
    {
        LoadGraphics();
    }

    void LoadGraphics()
    {
        // Load and initialize game graphics here
    }

    void OnCharacterPositionChanged(Vector2Int newPos)
    {
        UpdateGameDisplay(newPos);
    }

    void OnEffectsApplied()
    {
        // Play effects like sounds, animations, etc.
    }

    void OnGameEnded()
    {
        DisplayEndScreen();
    }

    void UpdateGameDisplay(Vector2Int position)
    {
        // Update the game's display based on the position
    }

    void DisplayEndScreen()
    {
        // Show end game screen
    }
}
