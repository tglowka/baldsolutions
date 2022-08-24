module Main where

import Control.Exception

-- types
type Identifier = String
type Environment =[(Identifier, Value)]

-- data
data Expression = Number Int
    | Boolean Bool
    | Var Identifier
    | Plus Expression Expression
    | Minus Expression Expression
    | Times Expression Expression
    | Divide Expression Expression
    | Power Expression Expression
    | Equals Expression Expression
    | Let Defn Expression
    | Lambda [Identifier] Expression
    | Apply Expression [Expression]
    | If Expression Expression Expression
    deriving (Show, Eq)

data Value = 
      NumberValue Int |
      BoolValue Bool |
      Closure [Identifier] Expression Environment 
      deriving (Show, Eq)

data Defn = Val Identifier Expression |
            Rec Identifier Expression
            deriving (Show, Eq)

data MyException = ThisException | ThatException deriving Show

instance Exception MyException

-- functions
eval :: Expression -> Environment -> Value
eval (Number number) environment = NumberValue number
eval (Boolean b) environment = BoolValue b
eval (Equals e1 e2) env = BoolValue $ (eval e1 env) == (eval e2 env)
eval (If g e1 e2) env = case  eval g env of   
                        (BoolValue True) -> eval e1 env
                        (BoolValue False) -> eval e2 env
                       
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

eval (Let definition expression) environment = 
      eval expression (extendEnvironment environment definition)

eval (Lambda identifiers expression) environment = Closure identifiers expression environment

eval (Apply f xs) environment = apply f' xs'
      where f' = eval f environment
            xs' = map (flip eval environment) xs

apply :: Value -> [Value] -> Value
apply (Closure identifiers expression environment) values = eval expression (zip identifiers values ++ environment)
apply _ _ = throw ThisException  

find :: Environment -> Identifier -> Value
find environment identifier = snd $ head $ filter (\(x', _) -> x' == identifier) environment

extendEnvironment environment (Val identifier expression) = (identifier, eval expression environment) : environment
extendEnvironment environment (Rec identifier (Lambda args expression)) = environment' where environment' = (identifier, Closure args expression environment'):environment
extendEnvironment _ _ = error "Only lambdas can be recursive"      



main :: IO ()
main = putStrLn "Hello, Haskell!"
