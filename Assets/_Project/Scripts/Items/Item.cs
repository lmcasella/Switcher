using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour, IUsable
{
    private ControlJugador _jugador;
    private Vector2 _lastPosition;
    
    public void Usar(ControlJugador usuario)
    {
        _jugador = usuario;
    }

    public void DejarDeUsar(ControlJugador usuario)
    {
        
    }

    public void Soltar()
    {
        transform.DOJump(_jugador.transform.position, 1, 1, 0.5f).OnComplete(() =>
        {
            _lastPosition = transform.position;
            _jugador = null;
        });
    }

    private void Start()
    {
        _lastPosition = transform.position;
    }

    private void Update()
    {
        if (_jugador)
            transform.position = Vector2.Lerp(transform.position, (Vector2)_jugador.transform.position + new Vector2(0.5f, 0.5f), Time.deltaTime*10);
        else
            HoveringEffect();
    }

    private void HoveringEffect()
    {
        float oscillationValue = Mathf.Sin(Time.time) * 0.25f;
        Vector2 targetPosition = new Vector2(_lastPosition.x, _lastPosition.y + oscillationValue);
        transform.position = Vector2.Lerp(transform.position, targetPosition, Time.deltaTime * 10);
    }
}
