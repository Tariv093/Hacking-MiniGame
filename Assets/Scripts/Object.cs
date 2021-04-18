using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Object : MonoBehaviour
{
    public Sprite filled, unfilled;
    BoxCollider2D coll;
    SpriteRenderer spr;
    Manager m;
    bool isOn;
    public int row, col;
    // Start is called before the first frame update
    void Start()
    {
        spr = GetComponent<SpriteRenderer>();
        m = GetComponentInParent<Manager>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isOn == true)
        {
            spr.sprite = unfilled;
        }
        else spr.sprite = filled;
    }
    public void SetGridIndices(int r, int c)
    {
        row = r;
        col = c;

    }
    public bool GetBool()
    {
        return isOn;
    }
    public void Switch(bool b)
    {
        isOn = b;
    }
   
   
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("enter"); 
    }
}
