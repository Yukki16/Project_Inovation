using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class FallingObject : NetworkBehaviour
{
    public enum RotationDirection
    {
        None,
        Y,
        X,
        Z,
        ALL_DIR
    }

    [SerializeField] protected FallingObjectSO fallingObjectSO;
    [SerializeField] protected RotationDirection rotationDir;
    [SerializeField] protected float moveSpeedDown = 1.5f;

    private int xRotDir;
    private int yRotDir;
    private int zRotDir;

    public FallingObjectSO GetFallingObjectSO() { return fallingObjectSO; }

    Renderer rend;
    float timerToDespawn = 0f;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        xRotDir = ChooseANumber(-1, 1);
        yRotDir = ChooseANumber(-1, 1);
        zRotDir = ChooseANumber(-1, 1);
    }

    protected virtual void Update()
    {
        if (IsServer)
        {
            MoveDown();
            RotateFallingObject();
            CheckIfFallingObjectCanBeDeleted();
            DestroyIfNotOnScreen();
        }
    }

    private void MoveDown()
    {
        Vector3 moveDir = new Vector3(0, moveSpeedDown, 0);
        transform.position -= moveDir * Time.deltaTime;
    }

    protected void DestroyCurrentFallingObject()
    {
        DestroyCurrentFallingObjectServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    protected void DestroyCurrentFallingObjectServerRpc()
    {
        Destroy(gameObject);
    }

    private void RotateFallingObject()
    {
        if (rotationDir != RotationDirection.None)
        {
            Vector3 moveDir = Vector3.zero;
            switch (rotationDir)
            {
                case RotationDirection.Y:
                    moveDir = new Vector3(0, yRotDir, 0);
                    break;
                case RotationDirection.X:
                    moveDir = new Vector3(xRotDir, 0, 0);
                    break;
                case RotationDirection.Z:
                    moveDir = new Vector3(0, 0, zRotDir);
                    break;
                case RotationDirection.ALL_DIR:
                    moveDir = new Vector3(xRotDir, yRotDir, zRotDir);
                    break;
            }
            float rotateSpeed = 75;
            transform.Rotate(moveDir, rotateSpeed * Time.deltaTime);
        }
    }

    private void CheckIfFallingObjectCanBeDeleted()
    {
        if (GameManager.Instance.GetLowestHeightOfAllPlayers() - transform.position.y > 20)
        {
            DestroyCurrentFallingObject();
        }
    }

    private int ChooseANumber(int number1, int number2)
    {
        int number = Random.Range(0, 2);
        if (number == 0)
        {
            return number1;
        }
        else return number2;
    }

    void DestroyIfNotOnScreen()
    {
        if (!rend.isVisible)
        {
            timerToDespawn += Time.deltaTime;
        }

        if (timerToDespawn > 15f)
        {
            this.GetComponent<NetworkObject>().Despawn();
        }
    }
}
