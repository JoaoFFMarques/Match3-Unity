using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continue : MonoBehaviour
{
    public GameObject m_ContinueButton;

    void Start()
    {

        if(File.Exists(Application.persistentDataPath + "/Stage.dat"))
        {               
            FileStream file = File.Open(Application.persistentDataPath + "/Stage.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            GameController.m_StageN = (int)bf.Deserialize(file);
            file.Close();
            for(int i = 0; i <= GameController.m_StageN; i++)
                GameController.m_IncrementalPoints += 10;
            m_ContinueButton.SetActive(true);
        }
        else
            m_ContinueButton.SetActive(false);
    }

    
}
