using UnityEngine;

public class MusicRequester : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip clipToPlay;

    private void Start()
    {
        if (MusicManager.Instance == null)
        {
            print("MusicManager.Instance no está seteado");
            return;
        }

        if (clipToPlay == null)
        {
            print("el clip de música no está asignado");
            return;
        }

        MusicManager.Instance.PlayAudio(clipToPlay);
    }
}