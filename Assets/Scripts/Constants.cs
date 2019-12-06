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
        public static string FloorTag = "Floor";
        public static string FloorTriggerTag = "FloorTrigger";
        public static int PlayerDoubleJumpAttemptsDefaultCount = 1;
        public static string WallTag = "Wall";
        public static string WallTriggerTag = "WallTrigger";
        public static double PlayerFloorJumpReloadTime = 0.2;
    }
}
