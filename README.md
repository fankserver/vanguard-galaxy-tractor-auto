# Tractor Auto

Lets the player ship's manual tractor beams also work automatically, scaled by
Engineering (Autopilot) mastery. Extra automatic beams are
`floor(clamp(level / maximumLevel, 0, 1) * manualBeams)`.
Manual targeting can borrow any free beam when its own pool is exhausted. Occupied
beams are never stolen. Native cargo, target-eligibility and crew/brig limits remain.

## Development status

Requires the unreleased VGModAPI equipment, skill-tree, and tooltip services. Public API 0.2.8 does not
contain this feature. Before release, set the loader dependency minimum to the
owner-selected API release containing it; no new release version is assumed here.

## Install and settings

Install BepInEx 5, the matching API build and `VGTractorAuto.dll` in
`BepInEx/plugins/`. VGModAPI is a hard dependency; without it BepInEx refuses the
consumer, leaving vanilla behavior. There is no Harmony fallback.

`BepInEx/config/vgtractorauto.cfg`, `General.Enabled` defaults to true. Setting false
restores vanilla beam behavior and omits new descriptions. Already-built module
stats stay cached, as in vanilla. The Engineering mastery tooltip shows the live
conversion percentage, including 0%; modules with manual beams show a static
explanatory line.

Remove the DLL to uninstall. The mod stores no game-save data.

## Build and test

```sh
make build VGAPI_DLL=/absolute/path/to/VGModAPI.Abstractions.dll
make test
```

The consumer contains no Harmony hooks or native game references. Do not bundle
API, game, Unity or BepInEx DLLs. `make deploy` is a separate explicitly authorized
action, not part of validation.

MIT — see [LICENSE](LICENSE).
