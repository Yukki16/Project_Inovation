using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private FallingObjectSO fallingObjectSO;
    [SerializeField] private FallingObject facturedObject;
    [SerializeField] private float moveSpeedDown = 1.5f;

    public FallingObjectSO GetFallingObjectSO() { return fallingObjectSO; }

    private void Update()
    {
        MoveDown();
        if (GameManager.Instance.GetLowestHeightOfAllPlayers() - transform.position.y > 10)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            player.FreezeMovement();
            Destroy();
        }
    }

    public void Destroy()
    {
        Transform transformO = Instantiate(facturedObject.fallingObjectSO.prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void MoveDown()
    {
        Vector3 moveDir = new Vector3(0, moveSpeedDown, 0);
        transform.position -= moveDir * Time.deltaTime;
    }

}
