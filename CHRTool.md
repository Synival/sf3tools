# CHRTool

**Command-line tool for making or manipulating sprites for Shining Force III.**

## About

Shining Force III's sprites are spread out among nearly 1,000 files across its four discs. These files have either the
`.CHR` or `.CHP` extension. Each `.CHR` file contains all the sprites needed for a particular scene or battle, and each
`.CHP` file contains a collection of sprites that can be dynamically loaded/unloaded for any scene or battle.

The `chrtool` provides tools for both .CHR and .CHP files to:

- **Decompile** existing `.CHR` / `.CHP` files to source code (`.SF3CHR` / `.SF3CHP` files)
- **Extract** frame images from `.CHR` / `.CHP` files into spritesheets
- **Compile** source code (`.SF3CHR` / `.SF3CHP` files) to `.CHR` / `.CHP` files using extracted or original spritesheets
- **Describe** the contents of any sprite-related file (`.CHR`, `.CHP`, `.SF3CHR`, `.SF3CHP`, `.SF3Sprite`, `.SF3CHRSprite`)

## Usage

The general syntax of `chrtool` is:

```
chrtool [GENERAL_OPTIONS] <command> [COMMAND_OPTIONS]
```

(Run `chrtool --help` for a list of all possible options and commands.)

## Commands

The following commands are available:

### `compile`

Compiles one or more `.SF3CHR` / `.SF3CHP` files (or all `.SF3CHR` / `.SF3CHP` files in a directory) into `.CHR` / `.CHP` files.

### `decompile`

Decompiles one or more `.CHR` / `.CHP` files (or all `.CHR` / `.CHP` files in a directory) into `.SF3CHR` / `.SF3CHP` files

### `extract-sheets`

Opens one or more `.CHR` / `.CHP` files (or all `.CHR` / `.CHP` files in a directory) and extracts frame images to
spitesheets in `.PNG` format.

Frames are identified in the file `Resources/FrameHashLookups.json`, which contains a list of MD5 hashes corresponding
to all known frame images, the sprites they belong to, the frame's name, and frame's direction. Details about how to
organize the spritesheet is conatined in each sprite's corresponding `.SF3Sprite` file located in the
`Resources/Sprites` directory.

### `update-hash-lookups`

\<TODO\>

### `describe`

\<TODO\>

## Manipulatable File Types

### `.CHR` files

Collections of sprites required for a specific scene or battle.

### `.CHP` files

Collections of `.CHR` files that each contain individual sprites that are loaded dynamically when required.

### `.SF3Sprite` files

Definition for a sprite that can be used in a `.CHR` file. Contains a list of frames and animations.

### `.SF3CHR` files

Source code for a `.CHR` file in JSON format. Contains a list of sprites with corresponding frames and
animations.

### `.SF3CHP` files

Source code for a `.CHP` file in JSON format. Contains a list of `.SF3CHR` objects.

### `.SF3CHRSprite` files

Source code with an individual sprite entry for a `.SF3CHR` file.
