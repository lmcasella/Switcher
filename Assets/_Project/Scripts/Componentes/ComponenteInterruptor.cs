namespace Componentes
{
    public class ComponenteInterruptor : ComponenteBinario, IUsable
    {
        public void Usar()
        {
            Encender(!Encendido);
        }
    }
}