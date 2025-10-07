# Comparing results from COMPLEXITY.md 
**Student Name:** Giecel Tumbaga 

---

## Comparison to Predictions

The measured times generally match the Big-O expectations:

- **Array**
  - Access by index is immediate (`O(1)`), as shown by retrieving `numbers[2]`.
  - Linear search times grow roughly with N, consistent with **O(n)**.  
    Example: `Array.Contains(99999)` took 0.405 ms for N=100,000.

- **List<int>**
  - Add at end and insert/remove operations worked correctly.
  - Membership checks (`Contains`) grew linearly with N, reflecting **O(n)**, though very small constants make even 100,000 elements check in ~0.057 ms.

- **Stack<T>**
  - Push/Pop operations scaled linearly with the number of elements, consistent with O(1) per operation.
  - Example: Pushing 100,000 items took 3.721 ms; popping took 11.656 ms.

- **Queue<T>**
  - Enqueue/Dequeue operations performed efficiently.
  - Example: Enqueuing 100,000 items: 2.167 ms; dequeuing: 1.230 ms.

- **Dictionary<int,bool>**
  - Membership checks (`ContainsKey`) were effectively constant time (**O(1)**), even for N=100,000.  
    Example: `Dictionary.ContainsKey(99999)` = 0.000 ms.

- **HashSet<int>**
  - Membership checks also showed **O(1)** behavior. Times were essentially 0 ms for all tested N.

---

## Observations and Notes

- **Small N Effects**: List and Array searches appear very fast for small datasets due to **CPU caching and low constant factors**, even though the operations are O(n).
- **Hash-based Structures**: Both `HashSet` and `Dictionary` maintained near-zero times regardless of size, demonstrating the power of hashing for membership tests.
- **Stack/Queue**: Even though individual operations are O(1), performing many sequential operations shows a cumulative effect (e.g., popping 100,000 items).

---

## Recommendations

- For **large datasets with frequent membership checks**, use **HashSet<T>** or **Dictionary<K,V>** due to constant-time lookup.
- For **small datasets or occasional membership checks**, `List<T>` or arrays may be sufficient.
- For sequential processing (LIFO/FIFO), `Stack<T>` or `Queue<T>` are optimal.

---

## Measured Results Summary

| Structure | N | Target | Time (ms) |
|-----------|---|--------|-----------|
| Array | 1,000 | 999 | 0.027 |
| Array | 1,000 | -1 | 0.027 |
| Array | 10,000 | 9999 | 0.095 |
| Array | 10,000 | -1 | 0.092 |
| Array | 100,000 | 99999 | 0.405 |
| Array | 100,000 | -1 | 0.327 |
| List<int> | 1,000 | 999 | 0.001 |
| List<int> | 1,000 | -1 | 0.001 |
| List<int> | 10,000 | 9999 | 0.007 |
| List<int> | 10,000 | -1 | 0.007 |
| List<int> | 100,000 | 99999 | 0.057 |
| List<int> | 100,000 | -1 | 0.057 |
| Dictionary<int,bool> | 1,000 | 999 | 0.000 |
| Dictionary<int,bool> | 1,000 | -1 | 0.000 |
| Dictionary<int,bool> | 10,000 | 9999 | 0.000 |
| Dictionary<int,bool> | 10,000 | -1 | 0.000 |
| Dictionary<int,bool> | 100,000 | 99999 | 0.000 |
| Dictionary<int,bool> | 100,000 | -1 | 0.000 |
| HashSet<int> | 1,000 | 999 | 0.000 |
| HashSet<int> | 1,000 | -1 | 0.000 |
| HashSet<int> | 10,000 | 9999 | 0.000 |
| HashSet<int> | 10,000 | -1 | 0.000 |
| HashSet<int> | 100,000 | 99999 | 0.000 |
| HashSet<int> | 100,000 | -1 | 0.000 |