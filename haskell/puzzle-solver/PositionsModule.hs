module PositionsModule where

import HelperModule

{-
  Description: The 'getRowPositions' function processes one row from the puzzle and returns list of digits' positions with the digits itself.
  Arguments:
  - String - the list of chars that represents one row from the puzzle
  Returns:
  - [(Int, Int)] - the list of tuples. First element - 0-based index of the digit, second element - the digit
-}
getRowPositions :: String -> [(Int, Int)]
getRowPositions [] = []
getRowPositions (x : xs) = getPositions (x : xs) [] 0
  where
    getPositions :: String -> [(Int, Int)] -> Int -> [(Int, Int)]
    getPositions [] positions _ = positions
    getPositions (x : xs) positions counter
      | x == '.' = getPositions xs positions (counter + 1)
      | otherwise = getPositions xs (positions ++ [(counter, toInt [x])]) (counter + 1)

{-
  Description: The function 'getBoardPositions' processes the whole puzzle and returns the digits' positions with the digits itself.
  Arguments:
  - [String] - the puzzle
  Returns:
  - [((Int, Int), Int)] - the list of digits' positions with the digits itself. Single element: ((x-coordinate, y-coordinate), digit)
-}
getBoardPositions :: [String] -> [((Int, Int), Int)]
getBoardPositions [] = []
getBoardPositions (x : xs) = func (x : xs) 0
  where
    func :: [String] -> Int -> [((Int, Int), Int)]
    func [] _ = []
    func (x : xs) rowIdx =
      [ ((fst y, rowIdx), snd y)
        | y <- getRowPositions x
      ]
        ++ func xs (rowIdx + 1)

{-
  Description: The 'getPossibleNeighboursPositions' function returns all possbile neighbours for given point.
  We assume that the puzzle might be showed like this:
                                                      (0,0)
                                                          ---------> (x)
                                                          |
                                                          |
                                                          |
                                                          v(y)
  Arguments:
  - Int - the maximal x index (breadth of the puzzle board)
  - Int - the maximal y index (length of the puzzle board)
  - (Int, Int) - the point coordinates
  Returns:
  - [(Int, Int)] - the list of positions of all possible neighbours of given point
-}
getPossibleNeighboursPositions :: Int -> Int -> (Int, Int) -> [(Int, Int)]
getPossibleNeighboursPositions maxX maxY (x, y)
  | x == 0 && y == 0 = [p5, p6, p8, p9] --left right corner
  | x > 0 && x < maxX && y == 0 = [p4, p5, p6, p7, p8, p9] --upper row (without corners)
  | x == maxX && y == 0 = [p4, p5, p7, p8] --right upper corner
  | x == 0 && y == maxY = [p2, p3, p5, p6] --left bottom corner
  | x > 0 && x < maxX && y == maxY = [p1, p2, p3, p4, p5, p6] --bottom row (without corners)
  | x == maxX && y == maxY = [p1, p2, p4, p5] --prawy dolny rog
  | x == 0 && y > 0 && y < maxY = [p2, p3, p5, p6, p8, p9] --left-most column
  | x == maxX && y > 0 && y < maxY = [p1, p2, p4, p5, p7, p8] --right-most column
  | otherwise = [p1, p2, p3, p4, p5, p6, p7, p8, p9] --other points (everything but first,last row/column)
  where
    p1 = (x -1, y -1)
    p2 = (x, y -1)
    p3 = (x + 1, y - 1)
    p4 = (x -1, y)
    p5 = (x, y)
    p6 = (x + 1, y)
    p7 = (x -1, y + 1)
    p8 = (x, y + 1)
    p9 = (x + 1, y + 1)

{-
  Description: The 'getCurrentNeighboursPositions function returns current neighbours of the given point.
  Arguments:
  - Int - the maximal x index (breadth of the puzzle board)
  - Int - the maximal y index (length of the puzzle board)
  - (Int, Int) - the point coordinates
  - [(Int, Int)] - currently marked points
  Returns:
  - [(Int, Int)] - the list of positions of all current neighbours of given point
-}
getCurrentNeighboursPositions :: Int -> Int -> (Int, Int) -> [(Int, Int)] -> [(Int, Int)]
getCurrentNeighboursPositions _ _ _ [] = []
getCurrentNeighboursPositions maxX maxY (x, y) markedPoints =
  [ x1
    | x1 <- markedPoints,
      y1 <- getPossibleNeighboursPositions maxX maxY (x, y),
      x1 == y1
  ]

{-
  Description: The 'getPointsToCheck' functions returns the list of points that have to be checked.
               By marking new fields of given point, some of the previously processed points might now have more neighbours than expected.
  Arguments:
  - (Int, Int) - the point coordinates
  Returns:
  - [(Int, Int)] - the list of the points to check
-}
getPointsToCheck :: (Int, Int) -> [(Int, Int)]
getPointsToCheck (x, y) =
  [ (x1, y1)
    | x1 <- [x -2 .. x + 2],
      y1 <- [y -2 .. y],
      (x1, y1) /= (x, y),
      (x1, y1) /= (x + 1, y),
      (x1, y1) /= (x + 2, y),
      x1 >= 0,
      y1 >= 0
  ]
