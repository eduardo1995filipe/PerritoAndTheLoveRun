using UnityEngine;

/// <summary>
/// Classe que define o comportamento das moedas.
/// Por cada trigger accionado a moeda ira ser destruida, e os pontos do jogador irao aumentar
/// </summary>
public class CoinBehaviour : MonoBehaviour {
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.increaseScore();
            Destroy(gameObject);
        }
    }
}
