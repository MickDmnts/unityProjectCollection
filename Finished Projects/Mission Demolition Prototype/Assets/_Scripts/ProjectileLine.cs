using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //Singleton

    [Header("Set in inspector")]
    public float minDist = 0.1f; //Min distance between the points

    LineRenderer line; //The LR
    GameObject _poi;
    List<Vector3> points; //The points

    private void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false; //Disable the LR
        points = new List<Vector3>();
    }

    public GameObject poi
    {
        get
        {
            return _poi;
        }
        set
        {
            _poi = value;
            if (_poi != null) //when _poi is set to something new, everything resets
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    //This can be used to clear the line directly
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        //This is called to add a point to the line
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return; //If the point isn't far enough from the last point then it returns
        }
        if (points.Count == 0) //If this is the launch point...
        {
            Vector3 launchPosDif = pt - Slingshot.LAUNCH_POS;
            //...it adds a extra bit of line to aid aiming later
            points.Add(pt + launchPosDif);
            points.Add(pt);
            line.positionCount = 2;
            //Sets the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            //Enables the LR
            line.enabled = true;
        }
        else
        {
            //Normal behaviour of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //Returns the location of the most recent added points
    public Vector3 lastPoint
    {
        get
        {
            if (points == null)
            {
                //If there are no points, return Vector3.zero;
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            //if there is no poi, search for one
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                }
                else
                {
                    return; //Return if we didnt find a poi
                }
            }
            else
            {
                return; //Return if we didnt find a poi
            }
        }
        //if there is a poi, it's location is added every FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            //Once FollowCam.POI is null, make the local poi null too
            poi = null;
        }
    }
}
