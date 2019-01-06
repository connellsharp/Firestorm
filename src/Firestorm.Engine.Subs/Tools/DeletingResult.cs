namespace Firestorm.Engine.Subs
{
    public enum DeletingResult
    {
        /// <summary>
        /// The Engine should continue to delete the item from the repository.
        /// </summary>
        Continue,
        
        /// <summary>
        /// The Engine should skip marking deleted in the repository because the action was handled with the event.
        /// </summary>
        Handled
    }
}