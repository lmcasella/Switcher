using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ModeloInterruptor : MonoBehaviour, IInterruptor, IUsable
{
    public event EventHandler<ArgumentosInterruptor> OnEncender;

    public class ArgumentosInterruptor : EventArgs
    {
        public bool encendido;

        public ArgumentosInterruptor(bool encendido)
        {
            this.encendido = encendido;
        }
    }

    [Header("Configuración")]
    [Tooltip("Estado inicial del interruptor, por defecto está apagado.")]
    public bool encendidoPorDefecto = false;
    // [SerializeField] private float delayGeneral = 0;
    // [SerializeField] private float delayAlEncender = 0;
    // [SerializeField] private float delayAlApagar = 0;
    
    public bool Encendido { get; private set; }
    public bool Habilitado { get; private set; } = true;

    /// <summary>
    /// Hace uso del interruptor, activando todos los componentes conectados.
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    public virtual void Usar()
    {
        throw new NotImplementedException("Se está usando el interruptor pero no se declaró un comportamiento!");
    }

    /// <summary>
    /// Enciende o apaga el interruptor si su uso está habilitado.
    /// </summary>
    /// <param name="valor">Prender o apagar (bool)</param>
    public virtual void Encender(bool valor)
    {
        if (!Habilitado) return;
        
        Encendido = valor;
        ArgumentosInterruptor argumentos = new ArgumentosInterruptor(Encendido);
        OnEncender?.Invoke(this, argumentos);
    }

    /// <summary>
    /// Habilita o deshabilita el uso del interruptor, sin mutar su estado de encendido.
    /// </summary>
    /// <param name="valor">Habilitar o deshabilitar (bool)</param>
    public virtual void Habilitar(bool valor)
    {
        Habilitado = valor;
    }

    private void Start()
    {
        Encender(false);
    }

    protected virtual void OnDrawGizmos()
    {
        GridSnapping.Snap(transform);
    }
}