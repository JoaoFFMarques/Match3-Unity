using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource MusicSource;
    public AudioSource SFXSource;
    public AudioClip SfxClear;    

    [Header("UI")]
    public GameObject m_Exit;
    public GameObject m_Menu;
    public Text m_MscText;
    public bool m_Music;
    public Text m_SfxText;
    public bool m_Sfx;

    public void PlaySFX(AudioClip sfxClip)
    {
        SFXSource.PlayOneShot(sfxClip, 0.7f);
    }
    public void GameQuit()
    {
        PlaySFX(SfxClear);
        Application.Quit();
    }
    public void GameStart()
    {
        PlaySFX(SfxClear);
        SceneManager.LoadScene("Stage1");        
    }
    public void CallExit()
    {
        PlaySFX(SfxClear);
        m_Exit.SetActive(true);
    }
    public void BackGameExit()
    {
        PlaySFX(SfxClear);
        m_Exit.SetActive(false);
    }
    public void CallMenu()
    {
        PlaySFX(SfxClear);
        m_Menu.SetActive(true);
    }
    public void BackGameMenu()
    {
        PlaySFX(SfxClear);
        m_Menu.SetActive(false);
    }
    public void MusicSwitch()
    {
        PlaySFX(SfxClear);
        if(!m_Music)
        {
            MusicSource.GetComponent<AudioSource>().mute = true;
            m_MscText.text = "MUSIC OFF";
            m_Music = true;
        }
        else
        {
            MusicSource.GetComponent<AudioSource>().mute = false;
            m_MscText.text = "MUSIC ON";
            m_Music = false;
        }
    }
    public void SFXSwitch()
    {
        PlaySFX(SfxClear);
        if(!m_Sfx)
        {
            SFXSource.GetComponent<AudioSource>().mute = true;
            m_SfxText.text = "SFX OFF";
            m_Sfx = true;
        }
        else
        {
            SFXSource.GetComponent<AudioSource>().mute = false;
            m_SfxText.text = "SFX ON";
            m_Sfx = false;
        }
    }
}
