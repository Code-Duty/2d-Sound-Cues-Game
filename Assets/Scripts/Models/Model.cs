using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;

public class Model : MonoBehaviour
{
    public event System.Action GameLoaded;
    public event System.Action<Vector2Int> CharacterPositionChanged;
    public event System.Action EffectsApplied;
    public event System.Action GameEnded;

    public Rigidbody2D rb;
    private Vector2Int characterPos;
    public float moveSpeed = 10f;
    private Camera camera;
    private Dictionary<Vector2Int, bool> mapCollisions; // Simplified for example purposes
    
    void Awake()
    {
        characterPos = new Vector2Int(0, 0);
        mapCollisions = new Dictionary<Vector2Int, bool>(); // Populate with map collisions
        rb = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }

    public void StartGame()
    {
        LoadResources();
        SetupInitialState();
        GameLoaded?.Invoke(); 
    }

    void LoadResources()
    {
        UnityEngine.Debug.Log("Load Resources");
        // Load resources such as textures, models, sounds, etc.
    }

    void SetupInitialState()
    {
        UnityEngine.Debug.Log("Setup Initial State");
        // Initial setup of the game state
    }

    public void UserClickedMovementKey(Vector2 moveDirection)
    {
        rb.velocity = moveDirection * moveSpeed;
        CharacterPositionChanged?.Invoke(Vector2Int.FloorToInt(rb.position));
    }

    bool VerifyMovementCollision(Vector2Int pos)
    {
        if (mapCollisions.ContainsKey(pos) && mapCollisions[pos] == false)
        {
            EffectsApplied?.Invoke();
            return false;
        }
        return true;
    }

    public void UserClickedAttackKey()
    {

        // Attack logic here
    }

    public void EndGame()
    {
        GameEnded?.Invoke();
    }
}
