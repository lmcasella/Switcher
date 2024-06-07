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

    // [Header("Componentes")]
    private ModeloInterruptor _interruptorAValidar;
    private ModeloMecanismo _mecanismoAValidar;
    
    private SpriteRenderer _sprite;

    private void OnEnable()
    {
        _interruptorAValidar.OnEncender += EstablecerSpriteAdecuado;
    }

    private void OnDisable()
    {
        _interruptorAValidar.OnEncender -= EstablecerSpriteAdecuado;
    }

    private void OnValidate()
    {
        if (TryGetComponent(out ModeloInterruptor interruptor))
            _interruptorAValidar = interruptor;

        if (TryGetComponent(out ModeloMecanismo mecanismo))
        {
            _mecanismoAValidar = mecanismo;
            _interruptorAValidar = mecanismo.Interruptor;
        }

        if (TryGetComponent(out SpriteRenderer sprite))
            _sprite = sprite;
        
        if(_interruptorAValidar)
            EstablecerSpriteAdecuado(_interruptorAValidar.encendidoPorDefecto);
    }

    private void Start()
    {
        EstablecerSpriteAdecuado(_interruptorAValidar.Encendido); 
    }

    private void EstablecerSpriteAdecuado(object sender, ModeloInterruptor.ArgumentosInterruptor e)
    {
        if(_interruptorAValidar)
            EstablecerSpriteAdecuado(e.encendido);
        if(_mecanismoAValidar)
            EstablecerSpriteAdecuado(e.encendido != _mecanismoAValidar.Invertido);
    }

    private void EstablecerSpriteAdecuado(bool valor)
    {
        _sprite.sprite = valor ? spriteActivo : spriteApagado;
    }
}
