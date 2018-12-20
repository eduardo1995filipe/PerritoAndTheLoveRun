using UnityEngine;

/// <summary>
/// Classe que contem os sons do player
/// </summary>
public class PlayerSounds : MonoBehaviour
{
    public AudioClip jumpSound;
    public AudioClip dieSound;
    public AudioClip coinSound;
    public AudioClip speedBonusSound;
    public AudioClip ScoreBonusSound;
    
    AudioSource audioSource;

    void Start ()
    {
        this.audioSource = GetComponent<AudioSource>();
	}

    public void PlayJump()
    {
        audioSource.clip = jumpSound;
        audioSource.Play();
    }

    public void PlayDie()
    {
        audioSource.clip = dieSound;
        audioSource.Play();
    }

    public void PlayCoin()
    {
        audioSource.clip = coinSound;
        audioSource.Play();
    }

    public void PlaySpeedBonus()
    {
        audioSource.clip = speedBonusSound;
        audioSource.Play();
    }
    
    public void PlayScoreBonus()
    {
        audioSource.clip = ScoreBonusSound;
        audioSource.Play();
    }
}
