using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BattleRoyale
{
    [CreateAssetMenu(fileName = "Stats", menuName = "BattleRoyale/Char Stats")]
    public class CharStatsController : ScriptableObject
    {
        public float fl_Speed = 100;
        public float fl_RunningSpeedIncrement = 1.5f;
        public float fl_RotationSpeed = 250;
        public float fl_JumpForce = 250;
        public float fl_MinAngle = -70;
        public float fl_MaxAngle = 90;
        public float fl_CameraSpeed = 24;

        public float fl_CrouchHeightOffSet = 0;
        public float fl_CrouchPosOffSet= 0;
    }
}
