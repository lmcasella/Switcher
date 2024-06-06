using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boton : ModeloInterruptor
{
    public override void Usar()
    {
        Encender(!Encendido);
    }
}
