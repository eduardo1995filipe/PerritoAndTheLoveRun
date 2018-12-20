using UnityEngine;

/// <summary>
/// Classe usada apenas para correr o som de quando o jogador perde as vidas todas
/// </summary>
public class GameOverSound : MonoBehaviour
{
    void Start ()
    {
        GetComponent<AudioSource>().Play();
	}
}
