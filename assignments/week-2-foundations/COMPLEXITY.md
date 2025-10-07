# Predicting Big-O on Different Structures and Their Operations  
**Student Name:** Giecel Tumbaga  

---

## Prediction Table

| **Structure** | **Operation** | **Big-O (Avg)** | **One-Sentence Rationale** |
|----------------|---------------|-----------------|-----------------------------|
| Array | Access by index | **O(1)** | Directly accesses memory location using the index. |
| Array | Search (unsorted) | **O(n)** | Must check each element until a match is found. |
| List&lt;T&gt; | Add at end | **O(1)** | Appending is constant on average; resizing happens occasionally. |
| List&lt;T&gt; | Insert at index | **O(n)** | Requires shifting all elements after the insert position. |
| Stack&lt;T&gt; | Push / Pop / Peek | **O(1)** | All operations occur at a single end of the structure. |
| Queue&lt;T&gt; | Enqueue / Dequeue / Peek | **O(1)** | Operates at fixed front and rear positions. |
| Dictionary&lt;K,V&gt; | Add / Lookup / Remove | **O(1)** (avg) | Uses hashing for direct access; worst case O(n) with collisions. |
| HashSet&lt;T&gt; | Add / Contains / Remove | **O(1)** (avg) | Uses hashing like Dictionary but stores only unique values. |