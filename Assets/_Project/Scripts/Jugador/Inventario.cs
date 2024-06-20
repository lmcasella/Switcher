using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[System.Serializable]
[RequireComponent(typeof(ControlJugador))]
public class Inventario
{
    public event Action<ControlJugador> OnPickup;
    public event Action<ControlJugador> OnDrop;

    public float tiempoRequeridoParaSoltar = 1;
    
    public ControlJugador Usuario { get; private set; }
    public Item Item { get; private set; }
    private Slider _UISliderReleaseIndicator;
    
    public float ReleaseTime { get; private set; }
    public bool EstaLlevandoItem => Item;
    private bool IsHoldingUseKey => Input.GetKey(Usuario.teclaUsar);

    public Inventario(ControlJugador usuario)
    {
        this.Usuario = usuario;
        _UISliderReleaseIndicator = this.Usuario.GetComponentInChildren<Slider>();
    }
    
    public void HandleInventory()
    {
        _UISliderReleaseIndicator.gameObject.SetActive(Item && IsHoldingUseKey);
        if (IsHoldingUseKey) // Count time while is pressing the key
        {
            _UISliderReleaseIndicator.value = ReleaseTime;
            ReleaseTime += Time.deltaTime;
        }
        else if (ReleaseTime != 0) // Once it's released, reset the time.
            ReleaseTime = 0;

        if (ReleaseTime > tiempoRequeridoParaSoltar)
        {
            Item?.Utilizar(this);
            ReleaseTime = 0;
        }

        if(Input.GetKeyDown(Usuario.teclaUsar))
            AccionTomarItem();
    }

    public void AccionTomarItem()
    {
        Item nuevoItem = ObtenerItem();
        // No hacer nada si el item es el mismo que ya tenemos o intenta reemplazarlo por la misma nada.
        if (nuevoItem == Item || !nuevoItem) return;
        
        SoltarItem();
        Item = nuevoItem;
        Item?.Usar(Usuario);
        OnPickup?.Invoke(Usuario);
    }
    
    public void AccionSoltarItem()
    {
        _UISliderReleaseIndicator.value = ReleaseTime;
        SoltarItem();
        Item = null;
        OnDrop?.Invoke(Usuario);
    }

    private void SoltarItem() => Item?.Soltar();
    
    private Item ObtenerItem()
    {
        Item item = null;
        Collider2D[] castHit = Physics2D.OverlapCircleAll(Usuario.transform.position, Usuario.Collider.radius);
        foreach (Collider2D collider in castHit)
        {
            if (collider.TryGetComponent(out Item objetoUsable))
            {
                if (objetoUsable == Item) continue;
                item = objetoUsable;
            }
        }

        return item;
    }
}