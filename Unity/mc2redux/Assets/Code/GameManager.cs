using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public CharacterStats selectedUnit;
    public int playerTeam;
    //public GameObject unitControls;
    public bool doubleClick;
    public bool overUIElement;
    public GameObject CameraMover;
    public float cameraSpeed = 0.3f;

    // Update is called once per frame
    void Update()
    {
        if (!overUIElement)
            HandleSelection();

        bool hasUnit = selectedUnit;
        //unitControls.SetActive(hasUnit);

        HandleCameraMovement();
    }

    void HandleSelection()
    {
        if (Input.GetButtonUp("Select"))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(ray, out hit, 100))
            {
                CheckHit(hit);
            }
        }
    }

    void HandleCameraMovement()
    {
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");

        Vector3 newPos = new Vector3(hor, 0, vert) * cameraSpeed;
        CameraMover.transform.position += newPos;
    }

    void CheckHit(RaycastHit hit)
    {
        if (hit.transform.GetComponent<CharacterStats>())
        {
            CharacterStats hitStats = hit.transform.GetComponent<CharacterStats>();

            if(hitStats.team == playerTeam)
            {
                if(selectedUnit == null)
                {
                    selectedUnit = hitStats;
                    selectedUnit.selected = true;
                }
                else
                {
                    selectedUnit.selected = false;
                    selectedUnit = hitStats;
                    selectedUnit.selected = true;
                }
            }
            else
            {
                if(selectedUnit == null)
                {
                    //Add enemy team logic here.
                }
            }
        }
        else
        {
            if(selectedUnit)
            {
                if(doubleClick)
                {
                    selectedUnit.run = true;
                }
                else
                {
                    doubleClick = true;
                    StartCoroutine("closeDoubleClick");
                }

                selectedUnit.MoveToPosition(hit.point);
            }
        }
    }

    IEnumerator closeDoubleClick()
    {
        yield return new WaitForSeconds(1);
        doubleClick = false;
    }

    public void EnterUIElement()
    {
        overUIElement = true;
    }

    public void ExitUIElement()
    {
        overUIElement = false;
    }

    public void ChangeStance()
    {
        if (selectedUnit)
        {
            selectedUnit.run = false;
            selectedUnit.crouch = !selectedUnit.crouch;
        }
    }
}
