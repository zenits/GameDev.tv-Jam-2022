using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum Environment
    {
        alive,
        dead,
    }

    public enum SceneNames
    {
        Title,
        Tutorial,
        Level1,
        Level2,
    }


    #region Static
    private static GameManager _instance;
    private static InputActions _inputs;

    public static GameManager Instance { get { return _instance; } }
    public static InputActions Inputs
    {
        get
        {
            if (_inputs == null)
                _inputs = new InputActions();

            return _inputs;
        }
    }
    #endregion


    #region Private members
    Environment currentEnvironment = Environment.alive;
    #endregion

    #region  Serialized Fields
    [Header("Scene")]
    [SerializeField] SceneNames currentScene = SceneNames.Title;

    [Header("Environment")]
    [SerializeField] GameObject live;
    [SerializeField] GameObject death;
    [SerializeField] float swapDelay = 1.5f;

    [Header("Player")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject ghost;
    [SerializeField] Transform startPoint;

    [Header("Camera")]
    [SerializeField] CameraFollow cameraFollow;

    [Header("Menu")]
    [SerializeField] GameObject PauseMenu;
    #endregion



    #region Awake/Start Events
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this.gameObject);
        }

    }

    private void Start()
    {
        InitializeScene();
    }
    private void OnEnable()
    {
    }
    private void OnDisable()
    {
        GameManager.Inputs.UI.Pause.started -= Pause;
        GameManager.Inputs.Title.Start.started -= StartGame;
    }

    #endregion


    #region Scene Manager
    private void InitializeScene()
    {
        switch (currentScene)
        {
            case SceneNames.Title:
                GameManager.Inputs.Title.Enable();
                GameManager.Inputs.UI.Disable();
                GameManager.Inputs.Player.Disable();
                GameManager.Inputs.Title.Start.started += StartGame;
                break;

            case SceneNames.Tutorial:
            case SceneNames.Level1:
            case SceneNames.Level2:
                ResetEnvironment();
                player.transform.position = startPoint.position;
                GameManager.Inputs.UI.Enable();
                GameManager.Inputs.UI.Pause.started += Pause;
                break;

            default:
                break;
        }
    }


    private void StartGame(InputAction.CallbackContext context)
    {
        GameManager.Inputs.Title.Start.started -= StartGame;
        SceneManager.LoadScene("Tutorial");
    }

    #endregion


    #region Pause Menu

    private void Pause(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
    }

    public void OnResume()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
    }
    public void OnRestart()
    {
        var activeScene = SceneManager.GetActiveScene();
        GameManager.Inputs.UI.Pause.started -= Pause;
        SceneManager.LoadScene(activeScene.name);
        Time.timeScale = 1;
    }
    public void OnGoToTitle()
    {
        Time.timeScale = 1;
        PauseMenu.SetActive(false);
        GameManager.Inputs.UI.Pause.started -= Pause;
        SceneManager.LoadScene("Title");
    }
    #endregion



    #region  Environment
    private void ResetEnvironment()
    {
        SetAlive();
    }

    public void SwapEnvironment()
    {
        Debug.Log("SwapEnvironment");
        StartCoroutine("Swap");
    }

    IEnumerator Swap()
    {
        GameManager.Inputs.Player.Disable();
        yield return new WaitForSeconds(swapDelay);
        switch (currentEnvironment)
        {
            case Environment.alive:
                SetDead();
                break;
            case Environment.dead:
                SetAlive();
                break;
            default:
                break;
        }
        GameManager.Inputs.Player.Enable();
        yield return 0;
    }


    void SetAlive()
    {
        Debug.Log("SetAlive");
        currentEnvironment = Environment.alive;
        death.SetActive(false);
        ghost.SetActive(false);

        player.transform.position = ghost.transform.position;
        live.SetActive(true);
        player.SetActive(true);

        player.GetComponent<IDamageable>().SetHealth(1);
        player.GetComponent<PlayerController>().Reset();
        cameraFollow.target = player.transform;
    }
    void SetDead()
    {
        Debug.Log("SetDead");
        currentEnvironment = Environment.dead;
        live.SetActive(false);
        player.SetActive(false);

        ghost.transform.position = player.transform.position;
        death.SetActive(true);
        ghost.SetActive(true);

        ghost.GetComponent<GhostController>().Reset();
        cameraFollow.target = ghost.transform;
    }

    #endregion

    #region Level Manager
    public void OnLevelExit()
    {
        GameManager.Inputs.UI.Disable();
        GameManager.Inputs.Player.Disable();
        GameManager.Inputs.Title.Start.started -= StartGame;
        GameManager.Inputs.UI.Pause.started -= Pause;
        StartCoroutine("NextLevel");
    }
    private IEnumerator NextLevel()
    {
        yield return new WaitForSeconds(1.5f);
        var maxScene = Enum.GetValues(typeof(SceneNames)).Cast<int>().Max();
        Debug.Log($"maxScene={maxScene}");
        Debug.Log( $"currentScene = {currentScene}" );
        currentScene = (SceneNames)Math.Min((int)currentScene + 1, (int)maxScene);
        SceneManager.LoadScene(currentScene.ToString());
        yield return 0;
    }
    #endregion
}
