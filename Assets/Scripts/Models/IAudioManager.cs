// Interface para gerar o áudio no jogo
public interface IAudioManager
{
    // Método para reproduzir um som
    void PlaySound(string soundName);

    // Método para parar a reprodução de um som
    void StopSound(string soundName);
}
