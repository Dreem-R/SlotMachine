using System;

namespace Slots.Core
{
    // Works out the exact RTP by checking every possible stop combination once
    public static class RtpCalculator
    {
        public static RtpReport Calculate(SlotModel model)
        {
            if (model == null) throw new ArgumentNullException(nameof(model));

            int[] stops = new int[model.ReelCount];
            long combinations = 0;
            long totalMultiplier = 0;
            long wins = 0;

            do
            {
                SpinResult result = model.Resolve(stops);
                combinations++;
                totalMultiplier += result.PayoutMultiplier;
                if (result.IsWin) wins++;
            }
            while (TryAdvance(stops, model));

            return new RtpReport((double)totalMultiplier / combinations, (double)wins / combinations, combinations);
        }

        // Counts through the stops like an odometer, returns false after the last combination
        private static bool TryAdvance(int[] stops, SlotModel model)
        {
            for (int i = stops.Length - 1; i >= 0; i--)
            {
                stops[i]++;
                if (stops[i] < model.GetReel(i).Length) return true;
                stops[i] = 0;
            }

            return false;
        }
    }
}
