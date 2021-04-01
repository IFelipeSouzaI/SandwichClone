# SandwichClone
Linkedin Ability Showcase - Sandwich Clone

# Scriptable Object (Ingredient)
- strID: identify what type of ingredient it is
```
B = bread, O = onion, T = Tomato, S = salad
```
- mesh: keep the ingredient mesh
- material: keep the ingredient material (its not being used since all the ingredient uses the same material)

# Scriptable Object (Level)
- Used to create a level, store the Ingredient what every row should have;
- topRow;
- topMiddleRow;
- bottomMiddleRow;
- bottomRow;

The logic used has two primordial elements: Pieces and Board Matrix

# Board Implementation
- A matrix of GameObjects 4x4 to keep all the pieces reference

# Piece Implementation
- Every piece has an (int) pieceID -> This is used to identify what piece need to move and the position in the board matrix (row = (int)ID/4, column = ID%4);
**ID Distribution (4x4 board):**
```
12 13 14 15
 8  9 10 11
 4  5  6  7
 0  1  2  3
```
- Every piece has a (str) ingredientID -> Used to indentify what type of ingredient it is;
- Every piece has a (int) ingredientAmount -> Keep how many pieces are attached;

# Piece Movement Implementation
- First, a unit vector is created based in the angle between the first and last touch position (This vector contains the direction what the piece should move);
- This vector + piece position give the target position in the board;
- If a piece was found in the target position, so the movement is possible;
- When a pice move to another, both pices has their ingredientAmount summed and the piece what is moving add their inverted ingredientID to the piece below;
**The ingredientID is inverted to simulate the rotation, example: (Bread, Tomato, Onion) BTO is fliped to B:**
**B will receive BTO inverted, so the final result is: B + OTB (Onion, Tomato, Bread) all the ingredients fliped;**
- When a movement is finished, a verification is called to see if all the ingredients are attached and to see if has a bread in top and bottom;

# Event System
- All the comunications between pieces and border are made using events system;
