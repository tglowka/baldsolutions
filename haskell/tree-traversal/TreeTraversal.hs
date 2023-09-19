module TreeTraversal where

import Control.Monad.State
import Control.Monad.Trans.State (evalState)
import Data.Char (isDigit, isNumber)

data Tree a = Node a (Tree a) (Tree a) | Null

astTree :: Tree String
astTree =
  Node
    "+"
    ( Node
        "-"
        (Node "10" Null Null)
        ( Node
            "/"
            (Node "16" Null Null)
            (Node "8" Null Null)
        )
    )
    ( Node
        "*"
        ( Node
            "-"
            (Node "20" Null Null)
            (Node "14" Null Null)
        )
        (Node "20" Null Null)
    )

intTree :: Tree Int
intTree =
  Node
    10
    ( Node
        5
        (Node 1 Null Null)
        ( Node
            7
            (Node 6 Null Null)
            (Node 8 Null Null)
        )
    )
    ( Node
        15
        ( Node
            12
            (Node 11 Null Null)
            (Node 14 Null Null)
        )
        (Node 20 Null Null)
    )

clone :: Tree a -> Tree a
clone Null = Null
clone (Node node left right) = Node node (clone left) (clone right)

preorder :: Tree a -> [a]
preorder Null = []
preorder (Node node left right) = node : preorder left ++ preorder right

inorder :: Tree a -> [a]
inorder Null = []
inorder (Node node left right) = inorder left ++ [node] ++ inorder right

postorder :: Tree a -> [a]
postorder Null = []
postorder (Node node left right) = postorder left ++ postorder right ++ [node]

-- first implementation of reverse polish notation
reversePolishNotation :: [String] -> Int
reversePolishNotation tokens = func tokens []
  where
    func :: [String] -> [Int] -> Int
    func [] stack = head stack
    func (token : tokens) (rightOp : leftOp : stack)
      | token == "+" = func tokens (leftOp + rightOp : stack)
      | token == "-" = func tokens (leftOp - rightOp : stack)
      | token == "/" = func tokens (leftOp `div` rightOp : stack)
      | token == "*" = func tokens (leftOp * rightOp : stack)
    func (x : xs) stack
      | isNumber (head x) = func xs (read x : stack)

-- second implementation of reverse polish notation based on state monad
reversePolishNotationStateMonad :: [String] -> Int
reversePolishNotationStateMonad tokens = evalState (reversePolishNotationStateMonad' tokens) []

reversePolishNotationStateMonad' :: [String] -> State [Int] Int
reversePolishNotationStateMonad' [] = do
  pop
reversePolishNotationStateMonad' (x : xs) = do
  eval x
  reversePolishNotationStateMonad' xs
  where
    eval token
      | all isDigit token = push $ read token
      | token == "+" = plus
      | token == "-" = minus
      | token == "*" = mul
      | token == "/" = div'
      | otherwise = undefined

push :: Int -> State [Int] ()
push x = do
  stack <- get
  put (x : stack)

pop :: State [Int] Int
pop = do
  stack <- get
  put (tail stack)
  return (head stack)

plus :: State [Int] ()
plus = do
  x <- pop
  y <- pop
  push (y + x)

minus :: State [Int] ()
minus = do
  x <- pop
  y <- pop
  push (y - x)

mul :: State [Int] ()
mul = do
  x <- pop
  y <- pop
  push (y * x)

div' :: State [Int] ()
div' = do
  x <- pop
  y <- pop
  push (y `div` x)