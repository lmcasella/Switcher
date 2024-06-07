using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ModeloMecanismo : MonoBehaviour
{
    [Header("Configuración")]
    [Tooltip("Interruptor con el que se activa")] 
    [SerializeField] private ModeloInterruptor interruptorAUsar;
    [SerializeField] private bool invertido;

    public bool Activado { get; private set; }
    public bool Invertido => invertido;
    
    public ModeloInterruptor Interruptor => interruptorAUsar;
    
    private void OnEnable()
    {
        interruptorAUsar.OnEncender += Activar;
    }

    private void OnDisable()
    {
        interruptorAUsar.OnEncender -= Activar;
    }

    public void Activar(object sender, ModeloInterruptor.ArgumentosInterruptor argumentos)
    {
        Activado = invertido ? !argumentos.encendido : argumentos.encendido;
        if(Activado) EstadoActivo();
        else EstadoInactivo();
    }

    public virtual void EstadoActivo()
    {
    }

    public virtual void EstadoInactivo()
    {
    }

#if UNITY_EDITOR 
    // ^ Esto sirve para indicar que esta pieza de código no debe ser compilada, porque lo único que queremos es mostrar guías en el editor.
    private void OnDrawGizmos()
    { 
        GridSnapping.Snap(transform);
        
        // Si no hay un interruptor, no hay por qué renderizar un enlace.
        if (!interruptorAUsar) return;
        GizmosLineaDeConexion();
    }

    /// <summary>
    /// Renderiza una linea de conexión en el editor, debe usarse dentro de OnDrawGizmos() o OnDrawGizmosSelected()
    /// </summary>
    private void GizmosLineaDeConexion()
    {
        Gizmos.color = interruptorAUsar.Encendido ? Color.green : Color.red;
        Gizmos.DrawLine(interruptorAUsar.transform.position, transform.position);
    }
    #endif
}