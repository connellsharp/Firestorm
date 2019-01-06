namespace Firestorm
{
    /// <summary>
    /// An <see cref="Acknowledgment"/> that signifies a new item has been added to a collection.
    /// </summary>
    public class CreatedItemAcknowledgment : Acknowledgment
    {
        public CreatedItemAcknowledgment(object identifier)
        {
            NewIdentifier = identifier;
        }

        public object NewIdentifier { get; private set; }
    }
}