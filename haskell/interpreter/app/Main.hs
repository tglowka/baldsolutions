module Main where

import Control.Exception

-- types
type Identifier = String
type Environment =[(Identifier, Value)]

-- data
data Expression = Number Int
    | Var Identifier
    | Plus Expression Expression
    | Minus Expression Expression
    | Times Expression Expression
    | Divide Expression Expression
    | Power Expression Expression
    | Let Identifier Expression Expression
    | Lambda [Identifier] Expression
    | Apply Expression [Expression]
    deriving Show

data Value = 
      NumberValue Int |
      Closure [Identifier] Expression Environment 
      deriving Show

data MyException = ThisException | ThatException deriving Show

instance Exception MyException

instance Eq Value where
    (==) (NumberValue x) (NumberValue y)  = x == y

-- functions
eval :: Expression -> Environment -> Value
eval (Number number) environment = NumberValue number
eval (Plus left righ) environment = NumberValue (left' + righ')
                                    where (NumberValue left') = eval left environment
                                          (NumberValue righ') = eval righ environment

eval (Minus left righ) environment = NumberValue (left' - righ')
                                    where (NumberValue left') = eval left environment
                                          (NumberValue righ') = eval righ environment

eval (Times left righ) environment = NumberValue (left' * righ')
                                    where (NumberValue left') = eval left environment
                                          (NumberValue righ') = eval righ environment

eval (Divide left righ) environment = NumberValue (left' `div` righ')
                                    where (NumberValue left') = eval left environment
                                          (NumberValue righ') = eval righ environment

eval (Power left righ) environment = NumberValue (left' ^ righ')
                                    where (NumberValue left') = eval left environment
                                          (NumberValue righ') = eval righ environment

eval (Var identifier) environment = find environment identifier

eval (Let identifier expression1 expression2) environment = 
      eval expression2 (extendEnvironment environment identifier expression1)

eval (Lambda identifiers expression) environment = Closure identifiers expression environment

eval (Apply f xs) environment = apply f' xs'
      where f' = eval f environment
            xs' = map (flip eval environment) xs

apply :: Value -> [Value] -> Value
apply (Closure identifiers expression environment) values = eval expression (zip identifiers values ++ environment)
apply _ _ = throw ThisException  

find :: Environment -> Identifier -> Value
find environment identifier = snd $ head $ filter (\(x', _) -> x' == identifier) environment

extendEnvironment :: Environment -> Identifier -> Expression -> Environment
extendEnvironment environment identifier expression = 
      (identifier, eval expression environment) : environment


main :: IO ()
main = putStrLn "Hello, Haskell!"
