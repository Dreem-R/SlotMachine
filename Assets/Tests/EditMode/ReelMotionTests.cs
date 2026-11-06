using NUnit.Framework;

namespace Slots.Core.Tests
{
    public class ReelMotionTests
    {
        private static ReelMotion CreateMotion()
        {
            return new ReelMotion(duration: 1.3f, targetSpeed: 14f, accelerationDuration: 0.25f, brakingDuration: 0.7f, overshoot: 1f);
        }

        [Test]
        public void Evaluate_StartsAtZero_AndLandsExactlyOnTheDistance()
        {
            ReelMotion motion = CreateMotion();

            Assert.That(motion.Evaluate(0f), Is.EqualTo(0f));
            Assert.That(motion.Evaluate(motion.Duration), Is.EqualTo((float)motion.Distance));
        }

        [Test]
        public void Evaluate_NeverJumps_BetweenFrames()
        {
            const float FrameTime = 1f / 240f;

            ReelMotion motion = CreateMotion();
            float maxStep = motion.Speed * FrameTime * 1.05f;
            float previous = 0f;

            for (float time = FrameTime; time <= motion.Duration; time += FrameTime)
            {
                float current = motion.Evaluate(time);
                Assert.That(current - previous, Is.LessThanOrEqualTo(maxStep), $"at {time}s");
                previous = current;
            }
        }

        [Test]
        public void Evaluate_PassesTheTarget_ThenSettlesBack()
        {
            ReelMotion motion = CreateMotion();
            float furthest = 0f;

            for (float time = 0f; time < motion.Duration; time += 0.005f)
            {
                furthest = System.Math.Max(furthest, motion.Evaluate(time));
            }

            Assert.That(furthest, Is.GreaterThan(motion.Distance));
        }
    }
}
