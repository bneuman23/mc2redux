using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public float health;
    public bool selected;
    public bool dead;
    public int team;
    public bool crouch;
    public bool run;
    public GameObject selectCude;
    PlayerControl plControl;

	// Use this for initialization
	void Start ()
    {
        plControl = GetComponent<PlayerControl>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
        selectCude.SetActive(selected);

        if (run)
        {
            crouch = false;
        }
	}

    public void MoveToPosition(Vector3 position)
    {
        plControl.moveToPosition = true;
        plControl.destPosition = position;
    }
}
