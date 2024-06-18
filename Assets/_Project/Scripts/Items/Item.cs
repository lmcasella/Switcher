using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        _jugador = null;
    }

    private void Update()
    {
        if (_jugador)
            transform.position = _jugador.transform.position;
    }
}
