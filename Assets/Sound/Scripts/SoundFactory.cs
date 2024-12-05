using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDatabank", menuName = "Databank/Sound")]
public class SoundFactory : ScriptableObject
{
    public List<AudioClip> sources;

    public AudioClip GetSound(string name) 
    {
        return name switch
        {
            SoundID.TEST_SOUND => sources.FirstOrDefault(x => x.name == name),
            _ => throw new System.NotImplementedException(),
        };
    }
}
