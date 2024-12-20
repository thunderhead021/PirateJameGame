using UnityEngine;

public class DetectPlayer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerCharacter"))
        {
            Debug.Log("Enter " + gameObject.name);
        }
        if (other.gameObject.CompareTag("Room"))
        {
            gameObject.SetActive(false);
            other.gameObject.SetActive(true);
        }
    }
}
