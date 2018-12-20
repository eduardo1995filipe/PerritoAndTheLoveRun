using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

/// <summary>
/// Classe onde estao descritos todos os comportamentos do jogador
/// </summary>
public class PlayerBehaviour : MonoBehaviour
{

    public float movementSpeed; //movimento do player
    public float jumpSpeed; //salto do player
    public Text lostLifeText; //texto que e mostrado quando perde uma vida
    public AudioSource musicSource; //musica de fundo enquando se joga

    private float fallMultiplier = 6f; //velocidade de queda do salto do jogador
    private float lowJumpMultiplier = 4.5f; //valor que provoca o abrandamento do salto quando ate este comecar a cair
    private bool wasJumpPressed = false; //previne o duplo salto
    private bool jumpInput = false; //se ha input de salto
    private Rigidbody2D playerRigidBody;
    private bool isOnSpikes = false; //to prevent he hits two spike tiles and prevents two lives loss
    
    void Start()
    {
        //mete a musica de fundo a tocar
        if (!musicSource.isPlaying)
        {
            musicSource.loop = true;
            musicSource.Play();
        }

        transform.position = GameManager.instance.getSpawnPos();
        movementSpeed = GameManager.instance.getMovementSpeed();
        jumpSpeed = GameManager.instance.getJumpSpeed();
        playerRigidBody = gameObject.GetComponent<Rigidbody2D>();
        lostLifeText = GameObject.Find("LostLife").GetComponent<Text>();
    }

    private void Update()
    {
        //apenas usado no controlo de computador
        //if (Input.GetKeyUp(KeyCode.Space))
        //{
        //    wasJumpPressed = false;
        //}
    }

    void FixedUpdate()
    {

        //caso o RigidBody estiver em modo sleeping
        if ((playerRigidBody != null) && playerRigidBody.IsSleeping())
        {
            playerRigidBody.WakeUp();
        }

        //controlos usados com o botao que aparece no ecra
        //controlo de computador <space> e mais fiavel. basta comentar estas linhas
        //e tirar o comentario das linhas do Update e as abaixo referidas

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            jumpInput = true;
        }

        if (CrossPlatformInputManager.GetButtonUp("Jump"))
        {
            jumpInput = false;
            wasJumpPressed = false;
        }

        //para o controlo com a tecla<space>
        //if (Input.GetButton("Jump"))
        //{
        //    jumpInput = true;
        //}
        //else
        //{
        //    jumpInput = false;
        //}

        //movimento horizontal
        if (GameManager.instance.isAlive() && gameObject && !isOnSpikes)
        {
            transform.position = new Vector2(transform.position.x + (movementSpeed * Time.deltaTime), transform.position.y);
            
        }

        

        //raycast que deteta se ha algo a direita do objeto
        handleRightTouch();
    
        //movimento aereo, faz a trajetoria do salto mais perfeita(para um jogo)
        //cai com mais aceleracao do que a que tem quando salta para cima
        if (playerRigidBody.velocity.y < 0)
            {
                playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (playerRigidBody.velocity.y > 0 && !jumpInput && !IsGrounded())
            {
                playerRigidBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }

        if (jumpInput && !wasJumpPressed)
        {
            wasJumpPressed = true; //previne o salto continuo
            if (IsGrounded())
            {
                playerRigidBody.velocity = Vector2.up * jumpSpeed;
                gameObject.GetComponent<PlayerSounds>().PlayJump();
            }
        }        
    }

    /// <summary>
    /// Sempre que o objeto detetado no raycast for 
    /// um tile com o tag "Ground" o jogador perde uma vida
    /// </summary>
    private void handleRightTouch()
    {
        float xOffset = 0.45f;
        float maxDistance = 0.06f;

        Vector2 rayStart = new Vector2(transform.position.x + xOffset, transform.position.y);

        Vector2 DebugRaydirection = new Vector2(maxDistance,0);

        Debug.DrawRay(rayStart, DebugRaydirection, Color.cyan);

        int rightObjectDetection = 1 << LayerMask.NameToLayer("SolidPlatforms");

        RaycastHit2D hit = Physics2D.Raycast(
            rayStart,
            Vector2.right,
            maxDistance,
            rightObjectDetection);
        
        if (hit.collider)
        {
            if (hit.collider.gameObject.CompareTag("Ground"))
            {
                LooseLife();
                return;
            }
        }
    }

    /// <summary>
    /// mostra durante um tempo o texto a dizer quantas vidas faltam
    /// </summary>
    public IEnumerator showNewLifeText()
    {
        lostLifeText.text = "Life Lost! :( " + GameManager.instance.numLives + " lives left";
        yield return new WaitForSeconds(1.5f);
        lostLifeText.text = "";
    }

    /// <summary>
    /// Raycasts que detetam se estou a tocar no chao ou numa plataforma que cai
    /// </summary>
    private bool IsGrounded()
    {
        float xOffset = 0.49f;
        float yOffset = 0.45f;
        float maxDistance = 0.15f;

        Vector2 leftRayStart = new Vector2(transform.position.x - xOffset, transform.position.y - yOffset);
        Vector2 rightRayStart = new Vector2(transform.position.x + xOffset, transform.position.y - yOffset);

        Vector2 DebugRaydirection = new Vector2(0, -maxDistance);

        Debug.DrawRay(leftRayStart, DebugRaydirection, Color.cyan);
        Debug.DrawRay(rightRayStart, DebugRaydirection, Color.cyan);

        int groundDetection = 1 << LayerMask.NameToLayer("SolidPlatforms");
        int breakableDetection = 1 << LayerMask.NameToLayer("Breakables");

        RaycastHit2D leftHit = Physics2D.Raycast(
            leftRayStart,
            Vector2.down,
            maxDistance,
            groundDetection | breakableDetection);

        RaycastHit2D rightHit = Physics2D.Raycast(
            rightRayStart,
            Vector2.down,
            maxDistance,
            groundDetection | breakableDetection);

        try
        {
            if (leftHit.collider)
            {
                return leftHit.collider.tag == "Ground"  || leftHit.collider.tag == "Breakable";
            }

            if (rightHit.collider)
            {
                return leftHit.collider.tag == "Ground" || leftHit.collider.tag == "Breakable";
            }
        }
        catch (System.NullReferenceException e)
        {
            print(e.ToString());
            return false;
        }
        return false;
    }
    
    /// <summary>
    /// Procedimento de perder uma vida
    /// o jogador volta a posicao base do nivel e perde os bonus caso os tenha
    /// </summary>
    public void LooseLife()
    {
        transform.position = GameManager.instance.getSpawnPos();
        movementSpeed = GameManager.instance.getMovementSpeed();
        jumpSpeed = GameManager.instance.getJumpSpeed();
        GameManager.instance.newLife(); //nova vida, mostrar texto e parar um pouco
        if (GameManager.instance.numLives != 0)
        {
            StartCoroutine(showNewLifeText());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Ground") && IsGrounded())
        {
            //tira a velocidade vertical apos queda
            //previne(nao totalmente, diminui a frequencia do bug) em que o objeto em queda penetra 
            //as plataformas, o que faz o jogador perder o jogo
            playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, 0);
        }

        if (collision.collider.gameObject.CompareTag("Spikes"))
            if (!isOnSpikes &&
                    (transform.position.y) >= collision.collider.transform.position.y)
            {
                isOnSpikes = true; //valor permite apenas a perda de uma vida, mesmo no caso do jogar estiver por cima de dois tiles de "spikes"
                LooseLife();
            }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // para a musica de fundo e carrega a cena de novo nivel
        if (collision.gameObject.CompareTag("EndOfLevel"))
        {
            if (musicSource.isPlaying)
            {
                musicSource.Stop();
            }
            int currentLevel = GameManager.instance.getCurrentLevel();
            int newLevel = currentLevel + 1;

            print("Current level --> " + currentLevel);
            print("New Level --> " + newLevel);

            GameManager.instance.setLevel(newLevel);
            SceneManager.LoadScene("NextLevelScene");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {

        if (collision.collider.gameObject.CompareTag("Spikes"))
        {
            isOnSpikes = false;
        }
    }
}
