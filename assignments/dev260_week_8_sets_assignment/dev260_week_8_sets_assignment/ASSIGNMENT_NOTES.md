# Assignment 8: Spell Checker & Vocabulary Explorer - Implementation Notes

**Name:** Giecel Tumbaga

## HashSet Pattern Understanding

**How HashSet<T> operations work for spell checking:**
HashSet<T> provides O(1) average-time complexity for lookups, which is critical for efficiently checking whether each word in a text exists in the dictionary. By storing the dictionary in a HashSet, duplicate words are automatically ignored, and membership checks are extremely fast. During text analysis, unique words from the input file are also stored in a HashSet, which allows us to quickly partition words into correctly spelled and misspelled sets without iterating over duplicates repeatedly. Combining HashSet with List<T> for all words enables both fast uniqueness checks and preservation of word order for frequency analysis.

## Challenges and Solutions

**Biggest challenge faced:**
The biggest challenge was efficiently categorizing words into correctly spelled and misspelled sets after analyzing a large text file. Initially, iterating through all words and checking dictionary membership caused slow performance and redundant processing, especially with repeated words.

**How you solved it:**
I leveraged the properties of `HashSet<T>` to store unique words from the text. By iterating only through the unique words and using `HashSet.Contains()` for dictionary lookups, I achieved fast O(1) checks while automatically avoiding duplicate processing. This approach significantly improved performance and simplified the logic for categorizing words.

**Most confusing concept:**
Understanding the interplay between `HashSet.Contains()` and case-insensitive comparisons was initially confusing. I had to ensure that the dictionary and analyzed text both used `StringComparer.OrdinalIgnoreCase` to avoid mismatches due to capitalization.

## Code Quality

**What you're most proud of in your implementation:**
I’m proud of the comprehensive text normalization and the way I leveraged HashSet operations to categorize words efficiently. The solution handles large texts quickly and avoids duplicate processing, which demonstrates effective use of the data structure.

**What you would improve if you had more time:**
I would improve the tokenization to handle contractions, hyphenated words, and Unicode characters more accurately. Additionally, I could add a feature to suggest corrections for misspelled words or provide frequency counts of each word in the text.

## Testing Approach

**How you tested your implementation:**
I tested using a variety of text files with known words and deliberately inserted misspellings. I checked the outputs of `GetMisspelledWords`, `GetUniqueWordsSample`, and `CheckWord` to ensure correctness.

**Test scenarios you used:**
- Text with mixed case (e.g., "Apple", "apple", "APPLE")  
- Text with punctuation and symbols (e.g., "word,", "word.")  
- Words not present in the dictionary  
- Words repeated multiple times to verify counting  
- Empty and very small files to test edge cases

**Issues you discovered during testing:**
- Initial normalization missed some punctuation characters, causing false misspellings  
- Case-sensitive comparisons led to unexpected misses before switching to `StringComparer.OrdinalIgnoreCase`

## HashSet vs List Understanding

**When to use HashSet:**
Use HashSet when you need fast lookups, automatic uniqueness, and set-based operations, such as checking dictionary membership or partitioning words into correct/misspelled sets.

**When to use List:**
Use List when order matters, duplicates are relevant, or you need to count occurrences, as in `allWordsInText` for frequency analysis.

**Performance benefits observed:**
HashSet lookups allowed checking thousands of words against the dictionary in near-constant time. The automatic uniqueness of HashSet simplified categorization and reduced memory usage by avoiding duplicates.

## Real-World Applications

**How this relates to actual spell checkers:**
Real spell checkers like Microsoft Word or Google Docs rely on similar principles: a dictionary for fast word lookup, text normalization to handle punctuation and capitalization, and frequency or context analysis to suggest corrections.

**What you learned about text processing:**
I learned the importance of consistent normalization, handling edge cases in real-world text, and combining multiple data structures for efficiency and functionality. Small variations in text can cause major differences if preprocessing is not thorough.

## Stretch Features

None implemented.

## Time Spent

**Total time:** 6 hours

**Breakdown:**
- Understanding HashSet concepts and assignment requirements: 1.5 hours
- Implementing the 6 core methods: 2.5 hours
- Testing different text files and scenarios: 1 hour
- Debugging and fixing issues: 0.75 hours
- Writing these notes: 0.25 hours

**Most time-consuming part:** 
Text normalization and ensuring consistent comparison across dictionary and text input took the longest, due to punctuation, mixed cases, and handling edge scenarios.

## Key Learning Outcomes

**HashSet concepts learned:**
I learned how HashSet enables O(1) lookups, automatic uniqueness, and set-based operations, which are extremely useful for text analysis and categorization.

**Text processing insights:**
Normalization and tokenization are critical for accurate analysis. Handling punctuation, whitespace, and capitalization consistently is key to reducing false negatives and positives.

**Software engineering practices:**
I practiced defensive programming (clearing collections before reuse), robust error handling for file I/O, and designing reusable helper methods like `NormalizeWord` to improve code clarity and maintainability.