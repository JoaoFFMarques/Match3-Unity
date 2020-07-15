using UnityEngine;

public class BoardController : MonoBehaviour
{
    private GameController _GameController;

    [Header("Board")]
    public GameObject[] m_Gems;
    public GameObject[,] m_Board;
    public int m_Collums;
    public int m_Rows;
    public float startX;
    public float startY;    

    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        m_Board = new GameObject[m_Collums, m_Rows];
        Spawner();
    }
    private void FixedUpdate()
    {
        if(!_GameController.m_GamePause)
        {
            TableCheck();
        }
    }    
    private void Spawner()
    {
        bool notEqualUp;
        bool notEqualSide;
        for(int i = 0; i < m_Collums; i++)
        {
            for(int j = 0; j < m_Rows; j++)
            {
                notEqualUp = false;
                notEqualSide = false;
                m_Board[i, j] = Instantiate(m_Gems[Random.Range(0, m_Gems.Length)], new Vector2(startX + i, startY - j), Quaternion.identity);
                m_Board[i, j].transform.SetParent(gameObject.transform);                

                if(i - 1 >= 0)
                {
                    while(!notEqualUp)
                    {
                        if(m_Board[i - 1, j].CompareTag(m_Board[i, j].tag))
                        {
                            Destroy(m_Board[i, j]);
                            m_Board[i, j] = Instantiate(m_Gems[Random.Range(0, m_Gems.Length)], new Vector2(startX + i, startY - j), Quaternion.identity);
                            m_Board[i, j].transform.SetParent(gameObject.transform);
                        }
                        else
                            notEqualUp = true;
                    }
                }
                if(j - 1 >= 0)
                {
                    while(!notEqualSide)
                    {
                        if(m_Board[i, j - 1].CompareTag(m_Board[i, j].tag))
                        {
                            Destroy(m_Board[i, j]);
                            m_Board[i, j] = Instantiate(m_Gems[Random.Range(0, m_Gems.Length)], new Vector2(startX + i, startY - j), Quaternion.identity);
                            m_Board[i, j].transform.SetParent(gameObject.transform);                            
                        }
                        else
                            notEqualSide = true;
                    }
                }
            }    
        }
    }
    private void TableCheck()
    {
        int current = 0;
        for(int i = 0; i < m_Collums; i++)
        {
            for(int j=0; j<m_Rows; j++)
            {
                if(m_Board[i,j].GetComponent<SpriteRenderer>().sprite == null)
                {
                    Transform pos = m_Board[i, j].transform;
                    Destroy(m_Board[i, j]);
                    m_Board[i, j] = Instantiate(m_Gems[Random.Range(0, m_Gems.Length)], new Vector2(pos.position.x, startY+1+j), Quaternion.identity);
                    m_Board[i, j].transform.SetParent(gameObject.transform);
                    current++;                    
                }
            }
        }
        if(current > 4)
            _GameController.m_Points += 20;
        else if(current > 3)
            _GameController.m_Points += 10;
        else if(current > 0)
            _GameController.m_Points += 5;

        _GameController.AddPoints();
    }

}
