using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Componentes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class ControlJugador : MonoBehaviour, IDamageable
{
    public event Action<ControlJugador> OnDeath;
    
    public enum NumeroJugador
    {
        J1, J2
    }

    [Header("Configuración")] 
    public NumeroJugador numeroJugador = NumeroJugador.J1;
    [SerializeField] private float velocidadMovimiento = 86;
    [SerializeField] private int vidaMaxima = 3;

    [Header("Teclas")] 
    public KeyCode teclaArriba = KeyCode.W;
    public KeyCode teclaIzquierda = KeyCode.A;
    public KeyCode teclaAbajo = KeyCode.S;
    public KeyCode teclaDerecha = KeyCode.D;
    public KeyCode teclaUsar = KeyCode.E;
    [SerializeField] private AudioClip sfxDamage;
    
    public bool IsDead => _vidas <= 0;
    public CircleCollider2D Collider { get; private set; }
    public Rigidbody2D Rigidbody { get; private set;}
    public Animator Animator { get; private set;}
    public TextMeshPro TextoDialogo { get; private set;}

    private Inventario _inventario;
    private IUsable _ultimoObjetoUsable;
    private float _vidas;

    private int Arriba => ValorDeTecla(teclaArriba);
    private int Izquierda => ValorDeTecla(teclaIzquierda);
    private int Abajo => ValorDeTecla(teclaAbajo);
    private int Derecha => ValorDeTecla(teclaDerecha);

    /// <summary>
    /// Dada una tecla, devuelve 1 si está siendo presionada, de lo contrario 0.
    /// Para esto se utiliza la condición ternaria: (booleano ? valor_si_verdadero : valor_si_falso)
    /// </summary>
    /// <param name="tecla">La tecla sobre la que queremos saber si está siendo presionada.</param>
    /// <returns>1 si está siendo presionada, de lo contrario 0.</returns>
    private int ValorDeTecla(KeyCode tecla) => Input.GetKey(tecla) ? 1 : 0;

    /// <summary>
    /// Devuelve un vector indicando la dirección a mover usando el input del usuario.
    /// El primer argumento es el eje X, las direcciones izquierda y derecha. El movimiento hacia la izquierda siempre es negativo.
    /// El segundo argumento es el eje Y, las direcciones arriba y abajo. El movimiento hacia abajo siempre es negativo.
    /// Se normaliza el resultado de dirección para que la velocidad no sea mayor cuando se mueva en diagonal por ejemplo.
    /// </summary>
    private Vector2 DireccionAMover => new Vector2(-Izquierda + Derecha, Arriba - Abajo).normalized;

    public void Revivir() => _vidas = vidaMaxima;
    
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Collider = GetComponent<CircleCollider2D>();
        Animator = GetComponent<Animator>();
        TextoDialogo = GetComponentInChildren<TextMeshPro>();
        _inventario = new Inventario(this);
        _vidas = vidaMaxima;
    }

    private void Update()
    {
        Animator.SetBool("dead", IsDead);
        if (IsDead) return;
        
        ComportamientoDeNavegacion();
        ComportamientoDeUsar();
        _inventario.HandleInventory();
    }

    /// <summary>
    /// Comportamiento general para navegar por el mundo.
    /// </summary>
    private void ComportamientoDeNavegacion()
    {
        // No queremos que el personaje se mueva mientras esté manteniendo presionada la tecla de usar.
        if (Input.GetKey(teclaUsar)) return;

        MoverEnDireccionA(DireccionAMover);
        AnimarControlador();
    }

    /// <summary>
    /// Comportamiento general para hacer uso de objetos.
    /// </summary>
    private void ComportamientoDeUsar()
    {
        if (Input.GetKeyDown(teclaUsar))
            UsarObjeto();

        if (Input.GetKeyUp(teclaUsar))
            DejarDeUsar();
    }

    private void UsarObjeto()
    {
        _ultimoObjetoUsable = ObtenerObjetoUsable();
        _ultimoObjetoUsable?.Usar(this);
    }

    private void DejarDeUsar()
    {
        _ultimoObjetoUsable?.DejarDeUsar(this);
    }
    

    /// <summary>
    /// Envía parámetros al animador tomando como referencia el input del usuario.
    /// </summary>
    private void AnimarControlador()
    {
        // Le proporcionamos al animador una velocidad de referencia
        Animator.SetFloat("speed", DireccionAMover.magnitude);
        
        // Cambia la dirección de los sprites únicamente si se está moviendo
        if (DireccionAMover != Vector2.zero)
        {
            Animator.SetFloat("x", DireccionAMover.x);
            Animator.SetFloat("y", DireccionAMover.y);
        }
    }

    /// <summary>
    /// Empuja el controlador en la dirección proporcionada usando la velocidad configurada desde el inspector.
    /// </summary>
    /// <param name="direccion">La dirección hacia la cual moverse.</param>
    private void MoverEnDireccionA(Vector2 direccion)
    {
        Rigidbody.velocity += direccion.normalized * velocidadMovimiento * Time.deltaTime;
    }
    
    private IUsable ObtenerObjetoUsable()
    {
        IUsable item = null;
        Collider2D[] castHit = Physics2D.OverlapCircleAll(transform.position, Collider.radius);
    
        foreach (Collider2D collider in castHit)
        {
            if (collider.TryGetComponent(out IUsable objetoUsable))
                item = objetoUsable;
        }

        return item;
    }

    private string PainDialog()
    {
        string[] textList = new string[] { "Ouch!", "Owie!", "Ah!", "Ouh!", "Aw!", "Ow!", "Ay!" };
        return textList[UnityEngine.Random.Range(0, textList.Length - 1)];
    }

    public void DealDamage()
    {
        _vidas -= 1;
        Animator.SetTrigger("damage");
        Rigidbody.velocity -= Rigidbody.velocity * 4;
        AudioSource.PlayClipAtPoint(sfxDamage, transform.position);

        if (IsDead)
        {
            OnDeath?.Invoke(this);
            return;
        }
        
        Decir(PainDialog(), 0.5f);
    }

    public void Decir(string texto, float delay)
    {
        StartCoroutine(MostrarDialogo(texto, delay));
    }

    private IEnumerator MostrarDialogo(string texto, float delay)
    {
        TextoDialogo.text = texto;
        yield return new WaitForSeconds(delay);
        TextoDialogo.text = string.Empty;
    }
}
