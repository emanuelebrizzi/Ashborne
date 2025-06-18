using UnityEngine;

public class AshEchoes : MonoBehaviour
{
    [SerializeField] int current = 0;

    public int Current => current;


    void Start()
    {
        ResetEchoes();
    }

    public void ResetEchoes()
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

    public void RemoveEchoes(int amount)
    {
        if (amount > 0)
        {
            current = Mathf.Max(0, current - amount);
        }
    }
    

    public void DropEchoes(Transform dropPoint)
    {
        // Logic to drop echoes, e.g., instantiate a visual effect or sound
        Debug.Log($"Dropped {current} Ash Echoes.");
        ResetEchoes();
    }

}
