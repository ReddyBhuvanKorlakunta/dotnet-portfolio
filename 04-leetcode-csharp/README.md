# LeetCode — C# solutions (your own work only)

**Profile:** _add your LeetCode profile URL here — do not add anyone else's._

How to use this folder for the vendor submission:
1. Link the profile in the submission email, next to the portfolio repo.
2. Mirror 10–15 of your best C# solutions here, one file per problem, with
   the problem name, approach in plain English, time/space complexity, and
   a test case. Clean, readable C# matters more here than clever one-liners.
3. Prefer problems that map to enterprise work: strings, collections,
   dictionaries, sorting, two pointers, hash maps — not just puzzles.
4. Never paste solutions you did not write, and never claim a LeetCode
   profile as a substitute for the front-end / WCAG portfolio in `01-` and `03-`.

## Template (copy per problem)
```csharp
// Problem: Two Sum
// Approach: one pass, dictionary of value -> index.
// Time: O(n), Space: O(n)
public class Solution {
    public int[] TwoSum(int[] nums, int target) {
        var seen = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++) {
            if (seen.TryGetValue(target - nums[i], out int j)) return new[] { j, i };
            seen[nums[i]] = i;
        }
        throw new ArgumentException("No solution");
    }
}
```
