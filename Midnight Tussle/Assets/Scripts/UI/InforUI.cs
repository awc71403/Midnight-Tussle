using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InforUI : MonoBehaviour
{
    #region TrackingVars
    public bool over;
    public Text ability;
    private RectTransform myrect;
    private Vector3 startingtransform;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        over = false;
        myrect = GetComponent<RectTransform>();
        startingtransform = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (over && (transform.position.x < Screen.width/2 && transform.position.x > 0) )
        {
            myrect.pivot = new Vector2(1, 1);
        }
        else
        {
            myrect.pivot = new Vector2(0, 1);
        }

        if (!over)
        {
            transform.position =startingtransform;
            myrect.pivot = new Vector2(1, 1);
        }
    }

}
