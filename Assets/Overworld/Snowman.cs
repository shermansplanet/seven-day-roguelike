using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    public SpriteRenderer[] renderers;
    public Transform shadow;

    private Vector3 footPosition;
    private Vector3 nextFootPosition;
    private Vector3 lastPosition;
    private bool footDown;

    private const float gaitSpeed = 4f;

    public void SetColors(Color[] colors)
    {
        for(int i=0; i<colors.Length; i++)
        {
            renderers[i].color = colors[i];
        }
    }

    public void Start()
    {
        footPosition = renderers[2].transform.position;
        nextFootPosition = footPosition;
    }

    public void Update()
    {
        Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
        float cycleTime = (Time.time * gaitSpeed) % 1;
        if(cycleTime > 0.5f)
        {
            if (!footDown)
            {
                footPosition = renderers[2].transform.position;
            }
            footDown = true;
            renderers[2].transform.position = footPosition;
            shadow.position = footPosition + Vector3.down * 0.5f;
        }
        else
        {
            if (footDown)
            {
                nextFootPosition = transform.position + velocity * 0.75f / gaitSpeed;
            }
            footDown = false;
            float midstepLerp = cycleTime * (0.5f - cycleTime) * 4;
            renderers[2].transform.position = 
                Vector3.Lerp(footPosition, nextFootPosition, cycleTime * 2) +
                new Vector3(0, Vector3.Distance(footPosition, nextFootPosition) * midstepLerp, 0);
            shadow.transform.position = Vector3.Lerp(footPosition, nextFootPosition, cycleTime * 2) + Vector3.down * 0.5f;
            renderers[2].transform.localEulerAngles = new Vector3(0, 0, midstepLerp * velocity.x * 10);
        }

        renderers[1].transform.position = Vector3.Lerp(renderers[0].transform.position, renderers[2].transform.position, 0.5f);
        renderers[1].transform.rotation = Quaternion.LookRotation(Vector3.forward, renderers[0].transform.position - renderers[2].transform.position);
    }

    public void SetSortingOrder(int order)
    {
        foreach(SpriteRenderer r in renderers)
        {
            r.sortingOrder = order;
        }
    }
}
