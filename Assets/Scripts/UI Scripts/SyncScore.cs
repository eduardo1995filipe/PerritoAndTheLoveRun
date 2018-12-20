using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// classe que imprime no ecra a pontuacao do jogador
/// </summary>
public class SyncScore : MonoBehaviour
{

    private Text scoreText;
    
    void Start()
    {
        this.scoreText = GetComponent<Text>();
    }

    void Update()
    {
        scoreText.text = "Pontuation: " + GameManager.instance.score;
    }
}
