namespace Componentes
{
    public class ComponenteBotonHold : ComponenteBinario, IUsable
    {
        public void Usar(ControlJugador usuario)
        {
            Encender(true);
        }

        public void DejarDeUsar(ControlJugador usuario)
        {
            Encender(false);
        }
    }
}