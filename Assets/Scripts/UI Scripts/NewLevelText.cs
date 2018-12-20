using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

/// <summary>
/// 
/// Classe que gere o que e mostrado no ecra apos chegar ao fim do nivel. 
/// Sera mostrado um anuncio no caso do jogador nao tiver chegado ao fim do ultimo nivel
/// 
/// </summary>
public class NewLevelText : MonoBehaviour
{
    
    private Text nextLevelText; 
    public GameObject nextLevelButton; //botao onde se carrega para ir para o nivel seguinte

    public AudioClip gameCompletedSound;
    public AudioClip levelCompletedSound;

    
    void Start ()
    {
        this.nextLevelText = GetComponent<Text>();

        int key = GameManager.instance.getCurrentLevel(); //onde e obtido o nivel atual

            //caso o nivel atual for maior que tres significa
            //que o jogo chegou ao fim visto que o jogo apenas 
            //tem 3 niveis  
            if (key > 3)
            {

                nextLevelButton.SetActive(false); //botao apenas interessa ser mostrado no caso de o jogo nao estar completo
                GetComponent<AudioSource>().clip = gameCompletedSound;
                GetComponent<AudioSource>().loop = true;
                GetComponent<AudioSource>().Play();
                nextLevelText.text = "Game Complete!!! You could escape! Now you can live with Tortilla and your lovely sons for the rest of your life! Be Happy!";
                Invoke("QuitGame", 3f);
            }
            else
            {
            
                nextLevelButton.SetActive(false);
                StartCoroutine(ShowAdWhenReady());
            
                GetComponent<AudioSource>().clip = levelCompletedSound;
                GetComponent<AudioSource>().loop = false;
                GetComponent<AudioSource>().Play();
                nextLevelText.text = "Good Job!!! Click on the button below after the add appears so you can go to level " + key + " !";
            }
	}

    /// <summary>
    /// Rotina que mostra o anuncio e, por tras, o botao para
    /// ir para o nivel seguinte
    /// </summary>
    /// <returns>
    /// devolve nulo se o anuncio nao tiver pronto
    /// </returns>
    IEnumerator ShowAdWhenReady()
    {
        while (!Advertisement.IsReady("video"))
            yield return null;

        Advertisement.Show("video", new ShowOptions() { resultCallback = HandleAdResults });

        nextLevelButton.SetActive(true);
    }

    private void HandleAdResults(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Failed:
                print("ad failed to play");
                break;
            case ShowResult.Finished:
                print("ad seen until the end");
                break;
            case ShowResult.Skipped:
                print("ad skipped by the user");
                break;
            default:
                print("something is going on...");
                break;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
