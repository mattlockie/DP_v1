using UnityEngine;
using System.Collections.Generic;

public class Effects : MonoBehaviour {
 
    [System.Serializable]
    public class Sound
    {
        public string name;
        public string sound;
    }

    public List<Sound> soundFX;

    public string GetSound(string _name)
    {
        Sound _sound = soundFX.Find(x => x.name == _name);
        if (_sound != null)
        {
            return _sound.sound;
        }
        else
        {
            return string.Empty;
        }
    }
}