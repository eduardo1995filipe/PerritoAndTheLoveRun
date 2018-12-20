using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Classe que contem uma funcao usada no botao para carregar para o proximo nivel
/// </summary>
public class NextLevelButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
