using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GorgaTech.ESP
{
    class W2S
    {
       
        public static double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
        public static Vector3 WorldToScreen(Vector3 player, Vector3 target, Vector3 camrotation) {
 
            /*Translate by - camerapos*/
            var camerapos = new Vector3(player.x, player.y, -player.z);
            var point = new Vector3(target.x, target.y, -target.z);
 
            point -= camerapos;
 
            /*Construct rotation matrix
            Rotations would be negative here to account for opposite direction of     rotations in Unity's left-handed coordinate system, 
            but since we're making them negative already this cancels out*/

            float cosX = (float)Math.Cos(DegreeToRadian(camrotation.x));
            float cosY = (float) Math.Cos(DegreeToRadian(camrotation.y));
            float cosZ = (float) Math.Cos(camrotation.z);
            float sinX = (float) Math.Sin(DegreeToRadian(camrotation.x));
            float sinY = (float) Math.Sin(DegreeToRadian(camrotation.y));
            float sinZ = (float) Math.Sin(camrotation.z);
 
            float[,] matrix = new float[3, 3];
            matrix[0, 0] = cosZ * cosY - sinZ * sinX * sinY;
            matrix[0, 1] = -cosX * sinZ;
            matrix[0, 2] = cosZ * sinY + cosY * sinZ * sinX;
            matrix[1, 0] = cosY * sinZ + cosZ * sinX * sinY;
            matrix[1, 1] = cosZ * cosX;
            matrix[1, 2] = sinZ * sinY - cosZ * cosY * sinX;
            matrix[2, 0] = -cosX * sinY;
            matrix[2, 1] = sinX;
            matrix[2, 2] = cosX * cosY;
 
            /*Apply rotation matrix to target point*/
            Vector3 rotatedPoint;
 
            rotatedPoint.x = matrix[0, 0] * point.x + matrix[0, 1] * point.y + matrix[0, 2] * point.z;
            rotatedPoint.y = matrix[1, 0] * point.x + matrix[1, 1] * point.y + matrix[1, 2] * point.z;
            rotatedPoint.z = matrix[2, 0] * point.x + matrix[2, 1] * point.y + matrix[2, 2] * point.z;
 
            /*Revert to left-handed coordinate system*/
            return new Vector3(rotatedPoint.x, rotatedPoint.y, -rotatedPoint.z);
        }
    }
}
