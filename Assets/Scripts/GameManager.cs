using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// singleton que gere o jogo
/// </summary>
public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public int numLives = 6; //numero de vidas
    public long score = 0; //pontuacao inicial

    public GameObject player;
    public Transform spawnPos1, spawnPos2, spawnPos3; //posicoes de spawn para os 3 niveis

    private bool isDead = false;
    private int scoreIncrease = 50; //incremendo por cada moeda

    public int currentLevel = 1; //nivel atual, comeca sempre no um

    //usado o awake porque e usado apenas uma vez, ao contrario do start
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //previne a destruicao dos objetos abaixo
            DontDestroyOnLoad(gameObject);
            DontDestroyOnLoad(spawnPos1);
            DontDestroyOnLoad(spawnPos2);
            DontDestroyOnLoad(spawnPos3);
            DontDestroyOnLoad(player);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        //vai buscar o unico player no caso de ele estar nulo
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        //se o numero de vidas estiver a zero vai ser carregada a cena do fim do jogo
        if (numLives == 0)
        {
            isDead = true;
            if (player.GetComponent<PlayerBehaviour>().musicSource.isPlaying)
            {
                player.GetComponent<PlayerBehaviour>().musicSource.Stop();
            }
            SceneManager.LoadScene("GameOverScene");
        }
    }

    internal bool isAlive()
    {
        return !isDead;
    }

    /// <summary>
    /// metodo usado para incrementar a pontuacao
    /// </summary>
    public void increaseScore()
    {
        player.GetComponent<PlayerSounds>().PlayCoin();
        score += scoreIncrease;
    }

    public void newLife()
    {
        player.GetComponent<PlayerSounds>().PlayDie();
        --numLives;
    }

    public void SetScoreIncrease(int newIncrease)
    {
        this.scoreIncrease = newIncrease;
    }

    public int GetScoreIncrease()
    {
        return this.scoreIncrease;
    }

    /// <summary>
    /// Devolve a posicao de spawn conforme o nivel em que esteja
    /// </summary>
    public Vector2 getSpawnPos()
    {
        switch (currentLevel)
        {
            case 1:
                return spawnPos1.position;

            case 2:
                return  spawnPos2.position;

            case 3:
                return  spawnPos3.position;

            default:
                return Vector2.zero;
        }
    }

    public void setLevel(int newLevel) 
    {
        this.currentLevel = newLevel;
    }

    public int getCurrentLevel() {

        return this.currentLevel;
    }
    
    public float getJumpSpeed()
    {
        switch (currentLevel)
        {
            case 1:
            case 2:
                return 8f;
            case 3:
                return 9f;
            default:
                return 8f;
        }
    }
    
    /// <summary>
    /// devolve a velocidade do jogador conforme o nivel em que esteja
    /// </summary>
    public float getMovementSpeed()
    {
        switch (currentLevel)
        {
            case 1:
                return 2.5f;
            case 2:
                return 4.5f;
            case 3:
                return 5.5f;
            default:
                return 2.5f;
        }
    }
}
