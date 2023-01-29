using UnityEngine;

public class Camera_movement : MonoBehaviour
{
    public Transform player;
    [SerializeField] Vector3 offset;
    [Range(1, 10)]
    [SerializeField] float smoothFactor;

    private void FixedUpdate()
    {
        Follow_player();
    }
    
    void Follow_player()
    {
        Vector3 playerpos = player.position + offset;
        Vector3 smoothpos = Vector3.Lerp(transform.position, playerpos, smoothFactor * Time.fixedDeltaTime);

        transform.position = smoothpos;

    }
}
