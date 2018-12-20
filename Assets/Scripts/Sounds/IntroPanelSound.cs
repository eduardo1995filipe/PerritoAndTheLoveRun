using UnityEngine;

/// <summary>
/// Classe que faz tocar o som inicial 
/// </summary>
public class IntroPanelSound : MonoBehaviour
{

    private AudioSource audioSource;
    
	void Start ()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.Play();
    }

    private void OnDestroy()
    {
        if(audioSource.isPlaying)
            audioSource.Stop();
    }
}
