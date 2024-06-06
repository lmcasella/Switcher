using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MecanismoPuerta : ModeloMecanismo
{
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    public override void EstadoActivo()
    {
        _collider.enabled = false;
    }

    public override void EstadoInactivo()
    {
        _collider.enabled = true;
    }
}
