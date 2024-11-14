# Kutie

Kutie is a drop-in utilities library for Unity whose purpose is to provide small features that I think
should be in Unity or C#, but aren't.

## Editor Utilities

### Open in Command Prompt

(Windows only)

This will create a context menu for the project pane where you can open the folder in a command prompt. On Windows, it checks the built in command prompts in this order:

- Windows Terminal
- Pwsh (PowerShell 7)
- Powershell
- Cmd

## Extensions

### Vector

This library provides extensions and utility functions in `Kutie.VectorUtil` to

- Project a vector onto the coordinate axes (`Vector3.ProjectXZ`)
- Replace a single axis of a vector (`Vector3.WithX`, etc.)
- Take the absolute value (`Vector3.Abs`) component-wise
- min/max (`VectorUtil.Min`, `VectorUtil.Max`)
- create random values component-wise (`VectorUtil.Random`)
- take the volume of a vector representing full extents (`Vector3.Volume`)

### Angles

This library provides functions to normalize angles and clamp them.

### MonoBehaviour

`MonoBehaviour` is extended to contain a function `Defer` to call a `System.Action` after a certain `YieldInstruction`.

Eg.

```c#
void Awake(){
	Defer(() => { Destroy(gameObject); }, new WaitForSeconds(5));
}
```

## Utilities

### Physics

`Kutie.PhysicsUtil` contains static functions to do sorted Ray/Box casts (non-allocating and allocating), by default sorting by distance.