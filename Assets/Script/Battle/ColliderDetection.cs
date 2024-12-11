using UnityEngine;
using UnityEngine.SceneManagement;

public class ColliderDetection : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            PlayerMovementController.instance.canMove = false;
            SoundManager.instance.PlaySoundTrigger(SoundID.TEST_SOUND);
            //save player postion
            //save info
            SceneManager.LoadScene("BattleScene");
        }
    }
}
