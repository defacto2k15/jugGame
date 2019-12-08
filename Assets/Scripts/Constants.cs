using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class Constants
    {
        public static string PlayerTag = "Player";
        public static string FloorTag = "Floor";
        public static string FloorTriggerTag = "FloorTrigger";
        public static string WallTag = "Wall";
        public static string WallTriggerTag = "WallTrigger";
        public static string KeyTag = "Key";
        public static string DoorBox = "DoorBox";
        public static string KeyHoleTag = "KeyHole";

        public static double PlayerFloorJumpReloadTime = 0.2;
        public static int PlayerDoubleJumpAttemptsDefaultCount = 1;
    }
}
