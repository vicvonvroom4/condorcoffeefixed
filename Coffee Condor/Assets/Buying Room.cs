using UnityEngine;

public class BuyingRooms : MonoBehaviour
{
    public int cost = 10;
    public bool Locked = false;

    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] GameObject UnlockNext1;
    [SerializeField] GameObject UnlockNext2;
    [SerializeField] GameObject UnlockNext3;
    [SerializeField] GameObject UnlockNext4;
    private Color startColor;

    private void Start()
    {
        startColor = sr.color;

        if (Locked)
        {
            sr.color = Color.gray; // Optional
            GetComponent<Collider2D>().enabled = false;
        }
    }


    private void OnMouseEnter()
    {
        if (Locked) return;
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        if (MoneyManager.main.Currency >= cost && !Locked)
        {
            MoneyManager.main.SpendCurrency(cost);

            UnlockRoom(UnlockNext1);
            UnlockRoom(UnlockNext2);
            UnlockRoom(UnlockNext3);
            UnlockRoom(UnlockNext4);
            Destroy(gameObject); // Now it destroys itself after unlocking
        }
    }


    private void UnlockRoom(GameObject roomToUnlock)
    {
        if (roomToUnlock == null) return;

        BuyingRooms br = roomToUnlock.GetComponent<BuyingRooms>();
        if (br != null)
        {
            br.Locked = false;
            br.sr.color = br.startColor; // Restore original color
        }

        Collider2D col = roomToUnlock.GetComponent<Collider2D>();
        if (col != null) col.enabled = true;
    }

}
