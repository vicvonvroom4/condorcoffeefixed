using UnityEngine;

public class BuyingRooms : MonoBehaviour
{
    public int cost = 10;
    public bool Locked = false;

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private GameObject[] roomsToUnlock;

    private Color startColor;

    private void Start()
    {
        startColor = sr.color;

        if (Locked)
        {
            sr.color = Color.gray;
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnMouseEnter()
    {
        if (!Locked)
            sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (Locked || MoneyManager.main.Currency < cost)
            return;

        MoneyManager.main.SpendCurrency(cost);

        foreach (var room in roomsToUnlock)
        {
            UnlockRoom(room);
        }

        Destroy(gameObject);
    }

    private void UnlockRoom(GameObject room)
    {
        if (room == null) return;

        if (room.TryGetComponent(out BuyingRooms br))
        {
            br.Locked = false;
            br.sr.color = br.startColor;
        }

        if (room.TryGetComponent(out Collider2D col))
        {
            col.enabled = true;
        }
    }
}
