module Test where

import Test.HUnit
import Control.Exception
import Control.Monad
import Main

assertException action = do
    result <- tryJust selectErrorCall (evaluate $ action)
    case result of
        Left val ->  assertFailure ("failure" ++ val)
        Right val ->  assertFailure "failure"
        
  where
    selectErrorCall :: MyException -> Maybe String
    selectErrorCall ThisException = Just "some error"
    selectErrorCall _ = Nothing

testNumber :: Test
testNumber =
    TestCase $ assertEqual "Function: eval. Number 11 gives NumberValue 11"
                            (NumberValue 11)
                            (eval (Number 11) [])

testVar :: Test
testVar =
    TestCase $ assertEqual "Function: eval. Var x gives x from environment"
                           (NumberValue 11)
                           (eval (Var "x") [("x", NumberValue 11)])                        

testPlus :: Test
testPlus = 
    TestCase $ assertEqual "Function: eval. Plus 11 and 10 gives NumberValue 1"
                           (NumberValue 21)
                           (eval (Plus (Number 11) (Number 10)) [])

testMinus :: Test
testMinus = 
    TestCase $ assertEqual "Function: eval. Minus 11 and 10 -> NumberValue 1"
                           (NumberValue 1)
                           (eval (Minus (Number 11) (Number 10)) [])

testTimes :: Test
testTimes = 
    TestCase $ assertEqual "Function: eval. Times 11 and 10 -> NumberValue 110"
                           (NumberValue 110)
                           (eval (Times (Number 11) (Number 10)) [])

testDivide :: Test
testDivide = 
    TestCase $ assertEqual "Function: eval. Divide 22 and 11 -> NumberValue 2"
                           (NumberValue 2)
                           (eval (Divide (Number 22) (Number 11)) [])

testPower :: Test
testPower = 
    TestCase $ assertEqual "Function: eval. Power 2 and 10 -> NumberValue 1024"
                           (NumberValue 1024)
                           (eval (Power (Number 2) (Number 10)) [])                           

testLet :: Test
testLet = 
    TestCase $ assertEqual "Function: eval. Let x = 10 in 2 * x gives 10"
                           (NumberValue 20)
                           (eval (( Let (Val "x" $ Number 10)) (Times (Number 2) (Var "x"))) [])


testLambda :: Test
testLambda =
    TestCase $ assertEqual "Function: eval. Let x = (y,z -> y + z ) in x 3 7 gives 10"
                           (NumberValue 10)
                           (eval (Let (Val "x" (Lambda ["y", "z"] (Plus (Var "y") (Var "z")))) (Apply (Var "x") [Number 3, Number 7])) []) 

testRecursiveLambda :: Test
testRecursiveLambda =
    TestCase $ assertEqual "Function: eval. Let sum = (x -> if x==0 then 0 else x + sum x-1) in x sum 10 gives 55"
                           (NumberValue 55)
                           (eval (Let (Rec "sum" (Lambda ["x"] (If (Equals (Var "x") (Number 0)) (Number 0) (Plus (Var "x") (Apply (Var "sum") [Minus (Var "x") (Number 1)]))))) (Apply (Var "sum") [Number 10])) [])

-- testLambdaError :: Test
-- testLambdaError =
--     TestCase $ assertException --TODO
--                            (eval (Let (Rec "sum" (Lambda ["x"] (If (Equals (Var "x") (Number 0)) (Number 0) (Plus (Var "x") (Apply (Var "sum") [Minus (Var "x") (Number 1)]))))) (Apply (Var "sum") (Number 3))))


main :: IO Counts
main = runTestTT $ TestList [
    testNumber,
    testVar,
    testPlus,
    testMinus,     
    testTimes,
    testDivide,    
    testPower,
    testLet,
    testLambda,
    testRecursiveLambda
    -- ,testLambdaError
    ]