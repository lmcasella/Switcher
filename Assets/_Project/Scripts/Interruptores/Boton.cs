public class Boton : ModeloInterruptor
{
    public override void Usar()
    {
        Encender(!Encendido);
    }
}
