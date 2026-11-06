namespace Slots.Core
{
    // Works out the exact return to player by checking every possible stop combination once.
    // Both results are fractions, so 0.95 means 95%
    public static class RtpCalculator
    {
        public static (double rtp, double hitFrequency) Calculate(SlotModel model)
        {
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

            return ((double)totalMultiplier / combinations, (double)wins / combinations);
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
