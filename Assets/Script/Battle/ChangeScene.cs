using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public SceneID SceneID;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy")) 
        {
            PlayerMovementController.instance.canMove = false;
            SoundManager.instance.PlaySoundTrigger(SoundID.TEST_SOUND);
            //remove enemy (pokemon style)
            Destroy(other.gameObject);
            //save player postion
            //save info
            ChangeToScene();
        }
    }

    public void ChangeToScene() 
    {
        Cursor.lockState = SceneID != SceneID.MainGameScene ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = SceneID != SceneID.MainGameScene;
        SceneManager.LoadScene(ScenesFactory.GetScene(SceneID));
    }
}
