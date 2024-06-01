using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    public Model model;
    public View view;

    void Start()
    {
        // Assuming Model and View are attached to the same GameObject or properly linked
        model = GetComponent<Model>();
        view = GetComponent<View>();
        view.Initialize(model);

        StartProgram();
    }

    void StartProgram()
    {
        try
        {
            view.ShowMainMenu();
            //Currently not working, fix to be applied
            //Button startGameButton = GameObject.Find("StartGameButton").GetComponent<Button>();
            //startGameButton.onClick.AddListener(ClickStartGameButton);
            //UnityEngine.Debug.Log(startGameButton.ToString());
            ClickStartGameButton();
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogException(ex);
        }
    }

    public void ClickStartGameButton()
    {
        UnityEngine.Debug.Log("startGameButton");
        model.StartGame();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            model.UserClickedMovementKey(horizontalInput);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            model.UserClickedJumpKey();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            model.UserClickedAttackKey();
        }
    }
}
