using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager main;

    [SerializeField] TextMeshProUGUI MoneyUI;


    public int currency;


    public int Currency
    {
        get => currency;
        set
        {
            currency = value; // Update the internal health value
            MoneyUI.text = currency.ToString(); // Update the health UI text

            /*if (currency < 0)
            {
                SceneManager.LoadScene("lose");
            }*/
        }
    }

    private void Awake()
    {
        main = this;
    }
    private void Start()
    {
        currency = 100;
        MoneyUI.text = currency.ToString();
    }
    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }
    public bool SpendCurrency(int amount)
    {
        if (amount <= currency)
        {
            // buy item
            currency -= amount;
            return true;
        }
        else
        {
            return false;
        }

    }
    private void Update()
    {
        MoneyUI.text = currency.ToString();
    }
    public void currentAmount()
    {

    }
}
