using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Compara dos interruptores y devuelve una señal usando la lógica AND
/// </summary>
public class InterruptorAND : ModeloInterruptor
{
    [SerializeField] private ModeloInterruptor A, B;
    
    private void OnEnable()
    {
        A.OnEncender += RecibirConexion;
        B.OnEncender += RecibirConexion;
    }
    
    private void OnDisable()
    {
        A.OnEncender -= RecibirConexion;
        B.OnEncender -= RecibirConexion;
    }
    
    private void RecibirConexion(object sender, ArgumentosInterruptor e)
    {
        Encender(A.Encendido && B.Encendido);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        MostrarConexionDeInterruptor(A);
        MostrarConexionDeInterruptor(B);
        Handles.color = Color.cyan;
        Handles.Label((Vector2)transform.position + Vector2.up*0.5f, "AND");
    }

    private void MostrarConexionDeInterruptor(ModeloInterruptor interruptor)
    {
        Gizmos.color = interruptor.Encendido ? Color.green : Color.red;
        Gizmos.DrawLine(interruptor.transform.position, transform.position);
    }
}