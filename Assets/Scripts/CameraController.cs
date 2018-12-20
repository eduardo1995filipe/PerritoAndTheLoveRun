using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe que faz a camara seguir o player 
/// </summary>
public class CameraController : MonoBehaviour 
{

    public GameObject player;       //guarda referencia para o player
    private Vector3 offset;        //variavel que guarda a distancia entre o jogador e a camara
    
    void Start()
    {
       this.offset = transform.position - player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
