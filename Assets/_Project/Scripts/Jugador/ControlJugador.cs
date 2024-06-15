using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Componentes;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(Animator))]
public class ControlJugador : MonoBehaviour, IDamageable
{
    [Header("Configuración")] [SerializeField]
    private float velocidadMovimiento = 86;

    [Header("Teclas")] [SerializeField] private KeyCode teclaArriba = KeyCode.W;
    [SerializeField] private KeyCode teclaIzquierda = KeyCode.A;
    [SerializeField] private KeyCode teclaAbajo = KeyCode.S;
    [SerializeField] private KeyCode teclaDerecha = KeyCode.D;
    [SerializeField] private KeyCode teclaUsar = KeyCode.E;
    [SerializeField] private AudioClip sfxDamage;

    private Rigidbody2D _rigidbody;
    private CircleCollider2D _collider;
    private Animator _animator;
    private TextMeshPro _textoDialogo;
    private IUsable _ultimoObjetoUsable;
    private float _vidas = 3;

    private int Arriba => ValorDeTecla(teclaArriba);
    private int Izquierda => ValorDeTecla(teclaIzquierda);
    private int Abajo => ValorDeTecla(teclaAbajo);
    private int Derecha => ValorDeTecla(teclaDerecha);

    public bool IsDead => _vidas <= 0;

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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _textoDialogo = GetComponentInChildren<TextMeshPro>();
    }

    private void Update()
    {
        _animator.SetBool("dead", IsDead);
        if (IsDead) return;
        
        ComportamientoDeNavegacion();
        ComportamientoDeUsar();
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
        {
            _ultimoObjetoUsable = ObtenerObjetoUsable();
            _ultimoObjetoUsable?.Usar();
        }

        if (Input.GetKeyUp(teclaUsar))
            _ultimoObjetoUsable?.DejarDeUsar();
    }

    /// <summary>
    /// Envía parámetros al animador tomando como referencia el input del usuario.
    /// </summary>
    private void AnimarControlador()
    {
        // Le proporcionamos al animador una velocidad de referencia
        _animator.SetFloat("speed", DireccionAMover.magnitude);
        
        // Cambia la dirección de los sprites únicamente si se está moviendo
        if (DireccionAMover != Vector2.zero)
        {
            _animator.SetFloat("x", DireccionAMover.x);
            _animator.SetFloat("y", DireccionAMover.y);
        }
    }

    /// <summary>
    /// Empuja el controlador en la dirección proporcionada usando la velocidad configurada desde el inspector.
    /// </summary>
    /// <param name="direccion">La dirección hacia la cual moverse.</param>
    private void MoverEnDireccionA(Vector2 direccion)
    {
        _rigidbody.velocity += direccion.normalized * velocidadMovimiento * Time.deltaTime;
    }
    
    /// <summary>
    /// Obtiene el interruptor que se encuentre dentro del área del personaje.
    /// </summary>
    /// <returns>El interruptor que se encuentre dentro del área del personaje siempre y cuando ambos tengan un collider activo.</returns>
    private IUsable ObtenerObjetoUsable()
    {
        IUsable interruptor = null;
        Collider2D[] castHit = Physics2D.OverlapCircleAll(transform.position, _collider.radius);
    
        foreach (Collider2D collider in castHit)
        {
            if (collider.TryGetComponent(out IUsable objetoUsable))
                interruptor = objetoUsable;
        }

        return interruptor;
    }

    public void DealDamage()
    {
        _vidas -= 1;
        _animator.SetTrigger("damage");
        _rigidbody.velocity -= _rigidbody.velocity * 4;
        AudioSource.PlayClipAtPoint(sfxDamage, transform.position);

        if (IsDead) return;
        
        Decir("Ouch!", 0.5f);
    }

    public void Decir(string texto, float delay)
    {
        StartCoroutine(MostrarDialogo(texto, delay));
    }

    private IEnumerator MostrarDialogo(string texto, float delay)
    {
        _textoDialogo.text = texto;
        yield return new WaitForSeconds(delay);
        _textoDialogo.text = string.Empty;
    }
}
