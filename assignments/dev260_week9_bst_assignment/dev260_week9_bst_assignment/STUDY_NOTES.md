# Assignment 9: BST File System Navigator - Implementation Notes

**Name:** Giecel Tumbaga

## Binary Search Tree Pattern Understanding

**How BST operations work for file system navigation:**
[Explain your understanding of how O(log n) searches, automatic sorting through in-order traversal, and hierarchical file organization work together for efficient file management]

Answer:
BST operations allow efficient file system navigation because each node can be searched in O(log n) time when the tree is balanced. This makes it quick to locate files or directories by name since decisions (go left or right) are made at every node based on comparisons. In-order traversal is especially useful because it automatically returns the stored items in sorted order, which makes listing files alphabetically straightforward. Mapping this to a file system structure helps mimic how directories are organized hierarchically, allowing fast searches and structured organization.

## Challenges and Solutions

**Biggest challenge faced:**
[Describe the most difficult part of the assignment - was it recursive tree algorithms, custom file/directory comparison logic, or complex BST deletion?]

Answer: The hardest part of the assignment was correctly handling the recursive BST operations, especially when dealing with left-recursive and right-recursive calls and keeping track of the return values. It was surprisingly easy to accidentally lose a reference or break the structure of the tree when modifying nodes.

**How you solved it:**
[Explain your solution approach and what helped you figure it out - research, debugging, testing strategies, etc.]

Answer: To solve this, I revisited lecture notes and reviewed several recursion diagrams to make sure I understood how values flow back up the call stack. I also added print statements during early testing to confirm that the recursion was moving in the direction I expected. Testing with small, simple trees (like trees with 1–3 nodes) helped me isolate where my logic was going wrong before trying bigger structures.

**Most confusing concept:**
[What was hardest to understand about BST operations, recursive thinking, or file system hierarchies?]

Answer: The most confusing part was BST deletion cases, especially when deleting a node with two children. Understanding why we replace it with the in-order successor—and making sure the successor itself gets removed properly—took some rereading and debugging. Keeping the tree valid after deletion required more careful thought than I expected.

## Code Quality

**What you're most proud of in your implementation:**
[Highlight the best aspect of your code - maybe your recursive algorithms, custom comparison logic, or efficient tree traversal]

Answer: I’m proud of how clean and consistent my recursive logic ended up. I made sure the helper methods were well-structured, and I documented the trickier parts of the code so future me could understand why certain decisions were made. My traversal methods are also very readable and follow the BST patterns we learned in class.

**What you would improve if you had more time:**
[Identify areas for potential improvement - perhaps better error handling, more efficient algorithms, or additional features]

Answer: I would improve error handling, especially around null nodes and invalid file/directory names. I would also explore making my comparison logic more flexible so it could support case-insensitive searching or different sorting modes. If time allowed, I’d add features like subtree size tracking or more interactive display options.

## Real-World Applications

**How this relates to actual file systems:**
[Describe how your implementation connects to tools like Windows File Explorer, macOS Finder, database indexing, etc.]

Answer: This assignment mirrors how file systems like those in Windows File Explorer or macOS Finder rely on hierarchical structures to organize files. While real file systems are more complex and don’t use a single BST underneath, the concept of navigating through parents and children is similar. The idea of sorted directory listings, quick searches, and structured navigation connects directly to real operating system behaviors and even database indexing.

**What you learned about tree algorithms:**
[What insights did you gain about recursive thinking, tree traversal, and hierarchical data organization?]

Answer: I learned how important recursion is for tree-based structures and how each operation—insert, search, delete, traverse—basically depends on the same pattern of checking a node and then moving left or right. I also gained a better understanding of why trees make it easier to represent hierarchical data and why maintaining structure is so important to ensure performance.

## Stretch Features

[If you implemented any extra credit features like file pattern matching or directory size analysis, describe them here. If not, write "None implemented"]

Answer: None Implemented

## Time Spent

**Total time:** 7 hours

**Breakdown:**

- Understanding BST concepts and assignment requirements: 1 hour
- Implementing the 8 core TODO methods: 3 hours
- Testing with different file scenarios: 1 hour
- Debugging recursive algorithms and BST operations: 1.5 hours
- Writing these notes: 0.5 hours

**Most time-consuming part:** The most time-consuming part was debugging the recursive logic for deletion, especially ensuring that the in-order successor replacement worked correctly and didn’t break the tree structure.
