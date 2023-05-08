using UnityEngine;

namespace Fight.Utils
{
    public static class Utils
    {
        public static bool RaycastFindLayer(Vector3 startPoint, Vector3 dir, float dist, int layerFind = 0)
        {
            RaycastHit2D[] hit;
            bool result = false;
            hit = Physics2D.RaycastAll(startPoint, dir, dist);

            for (int i = 0; i < hit.Length; i++)
            {
                if (hit[i].collider.gameObject.layer.Equals(layerFind))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }
    }
}
