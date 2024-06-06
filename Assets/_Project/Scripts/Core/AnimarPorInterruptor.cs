using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SpriteRenderer))]
public class AnimarPorInterruptor : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite spriteActivo;
    [SerializeField] private Sprite spriteApagado;

    [Header("Componentes")]
    [SerializeField] private ModeloInterruptor interruptorAValidar;
    
    private SpriteRenderer _sprite;

    private void OnEnable()
    {
        interruptorAValidar.OnEncender += EstablecerSpriteAdecuado;
    }

    private void OnDisable()
    {
        interruptorAValidar.OnEncender -= EstablecerSpriteAdecuado;
    }

    private void OnValidate()
    {
        if (TryGetComponent(out ModeloInterruptor interruptor))
            interruptorAValidar = interruptor;

        if (TryGetComponent(out ModeloMecanismo mecanismo))
            interruptorAValidar = mecanismo.Interruptor;

        if (TryGetComponent(out SpriteRenderer sprite))
            _sprite = sprite;
        
        if(interruptorAValidar)
            EstablecerSpriteAdecuado(interruptorAValidar.encendidoPorDefecto);
    }

    private void Start()
    {
        EstablecerSpriteAdecuado(interruptorAValidar.Encendido); 
    }

    private void EstablecerSpriteAdecuado(object sender, ModeloInterruptor.ArgumentosInterruptor e)
    {
        EstablecerSpriteAdecuado(e.encendido);
    }

    private void EstablecerSpriteAdecuado(bool valor)
    {
        _sprite.sprite = valor ? spriteActivo : spriteApagado;
    }
}
