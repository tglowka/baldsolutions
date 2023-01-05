module Main where

import HelperModule
import PositionsModule
import ReadFileModule

{-
  Description: The 'processBoardPosition' function solves the puzzle and returns the solution.
  Arguments:
  - Int - the maximal x index (breadth of the puzzle board)
  - Int - the maximal y index (length of the puzzle board)
  - [((Int, Int), Int)] - the list of points (digits) to process in order to solve the puzzle. Single element: ((x-coordinate, y-coordinate), digit)
  - [((Int, Int), Int)] - the list of points that have been already processed
  - [(Int, Int)] - the list of points that have been already marked
  Returns:
  - [(Int, Int)] - if solution exists -> list of the marked points on the board
                   otherwise -> [(-1, -1)]

-}
processBoardPosition :: Int -> Int -> [((Int, Int), Int)] -> [((Int, Int), Int)] -> [(Int, Int)] -> [(Int, Int)]
processBoardPosition _ _ [] _ markedPoints = markedPoints
processBoardPosition maxX maxY (point : restPoints) processedPoints markedPoints =
  isOK maxX maxY point combinations processedPoints markedPoints
  where
    pointCoordiantes = fst point
    poitnValue = snd point
    currentNeighbours = getCurrentNeighboursPositions maxX maxY pointCoordiantes markedPoints
    currentNeighboursCount = length currentNeighbours
    possibleNeighbours = getPossibleNeighboursPositions maxX maxY pointCoordiantes
    missingNeighboursCount = poitnValue - currentNeighboursCount
    unusedNeighbours = [x | x <- possibleNeighbours, x `notElem` currentNeighbours]
    combinations = getAllCombinations missingNeighboursCount unusedNeighbours
    isOK :: Int -> Int -> ((Int, Int), Int) -> [[(Int, Int)]] -> [((Int, Int), Int)] -> [(Int, Int)] -> [(Int, Int)]
    isOK _ _ _ [] _ _ = [(-1, -1)]
    isOK maxX maxY point (cmb : cmbs) processedPoints markedPoints
      | result == [(-1, -1)] = isOK maxX maxY point cmbs processedPoints markedPoints -- the given combination (cmb) does not fit -> checks other combinations
      | otherwise = evalNext processNext -- the given combination (cmb) is fine (i.e. does not corrupt processed points) -> process next point (next digit from puzzle)
      where
        result = markAndCheckNeighbours maxX maxY point cmb processedPoints markedPoints
        processNext = processBoardPosition maxX maxY restPoints ([point] ++ processedPoints) result
        evalNext resultNext
          | resultNext == [(-1, -1)] = isOK maxX maxY point cmbs processedPoints markedPoints -- if next processed point returns [(-1,-1)] then try to look for solution  with next combination
          | otherwise = resultNext

{-
  Description: The 'markAndCheckNeighbours' function marks new points of currently processed point and perform required checks.
               By marking new fields of given point, some of the previously processed points might now have more neighbours than expected.
               Previously processed points are checked here.
  Arguments:
  - Int - the maximal x index (breadth of the puzzle board)
  - Int - the maximal y index (length of the puzzle board)
  - ((Int, Int), Int) - currently processed point -> ((x-coordinate, y-coordinate), digit)
  - [(Int, Int)] - the list of points we want to mark for currently processed point
  - [((Int, Int), Int)] - the list of points that have been already processed
  - [(Int, Int)] - the list of points that have been already marked
  Returns: - if newly marked points does not corrupted currently processed points -> (the list of points that have been already marked) + (newly marked points)
           - otherwise (at least one of the previously processed points would have more neighbours that expected) -> [(-1,-1)]

-}
markAndCheckNeighbours :: Int -> Int -> ((Int, Int), Int) -> [(Int, Int)] -> [((Int, Int), Int)] -> [(Int, Int)] -> [(Int, Int)]
markAndCheckNeighbours _ _ _ [] _ markedPoints = markedPoints
markAndCheckNeighbours _ _ _ newMarkedPoints [] [] = newMarkedPoints
markAndCheckNeighbours maxX maxY ((x, y), z) newMarkedPoints processedPoints markedPoints
  | result = newMarkedPoints ++ markedPoints
  | otherwise = [(-1, -1)]
  where
    pointsToCheck =
      [ x1
        | x1 <- processedPoints,
          y1 <- getPointsToCheck (x, y),
          fst x1 == y1
      ]
    validatePointWithNewMarked :: [((Int, Int), Int)] -> [(Int, Int)] -> Bool
    validatePointWithNewMarked [] _ = True
    validatePointWithNewMarked (point : points) markedPoints
      | pointValue < currentNeighboursCount = False -- one of the previously processed points has more neighbours that expected
      | otherwise = validatePointWithNewMarked points markedPoints
      where
        pointCoordiantes = fst point
        pointValue = snd point
        currentNeighboursCount = length $ getCurrentNeighboursPositions maxX maxY pointCoordiantes markedPoints
    result = validatePointWithNewMarked pointsToCheck (newMarkedPoints ++ markedPoints)

main :: IO ()
main = do
  putStrLn "Enter file name: "
  filename <- getLine
  puzzle <- readPuzzle filename
  let maxX = length (head puzzle) -1
  let maxY = length puzzle -1
  let boardPositions = getBoardPositions puzzle
  let final = processBoardPosition maxX maxY boardPositions [] []
  let filledBoard = getFilledBoard maxX maxY final
  printFilledBoard filledBoard