using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public SoundFactory soundFactory;
    public AudioSource audioSource;

    public static SoundManager instance;


    private void Awake()
    {
        instance = this;
    }


    public void PlaySoundTrigger(string id) => PlaySound(id);


    public void PlaySoundLoop(string id) => PlaySound( id, true );


    public void PlaySound(string id, bool loop = false) 
    {
        audioSource.clip = soundFactory.GetSound(id);
        audioSource.loop = loop;
        audioSource.Play();
    }

}
