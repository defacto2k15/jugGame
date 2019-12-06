using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class KeyScriptOC : MonoBehaviour
    {
        public KeyBoxScriptOC KeyBox;

        public void AchievedKeyhole()
        {
            KeyBox.AchievedKeyhole();
        }
    }
}
