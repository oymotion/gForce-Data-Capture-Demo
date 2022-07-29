using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameCtrler
{
    class QuatUtils
    {

        /// <summary>
        /// Get Euler Angles from quaternoin
        /// https://stackoverflow.com/questions/12088610/conversion-between-euler-quaternion-like-in-unity3d-engine
        /// </summary>
        /// <param name="quat"></param>
        /// <returns>Vector for Euler angles in <pitch, roll, yaw></returns>
        public static Vector3 QuatToEuler(Vector4 quat)
        {
            Vector3 euler;

            float w = quat.W;
            float x = quat.X;
            float y = quat.Y;
            float z = quat.Z;

            // if the input quaternion is normalized, this is exactly one. Otherwise, this acts as a correction factor for the quaternion's not-normalizedness
            float unit = (x * x) + (y * y) + (z * z) + (w * w);

            // this will have a magnitude of 0.5 or greater if and only if this is a singularity case
            float test = x * w - y * z;

            if (test > 0.4995f * unit) // singularity at north pole
            {
                euler.X = (float)(2f * Math.Atan2(y, x));   // pitch
                euler.Y = (float)(Math.PI / 2);             // roll
                euler.Z = 0;                                // yaw
            }
            else if (test < -0.4995f * unit) // singularity at south pole
            {
                euler.X = (float)(-2f * Math.Atan2(y, x));  // pitch
                euler.Y = (float)(-Math.PI / 2);            // roll
                euler.Z = 0;                                // yaw
            }
            else // no singularity - this is the majority of cases
            {
                euler.X = (float)(Math.Atan2(2f * x * w + 2f * y * z, 1 - 2f * (z * z + w * w)));   // pitch
                euler.Y = (float)(Math.Asin (2f * (x * z - w * y)));                                // roll
                euler.Z = (float)(Math.Atan2(2f * x * y + 2f * z * w, 1 - 2f * (y * y + z * z)));   // yaw
            }

            euler.X += (float)Math.PI;
            if (euler.X >= (float)Math.PI) euler.X -= (float)(2 * Math.PI);

            // all the math so far has been done in radians. Before returning, we convert to degrees...
            euler *= (float)Rad2Deg;

            //...and then ensure the degree values are between 0 and 360
            euler.X %= 360;
            euler.Y %= 360;
            euler.Z %= 360;

            return euler;
        }

        public static Vector4 EulerToSimpleQuat(Vector3 euler)
        {
            Vector4 quat;

            float xOver2 = euler.X * Deg2Rad * 0.5f;
            float yOver2 = euler.Y * Deg2Rad * 0.5f;
            float zOver2 = euler.Z * Deg2Rad * 0.5f;

            float sinXOver2 = (float)Math.Sin(xOver2);
            float cosXOver2 = (float)Math.Cos(xOver2);
            float sinYOver2 = (float)Math.Sin(yOver2);
            float cosYOver2 = (float)Math.Cos(yOver2);
            float sinZOver2 = (float)Math.Sin(zOver2);
            float cosZOver2 = (float)Math.Cos(zOver2);

            float x = cosYOver2 * sinXOver2 * cosZOver2 + sinYOver2 * cosXOver2 * sinZOver2;
            float y = sinYOver2 * cosXOver2 * cosZOver2 - cosYOver2 * sinXOver2 * sinZOver2;
            float z = cosYOver2 * cosXOver2 * sinZOver2 - sinYOver2 * sinXOver2 * cosZOver2;
            float w = cosYOver2 * cosXOver2 * cosZOver2 + sinYOver2 * sinXOver2 * sinZOver2;

            quat.W = w;
            quat.X = x;
            quat.Y = y;
            quat.Z = z;

            return quat;
        }


        private const float Deg2Rad = (float)(Math.PI / 180f);
        private const float Rad2Deg = (float)(180f / Math.PI);
    }
}