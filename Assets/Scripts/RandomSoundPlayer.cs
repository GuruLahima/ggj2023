using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour {
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    public void PlayRandomSound() {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
