using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonSoundPlayer : MonoBehaviour
{
    public SoundType soundToPlay;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.instance.PlaySound(soundToPlay);
        });
    }
}
