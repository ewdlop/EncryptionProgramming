Here's how to implement a **piecewise function** using different approaches: `switch` expression, `switch` statement, or `if`-statement, depending on the language and preference.

### Example Problem
Define a piecewise function \( f(x) \) as follows:
\[
f(x) =
\begin{cases} 
x^2 & \text{if } x < 0, \\
2x + 1 & \text{if } 0 \leq x \leq 10, \\
3x - 5 & \text{if } x > 10.
\end{cases}
\]

---

### **1. Using a `switch` Expression (Java 17+ or C#)**
```java
// Java 17+ Example
public static int piecewiseFunction(int x) {
    return switch (x) {
        case int n when n < 0 -> x * x;      // x^2 if x < 0
        case int n when n >= 0 && n <= 10 -> 2 * x + 1;  // 2x + 1 if 0 <= x <= 10
        default -> 3 * x - 5;               // 3x - 5 if x > 10
    };
}
```

### **2. Using a `switch` Statement**
For languages without expression-based switches, you can use ranges in custom logic.

```java
// Java Example (using statements for conditions)
public static int piecewiseFunction(int x) {
    switch (true) { // Simulate range-based checks
        case (x < 0):
            return x * x; // x^2
        case (x >= 0 && x <= 10):
            return 2 * x + 1; // 2x + 1
        default:
            return 3 * x - 5; // 3x - 5
    }
}
```

---

### **3. Using an `if`-Statement**
This is the most universal approach, suitable for any language.

```java
// Java Example
public static int piecewiseFunction(int x) {
    if (x < 0) {
        return x * x;        // x^2
    } else if (x >= 0 && x <= 10) {
        return 2 * x + 1;    // 2x + 1
    } else {
        return 3 * x - 5;    // 3x - 5
    }
}
```

### Python Equivalent for the `if`-Statement
```python
def piecewise_function(x):
    if x < 0:
        return x ** 2  # x^2
    elif 0 <= x <= 10:
        return 2 * x + 1  # 2x + 1
    else:
        return 3 * x - 5  # 3x - 5
```

---

### **4. Python’s Dictionary-Based Switch (Using Lambda Functions)**
For a Pythonic twist, you can use dictionary mapping with custom ranges:

```python
def piecewise_function(x):
    conditions = {
        "negative": lambda x: x ** 2,
        "midrange": lambda x: 2 * x + 1,
        "large": lambda x: 3 * x - 5,
    }
    if x < 0:
        return conditions["negative"](x)
    elif 0 <= x <= 10:
        return conditions["midrange"](x)
    else:
        return conditions["large"](x)
```

---

### Which Approach to Use?
- **Switch expressions** are concise and ideal for modern languages with support (e.g., Java 17+, C#).
- **Switch statements** are less concise but provide structured logic.
- **If-statements** are the most flexible and widely supported.
- **Python dictionaries** offer a creative functional programming style for piecewise logic.

Let me know if you want to tailor the code to a specific language or application! 🚀
