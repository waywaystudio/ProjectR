// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using Random = System.Random;
//
// namespace Character
// {
//     public static class ProjectorUtility
//     {
//         public const int SamplingCount = 8;
//         
//         /// <summary>
//         /// Try Poisson Disc Sampling
//         /// </summary>
//         /// <param name="radius">Searching Area Radius</param>
//         /// <param name="minDistance">each element minimum distance</param>
//         /// <param name="maxPoints">element count</param>
//         /// <param name="sampleSize">count of try searching for Next element</param>
//         /// <returns>Vector2 list, count of maxPoints</returns>
//         public static List<Vector2> GetPointsInCircle(int radius, int minDistance, int maxPoints, int sampleSize)
//         {
//             var points = new List<Vector2>();
//             var random = new Random();
//
//             // Pick a random starting point
//             points.Add(new Vector2(random.Next(radius), random.Next(radius)));
//
//             while (points.Count < maxPoints)
//             {
//                 if (points.IsNullOrEmpty()) points.Add(new Vector2(random.Next(radius), random.Next(radius)));
//                 
//                 var point = points[random.Next(points.Count)];
//                 var added = false;
//
//                 for (var i = 0; i < sampleSize; i++)
//                 {
//                     var angle = 2 * Math.PI * random.NextDouble();
//                     var distance = minDistance + (random.NextDouble() * minDistance);
//                     var newPoint = new Vector2((int)(point.x + (Math.Cos(angle) * distance)),
//                         (int)(point.y + (Math.Sin(angle) * distance)));
//
//                     if (points.All(p =>
//                             (Math.Pow(p.x - newPoint.x, 2) + Math.Pow(p.y - newPoint.y, 2)) >= Math.Pow(minDistance, 2))
//                         && (Math.Pow(newPoint.x, 2) + Math.Pow(newPoint.y, 2)) < Math.Pow(radius, 2))
//                     {
//                         points.Add(newPoint);
//                         added = true;
//                         break;
//                     }
//                 }
//
//                 if (!added)
//                 {
//                     points.Remove(point);
//                 }
//             }
//
//             return points;
//         }
//     }
// }
