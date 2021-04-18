using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class Manager : MonoBehaviour
{
    
    public GameObject prefab;
    GameObject[] tiles;
    public int rowColNum;
    int moves, skillMove, tilesLeft;
    public Slider difficultySlider,skillSlider;
    public int difficulty, skillLvl;
    Vector2 inputVec;
    Camera cam;
    float time;
    public TMP_Text text,gameText;
    bool gameStart = false;
    GameObject[,] grid;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
      
    }
    
    private void TileSetup()
    {
        rowColNum += difficulty;
        grid = new GameObject[rowColNum, rowColNum];
        tiles = new GameObject[(rowColNum * rowColNum)];
        gameText.text = "Hack Initialized";
        skillMove = skillLvl;
        for (int i = 0; i < tiles.Length; i++)
        {
            GameObject temp = Instantiate(prefab, transform);
            tiles[i] = temp;

        }
        for (int r = 0; r < rowColNum; r++)
        {
            for (int c = 0; c < rowColNum; c++)
            {

                grid[r, c] = tiles[(r * rowColNum) + c];
                grid[r, c].GetComponent<Object>().SetGridIndices(r, c);
                //grid[r, c].GetComponent<Transform>().localPosition = Vector3.Lerp
                //    (grid[r, c].GetComponent<Transform>().position, new Vector3(c * 1.4f, r * 1.4f, 0), 1);
                grid[r, c].GetComponent<Transform>().localPosition =  new Vector3(c * 1.4f , r * 1.4f , 0); 

                int temp = UnityEngine.Random.Range(1, 21);
                if (temp <= 10)
                {
                   // Debug.Log(temp);
                    grid[r, c].GetComponent<Object>().Switch(true);
                    tilesLeft++;
                }
                else grid[r, c].GetComponent<Object>().Switch(false);

            }
        }
        gameStart = true;
    }
    public void OnClick()
    {
       if (gameStart == false) TileSetup();
    }
    // Update is called once per frame
    void Update()
    {
        text.text = "Moves: " + moves.ToString();
        if (gameStart == false)
        {
            difficultySlider.interactable = true;
            skillSlider.interactable = true;
            difficulty = (int)difficultySlider.value;
            skillLvl = (int)skillSlider.value;
        }
        else
        {
            difficultySlider.interactable = false;
            skillSlider.interactable = false;
        }
        if(gameStart == true)
        {
            if(tilesLeft <= 0)
            {
                gameText.text = "Hack Complete";
                gameStart = false;
            }
        }
    }
    public void Interaction(Object objInTile)
    {
        int temp = 0;
        for (int r = objInTile.row - 1; r <= objInTile.row+1; r++)
        {
            if (r >= 0 && r < rowColNum )
            {
                grid[r, objInTile.col].GetComponent<Object>().Switch(!grid[r, objInTile.col].GetComponent<Object>().GetBool());
            }

        }
        for (int c = objInTile.col - 1; c <= objInTile.col + 1; c++)
        {
           if( c >= 0 && c < rowColNum)
                grid[objInTile.row, c].GetComponent<Object>().Switch(!grid[objInTile.row, c].GetComponent<Object>().GetBool());
        }
        objInTile.Switch(!objInTile.GetBool());
        for(int i = 0; i < tiles.Length; i++)
        {
            if(tiles[i].GetComponent<Object>().GetBool() == true)
            {
                
                temp++;

            }
        }
        tilesLeft = temp;
        
        Debug.Log(tilesLeft);
        moves++;
        if (tilesLeft == 0) return;

    }
    public void OnMousePosition(InputValue value)
    {
        inputVec = cam.ScreenToWorldPoint(value.Get<Vector2>());

       // RaycastHit2D hit = Physics2D.Raycast(inputVec, Vector3.forward);
       // Debug.DrawRay(inputVec, Vector3.back, Color.blue, 2f);
       // Debug.Log(inputVec);
    }
    public void OnFire()
    {
        RaycastHit2D hit = Physics2D.Raycast(inputVec, Vector3.forward);
        Debug.DrawRay(inputVec, Vector3.back, Color.red);
        if(hit.collider != null)
        {
            if(hit.collider.tag == "Tile")
            Interaction(hit.collider.GetComponent<Object>());
            Debug.Log("Fire");
        }
    }
    public void OnRightClick()
    {
        RaycastHit2D hit = Physics2D.Raycast(inputVec, Vector3.forward);
        Debug.DrawRay(inputVec, Vector3.back, Color.red);
        if (hit.collider.tag == "Tile")
        {
            if(skillMove > 0)
            hit.collider.GetComponent<Object>().Switch(!hit.collider.GetComponent<Object>().GetBool());
            moves++;
            if (hit.collider.GetComponent<Object>().GetBool() == true)
            {
                tilesLeft--;
            }
            else tilesLeft++;
        }
       
    }
    public void Reset()
    {
        if (gameStart == true)
        {
            gameText.text = "Hack Uninitialized";
            moves = 0;
            for (int i = 0; i < tiles.Length; i++) Destroy(tiles[i].gameObject);
            gameStart = false;
        }
    }
}
