# 1. Setup
To load and run the functions you start ghci and load mtl package:
```
:set -package mtl
```
then load a module:
```
:load TreeTraversal
```
and you are ready to go. The complete list of functions defined in the module are:
- astTree - returns an example of the arithmetic expression AST tree.
- intTree - returns an example of a binary tree with int-valued as a nodes.
- clone - returns a clone of a tree given as first argument.
- preorder - traverse a tree and returns a collection of nodes in a pre-order fashion.
- inorder - traverse a tree and returns a collection of nodes in a in-order fashion. 
- postorder - traverse a tree and returns a collection of nodes in a post-order fashion. 
- reversePolishNotation - computes an expression represented as a valid collection of tokens in reverse polish notation 
- reversePolishNotationStateMonad - computes an expression represented as a valid collection of tokens in reverse polish notation (implementation with a state monad).

# 2. Examples
```
ghci> preorder $ clone intTree
[10,5,1,7,6,8,15,12,11,14,20]

ghci> preorder intTree 
[10,5,1,7,6,8,15,12,11,14,20]

ghci> preorder astTree 
["+","-","10","/","16","8","*","-","20","14","20"]

ghci> preorder $ clone astTree 
["+","-","10","/","16","8","*","-","20","14","20"]

ghci> reversePolishNotation $ postorder astTree 
128

ghci> reversePolishNotationStateMonad $ postorder astTree       
128

ghci> reversePolishNotation ["1", "2", "+"]
3

ghci> reversePolishNotationStateMonad ["1", "2", "+"]
3
```