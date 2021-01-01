using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton class GameMangaer

    public static GameManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    #endregion

    public float power = 10f;
    public Vector3 minPower;
    public Vector3 maxPower;

    Vector3 shootingForce;
    Vector4 shootForce;

    Vector3 startPoint;
    Vector3 endPoint;

    private Camera mainCamera;

    public Sausage sausage;
    public Trajectory trajectory;

    [SerializeField] private Collider sausageCollider;
    [SerializeField] private LayerMask levelMask;


    void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!IsGrounded())
        {
            trajectory.Hide();
        }
        else if (IsGrounded())
        {
            if (Input.GetMouseButtonDown(0)) //Strat dragging
            {
                OnDragStart();
                trajectory.Show();
            }

            if (Input.GetMouseButton(0)) //Dragging
            {
                OnDrag();
                trajectory.UpdateDots(sausage.pos, shootingForce * power);
            }

            if (Input.GetMouseButtonUp(0)) //Release button
            {
                OnDragEnd();
            }
        }
    }

    void OnDragStart()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7);
        startPoint = mainCamera.ScreenToWorldPoint(mousePos);
    }

    void OnDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 7);
        endPoint = mainCamera.ScreenToWorldPoint(mousePos);

        shootingForce = new Vector3(
        Mathf.Clamp(startPoint.x - endPoint.x, minPower.x, maxPower.x),
        Mathf.Clamp(startPoint.y - endPoint.y, minPower.y, maxPower.y), 
        0f);

        shootForce = new Vector4(shootingForce.x, shootingForce.y);
    }

    void OnDragEnd()
    {
        sausage.Push(shootForce * power);
        trajectory.Hide();
    }

    private bool IsGrounded()
    {
        //Коробка обновляется только при движении, если сосиска лежит в покое то возвращается false
        /*
        Physics.BoxCast(sausage.col.bounds.center + new Vector3 (sausage.col.bounds.center.x, sausage.col.bounds.extents.y, sausage.col.bounds.center.z),
            sausage.col.bounds.size, Vector3.down, out RaycastHit raycastHit, sausage.transform.rotation, 1f, levelMask);
        */

        //Пришлось пока прийти к такому костылю, лучи во все стороны
        Physics.Raycast(sausageCollider.bounds.center, Vector3.down, out RaycastHit raycastHitDown, .5f, levelMask);
        Physics.Raycast(sausageCollider.bounds.center, Vector3.up, out RaycastHit raycastHitUp, .5f, levelMask);
        Physics.Raycast(sausageCollider.bounds.center, Vector3.right, out RaycastHit raycastHitRight, .5f, levelMask);
        Physics.Raycast(sausageCollider.bounds.center, Vector3.left, out RaycastHit raycastHitLeft, .5f, levelMask);
        Physics.Raycast(sausageCollider.bounds.center, Vector3.forward, out RaycastHit raycastHitforward, .5f, levelMask);
        Physics.Raycast(sausageCollider.bounds.center, Vector3.back, out RaycastHit raycastHitBack, .5f, levelMask);

        if (raycastHitDown.collider || raycastHitUp.collider || raycastHitRight.collider || raycastHitLeft.collider || raycastHitforward.collider || raycastHitBack.collider)
        {
            return true;
        }
        else { return false; }
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}