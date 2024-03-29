# LiveSplit.Roboquest

This [LiveSplit](https://livesplit.org) component provides autosplit functionality for the game [Roboquest](https://store.steampowered.com/app/692890/Roboquest/). It also shows the player's horizontal speed on a configurable graph, and more features are planned.

![preview.jpg](/images/preview.jpg)

## Features

### Current

- Install the component via LiveSplit's Splits menu
- Autosplit functionality
- Use and show in-game time
- Show your horizontal speed on a configurable graph
- Configurable option to reset the timer upon death
- Configurable option to reset the timer when restarting the run in-game, when you leave the Game Over screen, or when you go to Basecamp

### In-progress

- Update splits/routes at speedrun.com
- Update the Resources section at speedrun.com
- Download split files via LiveSplit's Splits menu
- Adjust instructions accordingly

### Planned

- Show the player's chosen class
- Automatically/dynamically adjust the category/splits (mid-run) based on the route taken
- Choose which category/route to use from the component settings
- Show an icon (or change the theme/layout) when in certain scenarios (boss, swarm, challenge room, etc.)
- Show time/collectible grade. I'll only add this if the devs are okay with it, since it's already shown in the game itself. The only reason to include it here would be for people that will be covering the in-game grade interface with LiveSplit in their recordings/streams.
- Show perks/items. Again, only if the devs approve. This would be for people that will be covering the in-game perks/items interface with LiveSplit in their recordings/streams.
- Show health (including minions/drones?). Again, only if the devs approve. This would be for people that will be covering the in-game healthbar interface with LiveSplit in their recordings/streams.
- Support sub-level splits
- Support single-level runs

## Usage

#### Part 1

- Open LiveSplit
- Right-click LiveSplit's window
- Press `Edit Splits`
- Select `Roboquest` from the `Game Name` menu at the top of the `Splits Editor` window
- Choose an item from the `Run Category` menu in the `Splits Editor` window
- Press the `Activate` button in the `Splits Editor` window
- Press the `Settings` button in the `Splits Editor` window
- Adjust the options in the `Component Settings` window as desired, and then press `OK`
- Adjust other options in the `Splits Editor` window as desired, and then press `OK`
- More information related to splits and routes will be added here soon. For now, refer to the [speedrun.com resources page](https://www.speedrun.com/roboquest/resources)

#### Part 2

- Right-click LiveSplit's window
- Put your mouse over `Compare Against` and then click `Game Time` from the secondary context menu that appears

#### Part 3

- Right-click LiveSplit's window
- Press `Edit Layout`
- If you were previously using the ASL file, click the `Scriptable Auto Splitter` entry and then press the `-` icon at the top-left of the `Layout Editor` window. If you were not using the ASL file previously, you can skip this step
- Press the `+` icon at the top-left of the `Layout Editor` window
- From the context menu that appeared, go to `Other` and then left-click `Roboquest` in the secondary context menu that appears
- Double-click the new `Roboquest` entry in the main section of the `Layout Editor` window. In the new `Layout Settings` window, you should be on the `Roboquest` tab
- Adjust the options as desired (the defaults are sane), and then press `OK` to close the `Layout Settings` window
- Press `OK` to close the `Layout Editor` window

## Notes:

- Currently, this does not keep track of the route you have taken; you should title and organize your runs as appropriate. The autosplit functionality will only work (properly) if you have the correct number of splits configured.
- Splits for each route can be found at [the Roboquest page on speedrun.com](https://www.speedrun.com/roboquest) in [the Resources section](https://www.speedrun.com/roboquest/resources).
- There are two additional options related to the autosplitter functionality:
  - `Reset timer upon death`
    - (Default: Disabled) - When enabled, your LiveSplit timer will reset as soon as you die in-game
  - `Reset timer when restarting or going to Basecamp`
    - (Default: Enabled) - When enabled, your LiveSplit timer will reset when you restart the run via the in-game menu, when you leave the Game Over screen after death, or when you go to Basecamp
- The font used in the preview above is `Hemi Head Rg BoldItalic`

## Credits

- [SphereMJ / kugelrund](https://www.twitch.tv/spheremj) - They are responsible for a significant majority of the code in this component. [Here is a link to the component from which this was forked.](https://github.com/kugelrund/LiveSplit.MemoryGraph)
- [SuicideMachine](https://www.twitch.tv/suicidemachine) - Credited in the original component.
- [TravisDaily](https://github.com/TravisDaily) - Credited in the original component.
- [LongerWarrior](https://github.com/LongerWarrior) - Provided me with a significant amount of information that was required for making the original ASL script. Also reviewed the script at multiple stages to help improve the quality, size, performance, and maintainability.
- [Sotumney](https://www.twitch.tv/sotumney) - Managing the speedrunning community for Roboquest.
- [dmgvol](https://github.com/Dmgvol) - Pointer path improvements in a previous version.
- [Ero](https://github.com/just-ero) - Helped me learn the basics of memory analysis for games in Unreal Engine 4, also showed me how to update this more easily in the future. More recently, helped me significantly improve my understanding.
- [Tedder](https://github.com/TheTedder) - Helped me understand more about pointers in general.
- [Speedrun Tool Development server on Discord](https://discord.gg/N6wv8pW) - Helped with initial research, general questions, and brainstorming. Very active community that helped me hit the ground running.
- [Roboquest community on Discord](https://discord.gg/roboquest) - This is a welcoming community that is easy to be a part of, and I would not have been interested in speedrunning this if that were not the case.

## Other references

- [rtrger's Tomb Raider 3 Gold component](https://github.com/rtrger/Components/tree/master/TombRaider3Gold) - This is based on [fatalis' Lara Croft and the Guardian of Light component](https://github.com/fatalis/LiveSplit.LaraCroftGoL)
- [fatalis' Dishonored component](https://github.com/fatalis/LiveSplit.Dishonored)
- [BitPatty's Final Fantasy X component](https://github.com/BitPatty/LiveSplit.FFX)
- [Jujstme's DEATHLOOP component](https://github.com/Jujstme/LiveSplit.Deathloop)
