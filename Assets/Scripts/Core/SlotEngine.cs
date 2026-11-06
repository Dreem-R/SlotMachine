namespace Slots.Core
{
    // Decides spin results, one random stop per reel
    public sealed class SlotEngine
    {
        private readonly SlotModel model;
        private readonly IRandomSource random;

        public SlotEngine(SlotModel model, IRandomSource random)
        {
            this.model = model;
            this.random = random;
        }

        public SpinResult Spin()
        {
            int[] stops = new int[model.ReelCount];
            for (int i = 0; i < stops.Length; i++)
            {
                stops[i] = random.Next(model.GetReel(i).Length);
            }

            return model.Resolve(stops);
        }
    }
}
