using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public event System.Action GameLoaded;
    public event System.Action<Vector2> CharacterPositionChanged;
    public event System.Action EffectsApplied;
    public event System.Action GameEnded;

    public Rigidbody2D rb;
    public float moveSpeed = 10f;
    public int jumpPower;
    public float stopSpeed = 2f;
    private bool isJumping = false;
    private float horizontalInput;
    private Dictionary<Vector2Int, bool> mapCollisions; // Simplified for example purposes
    public Animator PlayerAnimator;

    public PlayerAttack PlayerAttack;

    void Awake()
    {
        mapCollisions = new Dictionary<Vector2Int, bool>(); // Populate with map collisions
        rb = GetComponent<Rigidbody2D>();
        //  Added this here, check later if according to MVM
        PlayerAnimator = GetComponent<Animator>();
        rb.gravityScale = 4;
        jumpPower = 24;

        PlayerAttack = GetComponent<PlayerAttack>();

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

    public void UserClickedMovementKey(float horizontalInput)
    {
        this.horizontalInput = horizontalInput;
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        CharacterPositionChanged?.Invoke(rb.position);
    }

    public void UserClickedJumpKey()
    {
        if (!isJumping)
        {
            PlayerAnimator.SetBool("is_jumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            isJumping = true;
            CharacterPositionChanged?.Invoke(rb.position);
        }
    }

    void Update()
    {
        if (Mathf.Abs(horizontalInput) > 0.01) // Only update if input is significant
        {
            // Exponential decay for smooth stopping
            horizontalInput *= Mathf.Pow(0.5f, Time.deltaTime * stopSpeed);
        }
        else
        {
            horizontalInput = 0; // Zero out the input to prevent drift
        }
        CharacterPositionChanged?.Invoke(rb.position);
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        CharacterPositionChanged?.Invoke(rb.position);
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
        PlayerAttack.ShootProjectile();
        PlayerAnimator.SetBool("is_shooting", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;
        PlayerAnimator.SetBool("is_jumping", false);
    }


    public void EndGame()
    {
        GameEnded?.Invoke();
    }
}
