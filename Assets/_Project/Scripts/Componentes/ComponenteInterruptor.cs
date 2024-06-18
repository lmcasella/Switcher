namespace Componentes
{
    public class ComponenteInterruptor : ComponenteBinario, IUsable
    {
        public void Usar(ControlJugador usuario)
        {
            Encender(!Encendido);
        }

        public void DejarDeUsar(ControlJugador usuario)
        {
            
        }
    }
}