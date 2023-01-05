module HelperModule where

{-
  Description: The 'toInt' function converts the list of chars to integer.
  Arguments:
  - [Char] - the list of chars to convert
  Returns:
  - Int - the converted integer
-}
toInt :: [Char] -> Int
toInt x = read x :: Int

{-
  Description: The 'getAllCombinations' generates all possible combinations without repetitions of given list elements.
  Arguments:
  - Int - size of the combination subset
  - Any - the list that combinations are made
  Returns:
  - [[Any]] - the list of all possible combinations without repetitions

-}
getAllCombinations :: Int -> [a] -> [[a]]
getAllCombinations 0 _ = [[]]
getAllCombinations _ [] = []
getAllCombinations n (x : xs) = (map (x :) (getAllCombinations (n -1) xs)) ++ (getAllCombinations n xs)

{-
  Description: The 'getFilledBoard' function generates the board with marked points. Point that has been marked is represented as 'x', others as '.'.
  Arguments:
  - Int - the maximal x index (breadth of the puzzle board)
  - Int - the maximal y index (length of the puzzle board)
  - [(Int, Int)] - the list of marked points
  Returns:
  - [String] - the list of strings. Each elements represents one row of the final solved board.
-}
getFilledBoard :: Int -> Int -> [(Int, Int)] -> [String]
getFilledBoard maxX maxY (x : xs) =
  getBoard 0 0 (x : xs) [] []
  where
    getBoard :: Int -> Int -> [(Int, Int)] -> String -> [String] -> [String]
    getBoard _ _ [] _ result = result
    getBoard counterX counterY points agg result
      | counterY > maxY = result
      | (counterX, counterY) `elem` points && counterX == maxX = getBoard 0 (counterY + 1) points [] (result ++ [agg ++ ['x']])
      | (counterX, counterY) `elem` points && counterX < maxX = getBoard (counterX + 1) counterY points (agg ++ ['x']) result
      | (counterX, counterY) `notElem` points && counterX == maxX = getBoard 0 (counterY + 1) points [] (result ++ [agg ++ ['.']])
      | (counterX, counterY) `notElem` points && counterX < maxX = getBoard (counterX + 1) counterY points (agg ++ ['.']) result

{-
  Description: The 'printFilledBoard' function prints the board.
  Arguments:
  - [String] - the board to print
  Returns:
  - IO - printed board
-}
printFilledBoard :: [String] -> IO ()
printFilledBoard [] = return ()
printFilledBoard (x : xs) =
  do
    putStrLn x
    printFilledBoard xs