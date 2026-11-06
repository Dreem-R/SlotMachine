using System;

namespace Slots.Core
{
    // How far a reel has travelled at any moment of a spin: speed up, cruise, then slow down
    // with a small overshoot. It's worked out from elapsed time, so the frame rate can't change
    // the path, and the speed lines up where the phases meet so there's no visible jerk
    public sealed class ReelMotion
    {
        private readonly float accelerationDuration;
        private readonly float brakingDuration;
        private readonly float overshoot;
        private readonly float brakingStartTime;
        private readonly float brakingStartDistance;
        private readonly float brakingDistance;

        public float Speed { get; }
        public float Duration { get; }
        public int Distance { get; }

        public float CruiseStartTime => accelerationDuration;

        public ReelMotion(float duration, float targetSpeed, float accelerationDuration, float brakingDuration, float overshoot)
        {
            this.accelerationDuration = accelerationDuration;
            this.brakingDuration = brakingDuration;
            this.overshoot = overshoot;

            Duration = duration;

            float cruiseDuration = Math.Max(0f, duration - accelerationDuration - brakingDuration);
            brakingStartTime = accelerationDuration + cruiseDuration;

            // Distance per unit of speed in each phase, the area under its speed curve.
            // The braking curve starts at (overshoot + 3) times its distance over its time
            float distancePerSpeed = accelerationDuration / 2f + cruiseDuration + brakingDuration / (overshoot + 3f);

            // A whole number of stops, then the speed is nudged so the reel lands exactly on one
            Distance = Math.Max(1, (int)Math.Round(targetSpeed * distancePerSpeed));
            Speed = Distance / distancePerSpeed;

            brakingDistance = Speed * brakingDuration / (overshoot + 3f);
            brakingStartDistance = Distance - brakingDistance;
        }

        public float Evaluate(float elapsed)
        {
            if (elapsed <= 0f) return 0f;
            if (elapsed >= Duration) return Distance;

            if (elapsed < accelerationDuration)
            {
                // Smoothstep speed curve, integrated to get distance
                float t = elapsed / accelerationDuration;
                return Speed * accelerationDuration * (t * t * t - t * t * t * t / 2f);
            }

            if (elapsed < brakingStartTime)
            {
                return Speed * (accelerationDuration / 2f + elapsed - accelerationDuration);
            }

            // Back ease-out: passes the target slightly, then settles back onto it
            float remaining = (elapsed - brakingStartTime) / brakingDuration - 1f;
            float eased = 1f + (overshoot + 1f) * remaining * remaining * remaining + overshoot * remaining * remaining;
            return brakingStartDistance + brakingDistance * eased;
        }
    }
}
