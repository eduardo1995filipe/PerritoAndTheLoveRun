using System.Collections;
using UnityEngine;

/// <summary>
/// Classe usada para definir o comportamento dos Tiles que deixam
/// de ficar estaticos apos o jogador os pisar
/// </summary>
public class BreakableBehaviour : MonoBehaviour
{

    private Vector2 originalPosition; //posicao original do tile. usada para reposicao do tile no sitio original

    private float fallDelay; //atraso da queda do tile apos ser pisado pelo jogador

    private bool isFalling = false; 

    private Rigidbody2D rb;

    private void Start()
    {
        //o atraso da queda varia conforme o nivel usado
        int currentLevel = GameManager.instance.getCurrentLevel();
        switch (currentLevel)
        {
            case 1:
                fallDelay = 0.2f;
                break;
            case 2:
            case 3:
                fallDelay = 0.15f;
                break;
            default: break;
        }

        originalPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Apos ficar invisivel no ecra a sua posicao volta ao normal
    /// </summary>
    private void OnBecameInvisible()
    {
        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            Invoke("ResetPosition", 1.5f);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && transform.position.y < collision.gameObject.transform.position.y)
        {
            //bloco que previne a repeticao de som. apos o tile estar em queda o som ira apenas ser tocado uma vez
            if (!isFalling)
            {
                isFalling = true;
                GetComponent<AudioSource>().Play();
                StartCoroutine(fall());

            }
        }
    }

    /// <summary>
    /// Apos o atraso definido em cima o RigidBody2D sera 
    /// transformado em dinamico, pelo que ira cair
    /// </summary>
    private IEnumerator fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    /// <summary>
    /// Funcao que mete o corpo rigido do tile de dinamico para estatico.
    /// Volta a coloca-lo na sua posicao original
    /// </summary>
    private void ResetPosition()
    {
        rb.bodyType = RigidbodyType2D.Static;
        transform.position = originalPosition;
        isFalling = false;
    }
}
