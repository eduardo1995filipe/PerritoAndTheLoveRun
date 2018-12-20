using System.Collections;
using UnityEngine;

/// <summary>
/// Classe que define o comportamento do bonus de pontos
/// </summary>
public class ScoreBonusBehaviour : MonoBehaviour
{

    public float scoreBonusTime = 3f; //tempo de bonus
    private new Renderer renderer;
    private new BoxCollider2D collider;

    private void Start()
    {
        this.renderer = gameObject.GetComponent<Renderer>();
        this.collider = gameObject.GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerSounds sound = collision.gameObject.GetComponent<PlayerSounds>();
            sound.PlayScoreBonus();

            int newScoreIncrease;
            do
            {
                newScoreIncrease = ((int)(Random.Range(1f, 20f) + 0.5f)) * 10;
            }
            while (newScoreIncrease == GameManager.instance.GetScoreIncrease());

            //o collider e o renderer tem de ser desativados e nao destruidos enquanto o bonus esta ativo
            //a sua destruicao nesta fase iria causar tambem a anulacao do bonus, sem esperar pelos devidos 3 segundos
            renderer.enabled = false;
            collider.enabled = false;

            StartCoroutine(changeScoreIncrease(newScoreIncrease));
        }
    }

    /// <summary>
    /// rotina que troca o valor de cada moeda ganha e espera pelo 
    /// tempo de bonus e depois volta ao valor normal
    /// </summary>
    private IEnumerator changeScoreIncrease(int newScoreIncrease)
    {
        print("score bonus, current point gain -> " + newScoreIncrease);
        int oldScoreIncrease = GameManager.instance.GetScoreIncrease();

        GameManager.instance.SetScoreIncrease(newScoreIncrease);
        yield return new WaitForSeconds(scoreBonusTime);
        GameManager.instance.SetScoreIncrease(oldScoreIncrease);
        print("Points will return to normal");
        Destroy(gameObject, scoreBonusTime);
    }
}
