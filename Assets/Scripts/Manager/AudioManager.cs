using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip gameplayMusic;
    [SerializeField][Range(0f, 1f)] float musicVolume = 0.5f;


    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
        }

        musicSource.loop = true;
        musicSource.volume = musicVolume;

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (GameManager.Instance != null)
        {
            GameManager.Instance.RegisterAudioManager(this);
        }
    }

    public void PlayMenuMusic()
    {

        musicSource.clip = menuMusic;
        musicSource.Play();

    }


    public void PlayGameMusic()
    {

        musicSource.clip = gameplayMusic;
        musicSource.Play();

    }
}