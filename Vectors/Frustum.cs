using System;

namespace SystemPlus.Vectors
{
    public class Frustum
    {
        #region Variables
        private Plane[] planes;

        /// <summary>
        /// Get the planes that make up the Frustum..
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public Plane this[int a]
        {
            get
            {
                if (a > 6 || a < 0) throw new ArgumentOutOfRangeException();
                else return planes[a];
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Builds a new Frustum and initializes six new planes.
        /// </summary>
        public Frustum()
        {
            planes = new Plane[6];
            for (int i = 0; i < 6; i++) planes[i] = new Plane(0, 0, 0, 0);
        }

        /// <summary>
        /// Builds the Planes so that they make up the left, right, up, down, front and back of the Frustum.
        /// </summary>
        /// <param name="clipMatrix">The combined projection and view matrix (usually from the camera).</param>
        public void UpdateFrustum(Matrix4 clipMatrix)
        {
            planes[0].Set(clipMatrix[3].w - clipMatrix[3].x, new Vector3(clipMatrix[0].w - clipMatrix[0].x, clipMatrix[1].w - clipMatrix[1].x, clipMatrix[2].w - clipMatrix[2].x));
            planes[1].Set(clipMatrix[3].w + clipMatrix[3].x, new Vector3(clipMatrix[0].w + clipMatrix[0].x, clipMatrix[1].w + clipMatrix[1].x, clipMatrix[2].w + clipMatrix[2].x));
            planes[2].Set(clipMatrix[3].w + clipMatrix[3].y, new Vector3(clipMatrix[0].w + clipMatrix[0].y, clipMatrix[1].w + clipMatrix[1].y, clipMatrix[2].w + clipMatrix[2].y));
            planes[3].Set(clipMatrix[3].w - clipMatrix[3].y, new Vector3(clipMatrix[0].w - clipMatrix[0].y, clipMatrix[1].w - clipMatrix[1].y, clipMatrix[2].w - clipMatrix[2].y));
            planes[4].Set(clipMatrix[3].w - clipMatrix[3].z, new Vector3(clipMatrix[0].w - clipMatrix[0].z, clipMatrix[1].w - clipMatrix[1].z, clipMatrix[2].w - clipMatrix[2].z));
            planes[5].Set(clipMatrix[3].w + clipMatrix[3].z, new Vector3(clipMatrix[0].w + clipMatrix[0].z, clipMatrix[1].w + clipMatrix[1].z, clipMatrix[2].w + clipMatrix[2].z));

            for (int i = 0; i < 6; i++)
            {
                float length = planes[i].Normal.Length();
                planes[i].D /= length;
                planes[i].Normal /= length;
            }
        }

        /// <summary>
        /// Builds the Planes so that they make up the left, right, up, down, front and back of the Frustum.
        /// </summary>
        public void UpdateFrustum(Matrix4 projectionMatrix, Matrix4 modelviewMatrix)
        {
            UpdateFrustum(modelviewMatrix * projectionMatrix);
        }

        /// <summary>
        /// True if the AxisAlignedBoundingBox is in (or partially in) the Frustum.
        /// </summary>
        /// <param name="box">AxixAlignedBoundingBox to check.</param>
        /// <returns>True if an intersection exists.</returns>
        public bool Intersects(AxisAlignedBoundingBox box)
        {
            Vector3 boxCenter = box.Center;
            Vector3 boxSize = box.Size;
            for (int i = 0; i < 6; i++)
            {
                Plane p = planes[i];

                float d = Vector3.DotProduct(boxCenter, p.Normal);//boxCenter.Dot(p.Normal);
                float r = Vector3.DotProduct(boxSize, Vector3.Abs(p.Normal));//boxSize.Dot(Vector3.Abs(p.Normal));
                float dpr = d + r;
                //float dmr = d - r;

                if (dpr < -p.D) return false;
            }
            return true;
        }
        #endregion
    }
}
