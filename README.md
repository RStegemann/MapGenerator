Disclaimer: The map generator in its current state aims to test algorithms and the general functionality, as such it does not aim to create visually pleasing maps, but rather functional tile-based ones. Furthermore as it was developed as part of my thesis the UI was programmed quickly to leave more time for functionality, so it will not be particularly comfortable to use.

# Controls
The program shows the generated map in the left part of the screen and utilizes a control panel on the right border. Holding right-click anywhere on the map continuously pans the camera towards the cursor, middle mouse-click centres the camera on the centre of the map and scrolling the mouse wheel zooms in-/out of the map. The control panel is used to configure all map generation settings.
Use the “Generate” Button to manually generate the map, “Export” to Export the map and “X” in the top right to close the program.

# Settings
The control panel utilizes a drop-down menu to swap between 4 different setting categories:
- General: The general tab allows configuration of the used algorithm and map size, as well as offering options to show height values on the tiles or continuously update the map when settings are changed.
- Terrain: The Terrain tab is used to import terrain textures and apply them to a certain range of height values to draw the map.
- Objects: The object tab is used to import object textures and configure a “spawner” to randomly place objects in a given area.
- Paths: This tab is used to draw a path of terrain textures between any number of waypoints. Terrain placed in this way replaces the ground texture that gets drawn based on the height value.
## General settings
- Algorithm: Selects the algorithm, which is used for map generation. Based on the selection a number of additional settings to configure the selected algorithm will become available. These options will be elaborated further below. 
    - Perlin Noise: Recommended for generating outdoor maps with good customization options
    - Spatial Subdivision: Highly random outdoor maps, low configuration. Faster for very big maps (>256x256)
    - Binary Space Partitioning: Indoor maps for dungeon systems. Doesn’t generate furniture as of now, can be used for caves when DLA-Subsetting is enabled.
- Output: Battlemap to draw Terrain, Heightmap to just check the grey values visualizing the generated height (Black (0) → White (1))
- Height Shadow: Enables shading of terrain tiles in grey shades similar to “heightmap” output type. Does not get exported as of now.
- Auto-Update: Automatically updates the map on setting changes. This is highly recommended to get a feeling for what the different settings do.
- Seed: Allows configuration of a certain seed or reroll the map quickly by using the “rnd” button.
- Size: Left width, right height.

## Terrain settings
Clicking “Add” opens a file explorer which can be used to import any number of textures into the program. All of them will be resized to a 100x100 pixel grid and be assigned to texture panel which allows setting the height value at which the texture should be used.
- Active-Toggle: Sets whether or not the texture should be drawn.
- Remove: Removes the texture from the program.
- MinHeight: Sets a value between 0 and 1. Any tile with a height value between this and the terrain texture with the next higher height value will use this texture.
As an example: Water with MinHeight 0, grass MinHeight 0.5, rock MinHeight 1 would draw Water between height 0 and 0.5, grass between 0.5 and 1 and Rock on every tile with height 1.

## Object settings
Similarly to the terrain menu a click on “Add” opens the filebrowser to import any number of textures, each of which will get added to its own panel. When a texture is imported the program determines it’s tile size based on the width/height ratio and resizes the texture to fit the 100x100 grid fields. The panel allows configuration of a spawner for every object:
- Place: Set active to place/move the spawner on the map via left click. Will disable movement of previously activated spawners.
- Size: Scales the size of the object in grid-fields based on its size ratio. It’s recommended to avoid decimal values.
- Spawn Area: Allows configuration of the rectangle size of the spawner. Left width, right height.
- Spawn Amount: Number of objects to place in the area. Avoids overlapping on multiple objects, does not guarantee creation of all objects.
- Seed: Seed for the spawner, allows rerolling placement rng.
- Target Ground: Only spawns objects on the selected terrain type.

## Paths settings
 Allows drawing of a “Texture” Path between waypoints. The main intended functionality of this is to allow drawing streets or rivers. Clicking “Add” adds a texture panel, which can be used to configure the path:
 - Add Texture: Adds the file browser to set a texture, which will get resized to 100x100. Replaces the ground texture at any given path field, therefore it should not have transparent spots.
 - Remove: Deletes the path
 - Placing Points: Sets the path as “active” to allow placing waypoints with leftclick.
 - Traversable Height: Two values between 0 and 1 to configure the minimum(left) and maximum(right) height the path can traverse. Useful to avoid seas or mountains while generating the path.
 - Width: Width of the path in fields. Will generate a path of width 1 first, then extend it based on traversable height settings, which can lead to choke points.    • 
 - Optimal Height: Value between Min and Max traversable Height, which should be favoured when calculating the path.
 - Height Weight: Weight to use for the optimal Height vs shortest path calculation.
 - Point list: Displays coordinates of all points and allows removal of specific waypoints.

# Algorithm Settings
## Perlin Noise
Perlin noise settings can be interpreted as levels of generating a mountain: While one octave generates the general shape, the second generates boulders along the mountain range and the third would add smaller rocks to roughen up the surface.
- Scale: Value between 3 and 300, the smaller the more zoomed-out the noise is.
- Octaves: Number of layered noise functions. The higher the more details will be generated using the lacunarity/persistence settings.
- Persistence: Increases effect of additional octaves to generate rougher details.
- Lacunarity: Adds additional details based on the number of octaves.

## Spatial Subdivision
Just offers a persistence setting to control details/interpolation. Not really recommended for battle maps, but might be interesting to generate a rough outline for a campaign map.

## Binary Space Partitioning
Aims to generate a given number of rooms on the map. Will set height values to 0 for corridors/rooms and 1 for walls. Requires disabling of Height Shading to see ground textures.
- Room Size: Allows setting a minimum(left) and maximum(right) Size for each edge of the room.
- Max Rooms: Maximum amount of rooms to generate on the map. Does not guarantee that the amount fits on the map and based on rng less rooms can be created.
- Hallway Size: Width of the corridors between rooms.
- Randomize Hallway Size: Randomizes the hallway size between 1 and the given number in Hallway Size.
- Use diffusion-limited aggregation: Activates the DLA-Sub-algorithm and it’s corresponding settings to simulate erosion.
    - Spawn Pos: Determines where to spawn the “walkers” used to simulate erosion, which majorly impacts the resulting pattern.
    - Neighbour Height: Height-Value between 0 and 1, used to determine at which neighbouring tiles (0 = floor, walls erode / 1 = walls, floor erodes) to start erosion.
    - Height Change: Value between -1 and 1, used in conjunction with Neighbour height to control erosion behaviour. To reduce floor tiles this needs to be set below 0. Default value is 0.
    - Erosion amount: Value between 0 and 1, indicating the percentage of eligible tiles which should be changed.
Recommended settings to erode a dungeon to a cave use a Neighbour height of 0, height change of 1, fully random spawn positions and a customized erosion amount based on the map. All additional DLA options are for creative use and experimentation.


## Usage Example
1. For speed of use, think for the favoured setting first and prepare a folder with all textures you might be interested in trying out
2. Open the program, ensure output is set to Battle-map and Auto-Update is on. If creating a very huge map (>150x150) consider swapping Auto-Update off to avoid lag on older machines.
3. Set the Size of the map
4. Choose your favoured algorithm
5. Adjust algorithm settings until you you get an interesting map, alternate with 6. for fine-tuning
6. Add Textures in the Terrain-Tab, set them active and adjust height-values until you get an interesting map. Alternate with 5. for fine-tuning.
7. Add objects in the object-tab.
8. Add Paths
9. Export using the “Export” Button

## Credits
Uses Standalone Filebrowser (https://github.com/gkngkc/UnityStandaloneFileBrowser) and the Grid Outline from Forgotten Adventures (https://www.forgotten-adventures.net)
