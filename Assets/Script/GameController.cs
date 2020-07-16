using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject m_Board;
    private string m_StageName;
    public static int m_StageN;
    public string m_NextScene;

    [Header("Audio")]
    public GameObject MusicSource;
    public GameObject SFXSource;
    public AudioClip SfxClear, SfxSelect, SfxSwap;

    [Header("Points")]
    public Slider m_PointsBar;
    public Text m_PointsTxt;
    public int m_Points;
    private int m_TotalPoints=90;
    public static int m_IncrementalPoints;

    [Header("Timer")]
    public Slider m_TimerBar;
    public Text m_TimerTxt;
    public float m_Time = 120f;
    private int m_IncremntTime;

    [Header("UI")]
    public bool m_GamePause;
    public Text m_StageText;
    public GameObject m_Menu;
    public GameObject m_Exit;
    public GameObject m_StageClear;
    public GameObject m_GameOver;
    public GameObject m_AllClear;
    public Text m_MscText;
    public bool m_Music;
    public Text m_SfxText;
    public bool m_Sfx;

    private void Start()
    {
        m_StageN += 1;
        m_IncrementalPoints += 10;
        m_TotalPoints += m_IncrementalPoints;
        m_IncremntTime = m_StageN / 10;
        m_Time += (m_IncremntTime * 10);
        StageName();
        SaveStage();
        m_PointsBar.maxValue = m_TotalPoints;
        m_StageText.text = m_StageName;
        MusicSource = GameObject.FindWithTag("MusicSource");
        SFXSource = GameObject.FindWithTag("SFXSource");
        if(MusicSource.GetComponent<AudioSource>().mute == true)
        {
            m_MscText.text = "MUSIC OFF";
            m_Music = true;
        }
        else
        {
            m_MscText.text = "MUSIC ON";
            m_Music = false;
        }

        if(SFXSource.GetComponent<AudioSource>().mute == true)
        {
            m_SfxText.text = "SFX OFF";
            m_Sfx = true;
        }
        else
        {
            m_SfxText.text = "SFX ON";
            m_Sfx = false;
        }
        MusicSource.GetComponent<AudioSource>().loop = true;
    }

    private void FixedUpdate()
    {
        if(!m_GamePause)
        {
            TimerCounter();
            if(m_Points >= m_TotalPoints)
                StageClear();
            if(m_Time <= 0)
                GameOver();

        }
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        SFXSource.GetComponent<AudioSource>().PlayOneShot(sfxClip, 0.7f);
    }
    public void AddPoints()
    {
        m_PointsBar.value = m_Points;
        m_PointsTxt.text = m_Points.ToString();
    }
    private void TimerCounter()
    {
        int mins = (int)m_Time / 60;
        int secs = (int)m_Time % 60;
        m_Time -= 1f * Time.fixedDeltaTime;
        m_TimerBar.value = m_Time;
        m_TimerTxt.text = mins.ToString("00")+":"+secs.ToString("00");
    }
    public void GameQuit()
    {
        PlaySFX(SfxClear);
        Application.Quit();
    }
    public void CallMenu()
    {
        PlaySFX(SfxClear);
        m_Menu.SetActive(true);
        m_Board.SetActive(false);
        m_GamePause = true;
    }
    public void BackGameMenu()
    {
        PlaySFX(SfxClear);
        m_Menu.SetActive(false);
        m_Board.SetActive(true);
        m_GamePause = false;
    }
    public void MusicSwitch()
    {
        PlaySFX(SfxClear);
        if(!m_Music)
        {
            MusicSource.GetComponent<AudioSource>().mute=true;
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
    public void CallExit()
    {
        PlaySFX(SfxClear);
        m_Exit.SetActive(true);
        m_Board.SetActive(false);
        m_GamePause = true;
    }
    public void BackGameExit()
    {
        PlaySFX(SfxClear);
        m_Exit.SetActive(false);
        m_Board.SetActive(true);
        m_GamePause = false;
    }
    private void StageClear()
    {
        PlaySFX(SfxClear);
        m_Board.SetActive(false);
        m_GamePause = true;
        if(m_StageName == "STAGE 999")
        {
            m_AllClear.SetActive(true);
        }
        else
            m_StageClear.SetActive(true);
    }
    public void NextStage()
    {
        PlaySFX(SfxClear);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void PlayAgain()
    {
        PlaySFX(SfxClear);
        m_StageN -= 1;
        m_IncrementalPoints -= 10;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void GameOver()
    {
        PlaySFX(SfxClear);
        m_GameOver.SetActive(true);
        m_Board.SetActive(false);
        m_GamePause = true;
    }
    private void StageName()
    {
        if(m_StageN > 99)
        {
            m_StageName = "STAGE " + m_StageN.ToString("000");
        }
        else
            m_StageName = "STAGE " + m_StageN.ToString("00");
    }    
    private void SaveStage()
    {
        
        BinaryFormatter bf = new BinaryFormatter();               
            
        FileStream file = File.Create(Application.persistentDataPath + "/Stage.dat");
        bf.Serialize(file, (m_StageN-1));
        file.Close();
       
    }
}
