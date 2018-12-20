using System.Collections;
using UnityEngine;

/// <summary>
/// Classe que define o comportamento do bonus de velocidade
/// </summary>
public class SpeedBonusBehaviour : MonoBehaviour
{

    private float speedBonusTime = 2.5f; //tempo de bonus
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
            PlayerBehaviour behaviour = collision.gameObject.GetComponent<PlayerBehaviour>();
            PlayerSounds sound = collision.gameObject.GetComponent<PlayerSounds>();
            sound.PlaySpeedBonus();

            float newSpeed;
            float oldSpeed = behaviour.movementSpeed;

            do
            {
                newSpeed = (float)((int)(Random.Range(1f, 8f) + 0.5f));
            }
            while (newSpeed == GameManager.instance.getMovementSpeed());
            
            //o collider e o renderer tem de ser desativados e nao destruidos enquanto o bonus esta ativo
            //a sua destruicao nesta fase iria causar tambem a anulacao do bonus, sem esperar pelos devidos 2.5 segundos
            renderer.enabled = false;
            collider.enabled = false;

            StartCoroutine(changeSpeed(newSpeed, oldSpeed, behaviour));
        }
    }

    /// <summary>
    /// rotina que troca de velocidade e espera pelo 
    /// tempo de bonus e depois volta a velocidade normal
    /// </summary>
    private IEnumerator changeSpeed(float newSpeed, float oldSpeed, PlayerBehaviour behaviour)
    {
        print("speed bonus, current point gain -> " + newSpeed);
        behaviour.movementSpeed = newSpeed;
        yield return new WaitForSeconds(speedBonusTime);
        behaviour.movementSpeed = oldSpeed;
        print("speed will return to normal");
        Destroy(gameObject, speedBonusTime);
    }
}
