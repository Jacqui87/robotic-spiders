# Robotic Spiders Wall Explorer

C# console application simulating a squad of autonomous robotic spiders exploring micro fractures on a rectangular wall. Each spider moves according to commands (`L`, `R`, `F`) while respecting wall boundaries. Fully unit tested and designed with OOP principles.

---

### 🌐 Features

- Move spiders with commands, respecting wall edges.  
- Validates wall size, starting position, and instructions.  
- Clear error messages for invalid input or out-of-bounds moves.  
- Interfaces (`ISpider`, `IWall`) for clean, testable architecture.  
- Unit tests with **xUnit** and **NSubstitute**.

---

## 🚀 Technical Test

**Overview**  
Spiders navigate a rectangular wall to capture diagnostics. Position is represented by `(X Y Direction)` and the wall is divided into a grid. Spiders execute commands sent as a string:  

- `L` → turn left 90°  
- `R` → turn right 90°  
- `F` → move forward one grid point  
- Up from `(x, y)` is `(x, y+1)`

**Input (3 lines):**  
1. Wall size: `0 0` (bottom-left) to `x y` (top-right)  
2. Spider start: `X Y Direction`  
3. Instructions: e.g., `FLFLFRFFLF`

**Output:**  
Final spider coordinates and heading, e.g., `5 7 Right`.

**Example Input:**

7 15
4 10 Left
FLFLFRFFLF

**Expected Output:**

5 7 Right
