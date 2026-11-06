# Slot Machine

A three-reel slot machine made in **Unity 6** for the Underpin Services Unity assignment, using the art they supplied. Credits are simulated, there's no real money involved.

**[Play the demo in your browser](https://dreem-r.github.io/SlotMachine/)**

## How to play

Pick a bet, then spin. You win when all three symbols on the middle row match.

| Action | Control |
|---|---|
| Spin | SPIN button, the lever, or `Space` |
| Change bet | Left and right arrows (10, 50 or 100) |
| Close a win popup | X button or `Space` |

You start with 1000 credits. If you can't afford the lowest bet, the game offers a fresh start.

## Paytable

Each reel has 20 stops, and a symbol appears more often when it has more of them.

| Symbol | Stops per reel | Three of a kind pays | Chance per spin |
|---|---|---|---|
| Seven | 2 | 200x bet | 0.10% |
| BAR | 3 | 30x bet | 0.34% |
| Bell | 6 | 10x bet | 2.70% |
| Cherry | 9 | 4x bet | 9.11% |

That works out to a **93.575% return to player** and a win about **1 spin in 8**. I chose these numbers myself, since the brief asks for payouts but doesn't give any.

## Running the project

1. Open the project in **Unity 6000.0.71f1** or a later 6000.0 version. PrimeTween downloads automatically from npm, so nothing needs importing by hand.
2. Open `Assets/Scenes/GameScene.unity` and press Play. A 16:9 Game view looks best, but the layout fits any window shape.

To run the tests, use Window > General > Test Runner > EditMode.

## How it works

- **The result comes first.** `SlotEngine` picks one random stop per reel before anything moves. The reels are only told where to land, so the animation can't affect the outcome.
- **Randomness.** Live spins use `RandomNumberGenerator`, which can't be predicted from earlier spins and has no modulo bias. It sits behind `IRandomSource`, so tests can use a seeded generator instead.
- **The odds are checked, not assumed.** `RtpCalculator` runs every one of the 8,000 stop combinations through `SlotModel.Resolve`, the same method live spins use. A test compares that against a hand-worked formula, and another simulates a million spins.
- **Reel motion.** `ReelMotion` works out the reel's position from elapsed time, so it follows the same path at any frame rate. It speeds up, cruises, then slows down with a small overshoot, and the speed stays continuous between phases.
- **Logic separate from visuals.** Everything in `Scripts/Core` is plain C# with no Unity dependency. The game and UI talk through C# events, so the UI can change without touching the rules.
- **Data-driven setup.** Symbols, reel strips, payouts, bets and starting credits are ScriptableObject assets. The on-screen paytable is built from the same asset, so it can't disagree with what the game pays.

## Project structure

```
Assets/
├── Scripts/
│   ├── Core/       # Rules, odds and credits, plain C#
│   ├── Data/       # SymbolSO and SlotConfigSO
│   ├── Machine/    # SlotGame, SlotMachine, Reel, LeverVisual, GameInput
│   └── UI/         # HUD, paytable and popups
├── Tests/EditMode/ # Win rule, RTP and reel motion tests
├── Data/           # Symbol and machine config assets
├── Art/            # Supplied art, renamed by purpose
├── Prefabs/        # Paytable row
└── WebGLTemplates/ # Page that fits the game to the browser window
docs/               # WebGL build, served by GitHub Pages as the demo
```

## Built with

- Unity 6 (6000.0.71f1), Universal Render Pipeline (2D Renderer)
- Unity Input System and uGUI with TextMeshPro
- [PrimeTween](https://github.com/KyryloKuzyk/PrimeTween) by Kyrylo Kuzyk, for popups and small UI effects

## Credits

The game art was supplied by Underpin Services for this assignment and belongs to them.

## Author

**Dreem-R**: [github.com/Dreem-R](https://github.com/Dreem-R)
