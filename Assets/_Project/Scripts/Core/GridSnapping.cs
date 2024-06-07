using UnityEngine;

public class GridSnapping
{
    /// <summary>
    /// Ajuste automático al grid del mundo.
    /// Se redondea la posición en cada eje del transform y se desplaza por el valor deseado en ambos ejes.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="desplazamiento"></param>
    public static void Snap(Transform transform, float desplazamiento = 0.5f)
    {
        float x = Mathf.Floor(transform.position.x);
        float y = Mathf.Floor(transform.position.y);
        Vector2 offset = Vector2.one * desplazamiento;
        transform.position = new Vector2(x, y) + offset;
    }
}