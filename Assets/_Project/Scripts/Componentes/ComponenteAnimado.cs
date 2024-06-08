using System;
using System.Collections;
using System.Collections.Generic;
using Componentes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpriteRenderer), typeof(ComponenteBinario))]
public class ComponenteAnimado : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite spriteActivo;
    [SerializeField] private Sprite spriteApagado;

    [Header("Config")] 
    [SerializeField] private bool detectarAlPresionar = true;
    [SerializeField] private bool detectarAlEncender = true;
    [SerializeField] private bool detectarAlApagar = true;

    private ComponenteBinario _interruptorAValidar;
    
    private SpriteRenderer _sprite;

    private void OnEnable()
    {
        if(detectarAlPresionar)
            _interruptorAValidar.OnPresionar += EstablecerSpriteAdecuado;
        
        if(detectarAlEncender)
            _interruptorAValidar.OnEncender += EstablecerSpriteAdecuado;
        
        if(detectarAlApagar)
            _interruptorAValidar.OnApagar += EstablecerSpriteAdecuado;
    }

    private void OnDisable()
    {
        _interruptorAValidar.OnPresionar -= EstablecerSpriteAdecuado;
        _interruptorAValidar.OnEncender -= EstablecerSpriteAdecuado;
        _interruptorAValidar.OnApagar -= EstablecerSpriteAdecuado;
    }
    
    private void EstablecerSpriteAdecuado(object sender, EventArgs e)
    {
        ComponenteBinario c = (ComponenteBinario)sender;
        EstablecerSpriteAdecuado(c.Encendido);
    }

    private void EstablecerSpriteAdecuado(bool valor)
    {
        _sprite.sprite = valor ? spriteActivo : spriteApagado;
    }
    
    private void OnValidate()
    {
        if (TryGetComponent(out ComponenteBinario interruptor))
            _interruptorAValidar = interruptor;

        if (TryGetComponent(out SpriteRenderer sprite))
            _sprite = sprite;
        
        if(_interruptorAValidar)
            EstablecerSpriteAdecuado(_interruptorAValidar.Encendido);
    }
}
