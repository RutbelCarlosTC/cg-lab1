using UnityEngine;

public class BallEntryTracker : MonoBehaviour
{
    public static BallEntryTracker Instance;

    private bool enteredFromBelow = false;
    private bool enteredFromTop = false;
    private bool scored = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void RegisterEntryFromTop()
    {
        enteredFromTop = true;
        Debug.Log("El balón entró desde arriba.");
    }

    public void RegisterEntryFromBelow()
    {
        enteredFromBelow = true;
        Debug.Log("El balón entró desde abajo.");
    }

    public bool EnteredFromTop(){
        return enteredFromTop;
    }

    public bool CanScore()
    {
        return !enteredFromBelow && !scored;
    }

    public void RegisterScore()
    {
        scored = true;
        Debug.Log("¡Punto válido!");
        ScoreManager.Instance.AddPoint();
    }

    public void Reset()
    {
        enteredFromBelow = false;
        enteredFromTop = false;
        scored = false;
    }
}
