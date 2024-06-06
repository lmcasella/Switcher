using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ModeloMecanismoPrueba : ModeloMecanismo
{
    SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void EstadoActivo()
    {
        _spriteRenderer.color = Color.green;
    }

    public override void EstadoInactivo()
    {
        _spriteRenderer.color = Color.red;
    }
}