using UnityEngine;

public class HoopTopTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && BallEntryTracker.Instance.CanScore())
        {
            BallEntryTracker.Instance.RegisterEntryFromTop();
            BallEntryTracker.Instance.RegisterScore();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            // Cuando la bola sale de la canasta, reseteamos el estado
            Invoke(nameof(Reset), 1f);
        }
    }

    private void Reset()
    {
        BallEntryTracker.Instance.Reset();
    }
}
