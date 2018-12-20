using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe introdutoria no jogo, onde apos o botao de comecar o jogo 
/// na "IntroScene" ser carregado ira carregar a cena do jogo("SampleScene")
/// </summary>
public class IntroScript : MonoBehaviour
{
    
    public void LoadGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
