using UnityEngine;

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
        UnityEngine.Debug.Log("Menu Shown");
        
    }

    void Update()
    {

    }

    void UpdateCameraPos(Vector2 pos)
    {
        UnityEngine.Debug.Log(pos);
        cam.transform.position = new Vector3(pos.x, pos.y, -50);
    }
    void OnGameLoaded()
    {
        LoadGraphics();
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
