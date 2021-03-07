using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private Unit myCharacter;


    // Start is called before the first frame update
    private void Awake()
    {
        myCharacter = GetComponent<Unit>();
    }

    void Update() {
    }
}
