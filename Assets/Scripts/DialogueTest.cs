using UnityEngine;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private PhoneUI phoneUI;
    [SerializeField] private DeliveryRoute testRoute;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            phoneUI.ShowDialogueSequence(
                testRoute.DialogueLines,
                OnSequenceComplete
            );
        }
    }

    private void OnSequenceComplete()
    {
        Debug.Log("Dialogue sequence finished.");
    }
}