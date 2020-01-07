using Sandbox.Game.EntityComponents;
using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using SpaceEngineers.Game.ModAPI.Ingame;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System;
using VRage.Collections;
using VRage.Game.Components;
using VRage.Game.GUI.TextPanel;
using VRage.Game.ModAPI.Ingame.Utilities;
using VRage.Game.ModAPI.Ingame;
using VRage.Game.ObjectBuilders.Definitions;
using VRage.Game;
using VRage;
using VRageMath;

namespace IngameScript
{
    partial class Program : MyGridProgram
    {
        // This file contains your actual script.
        //
        // You can either keep all your code here, or you can create separate
        // code files to make your program easier to navigate while coding.
        //
        // In order to add a new utility class, right-click on your project, 
        // select 'New' then 'Add Item...'. Now find the 'Space Engineers'
        // category under 'Visual C# Items' on the left hand side, and select
        // 'Utility Class' in the main area. Name it in the box below, and
        // press OK. This utility class will be merged in with your code when
        // deploying your final script.
        //
        // You can also simply create a new utility class manually, you don't
        // have to use the template if you don't want to. Just do so the first
        // time to see what a utility class looks like.
        // 
        // Go to:
        // https://github.com/malware-dev/MDK-SE/wiki/Quick-Introduction-to-Space-Engineers-Ingame-Scripts
        //
        // to learn more about ingame scripts.

        // Глобальные переменные
        IMyRemoteControl RemCon;
        List<IMyGyro> Gyros;
        float GyroMult = 10;
        Vector3D Forward;

        //конструктор  

        public Program()
        {
            RemCon = GridTerminalSystem.GetBlockWithName("RemCon") as IMyRemoteControl;
            Gyros = new List<IMyGyro>();
            GridTerminalSystem.GetBlocksOfType<IMyGyro>(Gyros);
            Runtime.UpdateFrequency = UpdateFrequency.Update1;
            Forward = RemCon.WorldMatrix.Forward;
            Forward = Vector3D.Reject(Forward, Vector3D.Normalize(RemCon.GetNaturalGravity()));
        }

        // методы

        public void Main()
        {
            // получим уклон по осям вперед/назад и влево-вправо
            Vector3D GravVector = Vector3D.Normalize(RemCon.GetNaturalGravity());
            GravVector = Vector3D.Reject(GravVector, RemCon.WorldMatrix.Down);
            float Pitch = (float)GravVector.Dot(RemCon.WorldMatrix.Backward);
            float Roll = (float)GravVector.Dot(RemCon.WorldMatrix.Left);
            float Yaw = (float)Forward.Dot(Vector3D.Reject(RemCon.WorldMatrix.Forward, Vector3D.Normalize(RemCon.GetNaturalGravity())));
            
            // For debug only
            // Echo("Pitch: " + Pitch + "\n Roll: " + Roll + "\n Yaw: " + Yaw);

            foreach (IMyGyro Gyro in Gyros)
            {
                Gyro.Pitch = Pitch * GyroMult;
                Gyro.Roll = Roll * GyroMult;
                Gyro.Yaw = Yaw * GyroMult;
            }

        }

    }
}
