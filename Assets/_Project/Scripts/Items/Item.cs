using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour, IUsable
{
    private ControlJugador _jugador;
    
    public void Usar(ControlJugador usuario)
    {
        _jugador = usuario;
    }

    public void DejarDeUsar(ControlJugador usuario)
    {
        
    }

    public void Soltar()
    {
        transform.DOJump(_jugador.transform.position, 1, 1, 0.5f);
        _jugador = null;
    }

    private void Update()
    {
        if (_jugador)
            transform.position = Vector2.Lerp(transform.position, (Vector2)_jugador.transform.position + new Vector2(0.5f, 0.5f), Time.deltaTime*10);
    }
}
