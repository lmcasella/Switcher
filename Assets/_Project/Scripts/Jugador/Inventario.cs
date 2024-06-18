using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ControlJugador))]
public class Inventario
{
    public event Action<ControlJugador> OnPickup;
    public event Action<ControlJugador> OnDrop;

    public float tiempoRequeridoParaSoltar = 1;
    
    private ControlJugador _usuario;
    private Item _item;
    private Slider _UISliderReleaseIndicator;
    
    public float ReleaseTime { get; private set; }
    private bool IsHoldingUseKey => Input.GetKey(_usuario.teclaUsar);

    public Inventario(ControlJugador usuario)
    {
        _usuario = usuario;
        _UISliderReleaseIndicator = _usuario.GetComponentInChildren<Slider>();
    }
    
    public void HandleInventory()
    {
        _UISliderReleaseIndicator.gameObject.SetActive(_item && IsHoldingUseKey);
        if (IsHoldingUseKey) // Count time while is pressing the key
        {
            _UISliderReleaseIndicator.value = ReleaseTime;
            ReleaseTime += Time.deltaTime;
        }
        else if (ReleaseTime != 0) // Once it's released, reset the time.
            ReleaseTime = 0;
        
        if (ReleaseTime > tiempoRequeridoParaSoltar)
            AccionSoltarItem();
        
        if(Input.GetKeyDown(_usuario.teclaUsar))
            AccionTomarItem();
    }

    private void AccionTomarItem()
    {
        Item nuevoItem = ObtenerItem();
        if(nuevoItem != _item)
            SoltarItem();
        _item = nuevoItem;
        _item?.Usar(_usuario);
        OnPickup?.Invoke(_usuario);
    }
    
    private void AccionSoltarItem()
    {
        _UISliderReleaseIndicator.value = ReleaseTime;
        SoltarItem();
        _item = null;
        OnDrop?.Invoke(_usuario);
    }

    private void SoltarItem() => _item?.Soltar();
    
    private Item ObtenerItem()
    {
        Item item = null;
        Collider2D[] castHit = Physics2D.OverlapCircleAll(_usuario.transform.position, _usuario.Collider.radius);
        foreach (Collider2D collider in castHit)
        {
            if (collider.TryGetComponent(out Item objetoUsable))
                item = objetoUsable;
        }

        return item;
    }
}