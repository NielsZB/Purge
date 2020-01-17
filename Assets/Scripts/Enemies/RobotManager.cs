using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotManager : MonoBehaviour
{
    List<RobotBehavior> readyRobots;

    public void RobotIsReady(RobotBehavior robot)
    {
        readyRobots.Add(robot);
    }

    public void RobotIsNotReady(RobotBehavior robot)
    {
        readyRobots.Remove(robot);
    }
}
