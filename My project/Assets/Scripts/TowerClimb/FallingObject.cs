using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class FallingObject : MonoBehaviour
{
    [SerializeField] private FallingObjectSO fallingObjectSO;
    [SerializeField] private FallingObject facturedObject;
    [SerializeField] private float moveSpeedDown = 1.5f;

    public FallingObjectSO GetFallingObjectSO() { return fallingObjectSO; }

    private void Update()
    {
        MoveDown();
        if (GameManager.Instance.GetLowestHeightOfAllPlayers() - transform.position.y > 20)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();
        if (player)
        {
            player.HitPlayer(gameObject);
            Destroy();
        }
    }

    public void Destroy()
    {
        GameObject newObj = new GameObject("Destroyed Falling Item");
        Transform transformO = Instantiate(facturedObject.fallingObjectSO.prefab, newObj.transform);
        transformO.localPosition = transform.position;
        Destroy(gameObject.transform.parent.gameObject);
    }

    private void MoveDown()
    {
        Vector3 moveDir = new Vector3(0, moveSpeedDown, 0);
        transform.position -= moveDir * Time.deltaTime;
    }

}
