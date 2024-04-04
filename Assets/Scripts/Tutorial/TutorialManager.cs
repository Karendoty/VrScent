using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform UI;

    [SerializeField] private GameObject startPanel;
    [SerializeField] private Animator welcomeTxt;
    [SerializeField] private Animator continueTxt;

    [SerializeField] private GameObject teleportPanel;
    [SerializeField] private Animator moveTxt;

    [SerializeField] private GameObject rotatePanel;
    [SerializeField] private Animator rotateTxt;

    [SerializeField] private GameObject scentPanel;
    [SerializeField] private Animator scentTxt;

    [SerializeField] private GameObject findPanel;
    [SerializeField] private Animator findTxt;

    [SerializeField] private GameObject finishPanel;
    [SerializeField] private Animator finishTxt;
    [SerializeField] private Animator continueTxt2;

    [Header("UI Position")]
    [SerializeField] private Transform UIPosition1;
    [SerializeField] private Transform UIPosition2;

    private bool hasInitated = false;
    private bool hasTeleported = false;
    private bool hasRotated = false;

    private string currentPanelOpen;
    [Header("Objects")]
    [SerializeField] private GameObject demoLeftController;
    [SerializeField] private GameObject demoRightController;
    [SerializeField] private GameObject scentObject;
    [SerializeField] private GameObject circle;
    [SerializeField] private GameObject bench;

    public FadeToBlack fade;


    // Start is called before the first frame update
    void Start()
    {
        startPanel.SetActive(true);
        teleportPanel.SetActive(false);
        rotatePanel.SetActive(false);
        scentPanel.SetActive(false);
        findPanel.SetActive(false);
        finishPanel.SetActive(false);

        currentPanelOpen = startPanel.name;
        welcomeTxt.Play("Fade In");
        continueTxt.GetComponent<TMP_Text>().alpha = 0.0f;
        StartCoroutine(StartBlink(continueTxt));
    }

    IEnumerator StartBlink(Animator anim)
    {
        yield return new WaitForSeconds(5f);
        anim.Play("blink");
    }

    public void PanelInputHandler()
    {
            if (currentPanelOpen == "Start" || currentPanelOpen == "Finish")
            {
                Debug.Log("Button 'A' presed");
                NextPanel();
            }
            hasInitated = true;
    }

    public void NextPanel()
    {
        Debug.Log(currentPanelOpen);

        switch (currentPanelOpen)
        {
            case "Start":
                startPanel.SetActive(false);
                teleportPanel.SetActive(true);
                demoLeftController.SetActive(true);
                circle.SetActive(true);

                moveTxt.Play("Fade In");
                demoLeftController.GetComponent<Animator>().Play("Forward");

                currentPanelOpen = teleportPanel.name;
                break;
            case "Teleport":
                teleportPanel.SetActive(false);
                rotatePanel.SetActive(true);
                demoLeftController.SetActive(false);
                demoRightController.SetActive(true);
                circle.SetActive(false);

                rotateTxt.Play("Fade In");
                demoRightController.GetComponent<Animator>().Play("Rotate");

                currentPanelOpen = rotatePanel.name;
                break;
            case "Rotate":
                rotatePanel.SetActive(false);
                scentPanel.SetActive(true);
                demoRightController.SetActive(false);
                scentObject.SetActive(true);

                scentTxt.Play("Fade In");

                currentPanelOpen = scentPanel.name;
                break;
            case "Scent":
                scentPanel.SetActive(false);
                findPanel.SetActive(true);
                //scentObject.SetActive(false);
                bench.SetActive(true);
                UI.position = UIPosition1.position;
                UI.rotation = UIPosition1.rotation;

                findTxt.Play("Fade In");

                currentPanelOpen = findPanel.name;
                break;
            case "Find":
                findPanel.SetActive(false);
                finishPanel.SetActive(true);
                //bench.SetActive(false);
                UI.position = UIPosition2.position;
                UI.rotation = UIPosition2.rotation;

                finishTxt.Play("Fade In");
                StartCoroutine(StartBlink(continueTxt2));

                currentPanelOpen = finishPanel.name;
                break;
            case "Finish":

                finishTxt.Play("Fade Out");
                continueTxt2.Play("Fade Out");
                StartCoroutine(NextScene());
                break;
        }

    }

    public void FirstTeleportation()
    {
        if (!hasTeleported && hasInitated)
        {
            Debug.Log("First Teleport!");
            NextPanel();
            hasTeleported = true;
        }
    }

    public void FirstRotation()
    {
        if (!hasRotated && hasInitated)
        {
            Debug.Log("First Rotate!");
            NextPanel();
            hasRotated = true;
        }
    }

    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2);

        fade.fadeDuration = 5f;
        fade.FadeOut();

        yield return new WaitForSeconds(6);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
