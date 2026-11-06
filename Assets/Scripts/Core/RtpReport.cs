namespace Slots.Core
{
    // The long-run numbers for a machine, Rtp and HitFrequency are fractions (0.95 is 95%)
    public readonly struct RtpReport
    {
        public double Rtp { get; }
        public double HitFrequency { get; }
        public long Combinations { get; }

        public RtpReport(double rtp, double hitFrequency, long combinations)
        {
            Rtp = rtp;
            HitFrequency = hitFrequency;
            Combinations = combinations;
        }
    }
}
