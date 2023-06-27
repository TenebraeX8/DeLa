# Instructionset
DeLa is using a Stack-machine thus needing only one byte for each instruction

## Environment
The runtime features a 4-Byte aligned expression stack, as well as a data-area containing the global variables specified in the script (Stack machine).

## Overview
| Instruction | Code | Description |
|---        |---   |---            |
| Push      | 0x00 | Pushes an immediate value to the stack |
| Pop       | 0x01 | Pops top from the stack |
| Load      | 0x02 | Loads the specified data address |
| Store     | 0x03 | Stores top to the specified data address |
| Add       | 0x0A | Adds the topmost 2 values and pushes the result on the stack |
| Sub       | 0x0B | Subtracts the topmost 2 values and pushes the result on the stack |
| Mul       | 0x0C | Multiplies the topmost 2 values and pushes the result on the stack |
| Div       | 0x0D | Divides the topmost 2 values and pushes the result on the stack |
| Mod       | 0x0E | Multiplies the topmost 2 values and pushes the remainder on the stack |
| Neg       | 0x0F | Negates the topmost value on the stack |
| INC       | 0x10 | Increases the topmost value on the stack by 1 |
| DEC       | 0x11 | Decreases the topmost value on the stack by 1 |
| SHR       | 0x12 | Shift Right on bitlevel |
| SHL       | 0x13 | Shift Left on bitlevel |
| Out       | 0x20 | Prints the current top of the Expression stack |


### Push
```Push imm32```
Pushes an immediate, signed integer value to the stack.
Example:

    ...         --Stack: {}
    PUSH 1      --Stack: {1}
    Push 7      --Stack: {1,7}

### Pop
```Pop```
Pops the top value from the stack
Example:

    ...         --Stack: {1,2,3,4}
    POP         --Stack: {1,2,3}
    POP         --Stack: {1,2}

### Load
```Load imm32```
Loads the signed integer value from the specified address of the data section and pushes it onto the stack
Example:

    Data: 
        0:  0x00000005
        4:  0x0000000A
    Code:
        ...         --Stack: {}
        LOAD 0      --Stack: {5}
        LOAD 4      --Stack: {5,10}

### Store
```Store imm32```
Stores the signed integer value from the top of the stack to the specified address of the data section and pops from the stack.
Example:

    Data: 
        0:  0x00000000
        4:  0x00000000
    Code:
        ...         --Stack: {5, 10}
        STORE 4     --Stack: {5}, Data 0: 0x00000004
        STORE 0     --Stack: {},  Data 0: 0x00000004, Data 1: 0x0000000A

### Add
```Add```
Adds the topmost values of the stack and pushes the result.
Example:

    Code:
        ...         --Stack: {1,2,3,4}
        ADD         --Stack: {1,2,7}
        ADD         --Stack: {1,9}

### Sub
```Sub```
Subtracts the topmost values of the stack and pushes the result.
Example:

    Code:
        ...         --Stack: {4,3,2,1}
        SUB         --Stack: {4,3,1}
        SUB         --Stack: {4,2}

### Mul
```Mul```
Multiplies the topmost values of the stack and pushes the result.
Example:

    Code:
        ...         --Stack: {1,2,3,4}
        MUL         --Stack: {1,2,12}
        MUL         --Stack: {1,24}

### Div
```Div```
Divides the topmost values of the stack and pushes the result.
Example:

    Code:
        ...         --Stack: {16,8,4,2}
        DIV         --Stack: {16,8,2}
        DIV         --Stack: {16,4}

### Mod
```Mod```
Divides the topmost values of the stack and pushes the remainder of the division.
Example:

    Code:
        ...         --Stack: {74,57,31,20}
        Mod         --Stack: {74,57,11}
        Mod         --Stack: {16,2}

### Neg
```Neg```
Negates the topmost values of the stack.
Example:

    Code:
        ...         --Stack: {20}
        Neg         --Stack: {-20}
        Neg         --Stack: {20}

### Inc
```Inc```
Increases the topmost values of the stack by 1.
Example:

    Code:
        ...         --Stack: {20}
        Inc         --Stack: {21}
        Inc         --Stack: {22}

### Dec
```Dec```
Decreases the topmost values of the stack by 1.
Example:

    Code:
        ...         --Stack: {20}
        Dec         --Stack: {19}
        Dec         --Stack: {18}


### Shr
```Shr```
Shifts the second-topmost value by the value on top to the right on a bitlevel
Example:

    Code:
        ...         --Stack: {16, 32, 4}
        Shr         --Stack: {16, 2}
        Shr         --Stack: {4}

### Shl
```Shl```
Shifts the second-topmost value by the value on top to the left on a bitlevel
Example:

    Code:
        ...         --Stack: {1, 1, 4}
        Shl         --Stack: {1, 16}
        Shl         --Stack: {65536}

### Out
```Out```
Prints the top of the stack.
Example: 

    Code:
        ...         --Stack: {20, 22}
        Out         --Stack: {20}, Console: "22"
        Out         --Stack: {}, Console: "2220"
