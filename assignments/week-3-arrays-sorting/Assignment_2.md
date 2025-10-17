# Assignment 2 – Week 3

## Part A – Board Game

**Game Chosen:** Connect Four  

**Board Size:** 6 rows × 7 columns  

**How to Play / Reset:**  
- Two players take turns dropping tokens (`X` and `O`) into one of the 7 columns.  
- Tokens “fall” to the lowest empty space in the selected column (gravity mechanic).  
- The first player to align **4 tokens in a row**, either horizontally, vertically, or diagonally, wins.  
- If the board fills completely with no winner, the game ends in a draw.  
- To reset and start a new game, players are prompted after each game whether they want to play again (`y/n`). Selecting `y` clears the board and resets the game state (`currentPlayer = 'X'`, `gameOver = false`, `winner = ""`).  

**How the 2D Array is Used:**  
- The board is implemented as a 2D array `char[6,7]`.  
- Each cell represents a position on the board and holds `' '` for empty, `'X'` for Player 1, or `'O'` for Player 2.  
- The array allows easy checking for win conditions by iterating through rows, columns, and diagonals.  
- Dropping a token is implemented by iterating **bottom-up** in the selected column to find the first empty slot.  

---

## Part B – Book Catalog

**Recursive Sort Implemented:** MergeSort  

**Normalization Rules:**  
1. Titles are converted to uppercase.  
2. Leading articles (`"THE "`, `"A "`, `"AN "`) are removed to improve alphabetical sorting.  
3. Whitespace is trimmed.  
4. If a title is only an article, the original title is preserved.  

**How the 2D Index Works:**  
- A **26×26 array** is used to index books by the first two letters of the normalized title.  
- `startIndex[i,j]` stores the first book starting with letters `i` and `j`.  
- `endIndex[i,j]` stores the index **after the last book** starting with letters `i` and `j`.  
- This allows the lookup algorithm to narrow down the search range to only books sharing the first two letters, reducing unnecessary comparisons.  

**Lookup Process:**  
1. Normalize user query using the same rules as above.  
2. Use the first two letters to retrieve the start and end index from the 2D index.  
3. Perform a binary search within this range for an exact match.  
4. If no exact match is found, display suggestions from the local range or the entire catalog.  

**Big-O Complexity:**  
- **MergeSort:** O(n log n) – sorts `n` books recursively.  
- **2D Indexed Lookup + Binary Search:** O(log k), where `k` is the number of books sharing the first two letters.  
- **Worst-case lookup (without index):** O(log n) using standard binary search across the entire catalog.  
- **Memory:** O(n) extra for MergeSort temporary arrays and O(26²) for the index.  

---

✅ **Summary:**  
- Part A demonstrates handling a **dynamic 2D array** for an interactive game with win/draw detection.  
- Part B demonstrates **recursive sorting, normalization, and multi-dimensional indexing** to perform efficient book lookups in a large dataset.