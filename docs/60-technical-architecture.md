# Technical Architecture (Planned)

- Engine: Unity (2022 LTS/2023/Unity 6 TBD)
- Scripting: C#
- Data: ScriptableObjects for ducks/traits; JSON saves
- Sim: pure C# (deterministic), isolated from Unity APIs
- Render: reads sim log and animates results
- Net (later): async match queue; seed-replay for verification
