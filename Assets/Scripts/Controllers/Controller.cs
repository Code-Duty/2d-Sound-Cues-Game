using System;
using UnityEngine;
using UnityEngine.UI;

// Classe Controller que gera a lógica do jogo
public class Controller : MonoBehaviour
{
    // Referências para os componentes Model e View
    public Model model;
    public View view;
    private IAudioManager audioManager;
    private IScoreManager scoreManager;
    private IGameStateManager gameStateManager;

    void Start()
    {
        // Inicializa os componentes
        model = GetComponent<Model>();
        view = GetComponent<View>();
        view.Initialize(model);

        audioManager = FindObjectOfType<AudioManager>();
        scoreManager = FindObjectOfType<ScoreManager>();
        gameStateManager = FindObjectOfType<GameStateManager>();

        // Redefine a pontuação e define o estado inicial do jogo
        scoreManager.ResetScore();
        gameStateManager.SetState("Playing");

        // Inicia o programa
        StartProgram();
    }

    // Método para iniciar o programa
    void StartProgram()
    {
        try
        {
            view.ShowMainMenu();
            ClickStartGameButton();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    // Método chamado quando o botão de iniciar jogo é clicado
    public void ClickStartGameButton()
    {
        Debug.Log("startGameButton");
        model.StartGame();
        audioManager.PlaySound("game_start");
        gameStateManager.SetState("InGame");
    }

    // Método Update é chamado a cada frame
    void Update()
    {
        HandleInput();
    }

    // Método para lidar com a entrada do usuário
    void HandleInput()
    {
        if (gameStateManager.GetState() != "InGame")
            return;

        float horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            model.UserClickedMovementKey(horizontalInput);
            audioManager.PlaySound("footstep");
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            model.UserClickedJumpKey();
            audioManager.PlaySound("jump");
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            model.UserClickedAttackKey();
            audioManager.PlaySound("attack");
        }
    }
}
