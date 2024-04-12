using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CircleDetect : MonoBehaviour
{
    public List<Transform> circleTransforms = new List<Transform>();
    public TMP_Text teleportTXT;
    private TutorialManager tutorialManager;

    private int teleports;
    private bool boolie;
    // Start is called before the first frame update
    void Start()
    {
        tutorialManager = GameObject.Find("TutorialManager").GetComponent<TutorialManager>();
    }

    private void Update()
    {
        if(teleports >= 3 && !boolie)
        {
            Debug.Log("All done");
            tutorialManager.FirstTeleportation();
            boolie = true;
        }
        teleportTXT.text = teleports.ToString() + " Teleports";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (teleports <= 3)
            {
                teleports++;
                gameObject.transform.position = circleTransforms[0].transform.position;
                circleTransforms.Remove(circleTransforms[0]);
            }
        }
    }
}
