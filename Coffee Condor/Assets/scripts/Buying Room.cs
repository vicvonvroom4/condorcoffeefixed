using UnityEngine;

public class BuyingRoom : MonoBehaviour
{
    public int cost = 10;
    public bool Locked = true;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color lockedColor = Color.gray;
    [SerializeField] private Color unlockedColor = Color.green;
    [SerializeField] private Color hoverColor = Color.cyan;

    private Color originalColor;
    private RoomGridManager manager;
    private int gridX, gridY;
    private bool purchased = false;

    private void Start()
    {
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        originalColor = sr.color;
        UpdateVisual();
    }

    public void Initialize(int x, int y, RoomGridManager manager, int baseCost)
    {
        this.gridX = x;
        this.gridY = y;
        this.manager = manager;
        this.cost = baseCost;

        sr = GetComponent<SpriteRenderer>(); // Safety check
        originalColor = sr.color;

        // Only first column starts unlocked
        if (x == 0)
            Unlock();
        else
            Lock();
    }

    private void OnMouseEnter()
    {
        if (!Locked && !purchased)
            sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        if (!Locked && !purchased)
            sr.color = Color.white;
    }

    private void OnMouseDown()
    {
        if (Locked || MoneyManager.main.Currency < cost || purchased)
            return;

        MoneyManager.main.SpendCurrency(cost);
        purchased = true;

        sr.color = Color.green;
        GetComponent<Collider2D>().enabled = false;

        // Unlock adjacent rooms
        manager.UnlockAdjacentRooms(gridX, gridY);

        // Do NOT destroy the room — we want to keep it visible and green
        // Destroy(gameObject);  remove this line
    }



    public void Unlock()
    {
        Locked = false;
        GetComponent<Collider2D>().enabled = true;
        UpdateVisual();
    }

    public void Lock()
    {
        Locked = true;
        GetComponent<Collider2D>().enabled = false;
        UpdateVisual();
    }
    public void UpdateVisual()
    {
        if (Locked)
        {
            sr.color = Color.gray;
            GetComponent<Collider2D>().enabled = false;
        }
        else if (purchased)
        {
            sr.color = Color.green;
            GetComponent<Collider2D>().enabled = false;
        }
        else
        {
            sr.color = Color.white;
            GetComponent<Collider2D>().enabled = true;
        }
    }


}
