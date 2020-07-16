using UnityEngine;
using System.Collections.Generic;

public class GemController : MonoBehaviour
{
    private static Color m_Color;
    private static GemController m_Previous;
    private GameController _GameController;

    private SpriteRenderer m_GemRender;
    public GameObject m_Effect;
    private bool m_IsSelected;
    private bool m_ThreeMatch;

    private Vector2[] m_Adjacent;

    private Vector2 firstPressPos;
    private Vector2 secondPressPos;
    private Vector2 currentSwipe;

    private Rigidbody2D m_Body;


    void Awake()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        m_GemRender = GetComponent<SpriteRenderer>();
        m_Body = GetComponent<Rigidbody2D>();
        m_Color = new Color(.5f, .5f, .5f, 1.0f);
        m_Previous = null;
        m_Adjacent = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    }
    private void Update()
    {
        CheckPosX();
        Swipe();
        
    }
    void OnMouseDown()
    {
        if(!_GameController.m_GamePause)
        {
            if(m_GemRender.sprite == null)
            {
                return;
            }

            if(m_IsSelected)
            {
                Deselect();
            }
            else
            {
                if(m_Previous == null)
                {
                    Select();
                }
                else
                {
                    if(GetAllAdjacentGems().Contains(m_Previous.gameObject))
                    {                        
                        Swap(this.GetComponent<GemController>(), m_Previous.m_GemRender);
                        m_Previous.ClearAllMatches();
                        m_Previous.Deselect();
                        ClearAllMatches();
                    }
                    else
                    {
                        m_Previous.GetComponent<GemController>().Deselect();
                        Select();
                    }
                }
            }
        }
    } 
    private void OnCollisionEnter2D()
    {        
        if(!_GameController.m_GamePause)
        {
            gameObject.GetComponent<GemController>().ClearAllMatches();
            if(m_Body.velocity.y == 0)
                ClearAllMatches();
        }
    }
    private void Select()
    {
        m_IsSelected = true;
        m_GemRender.color = m_Color;
        m_Previous = gameObject.GetComponent<GemController>();
        _GameController.PlaySFX(_GameController.SfxSelect);
    }
    private void Deselect()
    {
        m_IsSelected = false;
        m_GemRender.color = Color.white;
        m_Previous = null;
    }   
    private GameObject GetAdjacent(Vector2 side)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, side);
        if(hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }
    private List<GameObject> GetAllAdjacentGems()
    {
        List<GameObject> adjacentTiles = new List<GameObject>();
        for(int i = 0; i < m_Adjacent.Length; i++)
        {
            adjacentTiles.Add(GetAdjacent(m_Adjacent[i]));
        }
        return adjacentTiles;
    }
    private List<GameObject> FindMatch(Vector2 sides)
    { 
        List<GameObject> matchingTiles = new List<GameObject>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, sides); 
        while(hit.collider != null && hit.collider.GetComponent<SpriteRenderer>().sprite == m_GemRender.sprite)
        { 
            matchingTiles.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, sides);
        }
        return matchingTiles; 
    }
    private void ClearMatch(Vector2[] sides)
    {
        List<GameObject> matchingTiles = new List<GameObject>(); 
        for(int i = 0; i < sides.Length; i++) 
        {
            matchingTiles.AddRange(FindMatch(sides[i]));
        }
        if(matchingTiles.Count >= 2)
        {
            for(int i = 0; i < matchingTiles.Count; i++) 
            {
                ParticleEffector(matchingTiles[i].transform.position);
                matchingTiles[i].GetComponent<SpriteRenderer>().sprite = null;
            }
            m_ThreeMatch = true; 
        }
    }
    public void ClearAllMatches()
    {
        if(m_GemRender.sprite == null)
            return;

        ClearMatch(new Vector2[2] { Vector2.left, Vector2.right });
        ClearMatch(new Vector2[2] { Vector2.up, Vector2.down });
        if(m_ThreeMatch)
        {
            ParticleEffector(transform.position);
            m_GemRender.sprite = null;
            m_ThreeMatch = false;
            _GameController.PlaySFX(_GameController.SfxClear);
        }
    }
    public void Swap(GemController obj, SpriteRenderer render)
    {

        if(obj.m_GemRender.sprite == render.sprite)
        {
            return;
        }
        GameObject tempTag = new GameObject();
        Sprite tempSprite = render.sprite;
        tempTag.gameObject.tag = render.gameObject.tag;
        render.sprite = obj.m_GemRender.sprite;
        render.gameObject.tag = obj.m_GemRender.gameObject.tag;
        obj.m_GemRender.sprite = tempSprite;
        obj.m_GemRender.gameObject.tag = tempTag.gameObject.tag;
        Destroy(tempTag);

        _GameController.PlaySFX(_GameController.SfxSwap);
    }
    public void ParticleEffector(Vector2 pos)
    {
        GameObject effect = Instantiate(m_Effect, pos, Quaternion.Euler(-185.464f, 90, -90));
        effect.GetComponent<ParticleSystem>().textureSheetAnimation.AddSprite(m_GemRender.sprite);
        effect.GetComponent<ParticleSystem>().Play();     
    }
    private void CheckPosX()
    {
        float posX = transform.position.x;
        if(posX > 6)
            posX = 6;
        else if(posX < 6 && posX > 5)
            posX = 6;
        else if(posX < 5 && posX > 4)
            posX = 5;
        else if(posX < 4 && posX > 3)
            posX = 4;
        else if(posX < 3 && posX > 2)
            posX = 3;
        else if(posX < 2 && posX > 1)
            posX = 2;
        else if(posX < 1 && posX > 0)
            posX = 1;
        else if(posX < 0)
            posX = 0;

        transform.position = new Vector2(posX, transform.position.y);
    }
    public void Swipe()
    {
        if(m_Previous)
        {
            RaycastHit2D hit;
            Vector2 pos = m_Previous.gameObject.transform.position;

            if(Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);
                if(t.phase == TouchPhase.Began)
                    firstPressPos = new Vector2(t.position.x, t.position.y);
                
                if(t.phase == TouchPhase.Ended)
                {                    
                    secondPressPos = new Vector2(t.position.x, t.position.y);

                    currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                    currentSwipe.Normalize();

                    //swipe upwards
                    if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {                        
                        hit = Physics2D.Raycast(pos, Vector2.up);
                        if(hit.collider)
                            SwapChange(hit.collider.gameObject);
                    }
                    //swipe down
                    if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        hit = Physics2D.Raycast(pos, Vector2.down);
                        if(hit.collider)
                            SwapChange(hit.collider.gameObject);
                    }
                    //swipe left
                    if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        hit = Physics2D.Raycast(pos, Vector2.left);
                        if(hit.collider)
                            SwapChange(hit.collider.gameObject);
                    }
                    //swipe right
                    if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        hit = Physics2D.Raycast(pos, Vector2.right);
                        if(hit.collider)
                            SwapChange(hit.collider.gameObject);                        
                    }
                }
            }
        }
    }
    private void SwapChange(GameObject obj)
    {
        if(GetAllAdjacentGems().Contains(m_Previous.gameObject))
        {
            Swap(obj.GetComponent<GemController>(), m_Previous.m_GemRender);
            obj.GetComponent<GemController>().ClearAllMatches();
            m_Previous.Deselect();            
        }
    }
}
