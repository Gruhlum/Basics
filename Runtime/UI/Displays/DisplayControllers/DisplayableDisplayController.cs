namespace HexTecGames.Basics.UI
{
    public class DisplayableDisplayController : DisplayController<DisplayableDisplay, DisplayableObject>
    {
        protected virtual void Awake()
        {
            DisplayItems();
        }
    }
}