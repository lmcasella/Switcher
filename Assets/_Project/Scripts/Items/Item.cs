using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IUsable
{
    public void Usar()
    {
        Debug.Log("Podría ser parte de tu inventario si quisiera. :)");
    }

    public void DejarDeUsar()
    {
        throw new System.NotImplementedException();
    }
}
