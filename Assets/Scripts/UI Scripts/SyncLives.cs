using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script usado para mostrar o numero de vidas enquando se joga
/// </summary>
public class SyncLives : MonoBehaviour
{

    private Text livesText; 
    
	void Start ()
    {
        this.livesText = GetComponent<Text>();
	}
	
	void Update ()
    {
        livesText.text = "lives: " + GameManager.instance.numLives;
	}
}
