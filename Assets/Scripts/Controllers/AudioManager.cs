using System.Collections.Generic;
using UnityEngine;

// Classe AudioManager que implementa a interface IAudioManager
public class AudioManager : MonoBehaviour, IAudioManager
{
    // Dicionário para armazenar os AudioSources disponivéis 
    private Dictionary<string, AudioSource> audioSources = new Dictionary<string, AudioSource>();

    // Método Awake é chamado quando o script é iniciado
    private void Awake()
    {
        // Inicia o dicionário com os AudioSources encontrados nos filhos deste GameObject
        foreach (AudioSource source in GetComponentsInChildren<AudioSource>())
        {
            audioSources[source.clip.name] = source;
        }
    }

    // Implementação do método PlaySound da interface IAudioManager
    public void PlaySound(string soundName)
    {
        //Verifica se o som existe e reproduz
        if (audioSources.ContainsKey(soundName))
        {
            audioSources[soundName].Play();
        }
    }

    // Implementação do método StopSound da Interface IAudioManager
    public void StopSound(string soundName)
    {
        // Verifica se o som existe e dá stop no som
        if (audioSources.ContainsKey(soundName))
        {
            audioSources[soundName].Stop();
        }
    }
}
