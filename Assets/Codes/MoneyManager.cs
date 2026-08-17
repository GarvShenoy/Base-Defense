using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager main;

    [SerializeField] private int startMoney = 500;
    private int currentMoney;

    private void Awake()
    {
        main = this;
    }

    //Sets the Current money to the money given at the start of the level.
    private void Start()
    {
        currentMoney = startMoney;
    }

    public int GetMoney()
        {
             return currentMoney;
        }

    //Called to update money when money is earned.
    public void RecieveMoney(int earnedMoney)
        {
             currentMoney += earnedMoney;
             Debug.Log("Recieved Money. Current Money: " + currentMoney);
        }

    //Called to update money when money is spent.
    public void SpendMoney(int spentMoney)
        {
             currentMoney -= spentMoney;
             Debug.Log("Spent Money. Current Money: " + currentMoney);
        }
}

