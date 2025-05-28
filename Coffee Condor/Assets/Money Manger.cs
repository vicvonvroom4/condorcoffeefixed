using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager main;

    [SerializeField] private TextMeshProUGUI moneyText;
    public Transform SpawnPoint;
    public Transform[] OrderPoint;
    public Transform[] EatPoint;
    public Transform LeavePoint;

    private int currency;

    public int Currency
    {
        get => currency;
        set
        {
            currency = Mathf.Max(0, value); // Prevent negative currency
            moneyText.text = currency.ToString();
        }
    }
    void Awake()
    {
        main = this;
    }

    private void Start()
    {
        Currency = 100; // Use property to set value and update UI
    }

    public void IncreaseCurrency(int amount)
    {
        Currency += amount;
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            Currency -= amount;
            return true;
        }
        return false;
    }
    public bool HasFreeOrderPoints()
    {
        foreach (Transform point in OrderPoint)
        {
            ConsumerPoint ap = point.GetComponent<ConsumerPoint>();
            if (ap != null && !ap.IsOccupied)
            {
                return true;
            }
        }
        return false;
    }

}
