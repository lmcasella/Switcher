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
    public event Action<ControlJugador> OnUse;
    public event Action<ControlJugador> OnStopUsing;
    
    public enum NumeroJugador
    {
        J1, J2
    }

    [Header("Configuración")] 
    public NumeroJugador numeroJugador = NumeroJugador.J1;
    [SerializeField] private float velocidadMovimiento = 86;
    [SerializeField] private int vidaMaxima = 3;
    [SerializeField] private float tiempoRequeridoParaSoltar = 1;

    [Header("Teclas")] 
    [SerializeField] private KeyCode teclaArriba = KeyCode.W;
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
    
    private Item _item;
    private Slider _UISliderReleaseIndicator;
    
    private float _vidas;

    private int Arriba => ValorDeTecla(teclaArriba);
    private int Izquierda => ValorDeTecla(teclaIzquierda);
    private int Abajo => ValorDeTecla(teclaAbajo);
    private int Derecha => ValorDeTecla(teclaDerecha);

    public float ReleaseTime { get; private set; }
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

    public void Revivir() => _vidas = vidaMaxima;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _textoDialogo = GetComponentInChildren<TextMeshPro>();
        _UISliderReleaseIndicator = GetComponentInChildren<Slider>();
        _vidas = vidaMaxima;
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
        #region ITEM HANDLING
        bool holdingUseKey = Input.GetKey(teclaUsar);
        _UISliderReleaseIndicator.gameObject.SetActive(_item && holdingUseKey);
        if (holdingUseKey)
        {
            _UISliderReleaseIndicator.value = ReleaseTime;
            ReleaseTime += Time.deltaTime;
        }
        else if (ReleaseTime != 0)
            ReleaseTime = 0;
        #endregion

        if (Input.GetKeyDown(teclaUsar))
            UsarObjeto();

        if (Input.GetKeyUp(teclaUsar))
            DejarDeUsar();

        if (ReleaseTime > tiempoRequeridoParaSoltar)
            SoltarItem();
    }

    private void UsarObjeto()
    {
        _ultimoObjetoUsable = ObtenerObjetoUsable();
        _item = ObtenerItem();
        _ultimoObjetoUsable?.Usar(this);
    }

    private void DejarDeUsar()
    {
        _UISliderReleaseIndicator.value = ReleaseTime;
        _ultimoObjetoUsable?.DejarDeUsar(this);
    }

    private void SoltarItem()
    {
        _UISliderReleaseIndicator.value = ReleaseTime;
        _item?.Soltar();
        _item = null;
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
    
    private Item ObtenerItem()
    {
        Item item = null;
        Collider2D[] castHit = Physics2D.OverlapCircleAll(transform.position, _collider.radius);
    
        foreach (Collider2D collider in castHit)
        {
            if (collider.TryGetComponent(out Item objetoUsable))
                item = objetoUsable;
        }

        return item;
    }
    
    private IUsable ObtenerObjetoUsable()
    {
        IUsable item = null;
        Collider2D[] castHit = Physics2D.OverlapCircleAll(transform.position, _collider.radius);
    
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
        _animator.SetTrigger("damage");
        _rigidbody.velocity -= _rigidbody.velocity * 4;
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
        _textoDialogo.text = texto;
        yield return new WaitForSeconds(delay);
        _textoDialogo.text = string.Empty;
    }
}
