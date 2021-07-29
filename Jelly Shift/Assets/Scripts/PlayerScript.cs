using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [SerializeField] private PlayerSettings settings;
    private Camera cam;
    Rigidbody rb;
    [SerializeField] private GameObject player;
    [SerializeField] private Text textScore;
    public Transform playerFlip;
    private int score;
    public GameObject particle;
    public GameObject particle2;
    [SerializeField] GameObject startButton;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject BackFont;
    [SerializeField] GameObject tapToPlayText;
    Vector3 mousePos;
    Vector3 firstPos;
    Vector3 diff;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = ObjectManager.Instance.UiCamera;
        settings.isPlaying = false;
        restartButton.SetActive(false);
        BackFont.SetActive(true);
        resumeButton.SetActive(false);
        particle.SetActive(false);
        particle2.SetActive(false);
    }

    void Update()
    {
        if (settings.isPlaying == true || transform.position == new Vector3(0, 0.6f, 1))
        {
            settings.isPlaying = true;
            Move();
        }
        firstPos = Vector3.Lerp(firstPos, mousePos, .1f);

        if (Input.GetMouseButtonDown(0))
        {
            MouseDown(Input.mousePosition);
        }
        else if (Input.GetMouseButton(0))
        {
            MouseHold(Input.mousePosition);
            ScaleX();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PauseButton();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            ResumeButton();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartButton();
        }

        textScore.text = StringClass.TAG_SCORE + score;

    }

    void Move()
    {
        rb.velocity = new Vector3(0, 0, settings.speed * 1);
    }

    private void MouseDown(Vector3 inputPos)
    {
        mousePos = cam.ScreenToWorldPoint(inputPos);
        firstPos = mousePos;
    }

    private void MouseHold(Vector3 inputPos)
    {
        mousePos = cam.ScreenToWorldPoint(inputPos);
        diff = mousePos - firstPos;
        diff *= settings.sensitivity;
    }

    private void ScaleX()
    {
        player.transform.localScale = new Vector3(Mathf.Clamp(player.transform.localScale.x - diff.y, settings.borderMin, settings.borderMax), Mathf.Clamp(player.transform.localScale.y + diff.y, settings.borderMin, settings.borderMax), 0.5f);
        player.transform.position = new Vector3(player.transform.position.x, Mathf.Clamp(player.transform.position.y + diff.y / 2, settings.borderHeightMin, settings.borderHeightMax), player.transform.position.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringClass.TAG_COIN))
        {
            score++;
            Destroy(other.gameObject);
            Debug.Log(score);
        }

        if (other.CompareTag(StringClass.TAG_FINISHLINE))
        {
            settings.isPlaying = false;
            rb.isKinematic = true;
            transform.DOScale(new Vector3(3f, 3f, 0.5f), 0.2f);
            transform.DOMove(new Vector3(0, 3.5f, 155), 0.8f).OnComplete(() => Flip());
            transform.DORotate(new Vector3(360, 180, 180), 0.8f).OnComplete(() => { ScaleTween(); }); ;
            restartButton.SetActive(true);
            particle.SetActive(true);
            particle2.SetActive(true);
        }

        if (other.CompareTag(StringClass.TAG_WALL))
        {
            other.gameObject.AddComponent<Rigidbody>();
            other.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            other.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2, 2), Random.Range(5,15), Random.Range(5, 15)), ForceMode.Impulse);
            other.GetComponent<Rigidbody>().AddTorque(new Vector3(Random.Range(-15, 15), Random.Range(-15, 15), Random.Range(-15, 15)));
            FalsePlaying();
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, -5), ForceMode.Impulse);

            Invoke("ForceAdd", 0.7f);
            Invoke("TruePlaying", 1f);
        }

        if (other.CompareTag(StringClass.TAG_PARENTBLOCK))
        {
            settings.isPlaying = false;
            GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 22.5f), ForceMode.Impulse);
            Invoke("TruePlaying", 2f);
        }
    }

    private void Flip()
    {
        transform.DOMove(new Vector3(0, 1.55f, 155), 0.2f);
    }

    private void ScaleTween()
    {
        transform.DOScale(new Vector3(4f, 2f, 0.5f), 0.25f).SetEase(Ease.InOutFlash).SetLoops(2, LoopType.Yoyo).OnComplete(() => { ScaleTweenLast(); }); ;
    }

    private void ScaleTweenLast()
    {
        transform.DOScale(new Vector3(2.5f, 3.5f, 0.5f), 0.25f).SetLoops(2, LoopType.Yoyo);
    }

    public void StartButton()
    {
        settings.isPlaying = true;
        startButton.SetActive(false);
        BackFont.SetActive(false);
        tapToPlayText.SetActive(false);
    }

    public void RestartButton()
    {
        tapToPlayText.SetActive(true);
        SceneManager.LoadScene(StringClass.TAG_SCENE);
    }

    public void PauseButton()
    {
        resumeButton.SetActive(true);
        BackFont.SetActive(true);
        pauseButton.SetActive(false);
        player.GetComponent<Rigidbody>().isKinematic = true;
    }

    public void ResumeButton()
    {
        pauseButton.SetActive(true);
        BackFont.SetActive(false);
        resumeButton.SetActive(false);
        player.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void FalsePlaying()
    {
        settings.isPlaying = false;
    }
    private void TruePlaying()
    {
        settings.isPlaying = true;
    }

    private void ForceAdd()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 10f), ForceMode.Impulse);
    }
}
