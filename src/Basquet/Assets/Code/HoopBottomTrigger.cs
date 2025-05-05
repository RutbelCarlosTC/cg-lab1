using UnityEngine;

public class HoopBottomTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball") && !BallEntryTracker.Instance.EnteredFromTop())
        {
            BallEntryTracker.Instance.RegisterEntryFromBelow();
        }
    }
}
