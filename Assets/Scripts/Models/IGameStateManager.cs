// Interface para gerar os estados do jogo
public interface IGameStateManager
{
    // Método para definir o estado atual do jogo
    void SetState(string state);

    // Método para obter o estado atual do jogo
    string GetState();
}
