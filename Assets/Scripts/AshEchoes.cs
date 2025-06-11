using UnityEngine;

public class AshEchoes : MonoBehaviour
{
    [SerializeField] int currentEchoes = 0;

    public int CurrentAshEchoes => currentEchoes;
    public delegate void EchoesChangedHandler(int currentEchoes);
    public event EchoesChangedHandler OnEchoesChanged;


    void Start()
    {
        currentEchoes = 0;
    }

    // TODO: remove with a real update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddEchoes(10);
        }
    }

    public void AddEchoes(int amount)
    {
        if (amount > 0)
        {
            currentEchoes += amount;
            OnEchoesChanged?.Invoke(currentEchoes);
        }
    }

    public bool SpendEchoes(int amount)
    {
        if (amount > 0 && currentEchoes >= amount)
        {
            currentEchoes -= amount;
            OnEchoesChanged?.Invoke(currentEchoes);
            return true;
        }
        return false;
    }
}
