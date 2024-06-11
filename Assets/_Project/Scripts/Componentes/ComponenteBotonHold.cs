namespace Componentes
{
    public class ComponenteBotonHold : ComponenteBinario, IUsable
    {
        public void Usar()
        {
            Encender(true);
        }

        public void DejarDeUsar()
        {
            Encender(false);
        }
    }
}