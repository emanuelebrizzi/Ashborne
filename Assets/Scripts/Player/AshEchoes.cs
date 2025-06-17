using UnityEngine;

public class AshEchoes : MonoBehaviour
{
    [SerializeField] int current = 0;

    public int Current => current;


    void Start()
    {
        current = 0;
    }


    public void AddEchoes(int amount)
    {
        if (amount > 0)
        {
            current += amount;
        }
    }

}
