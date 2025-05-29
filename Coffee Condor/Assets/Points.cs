using UnityEngine;

public class ConsumerPoint : MonoBehaviour
{
    public ConsummerMovemnet CurrentConsumer { get; private set; }

    public bool IsOccupied => CurrentConsumer != null;

    public bool TryClaim(ConsummerMovemnet consumer)
    {
        if (CurrentConsumer == null)
        {
            CurrentConsumer = consumer;
            return true;
        }
        return false;
    }

    public void ClearConsumer()
    {
        CurrentConsumer = null;
    }
}
