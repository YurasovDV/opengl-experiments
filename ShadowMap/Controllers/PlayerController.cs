﻿using System;
using OpenTK;

namespace ShadowMap
{
    partial class ShadowMapEngine
    {
        protected Vector3 TryMove(Vector3 desiredPosition)
        {
            var oldPosition = Player.Position;

            var updated = new Vector3(desiredPosition);

            var closestX = (int)Math.Round(desiredPosition.X);
            var closestZ = (int)Math.Round(desiredPosition.Z);

            var previousClosestX = (int)Math.Round(oldPosition.X);
            var previousClosestZ = (int)Math.Round(oldPosition.X);

            float height;
            bool inside = Map.TryGetValue(closestX, closestZ, out height);

            float previousHeight;
            bool prevInside = Map.TryGetValue(previousClosestX, previousClosestZ, out previousHeight);

            if (!inside || !prevInside)
            {
                updated = oldPosition;
            }

            return updated;
        }
    }
}
