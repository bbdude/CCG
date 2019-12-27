using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{

    public Vector3 origSize;
    public Vector3 origPos;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Examine()
    {

    }

    public void Enlarge()
    {
        origSize = this.transform.localScale;
        origPos = this.transform.position;

        this.transform.position = new Vector3(0, 0, 0);
        this.transform.localScale = origSize + new Vector3(2, 2, 2);
        this.transform.tag = "EnlargedCard";
    }

    public void Shrink()
    {
        this.transform.position = origPos;
        this.transform.localScale = origSize;
        this.transform.tag = "Card";
    }
}
