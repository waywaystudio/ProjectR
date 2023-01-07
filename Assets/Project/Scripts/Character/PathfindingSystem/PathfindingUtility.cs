using UnityEngine;

namespace Character.Pathfinding
{
    public static class PathfindingUtility
    {
        public static Vector2 OutFromCircle(Vector2 inCirclePoint, Vector2 center, float radius)
        {
            // Find the vector between the inner point and the center
            var vX = inCirclePoint.x - center.x;
            var vY = inCirclePoint.y - center.y;
          
            // Scale the vector by the radius
            vX *= radius;
            vY *= radius;
          
            // Add the result back to the center to find the nearest point
            var nearestX = vX + center.x;
            var nearestY = vY + center.y;

            return new Vector2(nearestX, nearestY);
        }

        public class Cone
        {
            public Vector2 A { get; set; }
            public Vector2 B { get; set; }
            public Vector2 C { get; set; }
            public float Radius { get; set; }
        }

        public static Vector2 OutFromCone(Vector2 inConePoint, Cone cone)
        {
            // Find the center of the base of the cone
            var centerX = (cone.A.x + cone.B.x + cone.C.x) / 3.0f;
            var centerY = (cone.A.y + cone.B.y + cone.C.y) / 3.0f;

            // Calculate the distance from the given point to the center of the base of the cone
            var distance = Mathf.Sqrt(Mathf.Pow(inConePoint.x - centerX, 2) + Mathf.Pow(inConePoint.y - centerY, 2));

            // Find the point on the circumference of the base of the cone that is closest to the given point
            var angle = Mathf.Atan2(inConePoint.y - centerY, inConePoint.x - centerX);
            var nearestPoint = new Vector2
            {
                 x = centerX + cone.Radius * Mathf.Cos(angle),
                 y = centerY + cone.Radius * Mathf.Sin(angle)
            };

            return nearestPoint;
        }
    }
}
