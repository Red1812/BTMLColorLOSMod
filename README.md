# BTMLColorLOSMod
BattleTech Mod (using [ModTek](https://github.com/Mpstark/ModTek)) that changes the color of indirect firing lines in battle to a different color from direct.

You've been in the situation where the lines are both red and you move and then realize you actually don't have an alpha strike. The orange-yellow line is indirect fire:

<img width="201" alt="screen shot 2018-05-20 at 6 53 02 pm" src="https://user-images.githubusercontent.com/50124/40288821-11b0066e-5c83-11e8-98c2-aba640c7dc73.png">

The defaults for this mod setup the indirect line and the nearer target obstructed line to stand out. You can change settings as described below to make the lines very customized in appearance.

## Features
- Change the color of direct line of fire indicator drawn between the currently controlled mech and enemy targets
- Change the color of indirect line of fire indicator drawn between the currently controlled mech and enemy targets
- Change the color of the line of fire indicator for obstructed targets on the attacker and target sides of the obstruction
- Add dashes to the line of fire indicator drawn between the currently controlled mech and enemy targets
- Shift to different color settings with an easy button press

Added by Redferne in v0.6.4:

- Change the color of side and back line of fire indicator drawn between the currently controlled mech and enemy targets 
- Color attacker side when obstructed can keep its color as if not obstructed. Target side remains the same
- Indirect los depending on where you're facing.

## Download
Downloads can be found on [Github](https://github.com/Red1812/BTMLColorLOSMod/releases)

## Install
- [Install ModTek](https://github.com/Mpstark/ModTek)
- Put the `BTMLColorLOSMod.dll` and `mod.json` files into `\BATTLETECH\Mods\BTMLColorLOSMod` folder.
- If you want to change the settings do so in the mod.json.
- Start the game.

## Settings

### Line Settings
For each kind of line supported there is a key with its own settings in `mod.json`. Those keys are `direct`, `indirect`, `side`, `back`, `obstructedAttackerSide`, and `obstructedTargetSide`. They each have data structured like:

```json
{
  "active": true,
  "colors": [
    [255,   0,   0, 255],
    [  0, 255,   0, 255],
    [  0,   0, 255, 255],
    [  0,   0,   0, 255]
  ],
  "dashed": false,
  "thickness": 1
},
```

Setting | Type | Description
--- | --- | ---
`active` | `bool` | enable/disable changes to a particular type of line. if this is false the rest of the changes for a type of line will be ignored
`dashed` | `bool` | enabled/disable making a type of line dashed
`thickness` | `float` | change the size of a given line on-screen to be bigger or smaller. Vanilla game almost everything is 1.
`colors` | `float[][4]` | an array of RGBA color settings that can be used in-game to color the given line
`colorside` | `bool` | enable/disable color attacker side when obstructed to remain the same as if not obstructed. Target side is not affected.

The `color` variables below (`float[4]`) are RGBA colors. You can enter your colors as a number 0-255, or a float from 0 to 1. For example, pure red can be entered as `[255, 0, 0, 255]` (hex/web format) or `[1, 0, 0, 1]` (unity format). If you make the alpha channel `0`, you will probably not have a line at all. You can use a color picker like [the W3's](https://www.w3schools.com/colors/colors_picker.asp) or a color chart like [websitetip](http://websitetips.com/colorcharts/daxassist/)'s to find colors you like.

For unity-style numbers, you can use the color picker and then divide the individual values by 255 to get the decimal you need. For example hex color `#66ff33` / rgb color `rgb(102, 255, 51)` can be entered directly as `[102, 255, 51, 1]` or converted from `[102/255, 255/255, 51/255, 1]`, which works out to `[0.4, 1, 0.2, 1]`.

You can read about why the color sets exist in the release where they were made: https://github.com/janxious/BTMLColorLOSMod/releases/tag/v0.6

Here's some sample colors so you can see what I mean by RGBA:

<img width="273" alt="screen shot 2018-05-28 at 7 53 19 pm" src="https://user-images.githubusercontent.com/50124/40633724-78a0fa12-62bf-11e8-9227-5bcbeae4a0ae.png">

### Keybind Settings

Setting | Type | Description
--- | --- | ---
`nextColorKeyBinding` | `json` | container for setting up the keybinding. description of sub-attributes in this table 
`nextColorKeyBinding`:`active` | `bool | enable/disable the ability to key bind to switch colors and set up the default keybinding (defaults to active)
`nextColorKeyBinding`:`keys` | `string[]` | a series of keys you can press if this is active to switch to the next color set (defaults to shift+i)

### General Settings

Setting | Type | Description
--- | --- | ---
`debug` | `bool` | enabled/disable debug logging

## More Screenshots

Side angle

<img width="576" alt="screen shot 2018-05-20 at 10 57 16 pm" src="https://user-images.githubusercontent.com/50124/40288819-0ed5a9f8-5c83-11e8-8889-ddf65437f444.png">


With the settings on cyan instead of orange

<img width="490" alt="screen shot 2018-05-20 at 3 59 46 pm" src="https://user-images.githubusercontent.com/50124/40288810-0a8ad8f0-5c83-11e8-8234-8598580d4631.png">


Marvelous

<img width="937" alt="screen shot 2018-05-20 at 10 29 41 pm" src="https://user-images.githubusercontent.com/50124/40288822-13ecd8bc-5c83-11e8-8111-8d17f439f9f1.png">


Dashes!

<img width="478" alt="screen shot 2018-05-21 at 12 40 23 am" src="https://user-images.githubusercontent.com/50124/40314402-6610d090-5ce6-11e8-8700-92a0de80f015.png">


Obstructed Colors!

<img width="898" alt="screen shot 2018-05-21 at 10 12 46 am" src="https://user-images.githubusercontent.com/50124/40314408-6bf2caa4-5ce6-11e8-887f-c165546abb1a.png">
<img width="886" alt="screen shot 2018-05-21 at 10 27 46 am" src="https://user-images.githubusercontent.com/50124/40314409-6c17545a-5ce6-11e8-98f2-275870b183b6.png">

## Special Thanks

HBS, @Mpstark, @Morphyum, @gponick


## Maintainer Notes: New HBS Patch Instructions

* pop open VS
* grab the latest version of the assembly
* copy the new version of the methods in `original_src` over the existing ones
* see if anything important changed via git
