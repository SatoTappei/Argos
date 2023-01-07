using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAStarUnit : MonoBehaviour
{
    const float minPathUpdateTime = .2f;
    const float pathUpdateMoveThreshold = .5f;

    public Transform target;
    public float speed = 20;
    public float turnSpeed = 3;
    public float turnDst = 5;
    public float stoppingDst = 10;

    GridAStarPath path;

    void Start()
    {
        StartCoroutine(UpdatePath());
    }

    // �p�X�̌���������������H��A�j���[�V���������邱��΂�
    public void OnPathFound(Vector3[] waypoints, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = new GridAStarPath(waypoints, transform.position, turnDst, stoppingDst);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator UpdatePath()
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return new WaitForSeconds(.3f);
        }
        //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
        PathRequestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));

        float sqrMoveThreshold = pathUpdateMoveThreshold * pathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return new WaitForSeconds(minPathUpdateTime);

            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                // Unit�����p�X��v�����Ă���A�p�X�̌���������������R�[���o�b�N�ŒH��
                //PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                PathRequestManager.RequestPath(new PathRequest(transform.position,target.position,OnPathFound));
                targetPosOld = target.position;
            }
        }
    }

    IEnumerator FollowPath()
    {
        bool followingPath = true;
        int pathIndex = 0;
        transform.LookAt(path.LookPoints[0]);

        float speedPercent = 1;

        while (followingPath)
        {
            Vector2 pos2D = new Vector2(transform.position.x, transform.position.z);
            while (path.TurnBoundaries[pathIndex].HasCrossedLine(pos2D))
            {
                if (pathIndex == path.FinishLineIndex)
                {
                    followingPath = false;
                    break;
                }
                else
                {
                    pathIndex++;
                }
            }

            // ���̖ڕW�Ɍ������ߒ��ŏ��X�ɉ�]�����Ă���
            if (followingPath)
            {
                if (pathIndex >= path.SlowDownIndex && stoppingDst > 0)
                {
                    speedPercent = Mathf.Clamp01(path.TurnBoundaries[path.FinishLineIndex].DistanceFromPoint(pos2D) / stoppingDst);
                    if (speedPercent < 0.01f)
                    {
                        followingPath = false;
                    }
                }

                Quaternion targetRot = Quaternion.LookRotation(path.LookPoints[pathIndex] - transform.position);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
                transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);
            }

            yield return null;
        }
    }
}
