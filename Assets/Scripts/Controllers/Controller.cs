using System.Diagnostics;
using UnityEngine;

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
        view.ShowMainMenu();
        this.ClickStartGameButton();
    }

    void ClickStartGameButton()
    {
        model.StartGame();
    }

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if (verticalInput > 0 && (horizontalInput == 0 || Mathf.Abs(horizontalInput) < Mathf.Abs(verticalInput)))
        {
            Vector2 moveDirection = new Vector2(0, verticalInput);
            model.UserClickedMovementKey(moveDirection.normalized);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            model.UserClickedAttackKey();
    }
    }
}
