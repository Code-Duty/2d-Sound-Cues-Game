using UnityEngine;

// Classe GameStateManager que implementa a interface IGameStateManager
public class GameStateManager : MonoBehaviour, IGameStateManager
{
    // Variável para armazenar o estado atual do jogo
    private string currentState;

    // Define o estado atual do jogo
    public void SetState(string state)
    {
        currentState = state;
    }

    // Retorna o estado atual do jogo
    public string GetState()
    {
        return currentState;
    }
}
