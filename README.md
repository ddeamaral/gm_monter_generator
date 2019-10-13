# About this project

I created this project in response to a friend needing a toool that generated every permutation for an encounter in a D&D West Marches server. The use case to make this program was strong, as there were over 81,000 permutations that could be used, and only a select handful are applicable depending on the input given to the program.

# How to use it
If you use the publish script inside of the src file (publish.sh will publish all three platforms, but move.sh only moves the linux one to the root. 
You'll have to dig around in `./src/bin/Release/netcoreapp3.0/<runtime identifier>/publish/` for the self contained executable), then execute the output executable from the command line, the application will generate a macro.txt that is importable into the roll20 `'import ExportTable'` feature. From there, you will have a rollable table within the game to roll up a random valid encounter for a variable number of players, and a given difficulty and max rank experience, per the servers rules.

## Valid Flags
`-p` - use this flag to identify a player by their level. For Example
- `gm_monster.exe -p 5 -p 6 -p 6 -p 7`
    - This command tells the application you have 4 players, 1 level 5, 2 level 6's, and a level 7 player

`-d` - Use this flag for the difficulty you want the encounter to be. Only valid options are [deadly|hard], per the use case given. More can be added very easily in a future version, if so desired. 

`-r` - Use this flag to specify the targeted rank you'd like the adjusted experience cap to be used. Each rank (A-F and S) has a adjusted experience cap. This flag will identify which cap to use. Note, this does _not_ have to correspond with the player levels supplied to this application, it's just used to choose an experience cap.